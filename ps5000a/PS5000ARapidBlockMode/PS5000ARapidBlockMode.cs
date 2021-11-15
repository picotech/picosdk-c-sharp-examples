using System;
using System.IO;
using System.Text;
using System.Threading;

using PS5000AImports;
using PicoPinnedArray;
using PicoStatus;

namespace PS5000ARapidBlockMode
{
    struct ChannelSettings
    {
        public Imports.Range range;
        public bool enabled;
    }

    class PS5000ARapidBlockMode
    {

        private Imports.ps5000aBlockReady _callbackDelegate;

        private readonly short _handle;
        public const int BUFFER_SIZE = 1024;
        public const int MAX_CHANNELS = 4;
        public const int QUAD_SCOPE = 4;
        public const int DUAL_SCOPE = 2;

        uint _timebase = 8;
        short _oversample = 1;
        bool _scaleVoltages = true;

        ushort[] inputRanges = { 10, 20, 50, 100, 200, 500, 1000, 2000, 5000, 10000, 20000 };
        bool _ready = false;
        short _trig = 0;
        uint _trigAt = 0;
        int _sampleCount = 0;
        uint _startIndex = 0;
        bool _autoStop;

        /// <summary>
        /// ////////////////
        /// </summary>
        /// 
        int _channelCount;
        private ChannelSettings[] _channelSettings;


        short[][] appBuffers;
        short[][] buffers;

        bool _powerSupplyConnected;

        short _maxValue;

        int _noEnabledChannels;

        Imports.DeviceResolution _resolution = Imports.DeviceResolution.PS5000A_DR_8BIT;

        public Imports.Range _firstRange = Imports.Range.Range_10mV;
        public Imports.Range _lastRange = Imports.Range.Range_20V;

        private string RapidBlockFile = "RapidBlock.txt";

        /****************************************************************************
         * BlockCallback
         * used by data block collection calls, on receipt of data.
         * used to set global flags etc checked by user routines
         ****************************************************************************/
        void RapidBlockCallback(short handle, short status, IntPtr pVoid)
        {
            // flag to say done reading data
            _ready = true;
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

            Console.WriteLine("Max Value : {0}\n" , _maxValue);
            return (short)((mv * _maxValue) / inputRanges[ch]);
        }

