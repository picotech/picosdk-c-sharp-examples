/**************************************************************************
 *
 * Filename: PS5000AStreamingCon.cs
 * 
 * Description:
 *   This is a console-mode program that demonstrates how to use the
 *   PicoScope 5000 Series (ps5000a) driver API functions using .NET
 *
 * Examples:
 *    Collect a stream of data immediately
 *    Collect a stream of data when a trigger event occurs
 *    
 * Copyright (C) 2015-2018 Pico Technology Ltd. See LICENSE file for terms.
 *
 **************************************************************************/

using System;
using System.IO;
using System.Text;
using System.Threading;

using PS5000AImports;
using PicoPinnedArray;
using PicoStatus;

namespace PS5000AStreamingConsole
{
    struct ChannelSettings
    {
        public Imports.Range range;
        public bool enabled;
    }

    class StreamingCon
    {
        private readonly short _handle;
        int _channelCount;
        private ChannelSettings[] _channelSettings;


        short[][] appBuffers;
        short[][] buffers;

        ushort[] inputRanges = { 10, 20, 50, 100, 200, 500, 1000, 2000, 5000, 10000, 20000, 50000 };

        bool _autoStop;
        bool _powerSupplyConnected;
        bool _ready = false;

        short _maxValue;
        short _trig = 0;

        int _noEnabledChannels;
        int _sampleCount;
        
        uint _trigAt = 0;
        uint _startIndex;
        
        Imports.DeviceResolution _resolution = Imports.DeviceResolution.PS5000A_DR_8BIT;

        public Imports.Range _firstRange = Imports.Range.Range_10mV;
        public Imports.Range _lastRange = Imports.Range.Range_20V;

        private string StreamFile = "stream.txt";

        /****************************************************************************
        * Callback
        * Used by ps5000a data streaming collection calls, on receipt of data.
        * Used to set global flags etc checked by user routines
        ****************************************************************************/
        void StreamingCallback(short handle,
                                int noOfSamples,
                                uint startIndex,
                                short overflow,
                                uint triggerAt,
                                short triggered,
                                short autoStop,
                                IntPtr pVoid)
        {
            // used for streaming
            _sampleCount = noOfSamples;
            _startIndex = startIndex;
            _autoStop = autoStop != 0;

            _ready = true;

            // flags to show if & where a trigger has occurred
            _trig = triggered;
            _trigAt = triggerAt;

            if (_sampleCount != 0)
            {
                for (int ch = 0; ch < _channelCount * 2; ch += 2)
                {
                    if (_channelSettings[(int)(Imports.Channel.ChannelA + (ch / 2))].enabled)
                    {

                        Array.Copy(buffers[ch], _startIndex, appBuffers[ch], _startIndex, _sampleCount); //max
                        Array.Copy(buffers[ch + 1], _startIndex, appBuffers[ch + 1], _startIndex, _sampleCount);//min

                    }
                }
            }
        }

        /****************************************************************************
        * WaitForKey
        *  Waits for the user to press a key
        *  
        ****************************************************************************/
        private static void WaitForKey()
        {
            while (!Console.KeyAvailable) Thread.Sleep(100);

            if (Console.KeyAvailable)
            {
                Console.ReadKey(true); // clear the key
            }
        }


        /****************************************************************************
         * adc_to_mv
         *
         * Convert an 16-bit ADC count into millivolts
         ****************************************************************************/
        int adc_to_mv(int raw, int ch)
        {
            return (raw * inputRanges[ch]) / _maxValue;
        }

        /****************************************************************************
         * mv_to_adc
         *
         * Convert a millivolt value into a 16-bit ADC count
         *
         *  (useful for setting trigger thresholds)
         ****************************************************************************/
        short mv_to_adc(short mv, short ch)
        {
            return (short)((mv * _maxValue) / inputRanges[ch]);
        }

