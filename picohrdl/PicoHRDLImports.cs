/******************************************************************************
*
* Filename: PicoHRDLImports.cs
*  
* Description:
*  This file contains .NET wrapper calls corresponding to function calls 
*  defined in the HRDL.h C header file. 
*  It also has the enums required by the (wrapped) function calls.
*   
* Copyright © 2015-2018 Pico Technology Ltd. See LICENSE file for terms.
*
******************************************************************************/

using System.Text;
using System.Runtime.InteropServices;

namespace PicoHRDLImports
{
    class Imports
    {

        #region Constants
        private const string _DRIVER_FILENAME = "picohrdl.dll";

        public const int MAX_VOLTAGE_RANGE = 2500;

        // Constants to define model
        public const int HRDL_ADC_20 = 20;
        public const int HRDL_ADC_24 = 24;

        #endregion

        #region Driver Enums

        public enum HRDLInputs : short
        {
            HRDL_DIGITAL_CHANNELS,
            HRDL_ANALOG_IN_CHANNEL_1,
            HRDL_ANALOG_IN_CHANNEL_2,
            HRDL_ANALOG_IN_CHANNEL_3,
            HRDL_ANALOG_IN_CHANNEL_4,
            HRDL_ANALOG_IN_CHANNEL_5,
            HRDL_ANALOG_IN_CHANNEL_6,
            HRDL_ANALOG_IN_CHANNEL_7,
            HRDL_ANALOG_IN_CHANNEL_8,
            HRDL_ANALOG_IN_CHANNEL_9,
            HRDL_ANALOG_IN_CHANNEL_10,
            HRDL_ANALOG_IN_CHANNEL_11,
            HRDL_ANALOG_IN_CHANNEL_12,
            HRDL_ANALOG_IN_CHANNEL_13,
            HRDL_ANALOG_IN_CHANNEL_14,
            HRDL_ANALOG_IN_CHANNEL_15,
            HRDL_ANALOG_IN_CHANNEL_16,
            HRDL_MAX_ANALOG_CHANNELS = HRDL_ANALOG_IN_CHANNEL_16
        } 

        public enum HRDLDigitalIOChannel : short
        {   
          HRDL_DIGITAL_IO_CHANNEL_1 = 0x01,
          HRDL_DIGITAL_IO_CHANNEL_2 = 0x02,
          HRDL_DIGITAL_IO_CHANNEL_3 = 0x04,
          HRDL_DIGITAL_IO_CHANNEL_4 = 0x08,
          HRDL_MAX_DIGITAL_CHANNELS = 4
        } 

        public enum HRDLRange : short
        {
            HRDL_2500_MV,
            HRDL_1250_MV,
            HRDL_625_MV,
            HRDL_313_MV,
            HRDL_156_MV,
            HRDL_78_MV,
            HRDL_39_MV,  
            HRDL_MAX_RANGES
        }	

        public enum HRDLConversionTime : short
        {
            HRDL_60MS,
            HRDL_100MS,
            HRDL_180MS,
            HRDL_340MS,
            HRDL_660MS,
            HRDL_MAX_CONVERSION_TIMES
        }	

        public enum HRDLInfo : short
        {
            HRDL_DRIVER_VERSION,
            HRDL_USB_VERSION,
            HRDL_HARDWARE_VERSION,
            HRDL_VARIANT_INFO,
            HRDL_BATCH_AND_SERIAL,
            HRDL_CAL_DATE,	
            HRDL_KERNEL_DRIVER_VERSION, 
            HRDL_ERROR,
            HRDL_SETTINGS,
        } 

        public enum HRDLErrorCode : short
        {
            HRDL_OK,
            HRDL_KERNEL_DRIVER,
            HRDL_NOT_FOUND,
            HRDL_CONFIG_FAIL,
            HRDL_ERROR_OS_NOT_SUPPORTED,
            HRDL_MAX_DEVICES
        } 

