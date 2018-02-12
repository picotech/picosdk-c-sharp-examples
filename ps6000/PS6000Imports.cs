/******************************************************************************
*
* Filename: PS6000Imports.cs
*  
* Description:
*  This file contains .NET wrapper calls corresponding to function calls 
*  defined in the ps6000Api.h C header file. 
*  It also has the enums and structs required by the (wrapped) function calls.
*   
* Copyright © 2010-2018 Pico Technology Ltd. See LICENSE file for terms.
*
******************************************************************************/

using System;
using System.Runtime.InteropServices;
using System.Text;

namespace PS6000Imports
{
	class Imports
	{
		#region Constants
		private const string _DRIVER_FILENAME = "ps6000.dll";

		public const int MaxValue = 32512;

        public const uint PICO_OK = 0;
        
		#endregion

		#region Driver Enums

		public enum Channel : int
		{
			ChannelA,
			ChannelB,
			ChannelC,
			ChannelD,
			External,
			Aux,
			None,
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
		}

		public enum ReportedTimeUnits : int
		{
			FemtoSeconds,
			PicoSeconds,
			NanoSeconds,
			MicroSeconds,
			MilliSeconds,
			Seconds,
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
			Above,
			Below,
			Rising,
			Falling,
			RisingOrFalling,

			// Values for window threshold mode
			//
			Inside = Above,
			Outside = Below,
			Enter = Rising,
			Exit = Falling,
			EnterOrExit = RisingOrFalling,

			None = Rising,
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

		public enum PS6000DownSampleRatioMode : int
		{
			PS6000_RATIO_MODE_NONE,
			PS6000_RATIO_MODE_AGGREGATE,
			PS6000_RATIO_MODE_AVERAGE,
			PS6000_RATIO_MODE_DECIMATE,
			PS6000_RATIO_MODE_DISTRIBUTION
		}

		public enum PS6000BandwidthLimiter : int
		{
			PS6000_BW_FULL,
			PS6000_BW_20MHZ
		}

		public enum PS6000Coupling : int
		{
			PS6000_AC,
			PS6000_DC_1M,
			PS6000_DC_50R
		}

        public enum PS6000ExtraOperations : int
        {
	        PS6000_ES_OFF,
	        PS6000_WHITENOISE,
	        PS6000_PRBS // Pseudo-Random Bit Stream 
        }

        public enum IndexMode : int
        {
            PS6000_SINGLE,
            PS6000_DUAL,
            PS6000_QUAD,
            PS6000_MAX_INDEX_MODES
        }

        public enum WaveType : int
        {
            PS6000_SINE,
            PS6000_SQUARE,
            PS6000_TRIANGLE,
            PS6000_RAMP_UP,
            PS6000_RAMP_DOWN,
            PS6000_SINC,
            PS6000_GAUSSIAN,
            PS6000_HALF_SINE,
            PS6000_DC_VOLTAGE,
            PS6000_WHITE_NOISE,
            PS6000_MAX_WAVE_TYPES
        }

        public enum SweepType : int
        {
            PS6000_UP,
            PS6000_DOWN,
            PS6000_UPDOWN,
            PS6000_DOWNUP,
            PS6000_MAX_SWEEP_TYPES
        }

        public enum SigGenTrigType : int
        {
            PS6000_SIGGEN_RISING,
            PS6000_SIGGEN_FALLING,
            PS6000_SIGGEN_GATE_HIGH,
            PS6000_SIGGEN_GATE_LOW
        }

        public enum SigGenTrigSource : int
        {
            PS6000_SIGGEN_NONE,
            PS6000_SIGGEN_SCOPE_TRIG,
            PS6000_SIGGEN_AUX_IN,
            PS6000_SIGGEN_EXT_IN,
            PS6000_SIGGEN_SOFT_TRIG
        }

        #endregion

        #region Driver Structs
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
		public struct TriggerChannelProperties
		{
			public short ThresholdMajor;
			public ushort HysteresisMajor;
			public short ThresholdMinor;
			public ushort HysteresisMinor;
			public Channel Channel;
			public ThresholdMode ThresholdMode;


