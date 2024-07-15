/******************************************************************************
 *
 *  Filename: PS4000AStreamingCon.cs
 *  
 *  Description:
 *    This is a console-mode program that demonstrates how to use the
 *    ps4000a driver API functions using .NET
 *    
 *  Supported PicoScope models:
 *PS4000AImports
 *		PicoScope 4444
 *		PicoScope 4824
 *		PicoScope 4X24A (2,4, and 8 channels)
 *		
 *  Examples:
 *     Collect a stream of data immediately
 *     Collect a stream of data when a trigger event occurs
 *    
 *  Copyright © 2015-2024 Pico Technology Ltd. See LICENSE file for terms.
 *
 ******************************************************************************/

using System;
using System.IO;
using System.Text;
using System.Threading;

using PS4000AImports;
using PicoStatus;
using PicoPinnedArray;
using System.Runtime.InteropServices;
using ProbeScaling;
//using static PS4000AImports.Imports;

namespace PS4000AStreamingConsole
{
    class StreamingConSole
    {
        private readonly short _handle;
        int _channelCount;
        private ChannelSettings[] _channelSettings;

        short[][] appBuffers;
        short[][] buffers;

        bool _ready = false;
        short _trig = 0;
        uint _trigAt = 0;
        int _sampleCount;
        uint _startIndex;
        bool _autoStop;
        short _maxValue;

        //array for PicoConnect probe updates - currently used for PicoScope 4444 only!
        Imports.tPS4000AUserProbeInteractions[] ChannelProbes = new Imports.tPS4000AUserProbeInteractions[8];
        bool ProbeInteractionUpdate = false;
        bool Set_SetProbeInteraction = false;
        uint _Number_of_Probes = 0;

        public PicoConnectProbes.PicoConnectProbes.PicoConnectProbeRange _firstRange;
        public PicoConnectProbes.PicoConnectProbes.PicoConnectProbeRange _lastRange;

        private string StreamFile = "stream.txt";

