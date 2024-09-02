// This is an example of how to setup and capture in block mode for PicoScope 6000 Series PC Oscilloscope consuming the ps6000a driver

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DriverImports;
using PicoPinnedArray;

namespace RapidBlockModeExample
{

    class RapidBlockModeExampleCallback : IDisposable
    {

        private DefinitionBlockReady _callback; //Strong reference so the callback isn't garbage collected.
        private readonly ManualResetEvent _callbackEvent = new ManualResetEvent(false);
        private StandardDriverStatusCode _callbackStatus = StandardDriverStatusCode.Ok;

        /// <summary>
        /// Callback event for when data has been fully captured.
        /// </summary>
        private void BlockReadyCallback(short handle, StandardDriverStatusCode status, IntPtr pVoid)
        {
            _callbackStatus = status;
            _callbackEvent.Set();
        }
        /// <summary>
        /// Calls Runblock and exits when data has been fully captured.
        /// </summary>
        public StandardDriverStatusCode WaitForDataToBeCaptured(short handle)
        {
            //Wait for the data to be fully captured and ready to read.
            _callbackEvent.WaitOne();
            var status = _callbackStatus;

            if (status == StandardDriverStatusCode.Ok)
                Console.WriteLine("Data captured...");

            return status;
        }

        public static int numChannels;
        private static ps6000aDevice.ChannelSettings[] _channelSettings = new ps6000aDevice.ChannelSettings[10];
        /// <summary>
        /// Captures 1000 samples on Channel A and the data is written to Output.csv
        /// </summary>
        /// 
        public StandardDriverStatusCode RunRapidBlockModeExample(short handle, DeviceResolution resolution)
        {
            ulong numSamples = 1000;
            double idealTimeInterval = 0.000001; //1us (therefore 1us x 1000 samples = 1ms per Segment)
            Console.WriteLine("\nCHANGE ENABLED CHANNELS IN Main() as required!");
            var status = ps6000aDevice.InitializeChannelsAndRanges(handle, in _channelSettings, numChannels);
            if (status != StandardDriverStatusCode.Ok) return status;
            //ps6000aMemorySegments
            int memorysegments = 3;
            ulong nMaxSamples = 0;
            status = DriverImports.PS6000a.MemorySegments(handle, (ulong)memorysegments, out nMaxSamples);
            if (status != StandardDriverStatusCode.Ok)
                return status;
            //ps6000aSetNoOfCaptures
            status = DriverImports.PS6000a.SetNoOfCaptures(handle, (ulong)memorysegments);
            if (status != StandardDriverStatusCode.Ok)
                return status;
            else
                Console.WriteLine("\nCreated and Set " + memorysegments + " Memory Segments on the device...");

            _callback = BlockReadyCallback;
            double actualTimeInterval;
            double timeIndisposedMS;
            uint timebase;
            //Check Sample rate time base
            //Add function that creates "enabledChannelFlags" from enabled channels array!???????????????????
            EnabledChannelsAndPorts EnabledChannelsAndPorts_Flags = EnabledChannelsAndPorts.ChannelA;
            //(EnabledChannelsAndPorts)0x0000f0f0;//All channels on
            status = DriverImports.PS6000a.NearestSampleIntervalStateless(handle, EnabledChannelsAndPorts_Flags,
                                                                          idealTimeInterval, resolution,
                                                                          out timebase, out actualTimeInterval);
            
            if (status != StandardDriverStatusCode.Ok)
            {
                Console.WriteLine("NearestSampleIntervalStateless status: " + status);
                return status;
            }
            Console.WriteLine("\nSampling TimeInterval: " + actualTimeInterval);
            Console.WriteLine("Number of samples per channel: " + numSamples);
            //Set a trigger
            Console.WriteLine("\nSetting Trigger...");
            Console.WriteLine("ChA, Rising edge, threshold @1000 ADC count, AutoTrigger 5 seconds");
            status = DriverImports.PS6000a.SetSimpleTrigger(handle, 1, Channel.ChannelA, 1000, TriggerDirection.Rising, 0, 5000000);
            if (status != StandardDriverStatusCode.Ok)
            {
                Console.WriteLine("SetSimpleTrigger() status: " + status);
                return status;
            }
            //Set the number of pre and post trigger samples to half the number of samples.
            //Thus the middle sample should be the trigger point.
            Console.WriteLine("\nStarting Data Capture...");
            ulong noOfPreTriggerSamples = numSamples /2;
            Console.WriteLine("\nNumber of PreTriggerSamples " + noOfPreTriggerSamples);
            status = DriverImports.PS6000a.RunBlock(handle, noOfPreTriggerSamples, numSamples - noOfPreTriggerSamples,
                                                    timebase, out timeIndisposedMS, 0, _callback, IntPtr.Zero);
            if (status != StandardDriverStatusCode.Ok)
            {
                Console.WriteLine("RunBlock status: " + status);
                return status;
            }
            Console.WriteLine("\nWaiting for Data...");
            status = WaitForDataToBeCaptured(handle);
            if (status != StandardDriverStatusCode.Ok)
            {
                Console.WriteLine("Callback status: " + status);
                return status;
            }
            ///Create all buffers for each memory segment and channel
            DriverImports.Action Action_temp = DriverImports.Action.PICO_CLEAR_ALL | DriverImports.Action.PICO_ADD;
            
            // Set up the data arrays and pin them (3D Array - [Buffer set],[Channel],[Sample index] )
            short[][][] values = new short[memorysegments][][];
            PinnedArray<short>[,] pinned = new PinnedArray<short>[memorysegments, numChannels];

            for (ushort segment = 0; segment < memorysegments; segment++)
            {
                values[segment] = new short[numChannels][];
                Console.WriteLine("\nCreating pinned Arrays for Set " + segment + " of Data Buffers...");

                for (short channel = 0; channel < numChannels; channel++)
                {
                    if (_channelSettings[channel].enabled)
                    {
                        values[segment][channel] = new short[numSamples];
                        pinned[segment, channel] = new PinnedArray<short>(values[segment][channel]);

                        status = PS6000a.SetDataBuffers(handle,
                            (Channel)channel,
                            values[segment][channel],
                            null,
                            (int)numSamples,
                            DataType.PICO_INT16_T,
                            (ulong)segment,
                            RatioMode.PICO_RATIO_MODE_RAW,
                            Action_temp);
                        Action_temp = DriverImports.Action.PICO_ADD;//set to "ADD" for all other calls

                        if (status != StandardDriverStatusCode.Ok)
                        {
                            Console.WriteLine("\nError from function SetDataBuffers with status: " + status);
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Calling SetDataBuffer() for " + (Channel)channel);
                        }
                    }
                }
            }
            var downSampleRatioMode = RatioMode.PICO_RATIO_MODE_RAW;
            short [] overrangeflags = new short[memorysegments];//Channel Voltage overrange flags!
            status = DriverImports.PS6000a.GetValuesBulk(handle, 0, ref numSamples,
                                                        0, (ulong)(memorysegments-1),
                                                        1, downSampleRatioMode,
                                                        out overrangeflags[0]); 
            if (status == StandardDriverStatusCode.Ok)
            {
                Console.WriteLine("\nData has been copied safely to all memory buffers.");
                Console.WriteLine("Channel Overrange flags: (LSB is ChA) - " + overrangeflags[0]);
            }
            else
            {
                Console.WriteLine("\nError from function SetDataBuffers with status: " + status);
                return status;
            }
            
            /////////////////////////////////////////////////////////////////////////////////////////////////
            
            /* WriteDataToTextFile
            string filename = "Output.txt";
            for (short segments = 0; segments < memorysegments; segments++)
            {
                filename = "BlockSegment " + segments + ".txt";

                try
                {
                    //Record data in a .txt document.
                    using (var writer = new StreamWriter(filename))
                    {
                        writer.WriteLine("Segment " + segments + " SampleRate " + actualTimeInterval + " SamplesPerBlock "+ numSamples + " Trigger@Sample " + numSamples / 2);
                        for (short channel = 0; channel < numChannels; channel++)
                        {
                            if (_channelSettings[channel].enabled)
                                writer.Write( (Channel)channel + " ");
                        }
                        writer.Write("\n");
                        for (short sample = 0; sample < (short)numSamples; sample++)
                        {
                            for (short channel = 0; channel < numChannels; channel++)
                            {
                                if (_channelSettings[channel].enabled)
                                    writer.Write(values[segments][channel][sample] + " ");
                            }
                        writer.Write("\n");
                        }  
                        writer.Close();
                    }

                    Console.WriteLine("The captured data has been written to " + filename + ".");
                }
                catch (Exception e)
                {
                    Console.WriteLine("Failed to write data to " + filename + ". Please check that there are no other instances of " + filename + " being used.");
                    Console.WriteLine(e);
                }
            }
            */
            /////////////////////////////////////////////////////////////////////////////////////////////
            ps6000aDevice.WriteArrayToFiles(values, _channelSettings, actualTimeInterval, "RapidBlock Segment", (short)noOfPreTriggerSamples);
            return status;
        }
        static void Main(string[] args)
        {
            short handle = 0;
            var resolution = DeviceResolution.PICO_DR_8BIT;
            short MinValues, MaxValues = 0;
            DriverImports.StandardDriverStatusCode status = 0;
            ////////////////////////////////////////////////  Enabled/Disable channels as required!
            _channelSettings[0].enabled = true;//ChA
            _channelSettings[1].enabled = true;//ChB
            _channelSettings[2].enabled = true;//ChC
            _channelSettings[3].enabled = true;//ChD
            _channelSettings[4].enabled = false;//ChE
            _channelSettings[5].enabled = false;//ChF
            _channelSettings[6].enabled = false;//ChG
            _channelSettings[7].enabled = false;//ChH
            ///////////////////////////////////////////
            status = ps6000aDevice.OpenUnit(out handle, resolution, out numChannels);

            status = PS6000a.GetAdcLimits(handle, resolution, out MinValues, out MaxValues);
            if (status != StandardDriverStatusCode.Ok)
            {
                Console.WriteLine("GetAdcLimits returned status: " + status);
            }
            Console.WriteLine("GetAdcLimits() returned MaxValues: " + MaxValues);

            if (status == StandardDriverStatusCode.Ok)
            {
                using (RapidBlockModeExampleCallback bmCallback = new RapidBlockModeExampleCallback())
                {
                    status = bmCallback.RunRapidBlockModeExample(handle, resolution);
                }

                Console.WriteLine("RunRapidBlockModeExample exited with status: " + status);
            }
            status = PS6000a.Stop(handle);
            Console.WriteLine("Stopping unit with status: " + status);
            status = PS6000a.CloseUnit(handle);
            Console.WriteLine("Closed unit with status: " + status);
            Console.ReadLine();
        }
        public void Dispose()
        {
            _callbackEvent.Dispose();
        }
    }
  
}
