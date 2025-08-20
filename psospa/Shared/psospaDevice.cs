// This file contains shared functions for psospa API examples
// Copyright © 2024 Pico Technology Ltd. See LICENSE file for terms.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
//using System.Runtime.Remoting.Channels;
//using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DriverImports;
using ProbeScaling;
using PicoPinnedArray;

class psospaDevice
{
    public struct ChannelSettings
    {
        public bool enabled;
        public Coupling coupling;
        public long rangeMin;
        public long rangeMax;
        public enPicoProbeRangeInfo rangeType;
        public double AnalogueOffset;
        public BandwidthLimiter bandwidthLimiter;
    }

    /// <summary>
    /// Tranforms local ChannelSettings API structure to generic one
    /// </summary>
    public static void FormatChannelSettings(in ChannelSettings[] channelSetup, out ChannelSettingsGeneric[] channelSetupGeneric)
    {
        channelSetupGeneric = new ChannelSettingsGeneric[channelSetup.Length];
        int NoEnabledchannels = 0;

        //Convert sturture ChannelSettingsGeneric
        for (var channelcount = Channel.ChannelA; channelcount < (Channel)channelSetup.Length; channelcount++)
        {
            if (channelSetup[(int)channelcount].enabled)
            {
                channelSetupGeneric[(int)channelcount].enabled = true;
                channelSetupGeneric[(int)channelcount].driverRangeType = 1;//0 is default enum range APIs, 1 is for psospa range type
                //channelSetupGeneric[(int)channelcount].coupling = channelSetup[(int)channelcount].coupling;
                channelSetupGeneric[(int)channelcount].range = PicoConnectProbes.PicoConnectProbes.PicoConnectProbeRange.PICO_CONNECT_PROBE_OFF; //Not used
                channelSetupGeneric[(int)channelcount].rangeMin = channelSetup[(int)channelcount].rangeMin;
                channelSetupGeneric[(int)channelcount].rangeMax = channelSetup[(int)channelcount].rangeMax;
                channelSetupGeneric[(int)channelcount].rangeType = channelSetup[(int)channelcount].rangeType;
                channelSetupGeneric[(int)channelcount].AnalogueOffset = channelSetup[(int)channelcount].AnalogueOffset;
                //channelSetupGeneric[(int)channelcount].bandwidthLimiter = channelSetup[(int)channelcount].bandwidthLimiter;
                NoEnabledchannels++;
            }
            else
            {
                channelSetupGeneric[(int)channelcount].enabled = false;
            }
        }
    }

        /// <summary>
        /// Opens a psospa unit
        /// </summary>
        /// 
        public static StandardDriverStatusCode OpenUnit(out short handle, DeviceResolution resolution, out int numChannels)
    {
        numChannels = 0;

        PicoUsbPowerDetails picoUsbPowerDetailsTemp = new PicoUsbPowerDetails();
        var status = DriverImports.psospa.OpenUnit(out handle, null, resolution, ref picoUsbPowerDetailsTemp);
        if (handle <= 0 || status != StandardDriverStatusCode.Ok) return status;

        Console.WriteLine("The psospa Device has successfully opened with handle: " + handle);

        short requiredSize = 80;
        StringBuilder info = new StringBuilder(requiredSize);
        status = DriverImports.psospa.GetUnitInfo(handle, info, requiredSize, out requiredSize, InfoType.VariantInfo);
        if (status != StandardDriverStatusCode.Ok) return status;

        numChannels = (int)Char.GetNumericValue(info[1]); //2nd Char contains the number of channels of the device variant
        Console.WriteLine("This device has " + numChannels + " Channels");

        return status;
    }
    /// <summary>
    /// Closes a psospa unit
    /// </summary>
    public static void CloseUnit(short handle)
    {
        DriverImports.psospa.CloseUnit(handle);
    }
    /// <summary>
    /// Set up the psospa device during capture to wait until it has captured n waveforms, and to store them in the applying n memory segments.
    /// </summary>
    public static StandardDriverStatusCode SetUpWithNoOfWaveformsToCapture(short handle, ref ulong nSamples, ulong nWaveforms)
    {
        var status = DriverImports.psospa.MemorySegments(handle, nWaveforms, out var nMaxSamples);
        if (status != StandardDriverStatusCode.Ok) return status;

        status = DriverImports.psospa.SetNoOfCaptures(handle, nWaveforms);

        if (status == StandardDriverStatusCode.Ok)
            nSamples = Math.Min(nSamples, nMaxSamples);

        return status;
    }
    /// <summary>
    /// Enables a desired channel and disables all other channels.
    /// </summary>
    public static StandardDriverStatusCode InitializeChannels(short handle, List<Channel> channels, int numChannels)
    {
        var status = StandardDriverStatusCode.Ok;

        //Disable all channels by default.
        for (var channelToDisable = Channel.ChannelA; channelToDisable < (Channel)numChannels; channelToDisable++)
        {
            status = DriverImports.psospa.SetChannelOff(handle, channelToDisable);
            if (status != StandardDriverStatusCode.Ok) return status;
        }

        foreach (var channel in channels)
        {
            //Note: 50 Ohm coupling limits the max voltage range to 0.5V, passed in as nV(e+9), Probe none (x1)
            status = DriverImports.psospa.SetChannelOn(handle, channel,
                Coupling.DC50Ohm,
                (long)-0.500e+9, (long) 0.500e+9,
                enPicoProbeRangeInfo.PICO_PROBE_NONE_NV,
                0.0,
                BandwidthLimiter.BW_FULL);
            if (status == StandardDriverStatusCode.Ok)
                Console.WriteLine(channel + " enabled successfully.");
        }

        return status;
    }

