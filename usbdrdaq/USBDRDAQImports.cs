/******************************************************************************
*
* Filename: USBDRDAQImports.cs
*  
* Description:
*  This file contains .NET wrapper calls correseponding to  
*  function calls defined in the usbDrDaqApi.h C header file. 
*  It also has the enums and structs required by the (wrapped) 
*  function calls.
*   
* Copyright © 2012-2018 Pico Technology Ltd. See LICENSE file for terms.
*
******************************************************************************/

using System.Runtime.InteropServices;
using System.Text;

namespace DrDAQRemote
{
	class Imports
	{
		#region constants
		private const string _DRIVER_FILENAME = "usbdrdaq.dll";

		#endregion

		#region Driver enums

		public enum Inputs : int
		{
			USB_DRDAQ_CHANNEL_EXT1 = 1,				//Ext. sensor 1
			USB_DRDAQ_CHANNEL_EXT2,						//Ext. sensor 2
			USB_DRDAQ_CHANNEL_EXT3,						//Ext. sensor 3
			USB_DRDAQ_CHANNEL_SCOPE,					//Scope channel
			USB_DRDAQ_CHANNEL_PH,							//PH
			USB_DRDAQ_CHANNEL_RES,						//Resistance
			USB_DRDAQ_CHANNEL_LIGHT,					//Light
			USB_DRDAQ_CHANNEL_TEMP,						//Thermistor
			USB_DRDAQ_CHANNEL_MIC_WAVE,				//Microphone waveform
			USB_DRDAQ_CHANNEL_MIC_LEVEL,			//Microphone level
			USB_DRDAQ_MAX_CHANNELS = USB_DRDAQ_CHANNEL_MIC_LEVEL
		}

		public enum ScopeRange : int
		{
			USB_DRDAQ_1V25,
			USB_DRDAQ_2V5,
			USB_DRDAQ_5V,
			USB_DRDAQ_10V
		}

		public enum Wave : int
		{
			USB_DRDAQ_SINE,
			USB_DRDAQ_SQUARE,
			USB_DRDAQ_TRIANGLE,
			USB_DRDAQ_RAMP_UP,
			USB_DRDAQ_RAMP_DOWN,
			USB_DRDAQ_DC
		}

		public enum DigOut : int
		{
			USB_DRDAQ_GPIO_1 = 1,
			USB_DRDAQ_GPIO_2,
			USB_DRDAQ_GPIO_3,
			USB_DRDAQ_GPIO_4
		}

		public enum Info : int
		{
			USBDrDAQ_DRIVER_VERSION,
			USBDrDAQ_USB_VERSION,
			USBDrDAQ_HARDWARE_VERSION,
			USBDrDAQ_VARIANT_INFO,
			USBDrDAQ_BATCH_AND_SERIAL,
			USBDrDAQ_CAL_DATE,	
			USBDrDAQ_KERNEL_DRIVER_VERSION, 
			USBDrDAQ_ERROR,
			USBDrDAQ_SETTINGS,
		}			


		public enum _BLOCK_METHOD
		{
			BM_SINGLE,
			BM_WINDOW,
			BM_STREAM
		}

        #endregion

        #region Driver Imports

        [DllImport(_DRIVER_FILENAME, EntryPoint = "UsbDrDaqOpenUnit")]
			public static extern short OpenUnit(
				out short handle
		);

		[DllImport(_DRIVER_FILENAME, EntryPoint = "UsbDrDaqCloseUnit")]
			public static extern short CloseUnit(
				short handle
		);

		[DllImport(_DRIVER_FILENAME, EntryPoint = "UsbDrDaqGetUnitInfo")]
			public static extern short GetUnitInfo(	
				short	handle,		
				StringBuilder daqstring, 
				short	stringLen, 
				out short requiredSize, 
				Info    daqinfo
		);

		[DllImport(_DRIVER_FILENAME, EntryPoint = "UsbDrDaqRun")]
			public static extern short Run(
				short	        handle, 
				uint	        no_of_values, 
				_BLOCK_METHOD	method
		);

		[DllImport(_DRIVER_FILENAME, EntryPoint = "UsbDrDaqReady")]
			public static extern short Ready(
				short handle, 
				out short ready
		);

		[DllImport(_DRIVER_FILENAME, EntryPoint = "UsbDrDaqStop")]
			public static extern short Stop(
				short handle
		);

		[DllImport(_DRIVER_FILENAME, EntryPoint = "UsbDrDaqSetInterval")]
			public static extern short SetInterval(	
				short	handle,
				ref uint	us_for_block,
				uint	ideal_no_of_samples,
				ref Inputs	channels,
				short	no_of_channels
		);

		[DllImport(_DRIVER_FILENAME, EntryPoint = "UsbDrDaqSetTrigger")]
			public static extern short SetTrigger(
				short	handle,
				ushort	enabled,
				ushort	auto_trigger,
				ushort	auto_ms,
				ushort	channel,
				ushort	dir,
				short	threshold,
				ushort	hysterisis,
				float	delay
		);

