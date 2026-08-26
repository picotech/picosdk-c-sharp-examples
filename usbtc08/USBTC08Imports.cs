/******************************************************************************
*
* Filename: USBTC08Imports.cs
*
* Description:
*  This file contains .NET wrapper calls correseponding to function calls
*  defined in the usbtc08.h C header file.
*  It also has the enums required by the (wrapped) function calls.
*
* Copyright � 2011-2018 Pico Technology Ltd. See LICENSE file for terms.
*
******************************************************************************/

using System.Runtime.InteropServices;
using System.Text;

namespace USBTC08Imports
{
	class Imports
	{
		#region Constants
		private const string _DRIVER_FILENAME = "usbtc08.dll";

		#endregion

		#region Driver Enums

        public enum TempUnit : short
        {   USBTC08_UNITS_CENTIGRADE,
            USBTC08_UNITS_FAHRENHEIT,
            USBTC08_UNITS_KELVIN,
            USBTC08_UNITS_RANKINE
        }

        public enum MainsFrequency : short
        {
            USBTC08_MAINS_FIFTY_HERTZ = 0,
            USBTC08_MAINS_SIXTY_HERTZ = 1,
        }

        #endregion

        #region Driver Imports

        [DllImport(_DRIVER_FILENAME, EntryPoint = "usb_tc08_open_unit")]
		public static extern short TC08OpenUnit();

        [DllImport(_DRIVER_FILENAME, EntryPoint = "usb_tc08_close_unit")]
        public static extern short TC08CloseUnit(short handle);

        // usb_tc08_run returns int32_t: the sampling interval the driver
        // actually applied, or 0 on failure. Declaring it as short truncated
        // that value.
        [DllImport(_DRIVER_FILENAME, EntryPoint = "usb_tc08_run")]
        public static extern int TC08Run(short handle,
                                         int interval
                                         );

        [DllImport(_DRIVER_FILENAME, EntryPoint = "usb_tc08_stop")]
        public static extern short TC08Stop(short handle);

        [DllImport(_DRIVER_FILENAME, EntryPoint = "usb_tc08_get_formatted_info")]
        public static extern short TC08GetFormattedInfo(short handle,
                                                        StringBuilder unit_info,
                                                        short string_length
                                                        );

        // tc_type is int8_t in usbtc08.h, so it is declared sbyte here rather
        // than char: that keeps the marshalled width correct without relying on
        // the DllImport CharSet default.
        [DllImport(_DRIVER_FILENAME, EntryPoint = "usb_tc08_set_channel")]
        public static extern short TC08SetChannel(short handle,
                                                  short channel,
                                                  sbyte tc_type
                                                  );

        // overflow_flags points at one flag per channel (the cold junction plus
        // the eight inputs), so it is marshalled as an array. Using an array
        // rather than a raw pointer keeps this example free of unsafe code.
        [DllImport(_DRIVER_FILENAME, EntryPoint = "usb_tc08_get_single")]
        public static extern short TC08GetSingle(short handle,
                                                  float[] temp,
                                                  short[] overflow_flags,
                                                  TempUnit units
                                                  );

        [DllImport(_DRIVER_FILENAME, EntryPoint = "usb_tc08_get_temp")]
        public static extern int TC08GetTemp(short handle,
                                                  float[] temp_buffer,
                                                  int[] times_ms_buffer,
                                                  int buffer_length,
                                                  out short overflow_flag,
                                                  short channel,
                                                  TempUnit units,
                                                  short fill_missing
                                                  );

        [DllImport(_DRIVER_FILENAME, EntryPoint = "usb_tc08_get_minimum_interval_ms")]
        public static extern int TC08GetMinIntervalMS(short handle);

        [DllImport(_DRIVER_FILENAME, EntryPoint = "usb_tc08_set_mains")]
        public static extern short TC08SetMains(short handle,
                                               MainsFrequency freq
                                               );

        [DllImport(_DRIVER_FILENAME, EntryPoint = "usb_tc08_get_last_error")]
        public static extern short TC08GetLastError(short handle);

        #endregion
    }
}

