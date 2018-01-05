/******************************************************************************
*
* Filename: PLCM3Imports.cs
*  
* Description:
*  This file contains .NET wrapper calls correseponding to  
*  function calls defined in the plcm3Api.h C header file. 
*  It also has the enums and structs required by the (wrapped) 
*  function calls.
*   
* Copyright © 2011-2018 Pico Technology Ltd. See LICENSE file for terms.
*
******************************************************************************/

using System;
using System.Runtime.InteropServices;
using System.Text;

namespace PLCM3Imports
{
	class Imports
	{
		#region constants
		private const string _DRIVER_FILENAME = "plcm3.dll";

        public const int NUM_CHANNELS = 3;

		#endregion

		#region Driver enums

        public enum enPLCM3Channels : uint
        {
            PLCM3_CHANNEL_1 = 1,
            PLCM3_CHANNEL_2,
            PLCM3_CHANNEL_3,
            PLCM3_MAX_CHANNELS = PLCM3_CHANNEL_3
        }

        public enum enPLCM3DataType : uint
        {
            PLCM3_OFF,
            PLCM3_1_MILLIVOLT,
            PLCM3_10_MILLIVOLTS,
            PLCM3_100_MILLIVOLTS,
            PLCM3_VOLTAGE
        }

        public enum enIpDetailsType : uint
        {
            PLCM3_IDT_GET,
            PLCM3_IDT_SET,
        }

        public enum enCommunicationType : uint
        {
            PLCM3_CT_USB = 0x00000001,
            PLCM3_CT_ETHERNET = 0x00000002,
            PLCM3_CT_ALL = 0xFFFFFFFF
        }

		#endregion

		#region Driver Imports
		

        [DllImport(_DRIVER_FILENAME, EntryPoint = "PLCM3CloseUnit")]
        public static extern uint CloseUnit(short handle);

        [DllImport(_DRIVER_FILENAME, EntryPoint = "PLCM3Enumerate")]
        public static extern uint Enumerate(StringBuilder details,
                                              ref uint length,
                                              enCommunicationType comType);

        [DllImport(_DRIVER_FILENAME, EntryPoint = "PLCM3GetUnitInfo")]
        public static extern uint GetUnitInfo(short handle,
                                              StringBuilder infoString,
                                              short strLength,
                                              out short reqSize,
                                              uint info);

        [DllImport(_DRIVER_FILENAME, EntryPoint = "PLCM3GetValue")]
        public static extern uint GetValue(short handle,
                                              enPLCM3Channels channel,
                                              out uint value);


        [DllImport(_DRIVER_FILENAME, EntryPoint = "PLCM3IpDetails")]
        public static extern uint IpDetails(short handle,
                                               ref short enabled,
                                               StringBuilder ipaddress,
                                               ref ushort length,
                                               ref ushort listeningPort,
                                               enIpDetailsType type);


        [DllImport(_DRIVER_FILENAME, EntryPoint = "PLCM3OpenUnit")]
        public static extern uint OpenUnit(out short handle,
                                             StringBuilder serial);

        [DllImport(_DRIVER_FILENAME, EntryPoint = "PLCM3OpenUnitViaIp")]
        public static extern uint OpenUnitViaIp(out short handle,
                                                  StringBuilder serial,
                                                  String ipAddress);

        [DllImport(_DRIVER_FILENAME, EntryPoint = "PLCM3SetChannel")]
        public static extern uint SetChannel(short handle,
                                                    enPLCM3Channels channel,
                                                    enPLCM3DataType type);
											
        [DllImport(_DRIVER_FILENAME, EntryPoint = "PLCM3SetMains")]
        public static extern uint SetMains(out short handle,
                                                ushort sixty_hertz);
	
		#endregion
	}
}