    /// <summary>
    /// Enables a desired channel and disables all other channels.
    /// </summary>
    public static StandardDriverStatusCode InitializeChannelsAndRanges(short handle, in ChannelSettings[] channelSetup, int numChannels, out int NoEnabledchannels)
    {
        var status = StandardDriverStatusCode.Ok;
        NoEnabledchannels = 0;

        //Setup or Disable all channels
        for (var channelcount = Channel.ChannelA; channelcount < (Channel)numChannels; channelcount++)
        {
            if (channelSetup[(int)channelcount].enabled)
            {
                //Note: 50 Ohm coupling limits the max voltage range to 5V
                status = DriverImports.psospa.SetChannelOn(handle,
                    (Channel)channelcount,
                    channelSetup[(int)channelcount].coupling,
                    channelSetup[(int)channelcount].rangeMin,
                    channelSetup[(int)channelcount].rangeMax,
                    channelSetup[(int)channelcount].rangeType,
                    channelSetup[(int)channelcount].AnalogueOffset,
                    channelSetup[(int)channelcount].bandwidthLimiter);

                Console.WriteLine(channelcount + ": Coupling = " +
                    channelSetup[(int)channelcount].coupling + ", " +
                    channelSetup[(int)channelcount].rangeMax + ", AnalogueOffset = " +
                    channelSetup[(int)channelcount].rangeType + ", rangeType = " +
                    channelSetup[(int)channelcount].AnalogueOffset + ", Bandwidth Limiter = " +
                    channelSetup[(int)channelcount].bandwidthLimiter
                    );
                NoEnabledchannels++;
            }
            else
            {
                status = DriverImports.psospa.SetChannelOff(handle, channelcount);
                Console.WriteLine(channelcount + ": Disabled");
            }
            if (status != StandardDriverStatusCode.Ok)
                Console.WriteLine("Error! setting up channel " + channelcount + " %s", status);
        }
    return status;
    }
    /// <summary>
    /// Enables a specified digital port.
    /// </summary>
    public static StandardDriverStatusCode InitializeDigitalChannels(short handle, Channel port)
  {
    var logicThresholdLevelVolts = (double)1.25;
    var status = DriverImports.psospa.SetDigitalPortOn(handle, GetChannelDigitalPort(port), logicThresholdLevelVolts);

    return status;
  }

  /// <summary>
  /// Returns the enabled channel as a flag to be consumed by the driver.
  /// </summary>
  public static EnabledChannelsAndPorts GetChannelFlag(Channel channel)
  {
    switch (channel)
    {
      case Channel.ChannelA:
        return EnabledChannelsAndPorts.ChannelA;
      case Channel.ChannelB:
        return EnabledChannelsAndPorts.ChannelB;
      case Channel.ChannelC:
        return EnabledChannelsAndPorts.ChannelC;
      case Channel.ChannelD:
        return EnabledChannelsAndPorts.ChannelD;
      case Channel.ChannelE:
        return EnabledChannelsAndPorts.ChannelE;
      case Channel.ChannelF:
        return EnabledChannelsAndPorts.ChannelF;
      case Channel.ChannelG:
        return EnabledChannelsAndPorts.ChannelG;
      case Channel.ChannelH:
        return EnabledChannelsAndPorts.ChannelH;
      case Channel.Port0:
        return EnabledChannelsAndPorts.DigtalPort0;
      case Channel.Port1:
        return EnabledChannelsAndPorts.DigtalPort1;
      case Channel.Port2:
        return EnabledChannelsAndPorts.DigtalPort2;
      case Channel.Port3:
        return EnabledChannelsAndPorts.DigtalPort3;
      default:
        return EnabledChannelsAndPorts.None;
    }
  }

