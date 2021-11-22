/******************************************************************************
*
* Filename: PS5000AImports.cs
*  
* Description:
*  This file contains .NET wrapper calls corresponding to function calls 
*  defined in the ps5000aApi.h C header file. 
*  It also has the enums and structs required by the (wrapped) function calls.
*   
* Copyright © 2013-2018 Pico Technology Ltd. See LICENSE file for terms.
*
******************************************************************************/

using System;
using System.Runtime.InteropServices;
using System.Text;

namespace PS5000AImports
{
	class Imports
	{
		#region Constants
		private const string _DRIVER_FILENAME = "ps5000a.dll";

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
			None
		}

		public enum Range : uint
		{
			Range_10mV,
			Range_20mV,
			Range_50mV,
			Range_100mV,
			Range_200mV,
			Range_500mV,
			Range_1V,
			Range_2V,
			Range_5V,
			Range_10V,
			Range_20V,
			Range_MAX_RANGE
		}

		public enum ReportedTimeUnits : uint
		{
			FemtoSeconds,
			PicoSeconds,
			NanoSeconds,
			MicroSeconds,
			MilliSeconds,
			Seconds,
		}

		public enum ThresholdMode : uint
		{
			Level,
			Window
		}

		public enum ThresholdDirection : uint
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

		public enum PulseWidthType : uint
		{
			None,
			LessThan,
			GreaterThan,
			InRange,
			OutOfRange
		}

		public enum TriggerState : uint
		{
			DontCare,
			True,
			False,
		}

		public enum RatioMode : uint
		{
			None = 0,
			Aggregate = 1,
			Decimate = 2,
			Average = 4
		}

		public enum DeviceResolution : uint
		{
			PS5000A_DR_8BIT,
			PS5000A_DR_12BIT,
			PS5000A_DR_14BIT,
			PS5000A_DR_15BIT,
			PS5000A_DR_16BIT
		}

		public enum Coupling : uint
		{
			PS5000A_AC,
			PS5000A_DC
		}

		public enum SweepType : int
		{
			PS5000A_UP,
			PS5000A_DOWN,
			PS5000A_UPDOWN,
			PS5000A_DOWNUP,
			PS5000A_MAX_SWEEP_TYPES
		}

		public enum WaveType : int
		{
			PS5000A_SINE,
			PS5000A_SQUARE,
			PS5000A_TRIANGLE,
			PS5000A_RAMP_UP,
			PS5000A_RAMP_DOWN,
			PS5000A_SINC,
			PS5000A_GAUSSIAN,
			PS5000A_HALF_SINE,
			PS5000A_DC_VOLTAGE,
			PS5000A_WHITE_NOISE,
			PS5000A_MAX_WAVE_TYPES
		}

		public enum ExtraOperations : int
		{
			PS5000A_ES_OFF,
			PS5000A_WHITENOISE,
			PS5000A_PRBS // Pseudo-Random Bit Stream
		}

		public enum SigGenTrigType : int
		{
			PS5000A_SIGGEN_RISING,
			PS5000A_SIGGEN_FALLING,
			PS5000A_SIGGEN_GATE_HIGH,
			PS5000A_SIGGEN_GATE_LOW
		}

		public enum SigGenTrigSource : int
		{
			PS5000A_SIGGEN_NONE,
			PS5000A_SIGGEN_SCOPE_TRIG,
			PS5000A_SIGGEN_AUX_IN,
			PS5000A_SIGGEN_EXT_IN,
			PS5000A_SIGGEN_SOFT_TRIG
		}

		public enum IndexMode : int
		{
			PS5000A_SINGLE,
			PS5000A_DUAL,
			PS5000A_QUAD,
			PS5000A_MAX_INDEX_MODES
		}

		public enum BandwidthLimiter : uint
		{
			PS5000A_BW_FULL,
			PS5000A_BW_20MHZ
		}

		#endregion

		# region Driver Structs

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
		public struct TriggerConditionsV2
		{
			public TriggerState ChannelA;
			public TriggerState ChannelB;
			public TriggerState ChannelC;
			public TriggerState ChannelD;
			public TriggerState External;
			public TriggerState Aux;
			public TriggerState Pwq;
			 public TriggerState Digital;

