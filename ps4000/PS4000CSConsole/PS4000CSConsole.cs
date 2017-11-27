/*******************************************************************************
 *
 *  Filename: PS4000CSConsole.cs
 * 
 *  Description:
 *    This is a console-mode program that demonstrates how to use the
 *    ps4000 driver API functions using .NET
 *   
 *  Supported PicoScope models:
 *
 *		PicoScope 4223, 4224 & 4224 IEPE
 *		PicoScope 4423 & 4424
 *		PicoScope 4226 & 4227
 *		PicoScope 4262
 *		
 *  Examples:
 *     Collect a block of samples immediately
 *     Collect a block of samples when a trigger event occurs
 *     Collect samples using a rapid block capture with trigger
 *     Collect a stream of data immediately
 *     Collect a stream of data when a trigger event occurs
 *
 *  Copyright (C) 2009 - 2017 Pico Technology Ltd. See LICENSE file for terms.
 *  
 ******************************************************************************/

using System;
using System.IO;
using System.Threading;

using PS4000Imports;
using PicoPinnedArray;
using PicoStatus;

namespace PS4000CSConsole
{
    struct ChannelSettings
    {
        public bool DCcoupled;
        public Imports.Range range;
        public bool enabled;
    }

    class Pwq
    {
        public Imports.PwqConditions[] conditions;
        public short nConditions;
        public Imports.ThresholdDirection direction;
        public uint lower;
        public uint upper;
        public Imports.PulseWidthType type;

        public Pwq(Imports.PwqConditions[] conditions,
            short nConditions,
            Imports.ThresholdDirection direction,
            uint lower, uint upper,
            Imports.PulseWidthType type)
        {
            this.conditions = conditions;
            this.nConditions = nConditions;
            this.direction = direction;
            this.lower = lower;
            this.upper = upper;
            this.type = type;
        }
    }

    class ConsoleExample
    {
        private readonly short _handle;
        public const int BUFFER_SIZE = 1024;
        public const int MAX_CHANNELS = 4;
        public const int QUAD_SCOPE = 4;
        public const int DUAL_SCOPE = 2;

        uint _timebase = 8;
        short _oversample = 1;
        bool _scaleVoltages = true;

        short[][] appBuffers;
        short[][] buffers;

        ushort[] inputRanges = { 10, 20, 50, 100, 200, 500, 1000, 2000, 5000, 10000, 20000, 50000 };
        bool _ready = false;
        short _trig = 0;
        uint _trigAt = 0;
        int _sampleCount = 0;
        uint _startIndex = 0;
        bool _autoStop;
        private ChannelSettings[] _channelSettings;
        private int _channelCount;
        private Imports.Range _firstRange;
        private Imports.Range _lastRange;
        private Imports.ps4000BlockReady _callbackDelegate;

        /****************************************************************************
         * Callback
         * used by PS4000 data streaming collection calls, on receipt of data.
         * used to set global flags etc checked by user routines
         ****************************************************************************/
        void StreamingCallback(short handle,
                                int noOfSamples,
                                uint startIndex,
                                short ov,
                                uint triggerAt,
                                short triggered,
                                short autoStop,
                                IntPtr pVoid)
        {
            // used for streaming
            _sampleCount = noOfSamples;
            _startIndex = startIndex;
            _autoStop = autoStop != 0;

            // flag to say done reading data
            _ready = true;

            // flags to show if & where a trigger has occurred
            _trig = triggered;
            _trigAt = triggerAt;

            if (_sampleCount != 0)
            {
                for (int ch = 0; ch < _channelCount*2; ch+=2)
                {
                    if (_channelSettings[(int)(Imports.Channel.ChannelA + (ch/2))].enabled)
                    {

                            Array.Copy(buffers[ch], _startIndex, appBuffers[ch], _startIndex, _sampleCount); //max
                            Array.Copy(buffers[ch + 1], _startIndex, appBuffers[ch + 1], _startIndex, _sampleCount);//min

                    }
                }
            }
        }

        /****************************************************************************
         * Callback
         * used by ps4000 data block collection calls, on receipt of data.
         * used to set global flags etc checked by user routines
         ****************************************************************************/
        void BlockCallback(short handle, short status, IntPtr pVoid)
        {
            // flag to say done reading data
            _ready = true;
        }