  /// <summary>
  /// Returns the correct DigitalPort enum from the Channel enum.
  /// </summary>
  public static DigitalPort GetChannelDigitalPort(Channel channel)
  {
    switch (channel)
    {
      case Channel.Port0:
        return DigitalPort.DigitalPort0;
      case Channel.Port1:
        return DigitalPort.DigitalPort1;
      case Channel.Port2:
        return DigitalPort.DigitalPort2;
      case Channel.Port3:
        return DigitalPort.DigitalPort3;
      default:
        return DigitalPort.DigitalNone;
    }
  }

  /// <summary>
  /// This function sets up a rising edge trigger on a specified channel. Expecting 2Vpp signal.
  /// </summary>
  public static StandardDriverStatusCode SetTrigger(short handle, Channel channel, int autoTriggerDelay = 0)
  {
    //5% of signal range
    ushort hysteresis = (ushort)((UInt16.MaxValue / 100.0) * 5.0);

    var triggerChannelProperty = new TriggerChannelProperties(0, hysteresis, 0, hysteresis, channel);
    var triggerChannelProperties = new List<TriggerChannelProperties>() { triggerChannelProperty };
    var status = DriverImports.psospa.SetTriggerChannelProperties(handle, triggerChannelProperties.ToArray(),
      (short)triggerChannelProperties.Count, autoTriggerDelay);

    if (status != StandardDriverStatusCode.Ok)
      return status;

    var triggerCondition = new TriggerCondition(channel, TriggerState.True);
    var triggerConditions = new List<TriggerCondition>() { triggerCondition };
    status = DriverImports.psospa.SetTriggerChannelConditions(handle, triggerConditions.ToArray(), (short)triggerConditions.Count,
      DriverImports.Action.PICO_CLEAR_ALL | DriverImports.Action.PICO_ADD);

    if (status != StandardDriverStatusCode.Ok)
      return status;

    var thresholdDirection = new ThresholdDirection(channel, TriggerDirection.Rising, ThresholdMode.Level);
    var thresholdDirections = new List<ThresholdDirection>() { thresholdDirection };
    status = DriverImports.psospa.SetTriggerChannelDirections(handle, thresholdDirections.ToArray(), (short)thresholdDirections.Count);

    if (status == StandardDriverStatusCode.Ok)
      Console.WriteLine("Trigger has been setup successfully. Please input a 800mVpp signal onto channel: " + channel);

    return status;
  }

  /// <summary>
  /// This function sets up a pulse width trigger on a specified channel. Expecting 800mVpp signal.
  /// </summary>
  public static StandardDriverStatusCode SetTriggerAndPulseWidth(short handle, Channel channel, uint pulseWidthSampleCount, PulseWidthType pulseWidthType, int autoTriggerDelay = 0)
  {
    int hysteresis = 2048;

    var triggerChannelProperty = new TriggerChannelProperties((short)hysteresis, (ushort)hysteresis, 0, (ushort)hysteresis, channel);
    var triggerChannelProperties = new List<TriggerChannelProperties>() { triggerChannelProperty };
    var status = DriverImports.psospa.SetTriggerChannelProperties(handle, triggerChannelProperties.ToArray(),
      (short)triggerChannelProperties.Count, autoTriggerDelay);

    if (status != StandardDriverStatusCode.Ok)
      return status;

    var triggerCondition = new TriggerCondition(channel, TriggerState.True);
    var triggerConditionPulseWidth = new TriggerCondition(Channel.PulseWidthSource, TriggerState.True);
    var triggerConditions = new List<TriggerCondition>() { triggerCondition, triggerConditionPulseWidth };
    status = DriverImports.psospa.SetTriggerChannelConditions(handle, triggerConditions.ToArray(),
      (short)triggerConditions.Count,
      DriverImports.Action.PICO_CLEAR_ALL | DriverImports.Action.PICO_ADD);

    if (status != StandardDriverStatusCode.Ok)
      return status;

    var thresholdDirection = new ThresholdDirection(channel, TriggerDirection.Falling, ThresholdMode.Level);
    var thresholdDirections = new List<ThresholdDirection>() { thresholdDirection };
    status = DriverImports.psospa.SetTriggerChannelDirections(handle, thresholdDirections.ToArray(), (short)thresholdDirections.Count);

    if (status != StandardDriverStatusCode.Ok)
      return status;

    var pulseCondition = new TriggerCondition(channel, TriggerState.True);
    var pulseConditions = new List<TriggerCondition>() { pulseCondition };
    status = DriverImports.psospa.SetPulseWidthQualifierConditions(handle, pulseConditions.ToArray(),
      (short)pulseConditions.Count, DriverImports.Action.PICO_CLEAR_ALL | DriverImports.Action.PICO_ADD);

    if (status != StandardDriverStatusCode.Ok)
      return status;

    var pulseDirection = new ThresholdDirection(channel, TriggerDirection.Rising, ThresholdMode.Level);
    var pulseDirections = new List<ThresholdDirection> { pulseDirection };
    status = DriverImports.psospa.SetPulseWidthQualifierDirections(handle, pulseDirections.ToArray(),
      (short)pulseDirections.Count);

    if (status != StandardDriverStatusCode.Ok)
      return status;

    status = DriverImports.psospa.SetPulseWidthQualifierProperties(handle, pulseWidthSampleCount, 0, pulseWidthType);

    if (status != StandardDriverStatusCode.Ok)
      return status;

    if (status == StandardDriverStatusCode.Ok)
      Console.WriteLine("Trigger has been setup successfully with pulse width. Please input a 800mVpp signal onto channel: " + channel);

    return status;
  }