			public TriggerChannelProperties(
				short thresholdMajor,
				ushort hysteresisMajor,
				short thresholdMinor,
				ushort hysteresisMinor,
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
			public TriggerState ChannelA;
			public TriggerState ChannelB;
			public TriggerState ChannelC;
			public TriggerState ChannelD;
			public TriggerState External;
			public TriggerState Aux;
			public TriggerState Pwq;

			public TriggerConditions(
				TriggerState channelA,
				TriggerState channelB,
				TriggerState channelC,
				TriggerState channelD,
				TriggerState external,
				TriggerState aux,
				TriggerState pwq)
			{
				this.ChannelA = channelA;
				this.ChannelB = channelB;
				this.ChannelC = channelC;
				this.ChannelD = channelD;
				this.External = external;
				this.Aux = aux;
				this.Pwq = pwq;
			}
		}

		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		public struct PwqConditions
		{
			public TriggerState ChannelA;
			public TriggerState ChannelB;
			public TriggerState ChannelC;
			public TriggerState ChannelD;
			public TriggerState External;
			public TriggerState Aux;

			public PwqConditions(
				TriggerState channelA,
				TriggerState channelB,
				TriggerState channelC,
				TriggerState channelD,
				TriggerState external,
				TriggerState aux)
			{
				this.ChannelA = channelA;
				this.ChannelB = channelB;
				this.ChannelC = channelC;
				this.ChannelD = channelD;
				this.External = external;
				this.Aux = aux;
			}
		}
		#endregion

		#region Driver Imports
		#region Callback delegates
		public delegate void ps6000BlockReady(short handle, uint status, IntPtr pVoid);

		public delegate void ps6000StreamingReady(
												short handle,
												int noOfSamples,
												uint startIndex,
												short overflow,
												uint triggerAt,
												short triggered,
												short autoStop,
												IntPtr pVoid);

		public delegate void ps6000DataReady(
												short handle,
												int noOfSamples,
												short overflow,
												uint triggerAt,
												short triggered,
												IntPtr pVoid);
		#endregion

		[DllImport(_DRIVER_FILENAME, EntryPoint = "ps6000OpenUnit")]
        public static extern UInt32 OpenUnit(out short handle, StringBuilder serial);

		[DllImport(_DRIVER_FILENAME, EntryPoint = "ps6000CloseUnit")]
        public static extern UInt32 CloseUnit(short handle);

		[DllImport(_DRIVER_FILENAME, EntryPoint = "ps6000RunBlock")]
        public static extern UInt32 RunBlock(
												short handle,
												uint noOfPreTriggerSamples,
												uint noOfPostTriggerSamples,
												uint timebase,
												short oversample,
												out int timeIndisposedMs,
												uint segmentIndex,
												ps6000BlockReady lpps6000BlockReady,
												IntPtr pVoid);

		[DllImport(_DRIVER_FILENAME, EntryPoint = "ps6000Stop")]
        public static extern UInt32 Stop(short handle);

		[DllImport(_DRIVER_FILENAME, EntryPoint = "ps6000SetChannel")]
        public static extern UInt32 SetChannel(
												short handle,
												Channel channel,
												short enabled,
												PS6000Coupling coupling,
												Range range,
												float analogueOffset,
												PS6000BandwidthLimiter bandwidth);

		[DllImport(_DRIVER_FILENAME, EntryPoint = "ps6000SetDataBuffers")]
        public static extern UInt32 SetDataBuffers(
												    short handle,
												    Channel channel,
												    short[] bufferMax,
												    short[] bufferMin,
												    uint bufferLth,
												    PS6000DownSampleRatioMode mode);

        [DllImport(_DRIVER_FILENAME, EntryPoint = "ps6000SetDataBuffer")]
        public static extern UInt32 SetDataBuffer(
                                                    short handle,
                                                    Channel channel,
                                                    short[] buffer,
                                                    uint bufferLth,
                                                    PS6000DownSampleRatioMode mode);