        /****************************************************************************
         * SetDefaults - restore default settings
         ****************************************************************************/
        void SetDefaults()
        {
            for (int i = 0; i < _channelCount; i++) // reset channels to most recent settings
            {
                Imports.SetChannel(_handle, Imports.Channel.ChannelA + i,
                                   (short)(_channelSettings[(int)(Imports.Channel.ChannelA + i)].enabled ? 1 : 0),
                                   (short)(_channelSettings[(int)(Imports.Channel.ChannelA + i)].DCcoupled ? 1 : 0),
                                   _channelSettings[(int)(Imports.Channel.ChannelA + i)].range);
            }
        }

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
         * BlockDataHandler
         * - Used by all block data routines
         * - acquires data (user sets trigger mode before calling), displays 10 items
         *   and saves all to data.txt
         * Input :
         * - unit : the unit to use.
         * - text : the text to display before the display of data slice
         * - offset : the offset into the data buffer to start the display's slice.
         ****************************************************************************/
        void BlockDataHandler(string text, int offset)
        {
            uint sampleCount = BUFFER_SIZE;
            PinnedArray<short>[] minPinned = new PinnedArray<short>[_channelCount];
            PinnedArray<short>[] maxPinned = new PinnedArray<short>[_channelCount];

            int timeIndisposed;

            for (int i = 0; i < _channelCount; i++)
            {
                short[] minBuffers = new short[sampleCount];
                short[] maxBuffers = new short[sampleCount];
                minPinned[i] = new PinnedArray<short>(minBuffers);
                maxPinned[i] = new PinnedArray<short>(maxBuffers);
                int status = Imports.SetDataBuffers(_handle, (Imports.Channel)i, maxBuffers, minBuffers, (int)sampleCount);
            }

            /*  Verify the currently selected timebase index, and the maximum number of samples per channel with the current settings. */
            int timeInterval;
            int maxSamples;

            while (Imports.GetTimebase(_handle, _timebase, (int)sampleCount, out timeInterval, _oversample, out maxSamples, 0) != 0)
            {
                Console.WriteLine("Selected timebase {0} could not be used\n", _timebase);
                _timebase++;

            }
            Console.WriteLine("Timebase: {0}\toversample:{1}\n", _timebase, _oversample);

            /* Start it collecting, then wait for completion*/
            _ready = false;
            _callbackDelegate = BlockCallback;
            Imports.RunBlock(_handle, 0, (int)sampleCount, _timebase, _oversample, out timeIndisposed, 0, _callbackDelegate, IntPtr.Zero);

            Console.WriteLine("Waiting for data...Press a key to abort");

            while (!_ready && !Console.KeyAvailable)
            {
                Thread.Sleep(100);
            }

            if (Console.KeyAvailable) Console.ReadKey(true); // clear the key

            Imports.Stop(_handle);

            if (_ready)
            {
                short overflow;
                Imports.GetValues(_handle, 0, ref sampleCount, 1, Imports.DownSamplingMode.None, 0, out overflow);

                /* Print out the first 10 readings, converting the readings to mV if required */
                Console.WriteLine(text);
                Console.WriteLine("Value {0}", (_scaleVoltages) ? ("mV") : ("ADC Counts"));

                for (int ch = 0; ch < _channelCount; ch++)
                {
                    if (_channelSettings[ch].enabled)
                    {
                        Console.Write("   Ch{0}    ", (char)('A' + ch));
                    }
                }
                Console.WriteLine();

                for (int i = offset; i < offset + 10; i++)
                {
                    for (int ch = 0; ch < _channelCount; ch++)
                    {
                        if (_channelSettings[ch].enabled)
                        {
                            Console.Write("{0,6}    ", _scaleVoltages ?
                                adc_to_mv(maxPinned[ch].Target[i], (int)_channelSettings[(int)(Imports.Channel.ChannelA + ch)].range)  // If _scaleVoltages, show mV values
                                : maxPinned[ch].Target[i]);                                                                           // else show ADC counts
                        }
                    }
                    Console.WriteLine();
                }

                sampleCount = Math.Min(sampleCount, BUFFER_SIZE);
                TextWriter writer = new StreamWriter("block.txt", false);
                writer.Write("For each active channel(s), results shown are...");
                writer.WriteLine();
                writer.WriteLine("Time interval | Channel | Max Aggregated value - ADC Count & mV | Min Aggregated value  - ADC Count & mV");
                writer.WriteLine();

                // Display Header Text
                int chEnabledCount = 0;

                for (int ch = 0; ch < _channelCount; ch++)
                {
                    if (_channelSettings[ch].enabled)
                    {
                        writer.Write("Time (ns)  Ch   Max ADC  Max mV  Min ADC  Min mV  ");
                        chEnabledCount++;
                    }
                }
                writer.WriteLine();

                // Display Header/Data Separator   
                for (int i = 0; i < chEnabledCount; i++)
                {
                    if (i > 0)
                    {
                        writer.Write("--");                                               
                    }
                    writer.Write("------------------------------------------------");
                }
                writer.WriteLine();                

                // Display Data Values
                for (int i = 0; i < sampleCount; i++)
                {
                    for (int ch = 0; ch < _channelCount; ch++)
                    {
                        if (_channelSettings[ch].enabled)
                        {
                            writer.Write("{0,7}    ", (i * timeInterval));

                            writer.Write("Ch{0}  {1,7}  {2,6}  {3,7}  {4,6}  ",
                                           (char)('A' + ch),
                                           maxPinned[ch].Target[i],
                                           adc_to_mv(maxPinned[ch].Target[i],
                                                     (int)_channelSettings[(int)(Imports.Channel.ChannelA + ch)].range),
                                           minPinned[ch].Target[i],
                                           adc_to_mv(minPinned[ch].Target[i],
                                                     (int)_channelSettings[(int)(Imports.Channel.ChannelA + ch)].range));
                        }
                    }
                    writer.WriteLine();
                }
                writer.Close();
            }
            else
            {
                Console.WriteLine("data collection aborted");
                WaitForKey();
            }

            foreach (PinnedArray<short> p in minPinned)
            {
                if (p != null)
                    p.Dispose();
            }
            foreach (PinnedArray<short> p in maxPinned)
            {
                if (p != null)
                    p.Dispose();
            }
        }


