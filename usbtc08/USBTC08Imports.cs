/******************************************************************************
*
* Filename: USBTC08Imports.cs
*  
* Description:
*  This file contains .NET wrapper calls correseponding to function calls 
*  defined in the usbtc08.h C header file. 
*  It also has the enums required by the (wrapped) function calls.
*   
* Copyright © 2011-2018 Pico Technology Ltd. See LICENSE file for terms.
*
******************************************************************************/

using System.Runtime.InteropServices;
using System.Text;

namespace USBTC08Imports
{
	unsafe class Imports
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

        [DllImport(_DRIVER_FILENAME, EntryPoint = "usb_tc08_run")]
        public static extern short TC08Run(short handle,
                                           int interval
                                           );

        [DllImport(_DRIVER_FILENAME, EntryPoint = "usb_tc08_stop")]
        public static extern short TC08Stop(short handle);

        [DllImport(_DRIVER_FILENAME, EntryPoint = "usb_tc08_get_formatted_info")]
        public static extern short TC08GetFormattedInfo(short handle,
                                                        StringBuilder unit_info,
                                                        short string_length
                                                        );

        [DllImport(_DRIVER_FILENAME, EntryPoint = "usb_tc08_set_channel")]
        public static extern short TC08SetChannel(short handle,
                                                  short channel,
                                                  char tc_type
                                                  );

        [DllImport(_DRIVER_FILENAME, EntryPoint = "usb_tc08_get_single")]
        public static extern short TC08GetSingle(short handle,
                                                  float[] temp,
                                                  short *overflow_flags,
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