        /****************************************************************************
          * RapidBlockDataHandler
          * - Used by all the CollectBlockRapid routine
          * - acquires data (user sets trigger mode before calling), displays 10 items
          *   and saves all to data.txt
          * Input :
          * - nRapidCaptures : the user specified number of blocks to capture
          ****************************************************************************/
        private void RapidBlockDataHandler(ushort nRapidCaptures , int preTrigger)
        {
            uint status;
            int numChannels = _channelCount;
            uint numSamples = BUFFER_SIZE;

            // Run the rapid block capture
            int timeIndisposed;
            _ready = false;

            _timebase = 160;
            _oversample = 1;

            _callbackDelegate = RapidBlockCallback;
            status = Imports.RunBlock(_handle,
                        preTrigger,
                        (int)numSamples - preTrigger - 1,
                        _timebase,
                        out timeIndisposed,
                        0,
                        _callbackDelegate,
                        IntPtr.Zero);

            if (status != StatusCodes.PICO_OK)
            {
                Console.WriteLine("RunBlock status error 0x{0:X}", status);
                return;
            }


            Console.WriteLine("Waiting for data...Press a key to abort\n");

            while (!_ready && !Console.KeyAvailable)
            {
                Thread.Sleep(100);
            }

            if (Console.KeyAvailable)
            {
                Console.ReadKey(true); // clear the key
            }

            Imports.Stop(_handle);


            // Set up the data arrays and pin them
            short[][][] values = new short[nRapidCaptures][][];
            PinnedArray<short>[,] pinned = new PinnedArray<short>[nRapidCaptures, numChannels];

            for (ushort segment = 0; segment < nRapidCaptures; segment++)
            {
                values[segment] = new short[numChannels][];

                for (short channel = 0; channel < numChannels; channel++)
                {
                    if (_channelSettings[channel].enabled)
                    {
                        values[segment][channel] = new short[numSamples];
                        pinned[segment, channel] = new PinnedArray<short>(values[segment][channel]);

                        status = Imports.SetDataBuffer(_handle,
                                               (Imports.Channel)channel,
                                               values[segment][channel],
                                               (int)numSamples,
                                               segment,
                                               0);
                    }
                    else
                    {
                        status = Imports.SetDataBuffer(_handle,
                                   (Imports.Channel)channel,
                                    null,
                                    0,
                                    segment,
                                    0);

                    }
                }
            }

            // Read the data
            short[] overflows = new short[nRapidCaptures];
            
            status = Imports.GetValuesRapid(_handle, ref numSamples, 0, (ushort)(nRapidCaptures - 1), 1, Imports.RatioMode.None, overflows);

            if (status != StatusCodes.PICO_OK)
            {
                Console.WriteLine("GetValuesRapid status error 0x{0:X}", status);
                return;
            }


            /* Print out the first 10 readings, converting the readings to mV if required */
            Console.WriteLine("\nValues in {0}\n", (_scaleVoltages) ? ("mV") : ("ADC Counts"));

            for (int seg = 0; seg < nRapidCaptures; seg++)
            {
                Console.WriteLine("Capture {0}\n", seg);

                for (int i = 0; i < 10; i++)
                {
                    for (int chan = 0; chan < _channelCount; chan++)
                    {
                        Console.Write("{0}\t", _scaleVoltages ?
                                                adc_to_mv(pinned[seg, chan].Target[i], (int)_channelSettings[(int)(Imports.Channel.ChannelA + chan)].range) // If _scaleVoltages, show mV values
                                                : pinned[seg, chan].Target[i]);                                                                             // else show ADC counts
                    }
                    Console.WriteLine();
                }
                Console.WriteLine();
            }

            // Un-pin the arrays
            foreach (PinnedArray<short> p in pinned)
            {
                if (p != null)
                {
                    p.Dispose();
                }
            }

            //TODO: Do what ever is required with the data here.
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
                    Imports.GetUnitInfo(_handle, line, 80, out requiredSize, (uint)i);
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
                    Console.WriteLine("Channel {0} Voltage Range = Off\n", (char)('A' + ch));
                }
                else
                {
                    voltage = inputRanges[(int)_channelSettings[ch].range];
                    Console.WriteLine("Channel {0} Voltage Range = ", (char)('A' + ch));

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
         * CollectRapidBlockTriggered
         *  this function demonstrates how to collect a rapid block of data
         *  from the unit (start collecting on trigger)
         ***************************************************************************/
        void CollectRapidBlockTriggered()
        {
            ushort numRapidCaptures = 20;
            uint status;

            Console.WriteLine("Collect Rapid Block");

            status = Imports.SetNoOfRapidCaptures(_handle, numRapidCaptures);

            if (status != StatusCodes.PICO_OK)
            {
                Console.WriteLine("SetNoOfRapidCaptures status error 0x{0:X}", status);
            }

            int maxSamples;
            status = Imports.MemorySegments(_handle, numRapidCaptures, out maxSamples);

            if (status != StatusCodes.PICO_OK)
            {
                Console.WriteLine("MemorySegments status error 0x{0:X}", status);
            }

            Console.WriteLine("Collecting {0} rapid blocks. Press a key to start...", numRapidCaptures);
            WaitForKey();

            Imports.SetSigGenBuiltInV2(_handle,
                                        0,
                                        4000000,
                                        Imports.WaveType.PS5000A_SINE,
                                        100,
                                        100,
                                        0,
                                        0,
                                        Imports.SweepType.PS5000A_DOWN,
                                        Imports.ExtraOperations.PS5000A_ES_OFF,
                                        0,
                                        0,
                                        Imports.SigGenTrigType.PS5000A_SIGGEN_RISING,
                                        Imports.SigGenTrigSource.PS5000A_SIGGEN_NONE,
                                        0);

            /* Trigger enabled
             * Rising edge
             * Threshold = 1000mV */
            short triggerVoltage = mv_to_adc(1000, (short)Imports.Range.Range_1V);
            Imports.SetSimpleTrigger(_handle, 1, Imports.Channel.ChannelA, triggerVoltage, Imports.ThresholdDirection.Rising, 0, 0);

            RapidBlockDataHandler(numRapidCaptures, 1); // Collect 100000 pre-trigger samples

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
                Console.WriteLine("W - RapidBlock         V - Set voltages");
                Console.WriteLine("R - Change Resolution");
                Console.WriteLine("X - Exit");
                Console.WriteLine();
                Console.WriteLine("Operation:");

                ch = char.ToUpper(Console.ReadKey(true).KeyChar);

                Console.WriteLine("\n");

                switch (ch)
                {
                    case 'W':
                        CollectRapidBlockTriggered();
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

        private PS5000ARapidBlockMode(short handle, bool powerSupplyConnected)
        {
            _handle = handle;
            _powerSupplyConnected = powerSupplyConnected;
        }

        static void Main()
        {
            Console.WriteLine("PicoScope 5000 Series (ps5000a) Driver Rapid Block Data Collection Example Program.");
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
                if (count == 1)
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

            status = Imports.OpenUnit(out handle, null, Imports.DeviceResolution.PS5000A_DR_15BIT);

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

                PS5000ARapidBlockMode consoleExample = new PS5000ARapidBlockMode(handle, powerSupplyConnected);
                consoleExample.Run();

                Imports.CloseUnit(handle);
            }
        }
    }
}