  /// <summary>
  /// This function sets up a digital rising edge trigger on DIGITAL_CHANNEL5 on a specified port.
  /// </summary>
  public static StandardDriverStatusCode SetDigitalTrigger(short handle, Channel port)
  {
    var channel = DigitalChannel.DIGITAL_CHANNEL5;

    var digitalChannelDirection = new DigitalChannelDirections(channel, DigitalDirection.DIGITAL_DIRECTION_RISING);
    var digitalChannelDirections = new List<DigitalChannelDirections>() { digitalChannelDirection };
    var status = DriverImports.psospa.SetTriggerDigitalPortProperties(handle, port, digitalChannelDirections.ToArray(), 1);

    if (status != StandardDriverStatusCode.Ok)
      return status;

    var triggerCondition = new TriggerCondition(port, TriggerState.True);
    var triggerConditions = new List<TriggerCondition>() { triggerCondition };
    status = DriverImports.psospa.SetTriggerChannelConditions(handle, triggerConditions.ToArray(), (short)triggerConditions.Count,
                                                               DriverImports.Action.PICO_CLEAR_ALL | DriverImports.Action.PICO_ADD);

    if (status != StandardDriverStatusCode.Ok)
      return status;

    var thresholdDirection = new ThresholdDirection(port, TriggerDirection.Rising, ThresholdMode.Level);
    var thresholdDirections = new List<ThresholdDirection>() { thresholdDirection };
    status = DriverImports.psospa.SetTriggerChannelDirections(handle, thresholdDirections.ToArray(), (short)thresholdDirections.Count);

    if (status == StandardDriverStatusCode.Ok)
      Console.WriteLine("Trigger has been setup successfully. Please input a signal signal onto port: " + port + " channel: " + channel);

    return status;
  }

  /// <summary>
  /// Outputs a specified waveform from the AWG Output (frequency is set to 100kHz by default).
  /// </summary>
  public static StandardDriverStatusCode EnableAWGOutput(short handle, WaveType waveform, bool trigger = false, List<short> buffer = null, bool softwareTrigger = false, double frequency = 10E4)
  {
    if (softwareTrigger)
      trigger = true;

    double stopFrequency;
    double frequencyIncrement;
    double dwellTime;
    double peakToPeakVolts = 0.8; //800mVpp

    if (buffer == null)
      buffer = new List<short>();

    var status = DriverImports.psospa.SigGenWaveform(handle, waveform, buffer.ToArray(), (ulong)buffer.Count());
    if (status != StandardDriverStatusCode.Ok)
      return status;

    status = DriverImports.psospa.SigGenRange(handle, peakToPeakVolts, 0);
    if (status != StandardDriverStatusCode.Ok)
      return status;

    status = DriverImports.psospa.SigGenFrequency(handle, (ulong)frequency);
    if (status != StandardDriverStatusCode.Ok)
      return status;

    if (trigger)
    {
      var source = SiggenTrigSource.ScopeTrigger;

      if (softwareTrigger)
        source = SiggenTrigSource.SoftwareTrigger;

      status = DriverImports.psospa.SigGenTrigger(handle, SiggenTrigType.Rising, source, 1, 0);
      if (status != StandardDriverStatusCode.Ok)
        return status;
    }

    status = DriverImports.psospa.SigGenApply(handle, Convert.ToInt16(true),//SigGen Enable
                                               0,                           //SigGen Sweep enable
                                               Convert.ToInt16(trigger),    //SigGen trigger enable
                                               out frequency,
                                               out stopFrequency,
                                               out frequencyIncrement,
                                               out dwellTime);

    if (status == StandardDriverStatusCode.Ok)
      Console.WriteLine("AWG Setup successfully to output " + peakToPeakVolts + "Vpp " + frequency + "Hz " + waveform + " Wave.");

    return status;
  }

