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
using Microsoft.Win32;
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
        private static short MinValues, MaxValues = 0;

        /// <summary>
        /// Captures 1000 samples on Channel A and the data is written to Output.csv
        /// </summary>
        /// 
        public StandardDriverStatusCode RunRapidBlockModeExample(short handle, DeviceResolution resolution)
        {
            ulong numSamples = 1000;
            double idealTimeInterval = 0.000001; //1us (therefore 1us x 1000 samples = 1ms per Segment)
            Console.WriteLine("\nCHANGE ENABLED CHANNELS IN Main() as required!");
            int NoEnablechannels;
            var status = ps6000aDevice.InitializeChannelsAndRanges(handle, in _channelSettings, numChannels, out NoEnablechannels);
            if (status != StandardDriverStatusCode.Ok) return status;

            //ps6000aMemorySegments
            ulong memorysegments = 3;
            ulong nMaxSamples = 0;
            status = DriverImports.PS6000a.MemorySegments(handle, memorysegments, out nMaxSamples);
            if (status != StandardDriverStatusCode.Ok)
                return status;

            //ps6000aSetNoOfCaptures
            status = DriverImports.PS6000a.SetNoOfCaptures(handle, memorysegments);
            if (status != StandardDriverStatusCode.Ok)
                return status;
            else
                Console.WriteLine("\nCreated and Set " + memorysegments + " Memory Segments on the device...");

            _callback = BlockReadyCallback;
            double actualTimeInterval;
            double timeIndisposedMS;
            uint timebase;

            //Generate EnabledChannelsAndPorts mask for enabled channels
            EnabledChannelsAndPorts EnabledChannelsAndPorts_Flags = 0;
            for (short channel = 0; channel < numChannels; channel++)
            {
                if (_channelSettings[channel].enabled)
                {
                    EnabledChannelsAndPorts_Flags = EnabledChannelsAndPorts_Flags | (EnabledChannelsAndPorts)Math.Pow(2,channel);
                }
            }

            //Check Sample rate time base
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

            //Print reported capture time
            Console.WriteLine("\nReported scope capture time is " + timeIndisposedMS + "ms");

            //Waiting for Callback
            Console.WriteLine("\nWaiting for Data...");
            status = WaitForDataToBeCaptured(handle);
            if (status != StandardDriverStatusCode.Ok)
            {
                Console.WriteLine("Callback status: " + status);
                return status;
            }

            //Get and print the number of segments captures
            status = DriverImports.PS6000a.GetNoOfCaptures(handle, out ulong NumofCaptures);
            if (status != StandardDriverStatusCode.Ok)
            {
                Console.WriteLine("GetNoOfCaptures status: " + status);
                return status;
            }
            Console.WriteLine("Number of segments captured: " + NumofCaptures);

            //Create all buffers for each memory segment and channel
            DriverImports.Action Action_temp = DriverImports.Action.PICO_CLEAR_ALL | DriverImports.Action.PICO_ADD;//SetBuffer() action mask
            
            // Set up the data arrays and pin them (3D Array - [Buffer set],[Channel],[Sample index] )
            short[][][] values = new short[memorysegments][][];
            PinnedArray<short>[,] pinned = new PinnedArray<short>[memorysegments, numChannels];

            for (ulong segment = 0; segment < memorysegments; segment++)
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
                            segment,
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

            //GetValues from scope
            Console.WriteLine("\nDownloading Data to the PC...");
            var downSampleRatioMode = RatioMode.PICO_RATIO_MODE_RAW;
            short [] overrangeflags = new short[memorysegments];//Channel Voltage overrange flags
            status = DriverImports.PS6000a.GetValuesBulk(handle, 0, ref numSamples,//segment sample indexs - All (from -> to)
                                                        0, (memorysegments-1),//Set to request All segments (from -> to)
                                                        1, downSampleRatioMode,//Downsample ratio and mode
                                                        out overrangeflags[0]); 
            if (status == StandardDriverStatusCode.Ok)
            {
                Console.WriteLine("\nData has been copied safely to all memory buffers.");
                Console.WriteLine("Channel Overrange flag[0]: (LSB is ChA) - " + overrangeflags[0]);
            }
            else
            {
                Console.WriteLine("\nError from function SetDataBuffers with status: " + status);
                return status;
            }

            //Write each segment to a file
            Console.WriteLine("\nWriting each segment of Channel buffers to file...");
            PicoFileFunctions.WriteArrayToFiles(values, _channelSettings, actualTimeInterval, "RapidBlock_Segment", (short)noOfPreTriggerSamples, MaxValues);

            return status;
        }
        static void Main(string[] args)
        {
            short handle = 0;
            var resolution = DeviceResolution.PICO_DR_8BIT;
            //short MinValues, MaxValues = 0;
            DriverImports.StandardDriverStatusCode status = 0;
            //Turn all channels off
            for (int i = 0; i < _channelSettings.Length; i++)
            {
                _channelSettings[i].enabled = false;
            }
            //////////////  Enabled/Disable channels as required!  ///////////////////
            
            for (int i = 0; i < 4; i++)//Setup first 4 channels the same
            {
                _channelSettings[i].enabled = true;
                _channelSettings[i].coupling = Coupling.DC50Ohm;
                _channelSettings[i].range = ChannelRange.Range_500MV;
                _channelSettings[i].AnalogueOffset = 0;
                _channelSettings[i].bandwidthLimiter = BandwidthLimiter.BW_FULL;
            }
            //////////////////////////////////////////////////////////////////////////
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
