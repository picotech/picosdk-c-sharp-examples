/******************************************************************************
 * 
 *  Filename: PS4000AImports.cs
 *
 *  Description:
 *    This file contains .NET wrapper calls for the ps4000a driver API
 *    functions. It also has the enums and structs required by the 
 *    (wrapped) function calls.
 *   
 *  Copyright (C) 2014 - 2017 Pico Technology Ltd. See LICENSE file for terms.
 *
 ******************************************************************************/

using System;
using System.Runtime.InteropServices;
using System.Text;

namespace PS4000AImports
{
    class Imports
    {
        #region constants
        private const string _DRIVER_FILENAME = "ps4000a.dll";

        public const int MaxValue = 32767;

        public const int QUAD_SCOPE = 4; // PicoScope with 4 channels
        public const int OCTO_SCOPE = 8; // PicoScope with 8 channels

        public const int SIG_GEN_BUFFER_SIZE = 16384;

        public const Int64 AWG_PHASE_ACCUMULATOR = 4294967296;

        public const int AWG_DAC_FREQUENCY = 80000000;

        #endregion

        #region Driver enums

        public enum Coupling : int
        {
            AC,
            DC
        }

        public enum Channel : int
        {
            CHANNEL_A,
            CHANNEL_B,
            CHANNEL_C,
            CHANNEL_D,
            MAX_4_CHANNELS,
            CHANNEL_E = MAX_4_CHANNELS,
            CHANNEL_F,
            CHANNEL_G,
            CHANNEL_H,
            EXTERNAL,
            MAX_CHANNELS = EXTERNAL,
            TRIGGER_AUX,
            MAX_TRIGGER_SOURCES,
            PULSE_WIDTH_SOURCE = 0x10000000
        }

        public enum Range : int
        {
            Range_10MV,
            Range_20MV,
            Range_50MV,
            Range_100MV,
            Range_200MV,
            Range_500MV,
            Range_1V,
            Range_2V,
            Range_5V,
            Range_10V,
            Range_20V,
            Range_50V,
            Range_100V,
            Range_200V
        }

        public enum DeviceResolution : int
        {
            PS4000A_DR_8BIT,
            PS4000A_DR_12BIT,
            PS4000A_DR_14BIT,
            PS4000A_DR_15BIT,
            PS4000A_DR_16BIT
        }

        public enum ReportedTimeUnits : int
        {
            FemtoSeconds,
            PicoSeconds,
            NanoSeconds,
            MicroSeconds,
            MilliSeconds,
            Seconds
        }

        public enum ThresholdMode : int
        {
            Level,
            Window
        }

        public enum ThresholdDirection : int
        {
            // Values for level threshold mode
            //
            Above,              //using upper threshold
            Below,
            Rising,             // using upper threshold
            Falling,            // using upper threshold
            RisingOrFalling,    // using both thresholds
            AboveLower,         // using lower threshold
            BelowLower,         // using lower threshold
            RisingLower,        // using upper threshold
            FallingLower,       // using upper threshold

            // Values for window threshold mode
            //
            Inside = Above,
            Outside = Below,
            Enter = Rising,
            Exit = Falling,
            EnterOrExit = RisingOrFalling,
            PositiveRunt = 9,
            NegativeRunt,
            None = Rising
        }

        public enum DownSamplingMode : int
        {
            None = 0,
            Aggregate = 1,
            Decimate = 2,
            Average = 4
        }

        public enum PulseWidthType : int
        {
            None,
            LessThan,
            GreaterThan,
            InRange,
            OutOfRange
        }

        public enum TriggerState : int
        {
            DontCare,
            True,
            False
        }

        public enum Model : int
        {
            NONE = 0,
            PS4444 = 4444,
            PS4824 = 4824
        }

        public enum ConditionsInfo : int
        {
            Clear = 1,
            Add = 2
        }

        public enum IndexMode : int
        {
            PS4000A_SINGLE,
            PS4000A_DUAL,
            PS4000A_QUAD,
            PS4000A_MAX_INDEX_MODES
        }