        /****************************************************************************
         * RapidBlockDataHandler
         * - Used by all the CollectBlockRapid routine
         * - acquires data (user sets trigger mode before calling), displays 10 items
         *   and saves all to data.txt
         * Input :
         * - nRapidCaptures : the user specified number of blocks to capture
         ****************************************************************************/
        private void RapidBlockDataHandler(ushort nRapidCaptures)
        {
            short status;
            int numChannels = _channelCount;
            uint numSamples = BUFFER_SIZE;

            // Run the rapid block capture
            int timeIndisposed;
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



            Console.WriteLine("Waiting for data...Press a key to abort");

            while (!_ready && !Console.KeyAvailable)
            {
                Thread.Sleep(100);
            }

            if (Console.KeyAvailable) Console.ReadKey(true); // clear the key

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

                        status = Imports.SetDataBuffersRapid(_handle,
                                               (Imports.Channel)channel,
                                               values[segment][channel],
                                               (int)numSamples,
                                               segment);
                    }
                    else
                    {
                        status = Imports.SetDataBuffersRapid(_handle,
                                   (Imports.Channel)channel,
                                    null,
                                    0,
                                    segment);

                    }
                }
            }

            // Read the data
            short[] overflows = new short[nRapidCaptures];

            status = Imports.GetValuesRapid(_handle, ref numSamples, 0, (ushort)(nRapidCaptures - 1), overflows);

            /* Print out the first 10 readings, converting the readings to mV if required */
            Console.WriteLine("\nValues in {0}", (_scaleVoltages) ? ("mV") : ("ADC Counts"));

            for (int seg = 0; seg < nRapidCaptures; seg++)
            {
                Console.WriteLine("Capture {0}", seg);
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
                    p.Dispose();
            }

            //TODO: Do what ever is required with the data here.
        }


        /****************************************************************************
        *  WaitForKey
        *  Wait for user's keypress
        ****************************************************************************/
        private static void WaitForKey()
        {
            while (!Console.KeyAvailable) Thread.Sleep(100);
            if (Console.KeyAvailable) Console.ReadKey(true); // clear the key
        }

        /****************************************************************************
        *  SetTrigger
        *  this function sets all the required trigger parameters, and calls the 
        *  triggering functions
        ****************************************************************************/
        short SetTrigger(Imports.TriggerChannelProperties[] channelProperties, short nChannelProperties, Imports.TriggerConditions[] triggerConditions, short nTriggerConditions, Imports.ThresholdDirection[] directions, Pwq pwq, uint delay, short auxOutputEnabled, int autoTriggerMs)
        {
            short status;

            status = Imports.SetTriggerChannelProperties(_handle, channelProperties, nChannelProperties,
                                                            auxOutputEnabled, autoTriggerMs);

            if (status != StatusCodes.PICO_OK)
            {
                return status;
            }

            if ((status = Imports.SetTriggerChannelConditions(_handle, triggerConditions, nTriggerConditions)) != StatusCodes.PICO_OK)
            {
                return status;
            }

            if (directions == null)
            {
                directions = new Imports.ThresholdDirection[] { Imports.ThresholdDirection.None,
                                Imports.ThresholdDirection.None, Imports.ThresholdDirection.None,
                                Imports.ThresholdDirection.None, Imports.ThresholdDirection.None,
                                Imports.ThresholdDirection.None};
            }

            if ((status = Imports.SetTriggerChannelDirections(_handle,
                                                              directions[(int)Imports.Channel.ChannelA],
                                                              directions[(int)Imports.Channel.ChannelB],
                                                              directions[(int)Imports.Channel.ChannelC],
                                                              directions[(int)Imports.Channel.ChannelD],
                                                              directions[(int)Imports.Channel.External],
                                                              directions[(int)Imports.Channel.Aux])) != StatusCodes.PICO_OK)
            {
                return status;
            }

            status = Imports.SetTriggerDelay(_handle, delay);

            if (status != StatusCodes.PICO_OK)
            {
                return status;
            }

            if (pwq == null)
            {
                pwq = new Pwq(null, 0, Imports.ThresholdDirection.None, 0, 0, Imports.PulseWidthType.None);
            }

            status = Imports.SetPulseWidthQualifier(_handle, pwq.conditions,
                                                    pwq.nConditions, pwq.direction,
                                                    pwq.lower, pwq.upper, pwq.type);

            return status;
        }

        /****************************************************************************
         * CollectBlockImmediate
         *  this function demonstrates how to collect a single block of data
         *  from the unit (start collecting immediately)
         ****************************************************************************/
        void CollectBlockImmediate()
        {
            Console.WriteLine("Collect block immediate...");
            Console.WriteLine("Press a key to start");
            WaitForKey();

            SetDefaults();

            /* Trigger disabled	*/
            SetTrigger(null, 0, null, 0, null, null, 0, 0, 0);

            BlockDataHandler("First 10 readings", 0);
        }

        /****************************************************************************
        *  CollectBlockRapid
        *  this function demonstrates how to collect blocks of data
        * using the RapidCapture function
        ****************************************************************************/
        void CollectBlockRapid()
        {

            ushort numRapidCaptures = 1;
            bool valid = false;

            Console.WriteLine("Collect rapid block...");
            Console.WriteLine("Specify number of captures:");

            do
            {
                try
                {
                    numRapidCaptures = ushort.Parse(Console.ReadLine());
                    valid = true;
                }
                catch
                {
                    valid = false;
                    Console.WriteLine("\nEnter numeric values only");
                }

            } while (Imports.SetNoOfRapidCaptures(_handle, numRapidCaptures) > 0 || !valid);

            int maxSamples;
            Imports.MemorySegments(_handle, numRapidCaptures, out maxSamples);

            Console.WriteLine("Collecting {0} rapid blocks. Press a key to start", numRapidCaptures);

            WaitForKey();

            SetDefaults();

            /* Trigger is optional, disable it for now	*/
            SetTrigger(null, 0, null, 0, null, null, 0, 0, 0);

            RapidBlockDataHandler(numRapidCaptures);
        }

        /****************************************************************************
       * CollectBlockTriggered
       *  This function demonstrates how to collect a single block of data from the
       *  unit, when a trigger event occurs.
       ****************************************************************************/
        void CollectBlockTriggered()
        {
            short triggerVoltage = mv_to_adc(1000, (short)_channelSettings[(int)Imports.Channel.ChannelA].range); // ChannelInfo stores ADC counts
            Imports.TriggerChannelProperties[] sourceDetails = new Imports.TriggerChannelProperties[] {
        new Imports.TriggerChannelProperties(triggerVoltage,
                                             256*10,
                                             triggerVoltage,
                                             256*10,
                                             Imports.Channel.ChannelA,
                                             Imports.ThresholdMode.Level)};

            Imports.TriggerConditions[] conditions = new Imports.TriggerConditions[] {
              new Imports.TriggerConditions(Imports.TriggerState.True,
                                            Imports.TriggerState.DontCare,
                                            Imports.TriggerState.DontCare,
                                            Imports.TriggerState.DontCare,
                                            Imports.TriggerState.DontCare,
                                            Imports.TriggerState.DontCare,
                                            Imports.TriggerState.DontCare)};

            Imports.ThresholdDirection[] directions = new Imports.ThresholdDirection[]
	                                        { Imports.ThresholdDirection.Rising,
                                            Imports.ThresholdDirection.None, 
                                            Imports.ThresholdDirection.None, 
                                            Imports.ThresholdDirection.None, 
                                            Imports.ThresholdDirection.None,
                                            Imports.ThresholdDirection.None };

            Console.WriteLine("Collect block triggered...");


            Console.Write("Collects when value rises past {0}", (_scaleVoltages) ?
                          adc_to_mv(sourceDetails[0].ThresholdMajor,
                                    (int)_channelSettings[(int)Imports.Channel.ChannelA].range)
                                    : sourceDetails[0].ThresholdMajor);
            Console.WriteLine("{0}", (_scaleVoltages) ? ("mV") : ("ADC Counts"));

            Console.WriteLine("Press a key to start...");
            WaitForKey();

            SetDefaults();

            /* Trigger enabled
             * Rising edge
             * Threshold = 1000 mV */
            SetTrigger(sourceDetails, 1, conditions, 1, directions, null, 0, 0, 0);

            BlockDataHandler("Ten readings after trigger", 0);
        }

        /****************************************************************************
        * Initialise unit' structure with Variant specific defaults
        ****************************************************************************/
        void GetDeviceInfo()
        {
            int variant = 0;
            string[] description = {
                           "Driver Version    ",
                           "USB Version       ",
                           "Hardware Version  ",
                           "Variant Info      ",
                           "Serial            ",
                           "Cal Date          ",
                           "Kernel Ver        "
                         };
            System.Text.StringBuilder line = new System.Text.StringBuilder(80);

            if (_handle >= 0)
            {
                for (int i = 0; i < 7; i++)
                {
                    short requiredSize;
                    Imports.GetUnitInfo(_handle, line, 80, out requiredSize, i);

                    if (i == 3)
                    {
                        line.Length = 4;
                        variant = Convert.ToInt16(line.ToString());
                    }
                    Console.WriteLine("{0}: {1}", description[i], line);
                }

                switch (variant)
                {
                    case (int)Imports.Model.PS4223:
                        _firstRange = Imports.Range.Range_50MV;
                        _lastRange = Imports.Range.Range_100V;
                        _channelCount = DUAL_SCOPE;
                        break;

                    case (int)Imports.Model.PS4224:
                        _firstRange = Imports.Range.Range_50MV;
                        _lastRange = Imports.Range.Range_20V;
                        _channelCount = DUAL_SCOPE;
                        break;

                    case (int)Imports.Model.PS4423:
                        _firstRange = Imports.Range.Range_50MV;
                        _lastRange = Imports.Range.Range_100V;
                        _channelCount = QUAD_SCOPE;
                        break;

                    case (int)Imports.Model.PS4424:
                        _firstRange = Imports.Range.Range_50MV;
                        _lastRange = Imports.Range.Range_20V;
                        _channelCount = QUAD_SCOPE;
                        break;

                    case (int)Imports.Model.PS4226:
                        _firstRange = Imports.Range.Range_50MV;
                        _lastRange = Imports.Range.Range_20V;
                        _channelCount = DUAL_SCOPE;
                        break;

                    case (int)Imports.Model.PS4227:
                        _firstRange = Imports.Range.Range_50MV;
                        _lastRange = Imports.Range.Range_20V;
                        _channelCount = DUAL_SCOPE;
                        break;

                    case (int)Imports.Model.PS4262:
                        _firstRange = Imports.Range.Range_10MV;
                        _lastRange = Imports.Range.Range_20V;
                        _channelCount = DUAL_SCOPE;
                        break;
                }
            }
        }

        /****************************************************************************
         * Select input voltage ranges for channels A and B
         ****************************************************************************/
        void SetVoltages()
        {
            bool valid = false;

            /* See what ranges are available... */
            for (int i = (int)_firstRange; i <= (int)_lastRange; i++)
            {
                Console.WriteLine("{0} . {1} mV", i, inputRanges[i]);
            }

            /* Ask the user to select a range */
            Console.WriteLine("Specify voltage range ({0}..{1})", _firstRange, _lastRange);
            Console.WriteLine("99 - switches channel off");
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
                    catch
                    {
                        valid = false;
                        Console.WriteLine("\nEnter numeric values only");
                    }

                } while ((range != 99 && (range < (uint)_firstRange || range > (uint)_lastRange) || !valid));


                if (range != 99)
                {
                    _channelSettings[ch].range = (Imports.Range)range;
                    Console.WriteLine(" = {0} mV", inputRanges[range]);
                    _channelSettings[ch].enabled = true;
                }
                else
                {
                    Console.WriteLine("Channel Switched off");
                    _channelSettings[ch].enabled = false;
                }
            }
            SetDefaults();  // Set defaults now, so that if all but 1 channels get switched off, timebase updates to timebase 0 will work
        }

        /****************************************************************************
         *
         * Select _timebase, set _oversample to on and time units as nano seconds
         *
         ****************************************************************************/
        void SetTimebase()
        {
            int timeInterval;
            int maxSamples;
            bool valid = false;

            Console.WriteLine("Specify timebase");

            do
            {
                try
                {
                    _timebase = uint.Parse(Console.ReadLine());
                    valid = true;
                }
                catch
                {
                    valid = false;
                    Console.WriteLine("\nEnter numeric values only");
                }

            } while (!valid);

            while (Imports.GetTimebase(_handle, _timebase, BUFFER_SIZE, out timeInterval, 1, out maxSamples, 0) != 0)
            {
                Console.WriteLine("Selected timebase {0} could not be used", _timebase);
                _timebase++;
            }

            Console.WriteLine("Timebase {0} - {1} ns", _timebase, timeInterval);
            _oversample = 1;
        }


        /****************************************************************************
        * Stream Data Handler
        * - Used by the two stream data examples - untriggered and triggered
        * Inputs:
        * - preTrigger - the number of samples in the pre-trigger phase 
        *					(0 if no trigger has been set)
        ***************************************************************************/
        void StreamDataHandler(uint preTrigger)
        {
            uint sampleCount = BUFFER_SIZE * 10; /*  *10 is to make sure buffer large enough */
            appBuffers = new short[_channelCount * 2][];
            buffers = new short[_channelCount * 2][];

            uint totalsamples = 0;
            uint triggeredAt = 0;
            short status;

            uint sampleInterval = 1;

            for (int ch = 0; ch < _channelCount * 2; ch += 2) // create data buffers
            {
                appBuffers[ch] = new short[sampleCount];
                appBuffers[ch + 1] = new short[sampleCount];

                buffers[ch] = new short[sampleCount];
                buffers[ch + 1] = new short[sampleCount];

                status = Imports.SetDataBuffers(_handle, (Imports.Channel)(ch / 2), buffers[ch], buffers[ch + 1], (int)sampleCount);
            }

            Console.WriteLine("Waiting for trigger...Press a key to abort");
            _autoStop = false;
            status = Imports.RunStreaming(_handle, ref sampleInterval, Imports.ReportedTimeUnits.MicroSeconds,
                                        preTrigger, 1000000 - preTrigger, true, 1000, sampleCount);
            Console.WriteLine("Run Streaming : {0} ", status);

            Console.WriteLine("Streaming data...Press a key to abort");

            TextWriter writer = new StreamWriter("stream.txt", false);
            writer.Write("For each of the {0} Channels, results shown are....", _channelCount);
            writer.WriteLine();
            writer.WriteLine("Maximum Aggregated value ADC Count & mV, Minimum Aggregated value ADC Count & mV");
            writer.WriteLine();

            for (int i = 0; i < _channelCount; i++)
                writer.Write("Ch  Max ADC    Max mV   Min ADC    Min mV   ");
            writer.WriteLine();

            while (!_autoStop && !Console.KeyAvailable)
            {
                /* Poll until data is received. Until then, GetStreamingLatestValues wont call the callback */
                Thread.Sleep(100);
                _ready = false;
                Imports.GetStreamingLatestValues(_handle, StreamingCallback, IntPtr.Zero);

                if (_ready && _sampleCount > 0) /* can be ready and have no data, if autoStop has fired */
                {
                    if (_trig > 0)
                        triggeredAt = totalsamples + _trigAt;
                    totalsamples += (uint)_sampleCount;

                    Console.Write("\nCollected {0,4} samples, index = {1,5} Total = {2,5}", _sampleCount, _startIndex, totalsamples);

                    if (_trig > 0)
                        Console.Write("\tTrig at Index {0}", triggeredAt);

                    for (uint i = _startIndex; i < (_startIndex + _sampleCount); i++)
                    {
                        for (int ch = 0; ch < _channelCount * 2; ch += 2)
                        {
                            if (_channelSettings[ch / 2].enabled)
                            {

                                 writer.Write("Ch{0} {1,7}   {2,7}   {3,7}   {4,7}   ",
                                    (char)('A' + (ch / 2)),
                                    appBuffers[ch][i],
                                    adc_to_mv(appBuffers[ch][i], (int)_channelSettings[(int)(Imports.Channel.ChannelA + (ch/2))].range),
                                    appBuffers[ch + 1][i],
                                    adc_to_mv(appBuffers[ch + 1][i], (int)_channelSettings[(int)(Imports.Channel.ChannelA + (ch/2))].range));
                            }
                        }
                        writer.WriteLine();
                    }
                }
            }
            if (Console.KeyAvailable) Console.ReadKey(true); // clear the key

            Imports.Stop(_handle);
            writer.Close();

            if (!_autoStop)
            {
                Console.WriteLine("\ndata collection aborted");
            }
        }


        /****************************************************************************
         * CollectStreamingImmediate
         *  this function demonstrates how to collect a stream of data
         *  from the unit (start collecting immediately)
         ***************************************************************************/
        void CollectStreamingImmediate()
        {
            SetDefaults();

            Console.WriteLine("Collect streaming...");
            Console.WriteLine("Data is written to disk file (stream.txt)");
            Console.WriteLine("Press a key to start");
            WaitForKey();

            /* Trigger disabled	*/
            SetTrigger(null, 0, null, 0, null, null, 0, 0, 0);

            StreamDataHandler(0);
        }


        /****************************************************************************
        * CollectStreamingTriggered
        *  this function demonstrates how to collect a stream of data
        *  from the unit (start collecting on trigger)
        ***************************************************************************/
        void CollectStreamingTriggered()
        {
            short triggerVoltage = mv_to_adc(1000, (short)_channelSettings[(int)Imports.Channel.ChannelA].range); // ChannelInfo stores ADC counts

            Imports.TriggerChannelProperties[] sourceDetails = new Imports.TriggerChannelProperties[] {
        new Imports.TriggerChannelProperties( triggerVoltage, 
                                                256 * 10, 
                                                triggerVoltage, 
                                                256 * 10, 
                                                Imports.Channel.ChannelA, 
                                                Imports.ThresholdMode.Level )};

            Imports.TriggerConditions[] conditions = new Imports.TriggerConditions[] {
              new Imports.TriggerConditions(Imports.TriggerState.True,
                                            Imports.TriggerState.DontCare,
                                            Imports.TriggerState.DontCare,
                                            Imports.TriggerState.DontCare,
                                            Imports.TriggerState.DontCare,
                                            Imports.TriggerState.DontCare,
                                            Imports.TriggerState.DontCare)};

            Imports.ThresholdDirection[] directions = new Imports.ThresholdDirection[]
	                                        { Imports.ThresholdDirection.Rising,
                                            Imports.ThresholdDirection.None, 
                                            Imports.ThresholdDirection.None, 
                                            Imports.ThresholdDirection.None, 
                                            Imports.ThresholdDirection.None,
                                            Imports.ThresholdDirection.None };

            Console.WriteLine("Collect streaming triggered...");
            Console.WriteLine("Data is written to disk file (stream.txt)");
            Console.WriteLine("Press a key to start");
            WaitForKey();
            SetDefaults();

            /* Trigger enabled
             * Rising edge
             * Threshold = 1000mV */

            SetTrigger(sourceDetails, 1, conditions, 1, directions, null, 0, 0, 0);

            StreamDataHandler(100000);
        }

        /****************************************************************************
        * DisplaySettings 
        * Displays information about the user configurable settings in this example
        ***************************************************************************/
        void DisplaySettings()
        {
            int ch;
            int voltage;

            Console.WriteLine("\n\nReadings will be scaled in {0}", (_scaleVoltages) ? ("mV") : ("ADC counts"));

            for (ch = 0; ch < _channelCount; ch++)
            {
                voltage = inputRanges[(int)_channelSettings[ch].range];
                Console.Write("Channel {0} Voltage Range = ", (char)('A' + ch));

                if (voltage < 1000)
                {
                    Console.WriteLine("{0}mV", voltage);
                }
                else
                {
                    Console.WriteLine("{0}V", voltage / 1000);
                }
            }
            Console.WriteLine();
        }


        /****************************************************************************
        * Run - show menu and call user selected options
        ****************************************************************************/
        public void Run()
        {
            // setup devices
            GetDeviceInfo();
            _timebase = 1;


            _channelSettings = new ChannelSettings[MAX_CHANNELS];

            for (int i = 0; i < MAX_CHANNELS; i++)
            {
                _channelSettings[i].enabled = true;
                _channelSettings[i].DCcoupled = true;
                _channelSettings[i].range = Imports.Range.Range_5V;
            }

            // main loop - read key and call routine
            char ch = ' ';
            while (ch != 'X')
            {
                DisplaySettings();

                Console.WriteLine("");
                Console.WriteLine("B - immediate block              V - Set voltages");
                Console.WriteLine("T - triggered block              I - Set timebase");
                Console.WriteLine("R - rapid block                  A - ADC counts/mV");
                Console.WriteLine("S - immediate streaming          X - exit");
                Console.WriteLine("W - triggered streaming");
                Console.WriteLine("Operation:");

                ch = char.ToUpper(Console.ReadKey(true).KeyChar);

                Console.WriteLine("\n");
                switch (ch)
                {
                    case 'B':
                        CollectBlockImmediate();
                        break;

                    case 'T':
                        CollectBlockTriggered();
                        break;

                    case 'R':
                        CollectBlockRapid();
                        break;

                    case 'S':
                        CollectStreamingImmediate();
                        break;

                    case 'W':
                        CollectStreamingTriggered();
                        break;

                    case 'V':
                        SetVoltages();
                        break;

                    case 'I':
                        SetTimebase();
                        break;

                    case 'A':
                        _scaleVoltages = !_scaleVoltages;
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

        private ConsoleExample(short handle)
        {
            _handle = handle;
        }

        static void Main()
        {
            Console.WriteLine("C# PS4000 driver example program");
            Console.WriteLine("Version 1.1\n");

            //open unit and show splash screen
            Console.WriteLine("\n\nOpening the device...");
            short handle;
            short status = Imports.OpenUnit(out handle);
            Console.WriteLine("Handle: {0}", handle);

            if (status != StatusCodes.PICO_OK)
            {
                Console.WriteLine("Unable to open device");
                Console.WriteLine("Error code : {0}", status);
                WaitForKey();
            }
            else
            {
                Console.WriteLine("Device opened successfully\n");

                ConsoleExample consoleExample = new ConsoleExample(handle);
                consoleExample.Run();

                Imports.CloseUnit(handle);
            }
        }
    }
}  