			public TriggerConditionsV2(
				TriggerState channelA,
				TriggerState channelB,
				TriggerState channelC,
				TriggerState channelD,
				TriggerState external,
				TriggerState aux,
				TriggerState pwq,
				TriggerState digital)
			{
				this.ChannelA = channelA;
				this.ChannelB = channelB;
				this.ChannelC = channelC;
				this.ChannelD = channelD;
				this.External = external;
				this.Aux = aux;
				this.Pwq = pwq;
				this.Digital = digital;
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
		public delegate void ps5000aBlockReady(short handle, short status, IntPtr pVoid);

		public delegate void ps5000aStreamingReady(
												short handle,
												int noOfSamples,
												uint startIndex,
												short ov,
												uint triggerAt,
												short triggered,
												short autoStop,
												IntPtr pVoid);

		public delegate void ps5000DataReady(
												short handle,
												short status,
												int noOfSamples,
												short overflow,
												IntPtr pVoid);
		#endregion

		[DllImport(_DRIVER_FILENAME, EntryPoint = "ps5000aOpenUnit")]
		public static extern uint OpenUnit(out short handle, StringBuilder serial, DeviceResolution resolution);

		[DllImport(_DRIVER_FILENAME, EntryPoint = "ps5000aSetDeviceResolution")]
		public static extern uint SetDeviceResolution(short handle, DeviceResolution resolution);

		[DllImport(_DRIVER_FILENAME, EntryPoint = "ps5000aGetDeviceResolution")]
		public static extern uint GetDeviceResolution(short handle, out DeviceResolution resolution);

		[DllImport(_DRIVER_FILENAME, EntryPoint = "ps5000aCloseUnit")]
		public static extern uint CloseUnit(short handle);

		[DllImport(_DRIVER_FILENAME, EntryPoint = "ps5000aRunBlock")]
		public static extern uint RunBlock(
												short handle,
												int noOfPreTriggerSamples,
												int noOfPostTriggerSamples,
												uint timebase,
												out int timeIndisposedMs,
												uint segmentIndex,
												ps5000aBlockReady lpps5000aBlockReady,
												IntPtr pVoid);

		[DllImport(_DRIVER_FILENAME, EntryPoint = "ps5000aStop")]
		public static extern uint Stop(short handle);

		[DllImport(_DRIVER_FILENAME, EntryPoint = "ps5000aSetChannel")]
		public static extern uint SetChannel(
												short handle,
												Channel channel,
												short enabled,
												Coupling dc,
												Range range,
												float analogueOffset);

		[DllImport(_DRIVER_FILENAME, EntryPoint = "ps5000aMaximumValue")]
		public static extern uint MaximumValue(
												short handle,
												out short value);

		[DllImport(_DRIVER_FILENAME, EntryPoint = "ps5000aSetDataBuffer")]
		public static extern uint SetDataBuffer(
													short handle,
													Channel channel,
													short[] buffer,
													int bufferLth,
													uint segmentIndex,
													RatioMode  ratioMode);

		[DllImport(_DRIVER_FILENAME, EntryPoint = "ps5000aSetDataBuffers")]
		public static extern uint SetDataBuffers(
													short handle,
													Channel channel,
													short[] bufferMax,
													short[] bufferMin,
													int bufferLth,
													uint segmentIndex,
													RatioMode ratioMode);

        [DllImport(_DRIVER_FILENAME, EntryPoint = "ps5000aSetAutoTriggerMicroSeconds")]   
        public static extern uint SetAutoTriggerMicroSeconds( short handle,
                                                              uint autoTriggerMicroseconds);

		[DllImport(_DRIVER_FILENAME, EntryPoint = "ps5000aSetTriggerChannelDirections")]
		public static extern uint SetTriggerChannelDirections(
																short handle,
																ThresholdDirection channelA,
																ThresholdDirection channelB,
																ThresholdDirection channelC,
																ThresholdDirection channelD,
																ThresholdDirection ext,
																ThresholdDirection aux);

		[DllImport(_DRIVER_FILENAME, EntryPoint = "ps5000aGetTimebase")]
		public static extern uint GetTimebase(
												 short handle,
												 uint timebase,
												 int noSamples,
												 out int timeIntervalNanoseconds,
												 out int maxSamples,
												 uint segmentIndex);

		[DllImport(_DRIVER_FILENAME, EntryPoint = "ps5000aGetValues")]
		public static extern uint GetValues(
												short handle,
												uint startIndex,
												ref uint noOfSamples,
												uint downSampleRatio,
												RatioMode downSampleRatioMode,
												uint segmentIndex,
												out short overflow);

		[DllImport(_DRIVER_FILENAME, EntryPoint = "ps5000aSetPulseWidthQualifier")]
		public static extern uint SetPulseWidthQualifier(
															short handle,
															PwqConditions[] conditions,
															short nConditions,
															ThresholdDirection direction,
															uint lower,
															uint upper,
															PulseWidthType type);

		[DllImport(_DRIVER_FILENAME, EntryPoint = "ps5000aSetSimpleTrigger")]
		public static extern uint SetSimpleTrigger(
														short handle,
														short enable,
														Channel channel,
														short threshold,
														ThresholdDirection direction,
														uint delay,
														short autoTriggerMs);


		[DllImport(_DRIVER_FILENAME, EntryPoint = "ps5000aSetTriggerChannelProperties")]
		public static extern uint SetTriggerChannelProperties(
																	short handle,
																	TriggerChannelProperties[] channelProperties,
																	short nChannelProperties,
																	short auxOutputEnable,
																	int autoTriggerMilliseconds);

		[DllImport(_DRIVER_FILENAME, EntryPoint = "ps5000aSetTriggerChannelConditions")]
		public static extern uint SetTriggerChannelConditions(
																	short handle,
																	TriggerConditions[] conditions,
																	short nConditions);

		[DllImport(_DRIVER_FILENAME, EntryPoint = "ps5000aSetTriggerDelay")]
		public static extern uint SetTriggerDelay(short handle, uint delay);

		[DllImport(_DRIVER_FILENAME, EntryPoint = "ps5000aGetUnitInfo")]
		public static extern uint GetUnitInfo(
													short handle, 
													StringBuilder infoString, 
													short stringLength, 
													out short requiredSize, 
													uint info);

		[DllImport(_DRIVER_FILENAME, EntryPoint = "ps5000aRunStreaming")]
		public static extern uint RunStreaming(
													short handle,
													ref uint sampleInterval,
													ReportedTimeUnits sampleIntervalTimeUnits,
													uint maxPreTriggerSamples,
													uint maxPostTriggerSamples,
													short autoStop,
													uint downSamplingRatio,
													RatioMode downSampleRatioMode,
													uint overviewBufferSize);

		[DllImport(_DRIVER_FILENAME, EntryPoint = "ps5000aGetStreamingLatestValues")]
		public static extern uint GetStreamingLatestValues(
																short handle,
																ps5000aStreamingReady lpps5000aStreamingReady,
																IntPtr pVoid);

		[DllImport(_DRIVER_FILENAME, EntryPoint = "ps5000aSetNoOfCaptures")]
		public static extern uint SetNoOfRapidCaptures(
														short handle,
														uint nCaptures);

		[DllImport(_DRIVER_FILENAME, EntryPoint = "ps5000aMemorySegments")]
		public static extern uint MemorySegments(
													short handle,
													uint nSegments,
													out int nMaxSamples);


		[DllImport(_DRIVER_FILENAME, EntryPoint = "ps5000aGetValuesBulk")]
		public static extern uint GetValuesRapid(
													short handle,
													ref uint noOfSamples,
													uint fromSegmentIndex,
													uint toSegmentIndex,
													uint downSampleRatio,
													RatioMode downSampleRatioMode,
													short[] overflow);

		[DllImport(_DRIVER_FILENAME, EntryPoint = "ps5000aChangePowerSource")]
		public static extern uint ChangePowerSource(
														short handle,
														uint status);

		[DllImport(_DRIVER_FILENAME, EntryPoint = "ps5000aEnumerateUnits")]
		public static extern uint EnumerateUnits(
													out short count,
													StringBuilder serials,
													ref short serialLength);

		[DllImport(_DRIVER_FILENAME, EntryPoint = "ps5000aSetSigGenArbitrary")]
		public static extern uint SetSigGenArbitrary(
														short handle,
														int offsetVoltage,
														uint pkTopk,
														uint startDeltaPhase,
														uint stopDeltaPhase,
														uint deltaPhaseIncrement,
														uint dwellCount,
														short[] arbitaryWaveform,
														int arbitaryWaveformSize,
														SweepType sweepType,
														ExtraOperations operation,
														IndexMode indexMode,
														uint shots,
														uint sweeps,
														SigGenTrigType triggerType,
														SigGenTrigSource triggerSource,
														short extInThreshold);

		[DllImport(_DRIVER_FILENAME, EntryPoint = "ps5000aSetSigGenPropertiesArbitrary")]
		public static extern uint SetSigGenPropertiesArbitrary(
														short handle,
														uint startDeltaPhase,
														uint stopDeltaPhase,
														uint deltaPhaseIncrement,
														uint dwellCount,
														SweepType sweepType,
														uint shots,
														uint sweeps,
														SigGenTrigType triggerType,
														SigGenTrigSource triggerSource,
														short extInThreshold);

		[DllImport(_DRIVER_FILENAME, EntryPoint = "ps5000aSetSigGenPropertiesBuiltIn")]
		public static extern uint SetSigGenPropertiesBuiltIn(
														short handle,
														double startFrequency,
														double stopFrequency,
														double increment,
														double dwellTime,
														SweepType sweepType,
														uint shots,
														uint sweeps,
														SigGenTrigType triggerType,
														SigGenTrigSource triggerSource,
														short extInThreshold);

		[DllImport(_DRIVER_FILENAME, EntryPoint = "ps5000aSetSigGenBuiltInV2")]
		public static extern uint SetSigGenBuiltInV2(
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

		[DllImport(_DRIVER_FILENAME, EntryPoint = "ps5000aSigGenFrequencyToPhase")]
		public static extern uint SigGenFrequencyToPhase(
																short handle,
																double frequency,
																IndexMode indexMode,
																uint bufferLength,
																out uint phase);

		[DllImport(_DRIVER_FILENAME, EntryPoint = "ps5000aSigGenArbitraryMinMaxValues")]
		public static extern uint SigGenArbitraryMinMaxValues(
																short handle,
																out short minArbitraryWaveformValue,
																out short maxArbitraryWaveformValue,
																out uint minArbitraryWaveformSize,
																out uint maxArbitraryWaveformSize);

		[DllImport(_DRIVER_FILENAME, EntryPoint = "ps5000aSetBandwidthFilter")]
		public static extern uint SetBandwidthFilter(short handle, Channel channel, BandwidthLimiter bandwidth);

		[DllImport(_DRIVER_FILENAME, EntryPoint = "ps5000aPingUnit")]
		public static extern uint PingUnit(short handle);

		#endregion
	}
}
