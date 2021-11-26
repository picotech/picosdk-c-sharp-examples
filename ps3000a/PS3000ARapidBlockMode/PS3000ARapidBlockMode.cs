/**************************************************************************
 *
 * Filename: PS3000ARapidBlockMode.cs
 *
 * Description:
 *   This is a console-mode program that demonstrates how to use the
 *   PicoScope 3000 Series (ps3000a) driver functions. using .NET
 *
 * Supported PicoScope models:
 *
 *		PicoScope 3204A/B/D
 *		PicoScope 3205A/B/D
 *		PicoScope 3206A/B/D
 *		PicoScope 3207A/B
 *		PicoScope 3204 MSO & D MSO
 *		PicoScope 3205 MSO & D MSO
 *		PicoScope 3206 MSO & D MSO
 *		PicoScope 3404A/B/D/D MSO
 *		PicoScope 3405A/B/D/D MSO
 *		PicoScope 3406A/B/D/D MSO
 *		
 * Example:
 *   
 *    Collect Analogue samples in rapid block mode when a trigger event occurs
 *
 * Copyright (C) 2021 Pico Technology Ltd. See LICENSE file for terms.
 *  
 **************************************************************************/


using System;
using System.IO;
using System.Threading;
using System.Text;

using PS3000AImports;
using PicoPinnedArray;
using PicoStatus;

namespace PS3000AExample
{
    struct ChannelSettings
    {
        public Imports.Coupling DCcoupled;
        public Imports.Range range;
        public bool enabled;
    }

    class PS3000ARapidBlockMode
    {
        bool _ready = false;
        short _handle;
        private ChannelSettings[] _channelSettings;
        public const int MAX_CHANNELS = 4;
        private int channelCount = 4;
        bool _scaleVoltages = true;
        ushort[] inputRanges = { 10, 20, 50, 100, 200, 500, 1000, 2000, 5000, 10000, 20000, 50000 };
        uint _timebase = 8;
        short _oversample = 1;
        private int _channelCount = 2;
        public const int RAPID_BLOCK_BUFFER_SIZE = 100; // Number of samples per waveform capture for rapid block captures
        private Imports.ps3000aBlockReady _callbackDelegate;

        /****************************************************************************
		 * adc_to_mv
		 *
		 * Convert an 16-bit ADC count into millivolts
		 ****************************************************************************/
        int adc_to_mv(int raw, int ch)
        {
            return (raw * inputRanges[ch]) / Imports.MaxValue;
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
            return (short)((mv * Imports.MaxValue) / inputRanges[ch]);
        }

        /****************************************************************************
		 * BlockCallback
		 * used by ps3000a data block collection calls, on receipt of data.
		 * used to set global flags etc checked by user routines
		 ****************************************************************************/
        void BlockCallback(short handle, short status, IntPtr pVoid)
        {
            // flag to say done reading data
            if (status != (short)StatusCodes.PICO_CANCELLED)
            {
                _ready = true;
            }
        }

        public PS3000ARapidBlockMode(short handle)
        {
            _handle = handle;
        }

        static void Main(string[] args)
        {
            Console.WriteLine("PicoScope 3000 series (ps3000a) Driver C# Rapid block mode example");
            Console.WriteLine("\n Opening the device...");

            // open unit
            short handle;
            uint status = Imports.OpenUnit(out handle, null);
            if (status == StatusCodes.PICO_POWER_SUPPLY_NOT_CONNECTED || status == StatusCodes.PICO_USB3_0_DEVICE_NON_USB3_0_PORT)
            {
                status = Imports.ChangePowerSource(handle, status);
            }
            else if (status != StatusCodes.PICO_OK)
            {
                Console.WriteLine("An error has been encountered :{0}", status);
            }
            else
            {
                // Do nothing 
            }
            Console.WriteLine("\n Device Successfully opened !...");
            PS3000ARapidBlockMode RapidCapture = new PS3000ARapidBlockMode(handle);
            RapidCapture.ChannelSetup();


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
                    _channelSettings[i].range = Imports.Range.Range_5V;
                }

