/******************************************************************************
*
* Filename: PS4000Imports.cs
*  
* Description:
*  This file contains .NET wrapper calls corresponding to function calls 
*  defined in the ps4000Api.h C header file. 
*  It also has the enums and structs required by the (wrapped) function calls.
*   
* Copyright © 2009-2018 Pico Technology Ltd. See LICENSE file for terms.
*
******************************************************************************/

using System;
using System.Runtime.InteropServices;
using System.Text;

namespace PS4000Imports
{
	class Imports
	{
		#region Constants
		private const string _DRIVER_FILENAME = "ps4000.dll";

		public const int MaxValue = 32764;
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
            Range_100V,
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

		public enum DownSamplingMode : int
		{
			None,
			Aggregate,
            Average
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
			False,
		}

        public enum Model : int
        {
            NONE = 0,
            PS4223 = 4223,
            PS4224 = 4224,
            PS4423 = 4423,
            PS4424 = 4424,
            PS4226 = 4226,
            PS4227 = 4227,
            PS4262 = 4262,
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
        public delegate void ps4000BlockReady(short handle, short status, IntPtr pVoid);

		public delegate void ps4000StreamingReady(
												short handle,
												int noOfSamples,
												uint startIndex,
												short ov,
												uint triggerAt,
												short triggered,
												short autoStop,
												IntPtr pVoid);

		public delegate void ps4000DataReady(
												short handle,
												int noOfSamples,
												short overflow,
												uint triggerAt,
												short triggered,
												IntPtr pVoid);
		#endregion

		[DllImport(_DRIVER_FILENAME, EntryPoint = "ps4000OpenUnit")]
		public static extern short OpenUnit(out short handle);

		[DllImport(_DRIVER_FILENAME, EntryPoint = "ps4000CloseUnit")]
		public static extern short CloseUnit(short handle);

		[DllImport(_DRIVER_FILENAME, EntryPoint = "ps4000RunBlock")]
		public static extern short RunBlock(
												short handle,
												int noOfPreTriggerSamples,
												int noOfPostTriggerSamples,
												uint timebase,
												short oversample,
												out int timeIndisposedMs,
												ushort segmentIndex,
												ps4000BlockReady lpps4000BlockReady,
												IntPtr pVoid);

		[DllImport(_DRIVER_FILENAME, EntryPoint = "ps4000Stop")]
		public static extern short Stop(short handle);

		[DllImport(_DRIVER_FILENAME, EntryPoint = "ps4000SetChannel")]
		public static extern short SetChannel(
												short handle,
												Channel channel,
												short enabled,
												short dc,
												Range range);

        [DllImport(_DRIVER_FILENAME, EntryPoint = "ps4000SetDataBuffer")]
        public static extern short SetDataBuffer(
                                                short handle,
                                                Channel channel,
                                                short[] buffer,
                                                int bufferLth);

		[DllImport(_DRIVER_FILENAME, EntryPoint = "ps4000SetDataBuffers")]
		public static extern short SetDataBuffers(
												short handle,
												Channel channel,
												short[] bufferMax,
												short[] bufferMin,
												int bufferLth);

        [DllImport(_DRIVER_FILENAME, EntryPoint = "ps4000SetDataBufferWithMode")]
        public static extern short SetDataBufferWithMode(
                                                short handle,
                                                Channel channel,
                                                short[] buffer,
                                                int bufferLth,
                                                DownSamplingMode mode);

        [DllImport(_DRIVER_FILENAME, EntryPoint = "ps4000SetTriggerChannelDirections")]
		public static extern short SetTriggerChannelDirections(
												short handle,
												ThresholdDirection channelA,
												ThresholdDirection channelB,
												ThresholdDirection channelC,
												ThresholdDirection channelD,
												ThresholdDirection ext,
												ThresholdDirection aux);

		[DllImport(_DRIVER_FILENAME, EntryPoint = "ps4000GetTimebase")]
		public static extern short GetTimebase(
											 short handle,
											 uint timebase,
											 int noSamples,
											 out int timeIntervalNanoseconds,
											 short oversample,
											 out int maxSamples,
											 ushort segmentIndex);

		[DllImport(_DRIVER_FILENAME, EntryPoint = "ps4000GetValues")]
		public static extern short GetValues(
				short handle,
				uint startIndex,
				ref uint noOfSamples,
				uint downSampleRatio,
				DownSamplingMode downSampleDownSamplingMode,
				ushort segmentIndex,
				out short overflow);

		[DllImport(_DRIVER_FILENAME, EntryPoint = "ps4000SetPulseWidthQualifier")]
		public static extern short SetPulseWidthQualifier(
			short handle,
			PwqConditions[] conditions,
			short numConditions,
			ThresholdDirection direction,
			uint lower,
			uint upper,
			PulseWidthType type);

		[DllImport(_DRIVER_FILENAME, EntryPoint = "ps4000SetTriggerChannelProperties")]
		public static extern short SetTriggerChannelProperties(
			short handle,
			TriggerChannelProperties[] channelProperties,
			short numChannelProperties,
			short auxOutputEnable,
			int autoTriggerMilliseconds);

		[DllImport(_DRIVER_FILENAME, EntryPoint = "ps4000SetTriggerChannelConditions")]
		public static extern short SetTriggerChannelConditions(
			short handle,
			TriggerConditions[] conditions,
			short numConditions);

		[DllImport(_DRIVER_FILENAME, EntryPoint = "ps4000SetTriggerDelay")]
		public static extern short SetTriggerDelay(short handle, uint delay);

		[DllImport(_DRIVER_FILENAME, EntryPoint = "ps4000GetUnitInfo")]
		public static extern short GetUnitInfo(short handle, StringBuilder infoString, short stringLength, out short requiredSize, int info);

		[DllImport(_DRIVER_FILENAME, EntryPoint = "ps4000RunStreaming")]
		public static extern short RunStreaming(
			short handle,
			ref uint sampleInterval,
			ReportedTimeUnits sampleIntervalTimeUnits,
			uint maxPreTriggerSamples,
			uint maxPostPreTriggerSamples,
			bool autoStop,
			uint downSamplingRation,
			uint overviewBufferSize);

		[DllImport(_DRIVER_FILENAME, EntryPoint = "ps4000GetStreamingLatestValues")]
		public static extern short GetStreamingLatestValues(
			short handle,
			ps4000StreamingReady lpps4000StreamingReady,
			IntPtr pVoid);

		[DllImport(_DRIVER_FILENAME, EntryPoint = "ps4000SetNoOfCaptures")]
		public static extern short SetNoOfRapidCaptures(
			short handle,
			ushort nWaveforms);

		[DllImport(_DRIVER_FILENAME, EntryPoint = "ps4000MemorySegments")]
		public static extern short MemorySegments(
			short handle,
			ushort nSegments,
			out int nMaxSamples);

		[DllImport(_DRIVER_FILENAME, EntryPoint = "ps4000SetDataBufferBulk")]
		public static extern short SetDataBuffersRapid(
			short handle,
			Channel channel,
			short[] buffer,
			int bufferLth,
			ushort waveform);

		[DllImport(_DRIVER_FILENAME, EntryPoint = "ps4000GetValuesBulk")]
		public static extern short GetValuesRapid(
			short handle,
			ref uint noOfSamples,
			ushort fromSegmentIndex,
			ushort toSegmentIndex,
			short[] overflows);
		
        #endregion
	}
}