  /// <summary>
  /// Calls Runblock on the device
  /// </summary>
  public static StandardDriverStatusCode RunBlock(short handle, DeviceResolution resolution, ulong numSamples, Channel channel,
                                                  double idealTimeInterval, DefinitionBlockReady callback = null)
  {
    double actualTimeInterval;
    double timeIndisposedMS;
    uint timebase;
    byte roundFaster = 0;//if the exact sample interval requested is not available, the function should return the next faster or next slower interval available?

            var status = DriverImports.psospa.NearestSampleIntervalStateless(handle, GetChannelFlag(channel), 
                                                                      idealTimeInterval,
                                                                      roundFaster,
                                                                      resolution,
                                                                      out timebase,
                                                                      out actualTimeInterval);
    if (status != StandardDriverStatusCode.Ok) return status;

    //Set the number of pre and post trigger samples to half the number of samples.
    //Thus the middle sample should be the trigger point.
    return DriverImports.psospa.RunBlock(handle, numSamples / 2, numSamples / 2, timebase,
                                          out timeIndisposedMS, 0, callback, IntPtr.Zero);
  }

  public static void CancelDataCapture(ref bool exit)
  {
    System.Console.ReadKey(true);
    exit = true;
  }

  /// <summary>
  /// Waits for the data to be captured on the device
  /// </summary>
  public static StandardDriverStatusCode WaitForDataToBeCaptured(short handle, Channel channel)
  {
    short ready = 0;
    StandardDriverStatusCode status;

    bool exit = false;
    Console.WriteLine("PRESS ANY KEY TO EXIT EARLY.");
    Thread cancelWaitForCapture = new Thread(() => CancelDataCapture(ref exit));
    cancelWaitForCapture.Start();

    do
    {
      status = DriverImports.psospa.IsReady(handle, out ready);
      Thread.Sleep(1);
    } while (status == StandardDriverStatusCode.Ok && ready == 0 && !exit);

    if (ready != 0)
      DriverImports.psospa.Stop(handle);

    if (status == StandardDriverStatusCode.Ok)
      Console.WriteLine("Data captured on " + channel);

    return status;
  }

  /// <summary>
  /// Instantiate memory buffers for the driver to process data read from the device
  /// </summary>
  public static StandardDriverStatusCode ReadDataFromDevice(short handle, Channel channel, ulong numSamples, ref PinnedArray<short> pinned)
  {
    var downSampleRatioMode = RatioMode.PICO_RATIO_MODE_RAW;

        //List of new parameters added to SetDataBuffer for the psospa devices:
        //dataType            - Define type of data to be returned e.g. 8 bit data will be converted to 16bit data.
        //action              - Allows adding and removing of data buffers. See DriverImports.Action
        //Note: psospaSetDataBuffer could be used as an alternative to psospaSetDataBuffers
        var status = DriverImports.psospa.SetDataBuffers(handle, channel, pinned, null, numSamples,
                                                      DataType.PICO_INT16_T,
                                                      0, downSampleRatioMode, DriverImports.Action.PICO_CLEAR_ALL | DriverImports.Action.PICO_ADD);
    if (status != StandardDriverStatusCode.Ok) return status;

    short overflow;
    status = DriverImports.psospa.GetValues(handle, 0, ref numSamples, 0, downSampleRatioMode, 0, out overflow);
    if (status == StandardDriverStatusCode.Ok)
      Console.WriteLine("Data has been copied safely to a memory buffer.");

    return status;
  }

  public static StandardDriverStatusCode GetTimeToTriggerForWaveformIndex(short handle, double actualTimeInterval,
    ulong waveformIndex, out double timeToTriggerInMilliseconds)
  {
    TriggerInfo returnedTriggerInfo;
    timeToTriggerInMilliseconds = 0.0;

    var status = DriverImports.psospa.GetTriggerInfo(handle, out returnedTriggerInfo, waveformIndex, 1);
    if (status != StandardDriverStatusCode.Ok) return status;

    // Our timestamp counter is incremented based on our time interval, so in order to get the actual time in 
    // seconds, we need to multiply out the counter value by the actual time interval we've set up.
    // Then we multiply by 1000 to convert from seconds to milliseconds. 
    timeToTriggerInMilliseconds = 1000.0 * actualTimeInterval * returnedTriggerInfo.TimeStampCounter;
    return status;
  }
}