        [DllImport(_DRIVER_FILENAME, EntryPoint = "ps6000SetSimpleTrigger")]
        public static extern UInt32 SetSimpleTrigger(
                                                        short handle,
                                                        short enabled,
                                                        Channel channel,
                                                        short threshold,
                                                        ThresholdDirection direction,
                                                        uint delay,
                                                        short autoTriggerMs);

		[DllImport(_DRIVER_FILENAME, EntryPoint = "ps6000SetTriggerChannelDirections")]
        public static extern UInt32 SetTriggerChannelDirections(
												                    short handle,
												                    ThresholdDirection channelA,
												                    ThresholdDirection channelB,
												                    ThresholdDirection channelC,
												                    ThresholdDirection channelD,
												                    ThresholdDirection ext,
												                    ThresholdDirection aux);

		[DllImport(_DRIVER_FILENAME, EntryPoint = "ps6000GetTimebase")]
        public static extern UInt32 GetTimebase(
											        short handle,
											        uint timebase,
											        uint noSamples,
											        out int timeIntervalNanoseconds,
											        short oversample,
											        out uint maxSamples,
											        uint segmentIndex);

		[DllImport(_DRIVER_FILENAME, EntryPoint = "ps6000GetValues")]
        public static extern UInt32 GetValues(
				                                short handle,
				                                uint startIndex,
				                                ref uint noOfSamples,
				                                uint downSampleRatio,
				                                PS6000DownSampleRatioMode mode,
				                                uint segmentIndex,
				                                out short overflow);

		[DllImport(_DRIVER_FILENAME, EntryPoint = "ps6000SetPulseWidthQualifier")]
        public static extern UInt32 SetPulseWidthQualifier(
			                                                short handle,
			                                                PwqConditions[] conditions,
			                                                short numConditions,
			                                                ThresholdDirection direction,
			                                                uint lower,
			                                                uint upper,
			                                                PulseWidthType type);

		[DllImport(_DRIVER_FILENAME, EntryPoint = "ps6000SetTriggerChannelProperties")]
        public static extern UInt32 SetTriggerChannelProperties(
			short handle,
			TriggerChannelProperties[] channelProperties,
			short numChannelProperties,
			short auxOutputEnable,
			int autoTriggerMilliseconds);

		[DllImport(_DRIVER_FILENAME, EntryPoint = "ps6000SetTriggerChannelConditions")]
        public static extern UInt32 SetTriggerChannelConditions(
			short handle,
			TriggerConditions[] conditions,
			short numConditions);

		[DllImport(_DRIVER_FILENAME, EntryPoint = "ps6000SetTriggerDelay")]
        public static extern UInt32 SetTriggerDelay(short handle, uint delay);

		[DllImport(_DRIVER_FILENAME, EntryPoint = "ps6000GetUnitInfo")]
        public static extern UInt32 GetUnitInfo(short handle, StringBuilder infoString, short stringLength, out short requiredSize, uint info);

		[DllImport(_DRIVER_FILENAME, EntryPoint = "ps6000RunStreaming")]
        public static extern UInt32 RunStreaming(
			short handle,
			ref uint sampleInterval,
			ReportedTimeUnits sampleIntervalTimeUnits,
			uint maxPreTriggerSamples,
			uint maxPostPreTriggerSamples,
			short autoStop,
			uint downSamplingRation,
			PS6000DownSampleRatioMode mode,
			uint overviewBufferSize);

		[DllImport(_DRIVER_FILENAME, EntryPoint = "ps6000GetStreamingLatestValues")]
        public static extern UInt32 GetStreamingLatestValues(
			short handle,
			ps6000StreamingReady lpps6000StreamingReady,
			IntPtr pVoid);

		[DllImport(_DRIVER_FILENAME, EntryPoint = "ps6000SetNoOfCaptures")]
        public static extern UInt32 SetNoOfRapidCaptures(
			short handle,
			uint nWaveforms);