        /****************************************************************************
       * Stream Data Handler
       * - Used by the two stream data examples - untriggered and triggered
       * Inputs:
       * - unit - the unit to sample on
       * - preTrigger - the number of samples in the pre-trigger phase 
       *					(0 if no trigger has been set)
       ***************************************************************************/
        void StreamDataHandler(uint preTrigger)
        {
            int sampleCount = 1024 * 100; /*  *100 is to make sure buffer large enough */

            appBuffers = new short[_channelCount * 2][];
            buffers = new short[_channelCount * 2][];

            uint postTriggerSamples = 1000000; // Number of raw data samples
            int totalSamples = 0;
            uint triggeredAt = 0;
            uint sampleInterval = 1;
            uint status;

            // Use Pinned Arrays for the application buffers
            PinnedArray<short>[] appBuffersPinned = new PinnedArray<short>[_channelCount * 2];

            for (int ch = 0; ch < _channelCount * 2; ch += 2) // create data buffers
            {
                buffers[ch] = new short[sampleCount];
                buffers[ch + 1] = new short[sampleCount];

                appBuffers[ch] = new short[sampleCount];
                appBuffers[ch + 1] = new short[sampleCount];

                appBuffersPinned[ch] = new PinnedArray<short>(appBuffers[ch]);
                appBuffersPinned[ch + 1] = new PinnedArray<short>(appBuffers[ch + 1]);

                status = Imports.SetDataBuffers(_handle, (Imports.Channel)(ch / 2), buffers[ch], buffers[ch + 1], sampleCount, 0, Imports.RatioMode.None);
            }

            Console.WriteLine("Waiting for trigger...Press a key to abort");
            _autoStop = false;
            status = Imports.RunStreaming(_handle, ref sampleInterval, Imports.ReportedTimeUnits.MicroSeconds, preTrigger, postTriggerSamples, 1, 1, Imports.RatioMode.None, (uint)sampleCount);
            Console.WriteLine("Run Streaming : {0} ", status);

            Console.WriteLine("Streaming data...Press a key to abort");

            // Build File Header
            var sb = new StringBuilder();
            sb.AppendFormat("For each of the active channels, results shown are....");
            sb.AppendLine();
            sb.AppendLine("Maximum Aggregated value ADC Count & mV, Minimum Aggregated value ADC Count & mV");
            sb.AppendLine();

            string[] heading = { "Channel", "Max ADC", "Max mV", "Min ADC", "Min mV" };

            for (int ch = 0; ch < _channelCount; ch++)
            {
                if (_channelSettings[ch].enabled)
                {
                    sb.AppendFormat("{0,10} {1,10} {2,10} {3,10} {4,10}", heading[0], heading[1], heading[2], heading[3], heading[4]);
                }
            }
            sb.AppendLine();

            while (!_autoStop && !Console.KeyAvailable)
            {
                /* Poll until data is received. Until then, GetStreamingLatestValues wont call the callback */
                Thread.Sleep(0);
                _ready = false;
                status = Imports.GetStreamingLatestValues(_handle, StreamingCallback, IntPtr.Zero);

                Console.Write((status > StatusCodes.PICO_OK && status != StatusCodes.PICO_BUSY /*PICO_BUSY*/) ? "Status =  {0}\n" : "", status);

                if (_ready && _sampleCount > 0) /* can be ready and have no data, if autoStop has fired */
                {
                    if (_trig > 0)
                    {
                        triggeredAt = (uint)totalSamples + _trigAt;
                    }

                    totalSamples += _sampleCount;
                    Console.Write("\nCollected {0,4} samples, index = {1,5}, Total = {2,5}", _sampleCount, _startIndex, totalSamples);

                    if (_trig > 0)
                    {
                        Console.Write("\tTrig at Index {0}", triggeredAt);
                    }

                    // Build File Body
                    for (uint i = _startIndex; i < (_startIndex + _sampleCount); i++)
                    {
                        for (int ch = 0; ch < _channelCount * 2; ch +=2)
                        {
                            if (_channelSettings[ch / 2].enabled)
                            {
                                sb.AppendFormat("{0,10} {1,10} {2,10} {3,10} {4,10}",
                                                (char)('A' + (ch / 2)),
                                                appBuffersPinned[ch].Target[i],
                                                adc_to_mv(appBuffersPinned[ch].Target[i], (int)_channelSettings[(int)(Imports.Channel.ChannelA + (ch / 2))].range),
                                                appBuffersPinned[ch + 1].Target[i],
                                                adc_to_mv(appBuffersPinned[ch + 1].Target[i], (int)_channelSettings[(int)(Imports.Channel.ChannelA + (ch / 2))].range));
                            }
                        }
                        sb.AppendLine();
                    }
                }
            }

            if (Console.KeyAvailable)
            {
                Console.ReadKey(true); // clear the key
            }

            Imports.Stop(_handle);

            // Print contents to file
            using (TextWriter writer = new StreamWriter(StreamFile, false))
            {
                writer.Write(sb.ToString());
                writer.Close();
            }

            if (!_autoStop)
            {
                Console.WriteLine("\nData collection aborted - press any key to continue.");
                WaitForKey();
            }

            Console.WriteLine();
        }