		[DllImport(_DRIVER_FILENAME, EntryPoint = "UsbDrDaqGetValues")]
			public static extern short GetValues(
				short	handle,
				out short	values,
				ref uint	noOfValues, 
				out ushort	overflow,
				out uint	triggerIndex
		);

		[DllImport(_DRIVER_FILENAME, EntryPoint = "UsbDrDaqGetTriggerTimeOffsetNs")]
			public static extern short GetTriggerTimeOffsetNs(
				short	handle, 
				out long    time
		);

		[DllImport(_DRIVER_FILENAME, EntryPoint = "UsbDrDaqGetSingle")]
			public static extern short GetSingle(
				short	handle, 
				Inputs	channel, 
				out short	value, 
				out ushort	overflow
		);

		[DllImport(_DRIVER_FILENAME, EntryPoint = "UsbDrDaqOpenUnitAsync")]
			public static extern short OpenUnitAsync(
				out short status
		);

		[DllImport(_DRIVER_FILENAME, EntryPoint = "UsbDrDaqOpenUnitProgress")]
			public static extern short OpenUnitProgress(
				out short handle, 
				out short progress,
                out short complete
		);

		[DllImport(_DRIVER_FILENAME, EntryPoint = "UsbDrDaqGetScalings")]
			public static extern short GetScalings(
				short	handle, 
				Inputs	channel,
                out short nScales,
                out short currentScale,
                StringBuilder names, 
				short	namesSize
		);

		[DllImport(_DRIVER_FILENAME, EntryPoint = "UsbDrDaqSetScalings")]
			public static extern short SetScalings(
				short	handle, 
				Inputs	channel, 
				short	scalingNumber
		);

		[DllImport(_DRIVER_FILENAME, EntryPoint = "UsbDrDaqSetSigGenBuiltIn")]
			public static extern short SetSigGenBuiltIn(
				short	handle, 
				int	offsetVoltage,
				uint	pkToPk,
				short	frequency,
				Wave	waveType
		);

		[DllImport(_DRIVER_FILENAME, EntryPoint = "UsbDrDaqSetSigGenArbitrary")]
			public static extern short SetSigGenArbitrary(
				short	handle, 
				int	offsetVoltage,
				uint	pkToPk,
                ref short arbitraryWaveform,
				short	arbitraryWaveformSize,
				int	updateRate
		);

		[DllImport(_DRIVER_FILENAME, EntryPoint = "UsbDrDaqStopSigGen")]
			public static extern short StopSigGen(
				short	handle
		);

		[DllImport(_DRIVER_FILENAME, EntryPoint = "UsbDrDaqSetDO")]
			public static extern short SetDO(
				short	handle,
				DigOut	IOChannel,
				short	value
		);

		[DllImport(_DRIVER_FILENAME, EntryPoint = "UsbDrDaqSetPWM")]
			public static extern short SetPWM(
				short	handle,
				DigOut	IOChannel,
				ushort	period,
				byte	cycle
		);

		[DllImport(_DRIVER_FILENAME, EntryPoint = "UsbDrDaqGetInput")]
			public static extern short GetInput(
				short	handle,
				DigOut	IOChannel,
				short	pullUp,
                out short value
		);

		[DllImport(_DRIVER_FILENAME, EntryPoint = "UsbDrDaqStartPulseCount")]
			public static extern short StartPulseCount(
				short	handle,
				DigOut	IOChannel,
				short	direction
		);

		[DllImport(_DRIVER_FILENAME, EntryPoint = "UsbDrDaqGetPulseCount")]
			public static extern short GetPulseCount(
				short   handle,
				DigOut	IOChannel,
                out short count
		);

		[DllImport(_DRIVER_FILENAME, EntryPoint = "UsbDrDaqEnableRGBLED")]
			public static extern short EnableRGBLED(
				short   handle,
				short   enabled
		);

		[DllImport(_DRIVER_FILENAME, EntryPoint = "UsbDrDaqSetRGBLED")]
			public static extern short SetRGBLED(
				short	handle,
				ushort	red,
				ushort	green,
				ushort	blue
		);

		[DllImport(_DRIVER_FILENAME, EntryPoint = "UsbDrDaqGetChannelInfo")]
			public static extern short GetChannelInfo(
				short	handle,
                out float min,
                out float max,
                out short places,
                out short divider,
				DigOut	channel
		);

		[DllImport(_DRIVER_FILENAME, EntryPoint = "UsbDrDaqPingUnit")]
			public static extern short PingUnit(
				short handle
		);

        [DllImport(_DRIVER_FILENAME, EntryPoint = "UsbDrDaqGetValuesF")]
            public static extern short GetValuesF(
                short handle,
                out float values,
                ref uint noOfValues,
                out ushort overflow,
                out uint triggerIndex
        );

        [DllImport(_DRIVER_FILENAME, EntryPoint = "UsbDrDaqSetIntervalF")]
            public static extern short SetIntervalF(
                short handle,
                ref float us_for_block,
                uint ideal_no_of_samples,
                ref Inputs channels,
                short no_of_channels
        );

        #endregion
    }
}