        public enum WaveType : int
        {
            PS4000A_SINE,
            PS4000A_SQUARE,
            PS4000A_TRIANGLE,
            PS4000A_RAMP_UP,
            PS4000A_RAMP_DOWN,
            PS4000A_SINC,
            PS4000A_GAUSSIAN,
            PS4000A_HALF_SINE,
            PS4000A_DC_VOLTAGE,
            PS4000A_WHITE_NOISE,
            PS4000A_MAX_WAVE_TYPES
        }

        public enum SweepType : int
        {
            PS4000A_UP,
            PS4000A_DOWN,
            PS4000A_UPDOWN,
            PS4000A_DOWNUP,
            PS4000A_MAX_SWEEP_TYPES
        }

        public enum ExtraOperations : int
        {
            PS4000A_ES_OFF,
            PS4000A_WHITENOISE,
            PS4000A_PRBS // Pseudo-Random Bit Stream 
        }

        public enum SigGenTrigType : int
        {
            PS4000A_SIGGEN_RISING,
            PS4000A_SIGGEN_FALLING,
            PS4000A_SIGGEN_GATE_HIGH,
            PS4000A_SIGGEN_GATE_LOW
        }

        public enum SigGenTrigSource : int
        {
            PS4000A_SIGGEN_NONE,
            PS4000A_SIGGEN_SCOPE_TRIG,
            PS4000A_SIGGEN_AUX_IN,
            PS4000A_SIGGEN_EXT_IN,
            PS4000A_SIGGEN_SOFT_TRIG
        }

        public enum BandwidthLimiterFlags : int
        {
            PS4000A_BW_FULL_FLAG = (1 << 0),
            PS4000A_BW_20KHZ_FLAG = (1 << 1),
            PS4000A_BW_100KHZ_FLAG = (1 << 2),  //( default when current clamp detected: can be changed)
            PS4000A_BW_1MHZ_FLAG = (1 << 3)     //( default for 14 bits: can be changed)
        }

        public enum BandwidthLimiter : int
        {
            PS4000A_BW_FULL,
            PS4000A_BW_20KHZ,
            PS4000A_BW_100KHZ, // (default when current clamp detected: can be changed)
            PS4000A_BW_1MHZ
        }

        public enum ResistanceRange : int
        {
            PS4000A_RESISTANCE_315K = 0x00000200,
            PS4000A_RESISTANCE_1100K,
            PS4000A_RESISTANCE_10M,
            PS4000A_MAX_RESISTANCE_RANGES = (PS4000A_RESISTANCE_10M + 1) - PS4000A_RESISTANCE_315K,
            PS4000A_RESISTANCE_ADCV = 0x10000000
        }

        public enum PinStates : int
        {
            PS4000A_CAL_PINS_OFF,
            PS4000A_GND_SIGNAL,
            PS4000A_SIGNAL_SIGNAL
        }

        #endregion