                for (int i = 0; i < channelCount; i++) // reset channels to most recent settings
                {
                    Imports.SetChannel(_handle, Imports.Channel.ChannelA + i,
                                       (short)(_channelSettings[(int)(Imports.Channel.ChannelA + i)].enabled ? 1 : 0),
                                       _channelSettings[(int)(Imports.Channel.ChannelA + i)].DCcoupled,
                                       _channelSettings[(int)(Imports.Channel.ChannelA + i)].range,
                                       (float)0.0);
                }
            }
            CollectRapidBlock();

        }

        /****************************************************************************
		*  CollectRapidBlock
		*  this function demonstrates how to collect blocks of data
		* using the RapidCapture function
		****************************************************************************/
        private void CollectRapidBlock()
        {

            uint numRapidCaptures = 1;
            uint status = StatusCodes.PICO_OK;
            uint maxSegments = 0;
            bool valid = false;

            Console.WriteLine("Collect data in rapid block mode...");

            //This function returns the maximum number of segments allowed for the opened device.
            status = Imports.GetMaxSegments(_handle, out maxSegments);

            // The number of memory segments has to be greater than or equal to the number of captures required
            Console.WriteLine("Specify number of captures (max. {0}):", maxSegments);

            do
            {
                try
                {
                    numRapidCaptures = uint.Parse(Console.ReadLine());
                    valid = true;
                }
                catch
                {
                    valid = false;
                    Console.WriteLine("\nEnter numeric values only:");
                }

            } while (Imports.SetNoOfRapidCaptures(_handle, numRapidCaptures) > 0 || !valid);



            int maxSamples;
            //This function sets the number of memory segments that the scope will use
            Imports.MemorySegments(_handle, numRapidCaptures, out maxSamples);

            Console.WriteLine("Collecting {0} waveforms. Press a key to start.", numRapidCaptures);

            //wait until a key is pressed
            while (!Console.KeyAvailable)
            {
                Thread.Sleep(100);
            }

            if (Console.KeyAvailable)
            {
                Console.ReadKey(true); // clear the key
            }

            // Set simple trigger on Channel A

            short triggerVoltage = mv_to_adc(50, (short)_channelSettings[(int)Imports.Channel.ChannelA].range); // Threshold value to be used for "SetSimpleTrigger"

            status = Imports.SetSimpleTrigger(_handle, 1, Imports.Channel.ChannelA, triggerVoltage, Imports.ThresholdDirection.Rising, 0, 0);

            RapidBlockDataHandler(numRapidCaptures);
        }


        /****************************************************************************
         * RapidBlockDataHandler
         * - Used by the CollectRapidBlock routine
         * - acquires data
         * Input :
         * - nRapidCaptures : the user specified number of blocks to capture
         ****************************************************************************/
        private void RapidBlockDataHandler(uint nRapidCaptures)
        {
            int numChannels = _channelCount;
            int chan = 0;
            uint status;
            uint numSamples = RAPID_BLOCK_BUFFER_SIZE;
            uint numActualCaptures = 0;
            int timeInterval = 0;
            int maxSamples = 0;

            status = StatusCodes.PICO_INVALID_TIMEBASE;

            do
            {
                status = Imports.GetTimebase(_handle, _timebase, (int)numSamples, out timeInterval, _oversample, out maxSamples, 0);

                if (status != StatusCodes.PICO_OK)
                {
                    Console.WriteLine("Selected timebase {0} could not be used.\n", _timebase);
                    _timebase++;
                }

            }
            while (status != StatusCodes.PICO_OK);

            Console.WriteLine("Timebase: {0}\t Sampling Interval (ns): {1}\nMax Samples per Channel per Segment: {2}\n", _timebase, timeInterval, maxSamples);

            // Set up the data arrays and pin them
            short[][][] values = new short[nRapidCaptures][][];
            PinnedArray<short>[,] pinned = new PinnedArray<short>[nRapidCaptures, numChannels];

            for (uint segment = 0; segment < nRapidCaptures; segment++)
            {
                values[segment] = new short[numChannels][];

                for (short channel = 0; channel < numChannels; channel++)
                {
                    if (_channelSettings[channel].enabled)
                    {
                        values[segment][channel] = new short[numSamples];
                        pinned[segment, channel] = new PinnedArray<short>(values[segment][channel]);

                        status = Imports.SetDataBuffer(_handle, (Imports.Channel)channel, values[segment][channel], (int)numSamples, segment, Imports.RatioMode.None);

                        if (status != StatusCodes.PICO_OK)
                        {
                            Console.WriteLine("RapidBlockDataHandler:Imports.SetDataBuffer Channel {0} Status = 0x{1:X6}", (char)('A' + channel), status);
                        }

                    }
                    else
                    {
                        status = Imports.SetDataBuffer(_handle, (Imports.Channel)channel, null, 0, segment, Imports.RatioMode.None);

                        if (status != StatusCodes.PICO_OK)
                        {
                            Console.WriteLine("RapidBlockDataHandler:Imports.SetDataBuffer Channel {0} Status = 0x{1:X6}", (char)('A' + channel), status);
                        }

                    }
                }
            }

            // Setup array for overflow
            short[] overflows = new short[nRapidCaptures];

            // Run the rapid block capture
            int timeIndisposed = 0;
            _ready = false;

            _callbackDelegate = BlockCallback;

            Imports.RunBlock(_handle,
                        0,
                        (int)numSamples,
                        _timebase,
                        _oversample,
                        out timeIndisposed,
                        0,
                        _callbackDelegate,
                        IntPtr.Zero);

            Console.WriteLine("Waiting for data... Press a key to abort.");

            while (!_ready && !Console.KeyAvailable)
            {
                Thread.Sleep(100);
            }

            if (Console.KeyAvailable)
            {
                Console.ReadKey(true);
            }

            Imports.Stop(_handle);

             if (_ready)
                {
                // Obtain number of captures
                status = Imports.GetNoOfRapidCaptures(_handle, out numActualCaptures);
                Console.WriteLine("Rapid capture aborted. {0} complete blocks were captured\n", numActualCaptures);

                if (numActualCaptures == 0)
                {
                    return;
                }

                // Only display the blocks that were captured
                nRapidCaptures = numActualCaptures;

                // Retrieve the data
                status = Imports.GetValuesRapid(_handle, ref numSamples, 0, (nRapidCaptures - 1), 1, Imports.RatioMode.None, overflows);

                /* Print out the first 10 readings, converting the readings to mV if required */
                Console.WriteLine("\nValues in {0}", (_scaleVoltages) ? ("mV") : ("ADC Counts"));
                Console.WriteLine();
                for (uint seg = 0; seg < nRapidCaptures; seg++)
                {
                    Console.WriteLine("Capture {0}:\n", seg + 1);



                    for (chan = 0; chan < _channelCount; chan++)
                    {
                        if (_channelSettings[chan].enabled)
                        {
                            Console.Write("Ch. {0}\t", (char)('A' + chan));
                        }
                    }

                    Console.WriteLine("\n");

                    for (int i = 0; i < 10; i++)
                    {
                        for (chan = 0; chan < _channelCount; chan++)
                        {
                            if (_channelSettings[chan].enabled)
                            {
                                Console.Write("{0}\t", _scaleVoltages ?
                                                        adc_to_mv(pinned[seg, chan].Target[i], (int)_channelSettings[(int)(Imports.Channel.ChannelA + chan)].range) // If _scaleVoltages, show mV values
                                                        : pinned[seg, chan].Target[i]);                                                                             // else show ADC counts
                            }
                        }

                        Console.WriteLine();
                    }

                  //  Console.WriteLine();
                }

                // Un-pin the arrays
                foreach (PinnedArray<short> p in pinned)
                {
                    if (p != null)
                    {
                        p.Dispose();
                    }
                }
            }
        }
    }
}
