/**************************************************************************
 *
 * Filename: PS4000ABlockMode.cs
 *
 * Description:
 *   This is a console-mode program that demonstrates how to use the
 *   PicoScope 4000 Series (ps4000a) driver functions. using .NET
 *
 *		
 * Examples:
 *    Collect a block of samples immediately

 * Copyright (C) 2024 Pico Technology Ltd. See LICENSE file for terms.
 *  
 **************************************************************************/


using System;
using System.IO;
using System.Threading;
using System.Text;

using PS4000AImports;
using PicoPinnedArray;
using PicoStatus;
using ProbeScaling;

namespace PS4000AExample

{

    class PS4000ABlockMode
    {
        public const int MAX_CHANNELS = 8;//4
        uint sampleCount = 10000;
        short _handle;
        bool _ready = false;
        private Imports.ps4000aBlockReady _callbackDelegate;
        private static uint _timebase = 2;
        bool _scaleVoltages = true;
        private ChannelSettings[] _channelSettings;
        private int channelCount = 2;//4
        static short maxADCValue = 0;

        /****************************************************************************
		 * Callback
		 * used by ps4000a data block collection calls, on receipt of data.
		 * used to set global flags etc checked by user routines
		 ****************************************************************************/
        private void BlockCallback(short handle, short status, IntPtr pVoid)
        {
            // flag to say done reading data
            if (status != (short)StatusCodes.PICO_CANCELLED)
                _ready = true;
        }
        public PS4000ABlockMode(short handle)
        {
            _handle = handle;
        }
        static void Main(string[] args)
        {
            Console.WriteLine("PicoScope 4000 Series (ps4000a) Driver C# Block mode example");
            Console.WriteLine("\nOpening the device...");

            short handle;

            //Open unit 
            uint status = Imports.OpenUnit(out handle, null);
            if (status == StatusCodes.PICO_POWER_SUPPLY_NOT_CONNECTED || status == StatusCodes.PICO_USB3_0_DEVICE_NON_USB3_0_PORT)
            {
                status = Imports.ps4000aChangePowerSource(handle, status);
            }
            else if (status != StatusCodes.PICO_OK)
            {
                Console.WriteLine("An error has been encountered : {0}", status);
            }
            else
            {
                // Do nothing
            }
            Console.WriteLine("\nDevice successfully opened!..");

            
            status = Imports.MaximumValue(handle, out maxADCValue);
            if (status != StatusCodes.PICO_OK)
            {
                Console.WriteLine("MaximumValue - error has been encountered : {0}", status);
            }

            PS4000ABlockMode blockCapture = new PS4000ABlockMode(handle);
            blockCapture.ChannelSetup();
            blockCapture.BlockDataHandler("First 10 readings", 0);
            Imports.CloseUnit(handle);
        }

        /****************************************************************************
		 * Set Channel/(s) 
		 ****************************************************************************/
        private void ChannelSetup()
        {
            {
                _channelSettings = new ChannelSettings[MAX_CHANNELS];

                for (int i = 0; i < MAX_CHANNELS; i++)
                {
                    _channelSettings[i].enabled = true;
                    _channelSettings[i].DCcoupled = Imports.Coupling.DC;
                    _channelSettings[i].range = PicoConnectProbes.PicoConnectProbes.PicoConnectProbeRange.PICO_X1_PROBE_5V;
                }

                for (int i = 0; i < channelCount; i++) // reset channels to most recent settings
                {
                    Imports.SetChannel(_handle, Imports.Channel.CHANNEL_A + i,
                                       (short)(_channelSettings[(int)(Imports.Channel.CHANNEL_A + i)].enabled ? 1 : 0),
                                       _channelSettings[(int)(Imports.Channel.CHANNEL_A + i)].DCcoupled,
                                       _channelSettings[(int)(Imports.Channel.CHANNEL_A + i)].range,
                                       (float)0.0);
                }
            }


        }

