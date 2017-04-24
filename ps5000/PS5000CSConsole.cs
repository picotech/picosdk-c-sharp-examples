/**************************************************************************
 *
 * Filename: PS5000CSConsole.cs
 * 
 * Description:
 *   This is a console-mode program that demonstrates how to use the
 *   PicoScope 5000 Series (ps5000) driver API functions using .NET
 *
 * Supported PicoScope models:
 *
 *		PicoScope 5203
 *		PicoScope 5204
 * 
 * Examples:
 *    Collect a block of samples immediately
 *    Collect a block of samples when a trigger event occurs
 *    Collect a block using ETS
 *    Collect a stream of data immediately
 *    Collect a stream of data when a trigger event occurs
 *    Set Signal Generator, using built in or custom signals
 *    
 * Copyright (C) 2007 - 2017 Pico Technology Ltd. See LICENSE file for terms.   
 *    
 **************************************************************************/

using System;
using System.IO;
using System.Threading;

using PS5000Imports;
using PS5000PinnedArray;

namespace PS5000CSConsole
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

    ushort[] inputRanges = { 10, 20, 50, 100, 200, 500, 1000, 2000, 5000, 10000, 20000, 50000 };
    bool _ready = false;
    int _sampleCount;
    uint _startIndex;
    bool _autoStop;
    private ChannelSettings [] _channelSettings;
    private int _channelCount;
    private Imports.Range _firstRange;
    private Imports.Range _lastRange;
    private Imports.ps5000BlockReady _callbackDelegate;

    /****************************************************************************
     * Callback
     * used by PS5000 data streaimng collection calls, on receipt of data.
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
    }

    /****************************************************************************
     * Callback
     * used by PS5000 data block collection calls, on receipt of data.
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
      int sampleTime;
      Imports.SetEts(_handle, Imports.EtsMode.Off, 0, 0, out sampleTime); // Turn off ETS

      for (int i = 0; i < _channelCount; i++) // reset channels to most recent settings
      {
        Imports.SetChannel(_handle, Imports.Channel.ChannelA + i,
                           (short)(_channelSettings[(int)(Imports.Channel.ChannelA + i)].enabled ? 1 : 0),
                           (short) (_channelSettings[(int)(Imports.Channel.ChannelA + i)].DCcoupled ? 1 : 0),
                           _channelSettings[(int)(Imports.Channel.ChannelA + i)].range);
      }
    }

    /****************************************************************************
     * adc_to_mv
     *
     * If the user selects scaling to millivolts,
     * Convert an 16-bit ADC count into millivolts
     ****************************************************************************/
    int adc_to_mv(int raw, int ch)
    {
      return (_scaleVoltages) ? (raw * inputRanges[ch]) / Imports.MaxValue : raw;
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
        short [] minBuffers = new short[sampleCount];
        short [] maxBuffers = new short[sampleCount];
        minPinned[i] = new PinnedArray<short>(minBuffers);      
        maxPinned[i] = new PinnedArray<short>(maxBuffers);
        Imports.SetDataBuffers(_handle, (Imports.Channel)i, minBuffers, maxBuffers, (int)sampleCount);
      }

      /*  find the maximum number of samples, the time interval (in timeUnits),
       *		 the most suitable time units, and the maximum _oversample at the current _timebase*/
      int timeInterval;
      int maxSamples;
      while (Imports.GetTimebase(_handle, _timebase, (int)sampleCount, out timeInterval, _oversample, out maxSamples, 0) != 0)
      {
        _timebase++;
      }
      Console.WriteLine("Timebase: {0}\toversample:{1}", _timebase, _oversample);

      /* Start it collecting, then wait for completion*/
      _ready = false;
      _callbackDelegate = BlockCallback;
      Imports.RunBlock(_handle, 0, (int)sampleCount, _timebase, _oversample, out timeIndisposed, 0, _callbackDelegate,
                                     IntPtr.Zero);

      /*Console.WriteLine("Run Block : {0}", status);*/
      Console.WriteLine("Waiting for data...Press a key to abort");

      while (!_ready && !Console.KeyAvailable)
      {
        Thread.Sleep(100);
      }
      if(Console.KeyAvailable) Console.ReadKey(true); // clear the key

      Imports.Stop(_handle);

      if (_ready)
      {
        short overflow;
        Imports.GetValues(_handle, 0, ref sampleCount, 1, Imports.DownSamplingMode.None, 0, out overflow);

        /* Print out the first 10 readings, converting the readings to mV if required */
        Console.WriteLine(text);
        Console.WriteLine("Value {0}", (_scaleVoltages) ? ("mV") : ("ADC Counts"));

        for (int i = offset; i < offset + 10; i++)
        {
          for (int j = 0; j < _channelCount; j++)
          {
            if (_channelSettings[j].enabled)
            {
              Console.Write("{0}\t", adc_to_mv(minPinned[j].Target[i], (int)_channelSettings[(int)(Imports.Channel.ChannelA + j)].range));
            }
          }
          Console.WriteLine();
        }

        sampleCount = Math.Min(sampleCount, BUFFER_SIZE);
        TextWriter writer = new StreamWriter("data.txt", false);
        for (int i = 0; i < sampleCount; i++)
        {
          for (int j = 0; j < _channelCount; j++)
          {
            writer.Write("{0}", (i * timeInterval));
            if (_channelSettings[j].enabled)
            {
              writer.WriteLine(", {0}, {1}, {2}, {3}",
                               minPinned[j].Target[i],
                               adc_to_mv(minPinned[j].Target[i],
                                         (int)_channelSettings[(int)(Imports.Channel.ChannelA + j)].range),
                               maxPinned[j].Target[i],
                               adc_to_mv(maxPinned[j].Target[i],
                                         (int)_channelSettings[(int)(Imports.Channel.ChannelA + j)].range));
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
    }
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
      short [] overflows = new short[nRapidCaptures];

      status = Imports.GetValuesRapid(_handle, ref numSamples, 0, (ushort)(nRapidCaptures - 1), overflows);


      // Un-pin the arrays
      foreach (PinnedArray<short> p in pinned)
      {
        if (p != null)
          p.Dispose();
      }

      //TODO: Do what ever is required with the data here.
    }

    short SetTrigger(Imports.TriggerChannelProperties[] channelProperties, short nChannelProperties, Imports.TriggerConditions[] triggerConditions, short nTriggerConditions, Imports.ThresholdDirection[] directions, Pwq pwq, uint delay, short auxOutputEnabled, int autoTriggerMs)
    {
      short status;

      if (
        (status =
         Imports.SetTriggerChannelProperties(_handle, channelProperties, nChannelProperties, auxOutputEnabled,
                                             autoTriggerMs)) != 0)
      {
        return status;
      }

      if ((status = Imports.SetTriggerChannelConditions(_handle, triggerConditions, nTriggerConditions)) != 0)
      {
        return status;
      }

      if (directions == null) directions = new Imports.ThresholdDirection[] { Imports.ThresholdDirection.None, 
        Imports.ThresholdDirection.None, Imports.ThresholdDirection.None, Imports.ThresholdDirection.None, 
        Imports.ThresholdDirection.None, Imports.ThresholdDirection.None};

      if ((status = Imports.SetTriggerChannelDirections(_handle,
                                                        directions[(int)Imports.Channel.ChannelA],
                                                        directions[(int)Imports.Channel.ChannelB],
                                                        directions[(int)Imports.Channel.ChannelC],
                                                        directions[(int)Imports.Channel.ChannelD],
                                                        directions[(int)Imports.Channel.External],
                                                        directions[(int)Imports.Channel.Aux])) != 0)
      {
        return status;
      }

      if ((status = Imports.SetTriggerDelay(_handle, delay)) != 0)
      {
        return status;
      }

      if (pwq == null) pwq = new Pwq(null, 0, Imports.ThresholdDirection.None, 0, 0, Imports.PulseWidthType.None);

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

    void CollectBlockRapid()
    {

      ushort numRapidCaptures;

      Console.WriteLine("Collect rapid block...");
      Console.WriteLine("Specify number of captures:");
      do
      {
        numRapidCaptures = ushort.Parse(Console.ReadLine());
      } while (Imports.SetNoOfRapidCaptures(_handle, numRapidCaptures) > 0);

      int maxSamples;
      Imports.MemorySegments(_handle, numRapidCaptures, out maxSamples);

      Console.WriteLine("Collecting {0} rapid blocks. Press a key to start", numRapidCaptures);

      WaitForKey();

      SetDefaults();

      /* Trigger is optional, disable it for now	*/
      SetTrigger(null, 0, null, 0, null, null, 0, 0, 0);

      RapidBlockDataHandler(numRapidCaptures);
    }



    private static void WaitForKey()
    {
      while(!Console.KeyAvailable) Thread.Sleep(100);
      if (Console.KeyAvailable) Console.ReadKey(true); // clear the key
    }

    /****************************************************************************
     * CollectBlockEts
     *  this function demonstrates how to collect a block of
     *  data using equivalent time sampling (ETS).
     ****************************************************************************/

    void CollectBlockEts()
    {
      int ets_sampletime;
      short triggerVoltage = mv_to_adc(100, (short)_channelSettings[(int)Imports.Channel.ChannelA].range); // ChannelInfo stores ADC counts
      Imports.TriggerChannelProperties[] sourceDetails = new Imports.TriggerChannelProperties[] {
              new Imports.TriggerChannelProperties( triggerVoltage, triggerVoltage, 10, Imports.Channel.ChannelA, Imports.ThresholdMode.Level )};

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

      Console.WriteLine("Collect ETS block...");
      Console.WriteLine("Collects when value rises past {0}mV", adc_to_mv(sourceDetails[0].ThresholdMajor,
                       (int)_channelSettings[(int)Imports.Channel.ChannelA].range));
      Console.WriteLine("Press a key to start...");

      WaitForKey();

      SetDefaults();

      /* Trigger enabled
       * Rising edge
       * Threshold = 1500mV
       * 10% pre-trigger  (negative is pre-, positive is post-) */
      short status = SetTrigger(sourceDetails, 1, conditions, 1, directions, null, 0, 0, 0);

      Console.WriteLine("Set Trigger : {0}", status);

      /* Enable ETS in fast mode, the computer will store 100 cycles but interleave only 10 */
      status = Imports.SetEts(_handle, Imports.EtsMode.Fast, 20, 4, out ets_sampletime);
      Console.WriteLine("Set ETS : {0}", status);
      Console.WriteLine("ETS Sample Time is: {0}", ets_sampletime);

      BlockDataHandler("Ten readings after trigger", BUFFER_SIZE / 10 - 5); // 10% of data is pre-trigger
    }

    /****************************************************************************
     * CollectBlockTriggered
     *  this function demonstrates how to collect a single block of data from the
     *  unit, when a trigger event occurs.
     ****************************************************************************/
    void CollectBlockTriggered()
    {
      short triggerVoltage = mv_to_adc(100, (short)_channelSettings[(int)Imports.Channel.ChannelA].range); // ChannelInfo stores ADC counts
      Imports.TriggerChannelProperties[] sourceDetails = new Imports.TriggerChannelProperties[] {
        new Imports.TriggerChannelProperties(triggerVoltage,
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
      Console.WriteLine("Collects when value rises past {0}mV",
                        adc_to_mv(sourceDetails[0].ThresholdMajor,
                                  (int)_channelSettings[(int)Imports.Channel.ChannelA].range));
      Console.WriteLine("Press a key to start...");
      WaitForKey();

      SetDefaults();

      /* Trigger enabled
       * Rising edge
       * Threshold = 100mV */
      SetTrigger(sourceDetails, 1, conditions, 1, directions, null, 0, 0, 0);

      BlockDataHandler("Ten readings after trigger", 0);
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
                           "Error Code"
                         };
      System.Text.StringBuilder line = new System.Text.StringBuilder(80);

      if (_handle >= 0)
      {

        for (int i = 0; i < 5; i++)
        {
          short requiredSize;
          Imports.GetUnitInfo(_handle, line, 80, out requiredSize, i);
          Console.WriteLine("{0}: {1}", description[i], line);
        }
      }
    }

    /****************************************************************************
     * Select input voltage ranges for channels A and B
     ****************************************************************************/
    void SetVoltages()
    {
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
        uint range;
        do
        {
          Console.WriteLine("Channel: {0}", 'A' + ch);
          range = uint.Parse(Console.ReadLine());
        } while (range != 99 && (range < (uint)_firstRange || range > (uint)_lastRange));


        if (range != 99)
        {
          _channelSettings[ch].range = (Imports.Range)range;
          Console.WriteLine(" - {0} mV", inputRanges[range]);
          _channelSettings[ch].enabled = true;
        }
        else
        {
          Console.WriteLine("Channel Switched off");
          _channelSettings[ch].enabled = false;
        }
      }
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

      Console.WriteLine("Specify timebase (not 0): ");
      do
      {
        _timebase = uint.Parse(Console.ReadLine());
      } while (_timebase == 0);

      Imports.GetTimebase(_handle, _timebase, BUFFER_SIZE, out timeInterval, 1, out maxSamples, 0);
      Console.WriteLine("Timebase {0} - {1} ns", _timebase, timeInterval);
      _oversample = 1;
    }

    /****************************************************************************
     * Toggles the signal generator
     * - allows user to set frequency and waveform
     * - allows for custom waveform (values 0..4192) of up to 8192 samples long
     ***************************************************************************/
    void SetSignalGenerator()
    {
      short status;
      short waveform;
      long frequency;

      short[] arbitraryWaveform = new short[8192];
      short waveformSize = 0;

      Console.WriteLine("Enter frequency in Hz: "); // Ask user to enter signal frequency;
      do
      {
        frequency = long.Parse(Console.ReadLine());
      } while (frequency < 1000 || frequency > 10000000);

      if (frequency > 0) // Ask user to enter type of signal
      {
        Console.WriteLine("Signal generator On");
        Console.WriteLine("Enter type of waveform (0..9 or 99)");
        Console.WriteLine("0:\tSINE");
        Console.WriteLine("1:\tSQUARE");
        Console.WriteLine("2:\tTRIANGLE");
        Console.WriteLine("99:\tUSER WAVEFORM");
        Console.WriteLine("  \t(see manual for more)");

        do
        {
          waveform = short.Parse(Console.ReadLine());
        } while (waveform != 99 && (waveform < 0 || waveform >= (short)Imports.SiggenWaveType.MAX_WAVE_TYPES));

        if (waveform == 99) // custom waveform selected - user needs to select file
        {
          waveformSize = 0;

          Console.WriteLine("Select a waveform file to load: ");
          string fileName;
          fileName = Console.ReadLine();
          if (File.Exists(fileName))
          {
            TextReader reader = new StreamReader(fileName);
            // Having opened file, read in data - one number per line (at most 8192 lines), with values in (0..4191)
            string text = reader.ReadLine();
            while (text != null && waveformSize < arbitraryWaveform.Length)
            {
              arbitraryWaveform[waveformSize++] = short.Parse(text);
              text = reader.ReadLine();
            }
          }
          else
          {
            Console.WriteLine("Invalid filename");
            return;
          }
        }
      }
      else
      {
        waveform = 0;
        Console.WriteLine("Signal generator Off");
      }

      if (waveformSize > 0)
      {
        double delta = ((frequency * waveformSize) / 8192) * 4294967296.0 * 8e-9; // delta >= 10
        status = Imports.SetSiggenArbitrary(_handle, 0, 4000000, (uint)delta, (uint)delta, 0, 0, arbitraryWaveform,
                                   (uint)waveformSize, Imports.SiggenSweepType.Up, false, Imports.SiggenIndexMode.Single, 0, 0,
                                   Imports.SiggenTrigType.Rising, Imports.SiggenTrigSource.None, 0);
      }
      else
      {
        status = Imports.SetSiggenBuiltIn(_handle, 0, 4000000, (Imports.SiggenWaveType)waveform, frequency, frequency, 0, 0,
                                    Imports.SiggenSweepType.Up, false, 0, 0, Imports.SiggenTrigType.Rising, Imports.SiggenTrigSource.None, 0);
      }
      Console.WriteLine("Set Signal Generator : {0}", status);
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
      uint sampleCount = BUFFER_SIZE*10; /*  *10 is to make sure buffer large enough */
      short[][] minBuffers = new short[_channelCount][];
      short[][] maxBuffers = new short[_channelCount][];
      
      uint sampleInterval = 1;

      for (int i = 0; i < _channelCount; i++) // create data buffers
      {
        minBuffers[i] = new short[sampleCount];
        maxBuffers[i] = new short[sampleCount];
        Imports.SetDataBuffers(_handle, (Imports.Channel) i, minBuffers[i], maxBuffers[i], (int) sampleCount);
      }

      Console.WriteLine("Waiting for trigger...Press a key to abort");
      _autoStop = false;
      short status = Imports.RunStreaming(_handle, ref sampleInterval,  Imports.ReportedTimeUnits.MicroSeconds,
                                  preTrigger, 1000000 - preTrigger, true, 1000, sampleCount);
      Console.WriteLine("Run Streaming : {0} ", status);

      Console.WriteLine("Streaming data...Press a key to abort");

      TextWriter writer = new StreamWriter("test.out", false);
      while (!_autoStop && !Console.KeyAvailable)
      {
        /* Poll until data is received. Until then, GetStreamingLatestValues wont call the callback */
        Thread.Sleep(100);
        _ready = false;
        Imports.GetStreamingLatestValues(_handle, StreamingCallback, IntPtr.Zero);

        if (_ready && _sampleCount > 0) /* can be ready and have no data, if autoStop has fired */
        {
          Console.WriteLine("Collected {0} samples, index = {1}", _sampleCount, _startIndex);

          for (uint i = _startIndex; i < (_startIndex + _sampleCount); i++)
          {
            for (int j = 0; j < _channelCount; j++)
            {
              if (_channelSettings[j].enabled)
              {
                writer.Write("{0}, {1}, {2}, {3},",
                                 minBuffers[j][i],
                                 adc_to_mv(minBuffers[j][i], (int) _channelSettings[(int) (Imports.Channel.ChannelA + j)].range),
                                 maxBuffers[j][i],
                                 adc_to_mv(maxBuffers[j][i], (int) _channelSettings[(int) (Imports.Channel.ChannelA + j)].range));
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
        Console.WriteLine("data collection aborted");
        WaitForKey();
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
      Console.WriteLine("Data is written to disk file (test.out)");
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
      short triggerVoltage = mv_to_adc( 100, (short) _channelSettings[(int) Imports.Channel.ChannelA].range); // ChannelInfo stores ADC counts
      
      Imports.TriggerChannelProperties[] sourceDetails = new Imports.TriggerChannelProperties[] {
        new Imports.TriggerChannelProperties( triggerVoltage, triggerVoltage, 256 * 10, Imports.Channel.ChannelA, Imports.ThresholdMode.Level )};
      
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
      Console.WriteLine("Data is written to disk file (test.out)");
      Console.WriteLine("Press a key to start");
      WaitForKey();
      SetDefaults();

      /* Trigger enabled
       * Rising edge
       * Threshold = 100mV */

      SetTrigger(sourceDetails, 1, conditions, 1, directions, null, 0, 0, 0);

      StreamDataHandler(100000);
    }
    
    public void Run()
    {
      // setup devices
      GetDeviceInfo();
      _timebase = 1;

      _firstRange = Imports.Range.Range_100MV;
      _lastRange = Imports.Range.Range_20V;
      _channelCount = DUAL_SCOPE;
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
        Console.WriteLine("");
        Console.WriteLine("B - immediate block             V - Set voltages");
        Console.WriteLine("T - triggered block             I - Set timebase");
        Console.WriteLine("R - rapid block                 F - toggle signal generator on/off");
        Console.WriteLine("E - ETS block                   A - ADC counts/mV");
        Console.WriteLine("S - immediate streaming         ");
        Console.WriteLine("W - triggered streaming");
        Console.WriteLine("                                X - exit");
        Console.WriteLine("Operation:");

        ch = char.ToUpper(Console.ReadKey().KeyChar);

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

          case 'F':
            SetSignalGenerator();
            break;

          case 'E':
            CollectBlockEts();
            break;

          case 'V':
            SetVoltages();
            break;

          case 'I':
            SetTimebase();
            break;

          case 'A':
            _scaleVoltages = !_scaleVoltages;
            if (_scaleVoltages)
            {
              Console.WriteLine("Readings will be scaled in mV");
            }
            else
            {
              Console.WriteLine("Readings will be scaled in ADC counts");
            }
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
      Console.WriteLine("PicoScope 5000 Series (ps5000) Driver Example Program");
      Console.WriteLine("Version 1.1\n");

      //open unit and show splash screen
      Console.WriteLine("\n\nOpening the device...");
      short handle;
      short status = Imports.OpenUnit(out handle);
      Console.WriteLine("Handle: {0}", handle);
      if (status != 0)
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
