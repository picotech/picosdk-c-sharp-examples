/******************************************************************************
*
* Filename: USBPT104Imports.cs
*  
* Description:
*  This file contains .NET wrapper calls correseponding to  
*  function calls defined in the usbPT104Api.h C header file. 
*  It also has the enums and structs required by the (wrapped) 
*  function calls.
*   
* Copyright © 2015-2018 Pico Technology Ltd. See LICENSE file for terms.
*
******************************************************************************/

using System.Runtime.InteropServices;
using System.Text;

namespace USBPT104Imports
{
	class Imports
	{
		#region constants

        private const string _DRIVER_FILENAME = "USBPT104.dll";

        public const short USBPT104_OFF = 1;
        public const short  USBPT104_MIN_WIRES  = 2;
        public const short  USBPT104_3_WIRES    = 3;
        public const short  USBPT104_MAX_WIRES  = 4;

        #endregion

		#region Driver enums

        public enum UsbPt104Channels : uint
        {
            USBPT104_CHANNEL_1 = 1,
            USBPT104_CHANNEL_2,
            USBPT104_CHANNEL_3,
            USBPT104_CHANNEL_4,
            USBPT104_CHANNEL_5,
            USBPT104_CHANNEL_6,
            USBPT104_CHANNEL_7,
            USBPT104_CHANNEL_8,
            USBPT104_MAX_CHANNELS = USBPT104_CHANNEL_8
        } // USBPT104_CHANNELS

        public enum UsbPt104DataType : uint
        {
            USBPT104_OFF,
            USBPT104_PT100,
            USBPT104_PT1000,
            USBPT104_RESISTANCE_TO_375R,
            USBPT104_RESISTANCE_TO_10K,
            USBPT104_DIFFERENTIAL_TO_115MV,
            USBPT104_DIFFERENTIAL_TO_2500MV,
            USBPT104_SINGLE_ENDED_TO_115MV ,
            USBPT104_SINGLE_ENDED_TO_2500MV,
            USBPT104_MAX_DATA_TYPES

        } //USBPT104_DATA_TYPES;

        public enum IpDetailsType : uint
        {
            IDT_GET,
            IDT_SET,
        } //IP_DETAILS_TYPE;

        public enum CommunicationType :uint
        {
            CT_USB = 0x00000001,
            CT_ETHERNET = 0x00000002,
            CT_ALL =  0xFFFFFFFF
        } //COMMUNICATION_TYPE;

		#endregion

		#region Driver Imports
		#region Callback delegates
		
		#endregion

        [DllImport(_DRIVER_FILENAME, EntryPoint = "UsbPt104OpenUnit")]
        public static extern uint UsbPt104OpenUnit(out short handle,
                                                    StringBuilder serial
                                                    );

        [DllImport(_DRIVER_FILENAME, EntryPoint = "UsbPt104CloseUnit")]
        public static extern uint UsbPt104CloseUnit(short handle);


        [DllImport(_DRIVER_FILENAME, EntryPoint = "UsbPt104Enumerate")]
        public static extern uint UsbPt104Enumerate(StringBuilder details,
                                                        ref uint stringLength,
                                                        CommunicationType type
                                                    );

        [DllImport(_DRIVER_FILENAME, EntryPoint = "UsbPt104GetUnitInfo")]
        public static extern uint UsbPt104GetUnitInfo(short handle,
                                                        StringBuilder str,
                                                        short stringLength,
                                                        ref short requiredSize,
                                                        uint picoInfo
                                                    );

        [DllImport(_DRIVER_FILENAME, EntryPoint = "UsbPt104GetValue")]
        public static extern uint UsbPt104GetValue(short handle,
                                                    UsbPt104Channels channel,
                                                    out int value,
                                                    short filtered
                                                    );

        [DllImport(_DRIVER_FILENAME, EntryPoint = "UsbPt104IpDetails")]
        public static extern uint UsbPt104IpDetails(short handle,
                                                        ref short enabled,
                                                        StringBuilder ipaddress,
                                                        ref ushort length,
                                                        ref ushort listeningPort,
                                                        IpDetailsType type
                                                    );

        [DllImport(_DRIVER_FILENAME, EntryPoint = "UsbPt104OpenUnitViaIp")]
        public static extern uint UsbPt104OpenUnitViaIp(out short handle,
                                                            StringBuilder serial,
                                                            StringBuilder ipAddress
                                                        );

        [DllImport(_DRIVER_FILENAME, EntryPoint = "UsbPt104SetChannel")]
        public static extern uint UsbPt104SetChannel(short handle,
                                                        UsbPt104Channels channel,
                                                        UsbPt104DataType type,
                                                        short noOfWires
                                                    );

        [DllImport(_DRIVER_FILENAME, EntryPoint = "UsbPt104SetMains")]
        public static extern uint UsbPt104SetMains(short handle,
                                                    ushort sixty_hertz
                                                    );

		#endregion
	}
}