        /****************************************************************************
		 * BlockDataHandler
		 * - acquires data 
		 * * Input :
		 * - unit : the unit to use.
		 * - text : the text to display before the display of data slice
		 * - offset : the offset into the data buffer to start the display's slice.
		 ****************************************************************************/
        private void BlockDataHandler(string text, int offset)
        {
            //Setup databuffers
            uint status;
            PinnedArray<short>[] minPinned = new PinnedArray<short>[channelCount];
            PinnedArray<short>[] maxPinned = new PinnedArray<short>[channelCount];


            for (int i = 0; i < channelCount; i++)
            {
                short[] minBuffers = new short[sampleCount];
                short[] maxBuffers = new short[sampleCount];
                minPinned[i] = new PinnedArray<short>(minBuffers);
                maxPinned[i] = new PinnedArray<short>(maxBuffers);


                status = Imports.SetDataBuffers(_handle, (Imports.Channel)i, maxBuffers, minBuffers, (int)sampleCount, 0, Imports.DownSamplingMode.None);
                if (status != StatusCodes.PICO_OK)
                {
                    Console.WriteLine("BlockDataHandler:ps4000aSetDataBuffer Channel {0} Status = 0x{1:X6}", (char)('A' + i), status);
                }
            }

            /* Find the time interval (in nanoseconds) at the current _timebase. */          
            int timeInterval;
            int maxSamples;

            while (Imports.GetTimebase(_handle, _timebase, (int)sampleCount, out timeInterval, out maxSamples, 0) != 0)
            {
                Console.WriteLine("Selected timebase {0} could not be used.\n", _timebase);
                _timebase++;

            }
            Console.WriteLine("Timebase: {0}\t Sampling Interval (ns): {1}\n", _timebase, timeInterval);


            int timeIndisposed;
            Console.WriteLine("Collecting Device Data...\n");
            Console.WriteLine("Starting Block Capture");

            /* Start it collecting, then wait for completion*/
            _ready = false;
            _callbackDelegate = BlockCallback;
            status = Imports.RunBlock(_handle, 0, (int)sampleCount, _timebase, out timeIndisposed, 0, _callbackDelegate, IntPtr.Zero);
            while (!_ready)
            {
                Thread.Sleep(100);
            }
            Imports.Stop(_handle);
            if (_ready)
            {
                short overflow;
                Imports.GetValues(_handle, 0, ref sampleCount, 1, Imports.DownSamplingMode.None, 0, out overflow);

                /* Print out the first 10 readings, converting the readings to mV if required */
                Console.WriteLine(text);
                Console.WriteLine("readings will be in {0}", (_scaleVoltages) ? ("mV") : ("ADC Counts"));

                for (int ch = 0; ch < channelCount; ch++)
                {
                    if (_channelSettings[ch].enabled)
                    {
                        Console.Write("   Ch{0}    ", (char)('A' + ch));
                    }
                }
                Console.WriteLine();

                for (int i = offset; i < offset + 10; i++) //show data starting from  offset to 10, to print all values use samplecount
                {
                    for (int ch = 0; ch < channelCount; ch++)
                    {
                        if (_channelSettings[ch].enabled)
                        {
                            Console.Write("{0:N3}    ", _scaleVoltages ?
                                              Scaling.adc_to_mv(maxPinned[ch].Target[i], (uint)_channelSettings[(int)(Imports.Channel.CHANNEL_A + ch)].range, maxADCValue)  // If _scaleVoltages, show mV values
                                              : maxPinned[ch].Target[i]);                                                                           // else show ADC counts
                        }
                    }

                    Console.WriteLine();

                }

             }
            foreach (PinnedArray<short> p in minPinned)
            {
                if (p != null)
                {
                    p.Dispose();
                }
            }

            foreach (PinnedArray<short> p in maxPinned)
            {
                if (p != null)
                {
                    p.Dispose();
                }
            }
         }
    }
}