        /****************************************************************************
        * Select input voltage ranges for each channel
        ****************************************************************************/
        void SetVoltages()
        {
            bool valid = false;
            bool allChannelsOff = true;
            uint status;
            int noAllowedEnabledChannels;

            switch (_resolution)
            {
                case Imports.DeviceResolution.PS5000A_DR_15BIT:

                    noAllowedEnabledChannels = 2;
                    break;

                case Imports.DeviceResolution.PS5000A_DR_16BIT:

                    noAllowedEnabledChannels = 1;
                    break;

                default:

                    noAllowedEnabledChannels = _channelCount;
                    break;
            }

            _noEnabledChannels = 0;

            Console.WriteLine("Available voltage ranges are....\n");
            /* See what ranges are available... */

            for (int i = (int)_firstRange; i <= (int)_lastRange; i++)
            {
                Console.WriteLine("{0} . {1} mV", i, inputRanges[i]);
            }

            /* Ask the user to select a range */
            Console.WriteLine("Specify voltage range ({0}..{1})", (int)_firstRange, (int)_lastRange);
            Console.WriteLine("99 - switches channel off");

            do
            {
                for (int ch = 0; ch < _channelCount; ch++)
                {
                    Console.WriteLine("");
                    uint range = 8;

                    do
                    {
                        try
                        {
                            Console.WriteLine("Channel: {0}", (char)('A' + ch));
                            range = uint.Parse(Console.ReadLine());
                            valid = true;
                        }
                        catch (FormatException e)
                        {
                            valid = false;
                            Console.WriteLine("Error: " + e.Message);
                        }

                    } while ((range != 99 && (range < (uint)_firstRange || range > (uint)_lastRange) || !valid));


                    if (range != 99)
                    {
                        status = Imports.SetChannel(_handle, Imports.Channel.ChannelA + ch, 1, Imports.Coupling.PS5000A_DC, (Imports.Range)range, 0);
                        _channelSettings[ch].enabled = true;
                        _channelSettings[ch].range = (Imports.Range)range;
                        Console.WriteLine(" = {0} mV", inputRanges[range]);

                        _noEnabledChannels++;
                        allChannelsOff = false;
                    }
                    else
                    {
                        status = Imports.SetChannel(_handle, Imports.Channel.ChannelA + ch, 0, Imports.Coupling.PS5000A_DC, Imports.Range.Range_1V, 0);
                        _channelSettings[ch].enabled = false;
                        Console.WriteLine("Channel Switched off");

                    }

                    if (status != 0)
                    {
                        Console.WriteLine("Error setting channels\n Error code : {0}", status);
                    }
                }

                Console.Write(allChannelsOff ? "At least one channels must be enabled\n" : "");
                Console.Write(noAllowedEnabledChannels < _noEnabledChannels ? "Not allowed that many channels with resolution selected\n" : "");

            } while (allChannelsOff || noAllowedEnabledChannels < _noEnabledChannels);
        }