        public enum SettingsError : short
        {
	        SE_CONVERSION_TIME_OUT_OF_RANGE,
	        SE_SAMPLEINTERVAL_OUT_OF_RANGE,
	        SE_CONVERSION_TIME_TOO_SLOW,
	        SE_CHANNEL_NOT_AVAILABLE,
	        SE_INVALID_CHANNEL,
	        SE_INVALID_VOLTAGE_RANGE,
	        SE_INVALID_PARAMETER,
	        SE_CONVERSION_IN_PROGRESS,
	        SE_COMMUNICATION_FAILED,
	        SE_OK,
	        SE_MAX = SE_OK
        }

        public enum HRDLOpenProgress : short
        {
            HRDL_OPEN_PROGRESS_FAIL     = -1,
            HRDL_OPEN_PROGRESS_PENDING  = 0,
            HRDL_OPEN_PROGRESS_COMPLETE = 1
        }

        public enum BlockMethod : short
        {
            HRDL_BM_BLOCK,
            HRDL_BM_WINDOW,
            HRDL_BM_STREAM
        }

        public enum HRDLMainsRejection : short
        {
            HRDL_FIFTY_HERTZ,
            HRDL_SIXTY_HERTZ
        }

        #endregion

        #region Driver Imports

        [DllImport(_DRIVER_FILENAME, EntryPoint = "HRDLOpenUnit")]
        public static extern short HRDLOpenUnit();

        [DllImport(_DRIVER_FILENAME, EntryPoint = "HRDLCloseUnit")]
        public static extern short HRDLCloseUnit(short handle);

        [DllImport(_DRIVER_FILENAME, EntryPoint = "HRDLGetUnitInfo")]
        public static extern short GetUnitInfo(
            short handle,
            StringBuilder infoString,
            short stringLength,
            short info);

        [DllImport(_DRIVER_FILENAME, EntryPoint = "HRDLSetMains")]
        public static extern short SetMains(
            short handle,
            HRDLMainsRejection sixtyHertz);

        [DllImport(_DRIVER_FILENAME, EntryPoint = "HRDLSetAnalogInChannel")]
        public static extern short SetAnalogInChannel(
            short handle,
            short channel,
            short enabled,
            short range,
            short singleEnded);

        [DllImport(_DRIVER_FILENAME, EntryPoint = "HRDLSetDigitalIOChannel")]
        public static extern short SetDigitalIOChannel(
            short handle,
            short directionOut,
            short digitalOutPinState,
            short enabledDigitalIn);

        [DllImport(_DRIVER_FILENAME, EntryPoint = "HRDLSetInterval")]
        public static extern short SetInterval(
            short handle, 
            int sampleInterval_ms, 
            short conversionTime);

        [DllImport(_DRIVER_FILENAME, EntryPoint = "HRDLGetMinMaxAdcCounts")]
        public static extern short GetMinMaxAdcCounts(
            short handle,
            out int minAdc,
            out int maxAdc,
            short channel);

        [DllImport(_DRIVER_FILENAME, EntryPoint = "HRDLGetNumberOfEnabledChannels")]
        public static extern short GetNumberOfEnabledChannels(
            short handle,
            out short nEnabledChannels);

        [DllImport(_DRIVER_FILENAME, EntryPoint = "HRDLGetSingleValue")]
        public static extern short GetSingleValue(
            short handle,
            short channel,
            short range,
            short conversionTime,
            short singleEnded,
            out short overflow,
            out int value);

        [DllImport(_DRIVER_FILENAME, EntryPoint = "HRDLGetTimesAndValues")]
        public static extern short GetTimesAndValues(
            short handle,
            int[] times,
            int[] values,
            out short overflow,
            int noOfValues);

        [DllImport(_DRIVER_FILENAME, EntryPoint = "HRDLGetValues")]
        public static extern short GetValues(
            short handle,
            int[] values,
            out short overflow,
            int noOfValues);

        [DllImport(_DRIVER_FILENAME, EntryPoint = "HRDLReady")]
        public static extern short HRDLReady(
            short handle);

        [DllImport(_DRIVER_FILENAME, EntryPoint = "HRDLRun")]
        public static extern short HRDLRun(
            short handle,
            int nValues,
            short method);

        [DllImport(_DRIVER_FILENAME, EntryPoint = "HRDLStop")]
        public static extern short HRDLStop(
            short handle);

        #endregion

    }
}
