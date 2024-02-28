using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DriverImports
{
  public static class PS6000a
  {
    private const string DriverName = "ps6000a.dll";

    [DllImport(DriverName, EntryPoint = "ps6000aOpenUnit")]
    public static extern StandardDriverStatusCode OpenUnit(
      out short handle,
      StringBuilder serial,
      DeviceResolution resolution);

    [DllImport(DriverName, EntryPoint = "ps6000aCloseUnit")]
    public static extern StandardDriverStatusCode CloseUnit(
      short handle);

    [DllImport(DriverName, EntryPoint = "ps6000aGetUnitInfo")]
    public static extern StandardDriverStatusCode GetUnitInfo(
      short handle,
      StringBuilder info,
      short infoLength,
      out short requiredSize,
      InfoType infoType);

    [DllImport(DriverName, EntryPoint = "ps6000aEnumerateUnits")]
    public static extern StandardDriverStatusCode EnumerateUnits(
      out short count,
      StringBuilder serials,
      out short serialLength);

    [DllImport(DriverName, EntryPoint = "ps6000aGetAdcLimits")]
      public static extern StandardDriverStatusCode GetAdcLimits(
      short handle,
      DeviceResolution resolution,
      out short minValue,
      out short maxValue);


        [DllImport(DriverName, EntryPoint = "ps6000aSetChannelOn")]
    public static extern StandardDriverStatusCode SetChannelOn(
      short handle,
      Channel channel,
      Coupling coupling,
      ChannelRange range,
      double analogueOffset,
      BandwidthLimiter bandwidth);

    [DllImport(DriverName, EntryPoint = "ps6000aSetChannelOff")]
    public static extern StandardDriverStatusCode SetChannelOff(
      short handle,
      Channel channel);

    /// <summary>
    /// This function enables digital ports
    /// </summary>
    /// <param name="handle">The handle of the required device</param>
    /// <param name="port">The digital port to be enabled.</param>
    /// <param name="logicTresholdLevels">Array of thresholds for each channel to determine whether the signal is high or low. (Measured in ADC counts).</param>
    /// <param name="logicThresholdLevelLength">The number of logic threshold levels.</param>
    /// <param name="hysteresis">Hysteresis of the threshold levels.</param>
    /// <returns></returns>
    [DllImport(DriverName, EntryPoint = "ps6000aSetDigitalPortOn")]
    public static extern StandardDriverStatusCode SetDigitalPortOn(
      short handle,
      DigitalPort port,
      short[] logicTresholdLevels,
      short logicThresholdLevelLength,
      DigitalPortHysteresis hysteresis);

    [DllImport(DriverName, EntryPoint = "ps6000aSetDigitalPortOff")]
    public static extern StandardDriverStatusCode SetDigitalPortOff(
      short handle,
      DigitalPort port);

    [DllImport(DriverName, EntryPoint = "ps6000aRunBlock")]
    public static extern StandardDriverStatusCode RunBlock(short handle,
      ulong noOfPreTriggerSamples,
      ulong noOfPostTriggerSamples,
      uint timebase,
      out double timeIndisposedMs,
      ulong segmentIndex,
      DefinitionBlockReady lpPs6000aBlockReady,
      IntPtr pVoid);

    [DllImport(DriverName, EntryPoint = "ps6000aRunStreaming")]
    public static extern StandardDriverStatusCode RunStreaming(short handle,
        out double sampleInterval,
        uint sampleIntervalTimeUnits,
        ulong maxPreTriggerSamples,
        ulong maxPostTriggerSamples,
        short autoStop,
        ulong downSampleRatio,
        RatioMode downSampleRatioMode);

    [DllImport(DriverName, EntryPoint = "ps6000aGetStreamingLatestValues")]
    public static extern StandardDriverStatusCode GetStreamingLatestValues(
        short handle,
        IntPtr streamingDataInfos,
        ulong nStreamingDataInfos,
        ref StreamingDataTriggerInfo triggerInfo);

    [DllImport(DriverName, EntryPoint = "ps6000aIsReady")]
    public static extern StandardDriverStatusCode IsReady(
      short handle,
      out short ready);

    [DllImport(DriverName, EntryPoint = "ps6000aStop")]
    public static extern StandardDriverStatusCode Stop(
      short handle);

    [DllImport(DriverName, EntryPoint = "ps6000aNearestSampleIntervalStateless")]
    public static extern StandardDriverStatusCode NearestSampleIntervalStateless(
      short handle,
      EnabledChannelsAndPorts enabledChannelFlags,
      double timeIntervalRequested,
      DeviceResolution resolution,
      out uint timebase,
      out double timeIntervalAvailable);

    [DllImport(DriverName, EntryPoint = "ps6000aSetDataBuffers")]
    public static extern StandardDriverStatusCode SetDataBuffers(
      short handle,
      Channel channel,
      short[] bufferMax,
      short[] bufferMin,
      int nSamples,
      DataType dataType,
      ulong waveform,
      RatioMode downSampleRatioMode,
      Action action);

        [DllImport(DriverName, EntryPoint = "ps6000aSetDataBuffer")]
        public static extern StandardDriverStatusCode SetDataBuffer(
      short handle,
      Channel channel,
      short[] bufferMax,
      int nSamples,
      DataType dataType,
      ulong waveform,
      RatioMode downSampleRatioMode,
      Action action);

    [DllImport(DriverName, EntryPoint = "ps6000aGetValues")]
    public static extern StandardDriverStatusCode GetValues(
      short handle,
      ulong startIndex,
      ref ulong noOfSamples,
      ulong downSampleRatio,
      RatioMode downSampleRatioMode,
      ulong segmentIndex,
      out short overflow);

    [DllImport(DriverName, EntryPoint = "ps6000aGetValuesBulk")]
    public static extern StandardDriverStatusCode GetValuesBulk(
        short handle,
        ulong startIndex,
        ref ulong noOfSamples,
        ulong fromSegmentIndex,
        ulong toSegmentIndex,
        ulong downSampleRatio,
        RatioMode downSampleRatioMode,
        out short overflow);

    [DllImport(DriverName, EntryPoint = "ps6000aSetSimpleTrigger")]
    public static extern StandardDriverStatusCode SetSimpleTrigger(
        short handle,
        short enable,
        Channel channel,
        short threshold,
        TriggerDirection direction,
        ulong delay,
        uint autoTriggerMicroseconds);

    [DllImport(DriverName, EntryPoint = "ps6000aSetTriggerChannelProperties")]
    public static extern StandardDriverStatusCode SetTriggerChannelProperties(
      short handle,
      TriggerChannelProperties[] channelProperties,
      short numChannelProperties,
      short auxOutputEnable,
      int autoTriggerMicroseconds);

    [DllImport(DriverName, EntryPoint = "ps6000aSetTriggerChannelConditions")]
    public static extern StandardDriverStatusCode SetTriggerChannelConditions(
      short handle,
      TriggerCondition[] conditions,
      short numConditions,
      Action action);

    [DllImport(DriverName, EntryPoint = "ps6000aSetTriggerChannelDirections")]
    public static extern StandardDriverStatusCode SetTriggerChannelDirections(
      short handle,
      ThresholdDirection[] directions,
      short numDirections);

    [DllImport(DriverName, EntryPoint = "ps6000aSetTriggerDigitalPortProperties")]
    public static extern StandardDriverStatusCode SetTriggerDigitalPortProperties(
      short handle,
      Channel channel,
      DigitalChannelDirections[] digitalChannelDirections,
      short nDigitalChannelDirections);

    [DllImport(DriverName, EntryPoint = "ps6000aGetTriggerInfo")]
    public static extern StandardDriverStatusCode GetTriggerInfo(
      short handle,
      out TriggerInfo triggerInfo,
      ulong firstSegmentIndex,
      ulong segmentCount);

    [DllImport(DriverName, EntryPoint = "ps6000aMemorySegments")]
    public static extern StandardDriverStatusCode MemorySegments(
      short handle,
      ulong nSegments,
      out ulong nMaxSamples);

    [DllImport(DriverName, EntryPoint = "ps6000aSetNoOfCaptures")]
    public static extern StandardDriverStatusCode SetNoOfCaptures(
      short handle,
      ulong nCaptures);

    [DllImport(DriverName, EntryPoint = "ps6000aGetNoOfCaptures")]
    public static extern StandardDriverStatusCode GetNoOfCaptures(
      short handle,
      out ulong nCaptures);

        [DllImport(DriverName, EntryPoint = "ps6000aTriggerWithinPreTriggerSamples")]
    public static extern StandardDriverStatusCode TriggerWithinPreTriggerSamples(
      short handle,
      TriggerWithinPreTrigger state
    );

    [DllImport(DriverName, EntryPoint = "ps6000aGetTimebase")]
    public static extern StandardDriverStatusCode GetTimebase(
      short handle,
      uint timebase,
      ulong noSamples,
      out double timeIntervalNanoseconds,
      out ulong maxSamples,
      ulong segmentIndex);

    /// <summary>
    /// Defines a waveform for the signal generator.
    /// </summary>
    /// <param name="handle">The handle of the required device</param>
    /// <param name="waveType">The type of waveform to be generated.</param>
    /// <param name="buffer">Used for defining an Arbitrary waveform.</param>
    /// <param name="bufferLength">The length of the buffer for the Arbitrary waveform.</param>
    [DllImport(DriverName, EntryPoint = "ps6000aSigGenWaveform")]
    public static extern StandardDriverStatusCode SigGenWaveform(
      short handle,
      WaveType waveType,
      short[] buffer,
      ulong bufferLength
    );

    /// <summary>
    /// Defines the voltage range for the signal generator.
    /// </summary>
    /// <param name="handle">The handle of the required device</param>
    /// <param name="peakToPeakVolts">The peak-to-peak voltage, in volts, of the waveform signal.</param>
    /// <param name="offsetVolts">The voltage offset, in volts, to be applied to the waveform</param>
    [DllImport(DriverName, EntryPoint = "ps6000aSigGenRange")]
    public static extern StandardDriverStatusCode SigGenRange(
      short handle,
      double peakToPeakVolts,
      double offsetVolts
    );

    /// <summary>
    /// Defines the frequency for the signal generator.
    /// </summary>
    /// <param name="handle">The handle of the required device</param>
    /// <param name="frequencyHz">The frequency that the signal generator will produce.</param>
    [DllImport(DriverName, EntryPoint = "ps6000aSigGenFrequency")]
    public static extern StandardDriverStatusCode SigGenFrequency(
      short handle,
      double frequencyHz
    );

    /// <summary>
    /// 
    /// </summary>
    /// <param name="handle">The handle of the required device</param>
    /// <param name="triggerType">The type of trigger that will be applied to the signal generator</param>
    /// <param name="triggerSource">The source that will trigger the signal generator.</param>
    /// <param name="cycles">The number of signals outputted by the signal generator after triggering.</param>
    /// <param name="autoTriggerPs">The time in pico seconds that the signal generator will automatically trigger.</param>
    [DllImport(DriverName, EntryPoint = "ps6000aSigGenTrigger")]
    public static extern StandardDriverStatusCode SigGenTrigger(
      short handle,
      SiggenTrigType triggerType,
      SiggenTrigSource triggerSource,
      ulong cycles,
      ulong autoTriggerPs
    );

    /// <summary>
    /// This function enables or disables the signal generator.
    /// </summary>
    /// <param name="handle">The handle of the required device</param>
    /// <param name="sigGenEnabled">0 = Disabling the signal generator, 1 = Enabling the signal generator</param>
    /// <param name="sweepEnabled">0 = Sweep disabled, 1 = Sweep enabled</param>
    /// <param name="triggerEnabled">0 = Trigger enabled, 1 = Trigger disabled</param>
    /// <param name="automaticClockOptimisationEnabled">0 = Automatic Clock Optimisation disabled 1 = Automatic Clock Optimisation enabled</param>
    /// <param name="overrideAutomaticClockCalculation">Manually set the clock for the signal generator</param>
    /// <param name="frequency">The frequency that the signal generator will initially produce.</param>
    /// <param name="stopFrequency">The frequency at which the sweep reverses direction or returns to the initial frequency</param>
    /// <param name="frequencyIncrement">The amount of frequency increase or decrease in sweep mode</param>
    /// <param name="dwellTime">The time for which the sweep stays at each frequency, in seconds</param>
    [DllImport(DriverName, EntryPoint = "ps6000aSigGenApply")]
    public static extern StandardDriverStatusCode SigGenApply(
      short handle,
      short sigGenEnabled,
      short sweepEnabled,
      short triggerEnabled,
      short automaticClockOptimisationEnabled,
      short overrideAutomaticClockCalculation,
      out double frequency,
      out double stopFrequency,
      out double frequencyIncrement,
      out double dwellTime
    );

    [DllImport(DriverName, EntryPoint = "ps6000aSigGenSoftwareTriggerControl")]
    public static extern StandardDriverStatusCode SigGenSoftwareTriggerControl(
      short handle,
      SiggenTrigType triggerState
    );

    [DllImport(DriverName, EntryPoint = "ps6000aSetPulseWidthQualifierProperties")]
    public static extern StandardDriverStatusCode SetPulseWidthQualifierProperties(
      short handle,
      uint lower,
      uint upper,
      PulseWidthType type);

    [DllImport(DriverName, EntryPoint = "ps6000aSetPulseWidthQualifierConditions")]
    public static extern StandardDriverStatusCode SetPulseWidthQualifierConditions(
      short handle,
      TriggerCondition[] conditions,
      short nConditions,
      Action action);

    [DllImport(DriverName, EntryPoint = "ps6000aSetPulseWidthQualifierDirections")]
    public static extern StandardDriverStatusCode SetPulseWidthQualifierDirections(
      short handle,
      ThresholdDirection[] directions,
      short nDirections);

    [DllImport(DriverName, EntryPoint = "ps6000aSetDigitalPortInteractionCallback")]
    public static extern StandardDriverStatusCode SetDigitalPortInteractionCallback(
      short handle,
      DigitalPortCallback callback
    );
  }
}