        /****************************************************************************
        * Initialise unit' structure with Variant specific defaults
        ****************************************************************************/
        void GetDeviceInfo()
        {
            string[] description = {
                                       "Driver Version",
                                       "USB Version",
                                       "Hardware Version",
                                       "Variant Info",
                                       "Serial",
                                       "Calibration Date",
                                       "Kernel Version",
                                       "Digital Hardware",
                                       "Analogue Hardware",
                                       "Firmware 1",
                                       "Firmware 2"
                                    };

            System.Text.StringBuilder line = new System.Text.StringBuilder(80);

            if (_handle >= 0)
            {
                for (int i = 0; i < description.Length; i++)
                {
                    short requiredSize;
                    Imports.GetUnitInfo(_handle, line, 80, out requiredSize, (uint) i);
                    Console.WriteLine("{0}: {1}", description[i], line);

                    if (_powerSupplyConnected)
                    {
                        if (i == 3)
                        {
                            _channelCount = int.Parse(line[1].ToString());

                        }
                    }
                    else
                    {
                        _channelCount = 2;
                    }
                    _channelSettings = new ChannelSettings[_channelCount];
                    _noEnabledChannels = _channelCount;

                }

                Console.WriteLine();

                Imports.Range voltageRange = Imports.Range.Range_5V;

                for (int ch = 0; ch < _channelCount; ch++)
                {
                    Imports.SetChannel(_handle, Imports.Channel.ChannelA + ch, 1, Imports.Coupling.PS5000A_DC, voltageRange, 0);
                    _channelSettings[ch].enabled = true;
                    _channelSettings[ch].range = voltageRange;
                }
            }
        }

        /****************************************************************************
        * Display Device Settings
        ****************************************************************************/
        void DisplaySettings()
        {
            int ch;
            int voltage;
            uint status = StatusCodes.PICO_OK;

            for (ch = 0; ch < _channelCount; ch++)
            {
                if (!(_channelSettings[ch].enabled))
                {
                    Console.WriteLine("Channel {0} Voltage Range = Off\n", (char) ('A' + ch));
                }
                else
                {
                    voltage = inputRanges[(int) _channelSettings[ch].range];
                    Console.WriteLine("Channel {0} Voltage Range = ", (char) ('A' + ch));

                    if (voltage < 1000)
                    {
                        Console.WriteLine("{0}mV\n", voltage);
                    }
                    else
                    {
                        Console.WriteLine("{0}V\n", voltage / 1000);
                    }
                }
            }

            status = Imports.GetDeviceResolution(_handle, out _resolution);

            PrintResolution(_resolution);

            if (_powerSupplyConnected)
            {
                Console.WriteLine("DC Power Supply connected.");
            }
            else
            {
                Console.WriteLine("USB Powered.");

            }
        }

        /****************************************************************************
        * Outputs the resolution in text format to the console window
        ****************************************************************************/
        void PrintResolution(Imports.DeviceResolution resolution)
        {
            int res = 8; // 8-bits
 
            switch (resolution)
            {
                case Imports.DeviceResolution.PS5000A_DR_8BIT:

                    res = 8;
                    break;

                case Imports.DeviceResolution.PS5000A_DR_12BIT:

                    res = 12;
                    break;

                case Imports.DeviceResolution.PS5000A_DR_14BIT:

                    res = 14;
                    break;

                case Imports.DeviceResolution.PS5000A_DR_15BIT:

                    res = 15;
                    break;

                case Imports.DeviceResolution.PS5000A_DR_16BIT:

                    res = 16;
                    break;

                default:

                    break;
            }

            Console.WriteLine("Device Resolution: {0} bits", res);
        }

        /****************************************************************************
        * CollectStreamingImmediate
        *  this function demonstrates how to collect a stream of data
        *  from the unit (start collecting immediately)
        ***************************************************************************/
        void CollectStreamingImmediate()
        {
            Console.WriteLine("Collect streaming...");
            Console.WriteLine("Data is written to disk file (stream.txt)");
            Console.WriteLine("Press a key to start");
            WaitForKey();

            /* Trigger disabled	*/
            Imports.SetSimpleTrigger(_handle, 0, Imports.Channel.ChannelA, 0, Imports.ThresholdDirection.None, 0, 0);

            StreamDataHandler(0);
        }

        /****************************************************************************
        * Select resolution of device
        ****************************************************************************/
        void SetResolution()
        {
            bool valid;
            int maxSelection = 2;
            uint status;

            Console.WriteLine("0 : 8 bits");
            Console.WriteLine("1 : 12 bits");
            Console.WriteLine("2 : 14 bits");

            if (_noEnabledChannels <= 2)
            {
                Console.WriteLine("3 : 15 bits"); //can only use up to 2 channels with 15 bit mode     
                if (_noEnabledChannels < 2)
                {
                    Console.WriteLine("4 : 16 bits"); //can only use 1 channel with 16 bit mode
                    maxSelection = 4;
                }
                else
                {
                    maxSelection = 3;
                }
            }

            Console.WriteLine();

            do
            {
                try
                {
                    Console.WriteLine("Resolution: ");
                    _resolution = (Imports.DeviceResolution)(uint.Parse(Console.ReadLine()));
                    valid = true;
                }
                catch (FormatException e)
                {
                    valid = false;
                    Console.WriteLine("Error: " + e.Message);
                }

                if (_resolution > (Imports.DeviceResolution)maxSelection)
                {
                    Console.WriteLine("Please select a number stated above");
                    valid = false;
                }

            } while (!valid);

            if ((status = Imports.SetDeviceResolution(_handle, _resolution)) != 0)
            {
                Console.WriteLine("Resolution not set Error code: {0)", status);
            }

        }

