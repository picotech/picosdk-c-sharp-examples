/**************************************************************************
*
* Filename:    ps6000aImports.cs
*
* Description:
* This file contains .NET wrapper calls corresponding to function calls 
* defined in the psospa.h C header file. 
* 
* Copyright © 2024 Pico Technology Ltd. See LICENSE file for terms.
*
*************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using PicoConnectProbes;

namespace DriverImports
{
  public static class psospa
  {
    private const string DriverName = "psospa.dll";

    [DllImport(DriverName, EntryPoint = "psospaOpenUnit")]
    public static extern StandardDriverStatusCode OpenUnit(
      out short handle,
      StringBuilder serial,
      DeviceResolution resolution,
      ref PicoUsbPowerDetails powerDetails);  /*      <-- REVIEW POINTER TO C STRUCTURE    RETURN only  */

    [DllImport(DriverName, EntryPoint = "psospaGetUnitInfo")]
    public static extern StandardDriverStatusCode GetUnitInfo(
      short handle,
      StringBuilder info,
      short infoLength,
      out short requiredSize,
      InfoType infoType);

    [DllImport(DriverName, EntryPoint = "psospaCloseUnit")]
    public static extern StandardDriverStatusCode CloseUnit(
      short handle);

    [DllImport(DriverName, EntryPoint = "psospaMemorySegments")]
    public static extern StandardDriverStatusCode MemorySegments(
      short handle,
      ulong nSegments,
      out ulong nMaxSamples);

    [DllImport(DriverName, EntryPoint = "psospaMemorySegmentsBySamples")]
    public static extern StandardDriverStatusCode MemorySegmentsBySamples(int handle, ulong nSamples, ref ulong nMaxSegments);

    [DllImport(DriverName, EntryPoint = "psospaGetMaximumAvailableMemory")]
    public static extern StandardDriverStatusCode GetMaximumAvailableMemory(int handle, ref ulong nMaxSamples, DeviceResolution resolution);

    [DllImport(DriverName, EntryPoint = "psospaQueryMaxSegmentsBySamples")]
    public static extern StandardDriverStatusCode QueryMaxSegmentsBySamples(int handle, ulong nSamples, uint nChannelEnabled,
        ref ulong nMaxSegments, DeviceResolution resolution);   

    [DllImport(DriverName, EntryPoint = "psospaSetChannelOn")]
    public static extern StandardDriverStatusCode SetChannelOn(
      short handle,
      Channel channel,
      Coupling coupling,
      long rangeMin,
      long rangeMax,
      enPicoProbeRangeInfo rangeType,
      double analogueOffset,
      BandwidthLimiter bandwidth);

    [DllImport(DriverName, EntryPoint = "psospaSetChannelOff")]
    public static extern StandardDriverStatusCode SetChannelOff(
      short handle,
      Channel channel);

        /// <summary>
        /// This function enables digital ports
        /// </summary>
        /// <param name="handle">The handle of the required device</param>
        /// <param name="port">The digital port to be enabled.</param>
        /// <param name="logicThresholdLevelVolts">Threshold for each port to determine whether the signal is high or low. (Measured in ADC counts).</param>
        /// <returns></returns>
        [DllImport(DriverName, EntryPoint = "psospaSetDigitalPortOn")]
    public static extern StandardDriverStatusCode SetDigitalPortOn(
      short handle,
      DigitalPort port,
      double logicThresholdLevelVolts);

    [DllImport(DriverName, EntryPoint = "psospaSetDigitalPortOff")]
    public static extern StandardDriverStatusCode SetDigitalPortOff(
      short handle,
      DigitalPort port);

    [DllImport(DriverName, EntryPoint = "psospaGetTimebase")]
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
    [DllImport(DriverName, EntryPoint = "psospaSigGenWaveform")]
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
    [DllImport(DriverName, EntryPoint = "psospaSigGenRange")]
    public static extern StandardDriverStatusCode SigGenRange(
      short handle,
      double peakToPeakVolts,
      double offsetVolts
    );

  [DllImport(DriverName, EntryPoint = "psospa")]
    public static extern StandardDriverStatusCode SigGenWaveformDutyCycle(int handle, double dutyCyclePercent);

   /// <summary>
    /// 
    /// </summary>
    /// <param name="handle">The handle of the required device</param>
    /// <param name="triggerType">The type of trigger that will be applied to the signal generator</param>
    /// <param name="triggerSource">The source that will trigger the signal generator.</param>
    /// <param name="cycles">The number of signals outputted by the signal generator after triggering.</param>
    /// <param name="autoTriggerPs">The time in pico seconds that the signal generator will automatically trigger.</param>
    [DllImport(DriverName, EntryPoint = "psospaSigGenTrigger")]
    public static extern StandardDriverStatusCode SigGenTrigger(
      short handle,
      SiggenTrigType triggerType,
      SiggenTrigSource triggerSource,
      ulong cycles,
      ulong autoTriggerPs
    );

    /// <summary>
    /// Defines the frequency for the signal generator.
    /// </summary>
    /// <param name="handle">The handle of the required device</param>
    /// <param name="frequencyHz">The frequency that the signal generator will produce.</param>
    [DllImport(DriverName, EntryPoint = "psospaSigGenFrequency")]
    public static extern StandardDriverStatusCode SigGenFrequency(
      short handle,
      double frequencyHz
    );

    [DllImport(DriverName, EntryPoint = "psospa")]
    public static extern StandardDriverStatusCode SigGenFrequencySweep(
        int handle, double stopFrequencyHz,
        double frequencyIncrement,
        double dwellTimeSeconds,
        SweepType sweepType);

    [DllImport(DriverName, EntryPoint = "psospa")]
    public static extern StandardDriverStatusCode SigGenPhase(int handle, ulong deltaPhase);

    [DllImport(DriverName, EntryPoint = "psospa")]
    public static extern StandardDriverStatusCode SigGenPhaseSweep(int handle, ulong stopDeltaPhase, ulong deltaPhaseIncrement, ulong dwellCount,
        SweepType sweepType);

    [DllImport(DriverName, EntryPoint = "psospaSigGenSoftwareTriggerControl")]
    public static extern StandardDriverStatusCode SigGenSoftwareTriggerControl(
      short handle,
      SiggenTrigType triggerState
    );

    /// <summary>
    /// This function enables or disables the signal generator.
    /// </summary>
    /// <param name="handle">The handle of the required device</param>
    /// <param name="sigGenEnabled">0 = Disabling the signal generator, 1 = Enabling the signal generator</param>
    /// <param name="sweepEnabled">0 = Sweep disabled, 1 = Sweep enabled</param>
    /// <param name="triggerEnabled">0 = Trigger enabled, 1 = Trigger disabled</param>
    /// <param name="frequency">The frequency that the signal generator will initially produce.</param>
    /// <param name="stopFrequency">The frequency at which the sweep reverses direction or returns to the initial frequency</param>
    /// <param name="frequencyIncrement">The amount of frequency increase or decrease in sweep mode</param>
    /// <param name="dwellTime">The time for which the sweep stays at each frequency, in seconds</param>
    [DllImport(DriverName, EntryPoint = "psospaSigGenApply")]
    public static extern StandardDriverStatusCode SigGenApply(
      short handle,
      short sigGenEnabled,
      short sweepEnabled,
      short triggerEnabled,
      out double frequency,
      out double stopFrequency,
      out double frequencyIncrement,
      out double dwellTime
    );

    [DllImport(DriverName, EntryPoint = "psospaSigGenLimits")]
    public static extern StandardDriverStatusCode SigGenLimits(int handle, SigGenParameter parameter,
        out double minimumPermissibleValue, out double maximumPermissibleValue, out double step);

    [DllImport(DriverName, EntryPoint = "psospaSigGenFrequencyLimits")]
    public static extern StandardDriverStatusCode SigGenFrequencyLimits(
        int handle,
        WaveType waveType,
        out ulong numSamples,
        out double minStopFrequencyOut,
        out double maxStopFrequencyOut,
        out double minFrequencyStepOut,
        out double maxFrequencyStepOut,
        out double minDwellTimeOut,
        out double maxDwellTimeOut);

    [DllImport(DriverName, EntryPoint = "psospaSigGenPause")]
    public static extern StandardDriverStatusCode SigGenPause(int handle);

        [DllImport(DriverName, EntryPoint = "psospaSigGenRestart")]
    public static extern StandardDriverStatusCode SigGenRestart(int handle);

    [DllImport(DriverName, EntryPoint = "psospaSetSimpleTrigger")]
    public static extern StandardDriverStatusCode SetSimpleTrigger(
        short handle,
        short enable,
        Channel channel,
        short threshold,
        TriggerDirection direction,
        ulong delay,
        uint autoTriggerMicroseconds);

    [DllImport(DriverName, EntryPoint = "psospaTriggerWithinPreTriggerSamples")]
    public static extern StandardDriverStatusCode TriggerWithinPreTriggerSamples(
      short handle,
      TriggerWithinPreTrigger state
    );

    [DllImport(DriverName, EntryPoint = "psospaSetTriggerChannelProperties")]
    public static extern StandardDriverStatusCode SetTriggerChannelProperties(
      short handle,
      TriggerChannelProperties[] channelProperties,
      short numChannelProperties,
      int autoTriggerMicroseconds);

    [DllImport(DriverName, EntryPoint = "psospaSetTriggerChannelConditions")]
    public static extern StandardDriverStatusCode SetTriggerChannelConditions(
      short handle,
      TriggerCondition[] conditions,
      short numConditions,
      Action action);

    [DllImport(DriverName, EntryPoint = "psospaSetTriggerChannelDirections")]
    public static extern StandardDriverStatusCode SetTriggerChannelDirections(
      short handle,
      ThresholdDirection[] directions,
      short numDirections);

    [DllImport(DriverName, EntryPoint = "psospaSetTriggerDelay")]
    public static extern StandardDriverStatusCode SetTriggerDelay(int handle, ulong delay);

    [DllImport(DriverName, EntryPoint = "psospaSetPulseWidthQualifierProperties")]
    public static extern StandardDriverStatusCode SetPulseWidthQualifierProperties(
      short handle,
      uint lower,
      uint upper,
      PulseWidthType type);

    [DllImport(DriverName, EntryPoint = "psospaSetPulseWidthQualifierConditions")]
    public static extern StandardDriverStatusCode SetPulseWidthQualifierConditions(
      short handle,
      TriggerCondition[] conditions,
      short nConditions,
      Action action);

    [DllImport(DriverName, EntryPoint = "psospaSetPulseWidthQualifierDirections")]
    public static extern StandardDriverStatusCode SetPulseWidthQualifierDirections(
      short handle,
      ThresholdDirection[] directions,
      short nDirections);

    [DllImport(DriverName, EntryPoint = "psospaSetTriggerDigitalPortProperties")]
    public static extern StandardDriverStatusCode SetTriggerDigitalPortProperties(
      short handle,
      Channel channel,
      DigitalChannelDirections[] digitalChannelDirections,
      short nDigitalChannelDirections);

    [DllImport(DriverName, EntryPoint = "psospaSetPulseWidthDigitalPortProperties")]
    public static extern StandardDriverStatusCode SetPulseWidthDigitalPortProperties(
      short handle,
      Channel channel,
      DigitalChannelDirections[] digitalChannelDirections,
      short nDigitalChannelDirections);

    [DllImport(DriverName, EntryPoint = "psospaGetTriggerTimeOffset")]
    public static extern StandardDriverStatusCode GetTriggerTimeOffset(int handle, out long time,
        out PicoTimeUnits timeUnits, ulong segmentIndex);

    [DllImport(DriverName, EntryPoint = "psospaGetValuesTriggerTimeOffsetBulk")]
    public static extern StandardDriverStatusCode GetValuesTriggerTimeOffsetBulk(int handle, out long[] times, out PicoTimeUnits[] timeUnits,
        ulong fromSegmentIndex, ulong toSegmentIndex);

    [DllImport(DriverName, EntryPoint = "psospaSetDataBuffer")]
    public static extern StandardDriverStatusCode SetDataBuffer(
      short handle,
      Channel channel,
      short[] bufferMax,
      ulong nSamples,
      DataType dataType,
      ulong waveform,
      RatioMode downSampleRatioMode,
      Action action);

    [DllImport(DriverName, EntryPoint = "psospaSetDataBuffers")]
    public static extern StandardDriverStatusCode SetDataBuffers(
      short handle,
      Channel channel,
      short[] bufferMax,
      short[] bufferMin,
      ulong nSamples,
      DataType dataType,
      ulong waveform,
      RatioMode downSampleRatioMode,
      Action action);

    [DllImport(DriverName, EntryPoint = "psospaRunBlock")]
    public static extern StandardDriverStatusCode RunBlock(short handle,
      ulong noOfPreTriggerSamples,
      ulong noOfPostTriggerSamples,
      uint timebase,
      out double timeIndisposedMs,
      ulong segmentIndex,
      DefinitionBlockReady lppsospaBlockReady,
      IntPtr pVoid);

    [DllImport(DriverName, EntryPoint = "psospaIsReady")]
    public static extern StandardDriverStatusCode IsReady(
      short handle,
      out short ready);

    [DllImport(DriverName, EntryPoint = "psospaRunStreaming")]
    public static extern StandardDriverStatusCode RunStreaming(short handle,
        out double sampleInterval,
        uint sampleIntervalTimeUnits,
        ulong maxPreTriggerSamples,
        ulong maxPostTriggerSamples,
        short autoStop,
        ulong downSampleRatio,
        RatioMode downSampleRatioMode);

    [DllImport(DriverName, EntryPoint = "psospaGetStreamingLatestValues")]
    public static extern StandardDriverStatusCode GetStreamingLatestValues(
        short handle,
        IntPtr streamingDataInfos,
        ulong nStreamingDataInfos,
        ref StreamingDataTriggerInfo triggerInfo);

    [DllImport(DriverName, EntryPoint = "psospaNoOfStreamingValues")]
    public static extern StandardDriverStatusCode NoOfStreamingValues(int handle, out ulong noOfValues);

    [DllImport(DriverName, EntryPoint = "psospaGetValues")]
    public static extern StandardDriverStatusCode GetValues(
      short handle,
      ulong startIndex,
      ref ulong noOfSamples,
      ulong downSampleRatio,
      RatioMode downSampleRatioMode,
      ulong segmentIndex,
      out short overflow);

    [DllImport(DriverName, EntryPoint = "psospaGetValuesBulk")]
    public static extern StandardDriverStatusCode GetValuesBulk(
        short handle,
        ulong startIndex,
        ref ulong noOfSamples,
        ulong fromSegmentIndex,
        ulong toSegmentIndex,
        ulong downSampleRatio,
        RatioMode downSampleRatioMode,
        out short overflow);

    [DllImport(DriverName, EntryPoint = "psospaGetValuesAsync")]
    public static extern StandardDriverStatusCode GetValuesAsync(
        int handle, ulong startIndex, ulong noOfSamples,
        ulong downSampleRatio, RatioMode downSampleRatioMode,
        ulong segmentIndex, IntPtr lpDataReady, IntPtr pParameter);

    [DllImport(DriverName, EntryPoint = "psospaGetValuesBulkAsync")]
    public static extern StandardDriverStatusCode GetValuesBulkAsync(
        int handle, ulong startIndex, ulong noOfSamples, ulong fromSegmentIndex, ulong toSegmentIndex,
        ulong downSampleRatio, RatioMode downSampleRatioMode, IntPtr lpDataReady, IntPtr pParameter);

    [DllImport(DriverName, EntryPoint = "psospaGetValuesOverlapped")]
    public static extern StandardDriverStatusCode GetValuesOverlapped(
        int handle, ulong startIndex, out ulong noOfSamples,
        ulong downSampleRatio, RatioMode downSampleRatioMode,
        ulong fromSegmentIndex, ulong toSegmentIndex, out short overflow);

    [DllImport(DriverName, EntryPoint = "psospaStopUsingGetValuesOverlapped")]
      public static extern StandardDriverStatusCode StopUsingGetValuesOverlapped(
      short handle);

    [DllImport(DriverName, EntryPoint = "psospaGetNoOfCaptures")]
      public static extern StandardDriverStatusCode GetNoOfCaptures(
      short handle,
      out ulong nCaptures);

    [DllImport(DriverName, EntryPoint = "psospaGetNoOfProcessedCaptures")]
      public static extern StandardDriverStatusCode GetNoOfProcessedCaptures(
      short handle,
      out ulong nProcessedCaptures);

    [DllImport(DriverName, EntryPoint = "psospaStop")]
    public static extern StandardDriverStatusCode Stop(
      short handle);

    [DllImport(DriverName, EntryPoint = "psospaSetNoOfCaptures")]
    public static extern StandardDriverStatusCode SetNoOfCaptures(
      short handle,
      ulong nCaptures);

    [DllImport(DriverName, EntryPoint = "psospaGetTriggerInfo")]
    public static extern StandardDriverStatusCode GetTriggerInfo(
      short handle,
      out TriggerInfo triggerInfo,
      ulong firstSegmentIndex,
      ulong segmentCount);

    [DllImport(DriverName, EntryPoint = "psospaEnumerateUnits")]
    public static extern StandardDriverStatusCode EnumerateUnits(
      out short count,
      StringBuilder serials,
      out short serialLength);

   [DllImport(DriverName, EntryPoint = "psospaPingUnit")]
    public static extern StandardDriverStatusCode PingUnit(
      short handle);

    [DllImport(DriverName, EntryPoint = "psospaGetAnalogueOffsetLimits")]
    public static extern StandardDriverStatusCode GetAnalogueOffsetLimits(int handle, PicoConnectProbes.PicoConnectProbes.PicoConnectProbeRange range,
        Coupling coupling, out double maximumVoltage, out double minimumVoltage);

    [DllImport(DriverName, EntryPoint = "psospaGetMinimumTimebaseStateless")]
    public static extern StandardDriverStatusCode GetMinimumTimebaseStateless(int handle, EnabledChannelsAndPorts enabledChannelFlags,
        out uint timebase, out double timeInterval, DeviceResolution resolution);

    [DllImport(DriverName, EntryPoint = "psospaNearestSampleIntervalStateless")]
    public static extern StandardDriverStatusCode NearestSampleIntervalStateless(
      short handle,
      EnabledChannelsAndPorts enabledChannelFlags,
      double timeIntervalRequested,
      byte roundFaster,
      DeviceResolution resolution,
      out uint timebase,
      out double timeIntervalAvailable);

    [DllImport(DriverName, EntryPoint = "psospaSetDeviceResolution")]
    public static extern StandardDriverStatusCode SetDeviceResolution(int handle, DeviceResolution resolution);

    [DllImport(DriverName, EntryPoint = "psospaGetDeviceResolution")]
    public static extern StandardDriverStatusCode GetDeviceResolution(int handle, DeviceResolution resolution);

    [DllImport(DriverName, EntryPoint = "psospaQueryOutputEdgeDetect")]
    public static extern StandardDriverStatusCode QueryOutputEdgeDetect(int handle, out short state);

    [DllImport(DriverName, EntryPoint = "psospaSetOutputEdgeDetect")]
    public static extern StandardDriverStatusCode SetOutputEdgeDetect(int handle, short state);

    [DllImport(DriverName, EntryPoint = "psospaGetScalingValues")]
    public static extern StandardDriverStatusCode GetScalingValues(int handle, out ScalingFactors scalingValues, int nChannels);

    [DllImport(DriverName, EntryPoint = "psospaGetAdcLimits")]
      public static extern StandardDriverStatusCode GetAdcLimits(
      short handle,
      DeviceResolution resolution,
      out short minValue,
      out short maxValue);
        
    [DllImport(DriverName, EntryPoint = "psospaResetChannelsAndReportAllChannelsOvervoltageTripStatus")]
      public static extern StandardDriverStatusCode ResetChannelsAndReportAllChannelsOvervoltageTripStatus(
      short handle,
      out ChannelOvervoltageTripped allChannelsTrippedStatus,
      byte nChannelTrippedStatus);

    [DllImport(DriverName, EntryPoint = "psospaReportAllChannelsOvervoltageTripStatus")]
      public static extern StandardDriverStatusCode ReportAllChannelsOvervoltageTripStatus(
      short handle,
      out ChannelOvervoltageTripped allChannelsTrippedStatus,
      byte nChannelTrippedStatus); 

    [DllImport(DriverName, EntryPoint = "psospaSetDigitalPortInteractionCallback")]
    public static extern StandardDriverStatusCode SetDigitalPortInteractionCallback(
      short handle,
      DigitalPortCallback callback
    );
  }
}