        // Structures

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct TriggerChannelProperties
        {
            public Int16 ThresholdMajor;
            public UInt16 HysteresisMajor;
            public Int16 ThresholdMinor;
            public UInt16 HysteresisMinor;
            public Channel Channel;
            public ThresholdMode ThresholdMode;


            public TriggerChannelProperties(
                Int16 thresholdMajor,
                UInt16 hysteresisMajor,
                Int16 thresholdMinor,
                UInt16 hysteresisMinor,
                Channel channel,
                ThresholdMode thresholdMode)
            {
                this.ThresholdMajor = thresholdMajor;
                this.HysteresisMajor = hysteresisMajor;
                this.ThresholdMinor = thresholdMinor;
                this.HysteresisMinor = hysteresisMinor;
                this.Channel = channel;
                this.ThresholdMode = thresholdMode;
            }
        }



        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct TriggerConditions
        {
            public Channel Source;
            public TriggerState Condition;

            public TriggerConditions(
                Channel source,
                TriggerState condition)
            {
                this.Source = source;
                this.Condition = condition;
            }
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct TriggerDirections
        {
            public Channel Source;
            public ThresholdDirection Direction;

            public TriggerDirections(
                Channel source,
                ThresholdDirection direction)
            {
                this.Source = source;
                this.Direction = direction;
            }
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct PwqConditions
        {
            public TriggerState ChannelA;
            public TriggerState ChannelB;
            public TriggerState ChannelC;
            public TriggerState ChannelD;
            public TriggerState ChannelE;
            public TriggerState ChannelF;
            public TriggerState ChannelG;
            public TriggerState ChannelH;
            public TriggerState External;
            public TriggerState Aux;

            public PwqConditions(
                TriggerState channelA,
                TriggerState channelB,
                TriggerState channelC,
                TriggerState channelD,
                TriggerState channelE,
                TriggerState channelF,
                TriggerState channelG,
                TriggerState channelH,
                TriggerState external,
                TriggerState aux)
            {
                this.ChannelA = channelA;
                this.ChannelB = channelB;
                this.ChannelC = channelC;
                this.ChannelD = channelD;
                this.ChannelE = channelE;
                this.ChannelF = channelF;
                this.ChannelG = channelG;
                this.ChannelH = channelH;
                this.External = external;
                this.Aux = aux;
            }
        }

        #region Driver Imports
        #region Callback delegates
        public delegate void ps4000aBlockReady(short handle, short status, IntPtr pVoid);

        public delegate void ps4000aStreamingReady(
                                                short handle,
                                                int noOfSamples,
                                                uint startIndex,
                                                short overflow,
                                                uint triggerAt,
                                                short triggered,
                                                short autoStop,
                                                IntPtr pVoid);

        public delegate void ps4000aDataReady(
                                                short handle,
                                                UInt32 status,
                                                int noOfSamples,
                                                short overflow,
                                                IntPtr pVoid);
        #endregion

        [DllImport(_DRIVER_FILENAME, EntryPoint = "ps4000aOpenUnit")]
        public static extern UInt32 OpenUnit(out short handle, StringBuilder serial);

        [DllImport(_DRIVER_FILENAME, EntryPoint = "ps4000aOpenUnitWithResolution")]
        public static extern UInt32 OpenUnitWithResolution(out short handle, StringBuilder serial, DeviceResolution resolution);

        [DllImport(_DRIVER_FILENAME, EntryPoint = "ps4000aCloseUnit")]
        public static extern UInt32 CloseUnit(short handle);

        [DllImport(_DRIVER_FILENAME, EntryPoint = "ps4000aRunBlock")]
        public static extern UInt32 RunBlock(
                                                short handle,
                                                int noOfPreTriggerSamples,
                                                int noOfPostTriggerSamples,
                                                uint timebase,
                                                out int timeIndisposedMs,
                                                uint segmentIndex,
                                                ps4000aBlockReady lpps4000aBlockReady,
                                                IntPtr pVoid);

        [DllImport(_DRIVER_FILENAME, EntryPoint = "ps4000aChangePowerSource")]
        public static extern UInt32 ps4000aChangePowerSource(short handle, UInt32 status);

        [DllImport(_DRIVER_FILENAME, EntryPoint = "ps4000aStop")]
        public static extern UInt32 Stop(short handle);

        [DllImport(_DRIVER_FILENAME, EntryPoint = "ps4000aSetChannel")]
        public static extern UInt32 SetChannel(
                                                short handle,
                                                Channel channel,
                                                short enabled,
                                                short dc,
                                                Range range,
                                                float analogOffset);

        [DllImport(_DRIVER_FILENAME, EntryPoint = "ps4000aSetDataBuffer")]
        public static extern UInt32 SetDataBuffer(
                                                    short handle,
                                                    Channel channel,
                                                    short[] buffer,
                                                    int bufferLth,
                                                    uint segmentIndex,
                                                    DownSamplingMode RatioMode);

        [DllImport(_DRIVER_FILENAME, EntryPoint = "ps4000aSetDataBuffers")]
        public static extern UInt32 SetDataBuffers(
                                                    short handle,
                                                    Channel channel,
                                                    short[] bufferMax,
                                                    short[] bufferMin,
                                                    int bufferLth,
                                                    uint segmentIndex,
                                                    DownSamplingMode RatioMode);

        [DllImport(_DRIVER_FILENAME, EntryPoint = "ps4000aSetTriggerChannelDirections")]
        public static extern UInt32 SetTriggerChannelDirections(
                                                short handle,
                                                TriggerDirections[] directions,
                                                short nDirections);

        [DllImport(_DRIVER_FILENAME, EntryPoint = "ps4000aGetTimebase")]
        public static extern UInt32 GetTimebase(
                                                 short handle,
                                                 uint timebase,
                                                 int noSamples,
                                                 out int timeIntervalNanoseconds,
                                                 out int maxSamples,
                                                 uint segmentIndex);

        [DllImport(_DRIVER_FILENAME, EntryPoint = "ps4000aGetValues")]
        public static extern UInt32 GetValues(
                                                short handle,
                                                uint startIndex,
                                                ref uint noOfSamples,
                                                uint downSampleRatio,
                                                DownSamplingMode downSampleRatioMode,
                                                uint segmentIndex,
                                                out short overflow);

        [DllImport(_DRIVER_FILENAME, EntryPoint = "ps4000aSetTriggerChannelProperties")]
        public static extern UInt32 SetTriggerChannelProperties(
                                                                    short handle,
                                                                    TriggerChannelProperties[] channelProperties,
                                                                    short numChannelProperties,
                                                                    short auxOutputEnable,
                                                                    int autoTriggerMilliseconds);

        [DllImport(_DRIVER_FILENAME, EntryPoint = "ps4000aSetTriggerChannelConditions")]
        public static extern UInt32 SetTriggerChannelConditions(
                                                                    short handle,
                                                                    TriggerConditions[] conditions,
                                                                    short numConditions,
                                                                    ConditionsInfo info);

        [DllImport(_DRIVER_FILENAME, EntryPoint = "ps4000aSetTriggerDelay")]
        public static extern UInt32 SetTriggerDelay(short handle, uint delay);

        [DllImport(_DRIVER_FILENAME, EntryPoint = "ps4000aSetPulseWidthQualifierConditions")]
        public static extern UInt32 SetPulseWidthQualifierConditions(short handle,
                                                    TriggerConditions[] conditions,
                                                    short numConditions,
                                                    ConditionsInfo info);

        [DllImport(_DRIVER_FILENAME, EntryPoint = "ps4000aSetPulseWidthQualifierProperties")]
        public static extern UInt32 SetPulseWidthQualifierProperties(short handle,
                                                    ThresholdDirection direction,
                                                    uint lower,
                                                    uint upper,
                                                    PulseWidthType type);

        [DllImport(_DRIVER_FILENAME, EntryPoint = "ps4000aIsTriggerOrPulseWidthQualifierEnabled")]
        public static extern UInt32 IsTriggerOrPulseWidthQualifierEnabled(short handle, out short triggerEnabled, out short pulseWidthQualifierEnabled);

        [DllImport(_DRIVER_FILENAME, EntryPoint = "ps4000aSetSimpleTrigger")]
        public static extern UInt32 SetSimpleTrigger(
                                                        short handle,
                                                        short enable,
                                                        Channel channel,
                                                        short threshold,
                                                        ThresholdDirection direction,
                                                        uint delay,
                                                        short autotrigger_ms);

        [DllImport(_DRIVER_FILENAME, EntryPoint = "ps4000aGetUnitInfo")]
        public static extern UInt32 GetUnitInfo(short handle, StringBuilder infoString, short stringLength, out short requiredSize, uint info);

        [DllImport(_DRIVER_FILENAME, EntryPoint = "ps4000aRunStreaming")]
        public static extern UInt32 RunStreaming(
                                                    short handle,
                                                    ref uint sampleInterval,
                                                    ReportedTimeUnits sampleIntervalTimeUnits,
                                                    uint maxPreTriggerSamples,
                                                    uint maxPostPreTriggerSamples,
                                                    short autoStop,
                                                    uint downSampleRatio,
                                                    DownSamplingMode downSamplingRatioMode,
                                                    uint overviewBufferSize);

        [DllImport(_DRIVER_FILENAME, EntryPoint = "ps4000aGetStreamingLatestValues")]
        public static extern UInt32 GetStreamingLatestValues(
                                                                short handle,
                                                                ps4000aStreamingReady lpps4000aStreamingReady,
                                                                IntPtr pVoid);

        [DllImport(_DRIVER_FILENAME, EntryPoint = "ps4000aSetNoOfCaptures")]
        public static extern UInt32 SetNoOfRapidCaptures(
                                                            short handle,
                                                            uint nWaveforms);

        [DllImport(_DRIVER_FILENAME, EntryPoint = "ps4000aMemorySegments")]
        public static extern UInt32 MemorySegments(
                                                    short handle,
                                                    uint nSegments,
                                                    out int nMaxSamples);

        [DllImport(_DRIVER_FILENAME, EntryPoint = "ps4000aGetValuesBulk")]
        public static extern UInt32 GetValuesRapid(
                                                    short handle,
                                                    ref uint noOfSamples,
                                                    uint fromSegmentIndex,
                                                    uint toSegmentIndex,
                                                    int downSampleRatio,
                                                    DownSamplingMode downsampleratiomode,
                                                    short[] overflows);

        [DllImport(_DRIVER_FILENAME, EntryPoint = "ps4000aMaximumValue")]
        public static extern UInt32 MaximumValue(short handle, out short value);

        [DllImport(_DRIVER_FILENAME, EntryPoint = "ps4000aSetSigGenArbitrary")]
        public static extern UInt32 SetSigGenArbitrary(
                                                        short handle,
                                                        int offsetVoltage,
                                                        uint pkTopk,
                                                        uint startDeltaPhase,
                                                        uint stopDeltaPhase,
                                                        uint deltaPhaseIncrement,
                                                        uint dwellCount,
                                                        ref short arbitaryWaveform,
                                                        int arbitaryWaveformSize,
                                                        SweepType sweepType,
                                                        ExtraOperations operation,
                                                        IndexMode indexMode,
                                                        uint shots,
                                                        uint sweeps,
                                                        SigGenTrigType triggerType,
                                                        SigGenTrigSource triggerSource,
                                                        short extInThreshold);

        [DllImport(_DRIVER_FILENAME, EntryPoint = "ps4000aSetSigGenBuiltIn")]
        public static extern UInt32 SetSigGenBuiltIn(
                                                        short handle,
                                                        int offsetVoltage,
                                                        uint pkTopk,
                                                        WaveType waveType,
                                                        double startFrequency,
                                                        double stopFrequency,
                                                        double increment,
                                                        double dwellTime,
                                                        SweepType sweepType,
                                                        ExtraOperations operation,
                                                        uint shots,
                                                        uint sweeps,
                                                        SigGenTrigType triggerType,
                                                        SigGenTrigSource triggerSource,
                                                        short extInThreshold);

        [DllImport(_DRIVER_FILENAME, EntryPoint = "ps4000aSigGenFrequencyToPhase")]
        public static extern UInt32 SigGenFrequencyToPhase(
                                                                short handle,
                                                                double frequency,
                                                                IndexMode indexMode,
                                                                uint bufferLength,
                                                                ref uint phase);

        [DllImport(_DRIVER_FILENAME, EntryPoint = "ps4000aEnumerateUnits")]
        public static extern UInt32 EnumerateUnits(
                                                    out short count,
                                                    StringBuilder serials,
                                                    ref short serialLength);

        [DllImport(_DRIVER_FILENAME, EntryPoint = "ps4000aSetDeviceResolution")]
        public static extern UInt32 SetDeviceResolution(short handle, DeviceResolution resolution);

        [DllImport(_DRIVER_FILENAME, EntryPoint = "ps4000aGetDeviceResolution")]
        public static extern UInt32 GetDeviceResolution(short handle, out DeviceResolution resolution);

        [DllImport(_DRIVER_FILENAME, EntryPoint = "ps4000aSetCalibrationPins")]
        public static extern UInt32 SetCalibrationPins(
                                                        short handle,
                                                        PinStates pinStates,
                                                        WaveType waveType,
                                                        double frequency,
                                                        uint amplitude,
                                                        uint offset);

        [DllImport(_DRIVER_FILENAME, EntryPoint = "ps4000aGetCommonModeOverflow")]
        public static extern UInt32 GetCommonModeOverflow(short handle, ref ushort overflow);

        #endregion
    }
}