        /*************************************************************************************
        * Run
        *  main menu
        *  
        **************************************************************************************/
        public void Run()
        {
            GetDeviceInfo();

            // main loop - read key and call routine
            char ch = ' ';

            while (ch != 'X')
            {
                Console.WriteLine();

                DisplaySettings();

                Imports.MaximumValue(_handle, out _maxValue); // Set max. ADC Counts

                Console.WriteLine("\n");
                Console.WriteLine("S - Run Streaming         V - Set voltages");
                Console.WriteLine("R - Change Resolution");
                Console.WriteLine("X - Exit");
                Console.WriteLine();
                Console.WriteLine("Operation:");

                ch = char.ToUpper(Console.ReadKey(true).KeyChar);

                Console.WriteLine("\n");

                switch (ch)
                {
                    case 'S':
                        CollectStreamingImmediate();
                        break;

                    case 'V':
                        SetVoltages();
                        break;

                    case 'R':
                        SetResolution();
                        break;

                    case 'X':
                        /* Handled by outer loop */
                        break;

                    default:
                        Console.WriteLine("Invalid operation.");
                        break;
                }
            }

        }

        private StreamingCon(short handle, bool powerSupplyConnected)
        {
            _handle = handle;
            _powerSupplyConnected = powerSupplyConnected;
        }

        static void Main()
        {
            Console.WriteLine("PicoScope 5000 Series (ps5000a) Driver Streaming Data Collection Example Program.");
            Console.WriteLine("Version 1.4\n");

            Console.WriteLine("Enumerating devices...\n");

            short count = 0;
            short serialsLength = 40;
            StringBuilder serials = new StringBuilder(serialsLength);

            uint status = Imports.EnumerateUnits(out count, serials, ref serialsLength);

            if (status != StatusCodes.PICO_OK)
            {
                Console.WriteLine("No devices found.\n");
                Console.WriteLine("Error code : {0}", status);
                Console.WriteLine("Press any key to exit.\n");
                WaitForKey();
                Environment.Exit(0);
            }
            else
            {
                if(count == 1)
                {
                    Console.WriteLine("Found {0} device:", count);
                }
                else
                {
                    Console.WriteLine("Found {0} devices", count);
                }

                Console.WriteLine("Serial(s) {0}", serials);

            }

            // Open unit and show splash screen
            Console.WriteLine("\nOpening device...");

            short handle;

            status = Imports.OpenUnit(out handle, null, Imports.DeviceResolution.PS5000A_DR_8BIT);

            bool powerSupplyConnected = true;

            Console.WriteLine("Handle: {0}", handle);

            if (status == StatusCodes.PICO_POWER_SUPPLY_NOT_CONNECTED || status == StatusCodes.PICO_USB3_0_DEVICE_NON_USB3_0_PORT)
            {
                status = Imports.ChangePowerSource(handle, status);
                powerSupplyConnected = false;
            }
            else if (status != StatusCodes.PICO_OK)
            {
                Console.WriteLine("Cannot open device error code: " + status.ToString());
                System.Environment.Exit(-1);
            }
            else
            {
                // Do nothing - power supply connected
            }

            if (status != StatusCodes.PICO_OK)
            {
                Console.WriteLine("Unable to open device.");
                Console.WriteLine("Error code : {0}", status);
                WaitForKey();
            }
            else
            {
                Console.WriteLine("Device opened successfully.\n");

                StreamingCon consoleExample = new StreamingCon(handle, powerSupplyConnected);
                consoleExample.Run();

                Imports.CloseUnit(handle);
            }
        }
    }
}