		[DllImport(_DRIVER_FILENAME, EntryPoint = "ps6000MemorySegments")]
        public static extern UInt32 MemorySegments(
			short handle,
			uint nSegments,
			out uint nMaxSamples);

		[DllImport(_DRIVER_FILENAME, EntryPoint = "ps6000SetDataBufferBulk")]
        public static extern UInt32 SetDataBuffersRapid(
			short handle,
			Channel channel,
			short[] buffer,
			uint bufferLth,
			uint waveform,
            PS6000DownSampleRatioMode mode);

		[DllImport(_DRIVER_FILENAME, EntryPoint = "ps6000GetValuesBulk")]
        public static extern UInt32 GetValuesRapid(
			short handle,
			ref uint noOfSamples,
			uint fromSegmentIndex,
			uint toSegmentIndex,
			uint	downSampleRatio,
			PS6000DownSampleRatioMode mode,
			short[] overflows);

        [DllImport(_DRIVER_FILENAME, EntryPoint = "ps6000SetSigGenArbitrary")]
        public static extern UInt32 SetSigGenArbitrary(
           short handle,
           int offsetVoltage,
           uint pkTopk,
           uint startDeltaPhase,
           uint stopDeltaPhase,
           uint deltaPhaseIncrement,
           uint dwellCount,
           short [] arbitaryWaveForm,
           int arbitaryWaveFormSize,
           SweepType sweepType,
           PS6000ExtraOperations operation,
           IndexMode indexMode,
           uint shots,
           uint sweeps,
           SigGenTrigType triggerType,
           SigGenTrigSource triggerSource,
           short extInThreshold);

        [DllImport(_DRIVER_FILENAME, EntryPoint = "ps6000SetSigGenBuiltIn")]
        public static extern UInt32 SetSigGenBuiltIn(
           short handle,
           int offsetVoltage,
           uint pkTopk,
           WaveType waveType,
           float startFrequency,
           float stopFrequency,
           float increment,
           float dwellTime,
           SweepType sweepType,
           PS6000ExtraOperations operation,
           uint shots,
           uint sweeps,
           SigGenTrigType triggerType,
           SigGenTrigSource triggerSource,
           short extInThreshold);

        [DllImport(_DRIVER_FILENAME, EntryPoint = "ps6000SetSigGenBuiltInV2")]
        public static extern UInt32 SetSigGenBuiltInV2(
           short handle,
           int offsetVoltage,
           uint pkTopk,
           WaveType waveType,
           double startFrequency,
           double stopFrequency,
           double increment,
           double dwellTime,
           SweepType sweepType,
           PS6000ExtraOperations operation,
           uint shots,
           uint sweeps,
           SigGenTrigType triggerType,
           SigGenTrigSource triggerSource,
           short extInThreshold);


         [DllImport(_DRIVER_FILENAME, EntryPoint = "ps6000SigGenArbitraryMinMaxValues")]
         public static extern UInt32 SigGenArbitraryMinMaxValues(
                                                                    short handle,
                                                                    out short minArbitraryWaveformValue,
                                                                    out short maxArbitraryWaveformValue,
                                                                    out uint minArbitraryWaveformSize,
                                                                    out uint maxArbitraryWaveformSize
                                                                );

         [DllImport(_DRIVER_FILENAME, EntryPoint = "ps6000SigGenFrequencyToPhase")]
         public static extern UInt32 SigGenFrequencyToPhase(
                                                                 short handle,
                                                                 double frequency,
                                                                 IndexMode indexMode,
                                                                 uint bufferLength,
                                                                 ref uint phase
                                                           );

         [DllImport(_DRIVER_FILENAME, EntryPoint = "ps6000EnumerateUnits")]
         public static extern UInt32 EnumerateUnits(
                                                     out short count,
                                                     StringBuilder serials,
                                                     ref short serialLength
                                                   );   


		#endregion
	}
}