        /****************************************************************************
        * Callback
        * Used by PS4000a data streaming collection calls, on receipt of data.
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
                    if (_channelSettings[(int)(Imports.Channel.CHANNEL_A + (ch / 2))].enabled)
                    {

                        Array.Copy(buffers[ch], _startIndex, appBuffers[ch], _startIndex, _sampleCount); //max
                        Array.Copy(buffers[ch + 1], _startIndex, appBuffers[ch + 1], _startIndex, _sampleCount); //min

                    }
                }
            }
        }

        /****************************************************************************
        * Callback
        * Used by PS4000a ProbeInteractions probe(s) changes only reported
        ****************************************************************************/
        //private void ProbeInteractionCallback(short handle, uint status, IntPtr tPS4000AUserProbeInteractions, uint nProbes)
        private void ProbeInteractionCallback(short handle,
                                        UInt32 status,
                                        IntPtr probes,
                                    uint nProbes)
        {
            _Number_of_Probes = nProbes;
            IntPtr ptr = probes;
            Imports.tPS4000AUserProbeInteractions[] probeUpdates = new Imports.tPS4000AUserProbeInteractions[nProbes];

            for (int i = 0; i < nProbes; i++)
            {
                //marshal the pointer to an array of UserProbeInteractions
                Imports.tPS4000AUserProbeInteractions probeInteraction =
                    (Imports.tPS4000AUserProbeInteractions)Marshal.PtrToStructure(ptr, typeof(Imports.tPS4000AUserProbeInteractions));
                probeUpdates[i] = probeInteraction;
                ptr += Marshal.SizeOf(typeof(Imports.tPS4000AUserProbeInteractions));
            }

            uint j = 0;
            for (j = 0; j < nProbes; j++)
            {
                ChannelProbes[(int)probeUpdates[j].Channel] = probeUpdates[j];//copy probe update(s) to correct array element
            }
            ProbeInteractionUpdate = true;
            //////////////////////////////////////////////
            //Could be moved outside of the Callback
            if (ProbeInteractionUpdate == true )
            {
                if(status != 0)
                    Console.Write("\n\nProbeInteractionCallback Status is " + status);
                for (j = 0; j < 4; j++)
                {
                    Console.Write("\n\n{0}  Enabled = {1}", ChannelProbes[j].Channel, ChannelProbes[j].Enabled);
                    Console.Write("\nProbe is " + ChannelProbes[j].ProbeName);
                    Console.Write(" and is connected = {0} \n", ChannelProbes[j].Connected);
                    Console.Write("Channel range is {0}\n", ChannelProbes[j].RangeCurrent);
                }              
                Console.Write("\nNumber of probe changes is {0}\n", _Number_of_Probes);
                Console.ReadKey(true); // clear the key
                ProbeInteractionUpdate = false;
            }
            ///////////////////////////////////////
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
       * Stream Data Handler
       * - Used by the two stream data examples - untriggered and triggered
       * Inputs:
       * - unit - the unit to sample on
       * - preTrigger - the number of samples in the pre-trigger phase 
       *					(0 if no trigger has been set)
       ***************************************************************************/
        void StreamDataHandler(uint preTrigger)
        {
            int tempBufferSize = 1024 * 100; /*  *100 is to make sure buffer large enough */

            appBuffers = new short[_channelCount * 2][];
            buffers = new short[_channelCount * 2][];

            short setAutoStop = 1;
            
            uint totalSamples = 0;
            uint triggeredAt = 0;
            uint sampleInterval = 1;
            uint downSampleRatio = 1;
            uint status = StatusCodes.PICO_OK;
            uint maxPostTriggerSamples = 100000 - preTrigger;

            // Use Pinned Arrays for the application buffers
            PinnedArray<short>[] appBuffersPinned = new PinnedArray<short>[_channelCount * 2];

            for (int ch = 0; ch < _channelCount * 2; ch += 2) // create data buffers
            {
                if (_channelSettings[ch / 2].enabled == true)
                {
                    buffers[ch] = new short[tempBufferSize];
                    buffers[ch + 1] = new short[tempBufferSize];

                    appBuffers[ch] = new short[tempBufferSize];
                    appBuffers[ch + 1] = new short[tempBufferSize];

                    appBuffersPinned[ch] = new PinnedArray<short>(appBuffers[ch]);
                    appBuffersPinned[ch + 1] = new PinnedArray<short>(appBuffers[ch + 1]);

                    status = Imports.SetDataBuffers(_handle, (Imports.Channel)(ch / 2), buffers[ch], buffers[ch + 1], tempBufferSize, 0, Imports.DownSamplingMode.None);
                }
            }
            Console.WriteLine("Waiting for trigger...Press a key to abort");
            _autoStop = false;

            status = Imports.RunStreaming(_handle, ref sampleInterval, Imports.ReportedTimeUnits.MicroSeconds, preTrigger, maxPostTriggerSamples, setAutoStop, downSampleRatio, 
                                                Imports.DownSamplingMode.None, (uint)tempBufferSize);         
            Console.WriteLine("Run Streaming : {0} ", status);
            Console.WriteLine("Streaming data...Press a key to abort");

            // Build File Header            
            var sb = new StringBuilder();
            //Get the scaling Info for each channel
            PicoProbeScaling[] ChannelRangeInfo = new PicoProbeScaling[_channelCount];
            PicoProbeScaling ChannelRangeInfoTemp = new PicoProbeScaling();
            for (int chtemp = 0; chtemp < _channelCount; chtemp++)
            {
                if (_channelSettings[chtemp].enabled)
                { 
                    if (Scaling.getRangeScaling(ChannelProbes[chtemp].RangeCurrent, out ChannelRangeInfoTemp))
                        ChannelRangeInfo[chtemp] = ChannelRangeInfoTemp;
                    else
                    {
                        ChannelRangeInfo[chtemp] = ChannelRangeInfoTemp;
                        Console.Write("\nWarning: Channel {0} Range cannot be found, using default range of: {1}",
                            (char)('A' + _channelCount), ChannelRangeInfoTemp.Probe_Range_text);
                    }
                    sb.AppendLine("Channel" + (char)('A' + chtemp));//(string)("A" + chtemp));
                    sb.AppendLine("Probe is " + ChannelProbes[chtemp].ProbeName);
                    sb.AppendLine("Channel range is " + ChannelProbes[chtemp].RangeCurrent);
                    sb.AppendLine();

                }
            }
            sb.AppendLine("For each of the enabled Channels, results shown are....");
            sb.AppendLine("Maximum Aggregated value ADC Count & units, Minimum Aggregated value ADC Count & units");
            sb.AppendLine();
            string[] heading = { "Channel", "MaxBuf ADC", "MaxBuf Unit", "MinBuf ADC", "MinBuf Unit" };
            for (int i = 0; i < _channelCount; i++)
            {
                if (_channelSettings[i].enabled)
                {
                    sb.AppendFormat("CHANNEL-{0}_MaxBuf-ADC {1} MinBuf-ADC Scaled ",
                        (char)('A' + i),
                        ChannelRangeInfo[i].Probe_Range_text);
                }
            }
            sb.AppendLine();

            while (!_autoStop && !Console.KeyAvailable)
            {
                /* Poll until data is received. Until then, GetStreamingLatestValues wont call the callback */
                Thread.Sleep(1);
                _ready = false;
                status = Imports.GetStreamingLatestValues(_handle, StreamingCallback, IntPtr.Zero);

                Console.Write((status > StatusCodes.PICO_OK && status != StatusCodes.PICO_BUSY /*PICO_BUSY*/) ? "Status =  {0}\n" : "", status);

                if (_ready && _sampleCount > 0) /* can be ready and have no data, if autoStop has fired */
                {
                    if (_trig > 0)
                    {
                        triggeredAt = (uint)totalSamples + _trigAt;
                    }
                    totalSamples += (uint) _sampleCount;

                    Console.Write("\nCollected {0,4} samples, index = {1,5}, Total = {2,5}", _sampleCount, _startIndex, totalSamples);

                    if (_trig > 0)
                    {
                        Console.Write("\tTrig at Index {0}", triggeredAt);
                    }

                    // Build File Body
                    for (uint i = _startIndex; i < (_startIndex + _sampleCount); i++)
                    {
                        for (int ch = 0; ch < _channelCount * 2; ch += 2)
                        {                      
                            if (_channelSettings[ch / 2].enabled)
                            {
                                sb.AppendFormat("{0} {1:###.0##e+00} {2} {3:###.0##e+00} ",
                                                appBuffersPinned[ch].Target[i],//Write Max buffer samples both ADC and scaled

                                                Scaling.adc_to_scaled_value(appBuffersPinned[ch].Target[i],
                                                ChannelRangeInfo[(int)(Imports.Channel.CHANNEL_A + (ch / 2))], _maxValue),
                                                
                                                appBuffersPinned[ch + 1].Target[i],//Write Min buffer samples both ADC and scaled

                                                Scaling.adc_to_scaled_value(appBuffersPinned[ch + 1].Target[i],
                                                ChannelRangeInfo[(int)(Imports.Channel.CHANNEL_A + (ch / 2))], _maxValue)
                                                );
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
                Console.WriteLine();
                Console.WriteLine("Data collection aborted - press any key to continue.");
                WaitForKey();
            }
        }

        /****************************************************************************
        * Select input voltage ranges for each channel
        ****************************************************************************/
        void SetVoltages()
        {
            bool valid = false;
            bool allChannelsOff = true;
            uint status;

            /* Ask the user to select a range */
            //Console.WriteLine("Specify voltage range ({0}..{1})", (int)_firstRange, (int)_lastRange);
            Console.WriteLine("99 - switches channel off");           
            do
            {                
                for (int ch = 0; ch < _channelCount; ch++)
                {
                    Console.WriteLine("");
                    uint rangeinput = 99;
                    PicoConnectProbes.PicoConnectProbes.PicoConnectProbeRange range =
                        PicoConnectProbes.PicoConnectProbes.PicoConnectProbeRange.PICO_X1_PROBE_5V;
                    do
                    {
                        try
                        {
                            Console.WriteLine("Channel: {0}", (char)('A' + ch));
                            if (Set_SetProbeInteraction == true)
                            {   //Print Probe name and its current range
                                Console.WriteLine("Probe connected : {0}", ChannelProbes[ch].ProbeName);
                                Console.WriteLine("Probe Range : {0}\n", ChannelProbes[ch].RangeCurrent);
                            }
                            else //Or standard voltage range
                                Console.WriteLine("Voltage Range : {0}\n", (Scaling.Std_Voltage_Range)ChannelProbes[ch].RangeCurrent);

                            _firstRange = PicoConnectProbes.PicoConnectProbes.PicoConnectProbeRange.PICO_X1_PROBE_10MV;
                            _lastRange = PicoConnectProbes.PicoConnectProbes.PicoConnectProbeRange.PICO_X1_PROBE_20V;

                            //Print standard voltage ranges OR PicoConnectProbes ranges
                            if (ChannelProbes[ch].ProbeName == PicoConnectProbes.PicoConnectProbes.PicoConnectProbe.PICO_CONNECT_PROBE_NONE)
                            {
                                //muiltple enums for the same value, just cast to standard voltage ranges-
                                Console.WriteLine("Specify voltage range ({0}..{1})", (Scaling.Std_Voltage_Range)_firstRange, (Scaling.Std_Voltage_Range)_lastRange);    
                                Console.WriteLine("                Enter ({0}..{1})", (int)_firstRange, (int)_lastRange);
                            }
                            else
                            {
                                _firstRange = ChannelProbes[ch].RangeFirst; //copy probe settings to use
                                _lastRange = ChannelProbes[ch].RangeLast;
                                //muiltple enums for the same value, just cast to standard voltage ranges-
                                if (ChannelProbes[ch].RangeLast <= PicoConnectProbes.PicoConnectProbes.PicoConnectProbeRange.PICO_X1_PROBE_50V)
                                    Console.WriteLine("Specify voltage range ({0}..{1})", (Scaling.Std_Voltage_Range)_firstRange, (Scaling.Std_Voltage_Range)_lastRange);
                                else
                                    Console.WriteLine("Specify range ({0}..{1})", ChannelProbes[ch].RangeFirst, ChannelProbes[ch].RangeLast);                              
                                Console.WriteLine("        Enter ({0}..{1})", (int)ChannelProbes[ch].RangeFirst, (int)ChannelProbes[ch].RangeLast);
                            }
                            //Keyboard input to range value
                            rangeinput = uint.Parse(Console.ReadLine());
                            if (rangeinput == 99)//value to turn channel off
                                range = PicoConnectProbes.PicoConnectProbes.PicoConnectProbeRange.PICO_CONNECT_PROBE_OFF;
                            else
                                range = (PicoConnectProbes.PicoConnectProbes.PicoConnectProbeRange)rangeinput;
                            valid = true;
                        }
                        catch (FormatException e)
                        {
                            valid = false;
                            Console.WriteLine("Error: " + e.Message);
                        }
                    } while( (rangeinput != 99) && //While - channel not off & out of range & valid input 
                                (rangeinput < (uint)_firstRange || rangeinput > (uint)_lastRange) && valid );

                // Apply User settings-
                if (range != PicoConnectProbes.PicoConnectProbes.PicoConnectProbeRange.PICO_CONNECT_PROBE_OFF)
                    {
                        status = Imports.SetChannel(_handle, Imports.Channel.CHANNEL_A + ch, 1, Imports.Coupling.DC,
                            range, 0);
                        if (status != StatusCodes.PICO_OK)
                        {
                            Console.WriteLine("Error calling SetChannel Error code : {0}\n", status);
                        }
                        else
                        {
                            _channelSettings[ch].enabled = true;
                            ChannelProbes[ch].Enabled = 1;
                            _channelSettings[ch].range = range;                          
                            Console.WriteLine(" = {0}", range);                         
                            allChannelsOff = false;
                            ChannelProbes[ch].RangeCurrent = range;
                        }
                    }
                    else
                    {
                        status = Imports.SetChannel(_handle, Imports.Channel.CHANNEL_A + ch, 0, Imports.Coupling.DC,
                            PicoConnectProbes.PicoConnectProbes.PicoConnectProbeRange.PICO_CONNECT_PROBE_OFF,
                            0);
                        if (status != StatusCodes.PICO_OK)
                        {
                            Console.WriteLine("Error calling SetChannel Error code : {0}\n", status);
                        }
                        else
                        {
                            _channelSettings[ch].enabled = false;
                            ChannelProbes[ch].Enabled = 0;
                            Console.WriteLine("Channel Switched off");
                            ChannelProbes[ch].RangeCurrent = range;//PICO_CONNECT_PROBE_OFF
                        }
                    }

                    if (status != StatusCodes.PICO_OK)
                    {
                        Console.WriteLine("Error setting channels\n Error code : {0}", status);
                    }
                }
                Console.Write(allChannelsOff ? "At least one channels must be enabled\n" : "");
            } while (allChannelsOff);
        }

        /****************************************************************************
        * Initialise unit' structure with Variant specific defaults
        ****************************************************************************/
        void GetDeviceInfo()
        {
            Imports.MaximumValue(_handle, out _maxValue);
            Console.WriteLine("{0}\n", _maxValue);

            string[] description = {
                                       "Driver Version",
                                       "USB Version",
                                       "Hardware Version",
                                       "Variant Info",
                                       "Serial",
                                       "Cal Date",
                                       "Kernel",
                                       "Digital H/W",
                                       "Analogue H/W",
                                       "Firmware 1",
                                       "Firmware 2"
                                    };

            System.Text.StringBuilder line = new System.Text.StringBuilder(80);
            Set_SetProbeInteraction = false;

            if (_handle >= 0)
            {
                for (int i = 0; i < 11; i++)
                {
                    short requiredSize;
                    Imports.GetUnitInfo(_handle, line, 80, out requiredSize, (DriverImports.InfoType)i);
                    Console.WriteLine("{0}: {1}", description[i], line);

                    if (i == 3) // Variant information
                    {
                        _channelCount = int.Parse(line[1].ToString());
                        _channelSettings = new ChannelSettings[_channelCount];

                         _firstRange = PicoConnectProbes.PicoConnectProbes.PicoConnectProbeRange.PICO_X1_PROBE_10MV;
                         _lastRange = PicoConnectProbes.PicoConnectProbes.PicoConnectProbeRange.PICO_X1_PROBE_20V;

                        if(string.Join(",", line) =="4444")// PicoScope 4444 for PicoConnect Probes          
                            Set_SetProbeInteraction = true;
                    }
                }
                if (Set_SetProbeInteraction == true)
                    SetProbeInteraction();

                Console.WriteLine("\nChannel Voltage Ranges:\n");

                for (int ch = 0; ch < _channelCount; ch++)
                {
                    Imports.SetChannel(_handle, Imports.Channel.CHANNEL_A + ch, 1, Imports.Coupling.DC,
                        PicoConnectProbes.PicoConnectProbes.PicoConnectProbeRange.PICO_X1_PROBE_5V,
                        0);
                    _channelSettings[ch].enabled = true;
                    ChannelProbes[ch].Enabled = 1;
                    _channelSettings[ch].range = PicoConnectProbes.PicoConnectProbes.PicoConnectProbeRange.PICO_X1_PROBE_5V;
                    ChannelProbes[ch].RangeCurrent = PicoConnectProbes.PicoConnectProbes.PicoConnectProbeRange.PICO_X1_PROBE_5V;
                    Console.WriteLine("Channel {0}: 5V", (char)('A' + ch));
                }
            }
        }

        /****************************************************************************
        * Register PicoConnect Probe Callback for probe updates
        ****************************************************************************/
        void SetProbeInteraction()
        {
            uint status = 0;
            status = Imports.SetProbeInteractionCallback(_handle, ProbeInteractionCallback); //Register Probe Callback
            if (status != StatusCodes.PICO_OK)
            {
                Console.WriteLine("Error calling SetProbeInteractionCallback\n Error code : {0}", status);
            }
            else
                Console.WriteLine("Setting Probe Interaction Callback for Pico Connect probes");
            Thread.Sleep(3000);
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
            Imports.SetSimpleTrigger(_handle, 0, Imports.Channel.CHANNEL_A, 0, Imports.ThresholdDirection.None, 0, 0);

            StreamDataHandler(0);
        }

        /****************************************************************************
         * CollectStreamingTriggered
         *  this function demonstrates how to collect a stream of data
         *  from the unit (start collecting on trigger)
         ***************************************************************************/
        void CollectStreamingTriggered()
        {
            //Get channel A range scaling info
            PicoProbeScaling ChannelRangeInfoTrigger = new PicoProbeScaling();
            Scaling.getRangeScaling(ChannelProbes[(int)Imports.Channel.CHANNEL_A].RangeCurrent, out ChannelRangeInfoTrigger);
            //And pass to scaled_value_to_adc, and set trigger voltage value
            short triggerVoltage = (short)Scaling.scaled_value_to_adc(1, ChannelRangeInfoTrigger, _maxValue);

            Console.WriteLine("Collect streaming triggered...");
            Console.WriteLine("Data is written to disk file (stream.txt)");
            Console.WriteLine("Press a key to start");
            WaitForKey();

            /* Trigger enabled
             * Rising edge
             * Threshold = 1000mV */
            Imports.SetSimpleTrigger(_handle, triggerVoltage, Imports.Channel.CHANNEL_A, 0, Imports.ThresholdDirection.Rising, 0, 0);
            
            StreamDataHandler(100000);
        }

        /*************************************************************************************
        * Run
        *  Main menu
        *  
        **************************************************************************************/
        public void Run()
        {
            GetDeviceInfo();
            ChannelProbes[0].Channel = Imports.Channel.CHANNEL_A;
            ChannelProbes[1].Channel = Imports.Channel.CHANNEL_B;
            ChannelProbes[2].Channel = Imports.Channel.CHANNEL_C;
            ChannelProbes[3].Channel = Imports.Channel.CHANNEL_D;

            // main loop - read key and call routine
            char ch = ' ';

            while (ch != 'X')
            {
                Console.WriteLine("\n");
                Console.WriteLine("S - Immediate streaming         V - Set Channel Ranges");
                Console.WriteLine("W - Triggered streaming         ");
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

                    case 'W':
                        CollectStreamingTriggered();
                        break;

                    case 'V':
                        SetVoltages();
                        break;

                    case 'X':
                        /* Handled by outer loop */
                        break;

                    default:
                        Console.WriteLine("Invalid operation");
                        break;
                }
            }
        }

        private StreamingConSole(short handle)
        {
            _handle = handle;
        }

        static void Main()
        {
            Console.WriteLine("PicoScope 4000 Series (ps4000a) Driver Streaming Data Collection Example Program.");
            Console.WriteLine("Version 1.3\n");
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
            Console.WriteLine("\n\nOpening the device...");
            short handle;
            status = Imports.OpenUnit(out handle, null);
            Console.WriteLine("Handle: {0}", handle);

            if (status != StatusCodes.PICO_OK && handle != 0)
            {
                status = Imports.ps4000aChangePowerSource(handle, status);
            }

            if (status != StatusCodes.PICO_OK)
            {
                Console.WriteLine("Unable to open device");
                Console.WriteLine("Error code : {0}", status);
                WaitForKey();
            }
            else
            {
                Console.WriteLine("Device opened successfully\n");

                StreamingConSole consoleExample = new StreamingConSole(handle);
                consoleExample.Run();

                Imports.CloseUnit(handle);
            }
        }
    }
}
