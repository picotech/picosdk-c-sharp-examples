/******************************************************************************
*
* Filename: PL1000Imports.cs
*  
* Description:
*  This file contains .NET wrapper calls corresponding to function calls 
*  defined in the pl1000Api.h C header file. 
*  It also has the enums required by the (wrapped) function calls.
*   
* Copyright © 2012-2018 Pico Technology Ltd. See LICENSE file for terms.
*
******************************************************************************/

using System.Runtime.InteropServices;
using System.Text;

namespace PL1000Imports
{
  class Imports
  {
    #region Constants
    private const string _DRIVER_FILENAME = "pl1000.dll";

    #endregion

    #region Driver Enums

    public enum PicoErrorCode
    {
      PICO_OK = 0,
      PICO_MAX_UNITS_OPENED = 1,
      PICO_MEMORY_FAIL = 2,
      PICO_NOT_FOUND = 3,
      PICO_FW_FAIL = 4,
      PICO_OPEN_OPERATION_IN_PROGRESS = 5,
      PICO_OPERATION_FAILED = 6,
      PICO_NOT_RESPONDING = 7,
      PICO_CONFIG_FAIL = 8,
      PICO_KERNEL_DRIVER_TOO_OLD = 9,
      PICO_EEPROM_CORRUPT = 0x0A,
      PICO_OS_NOT_SUPPORTED = 0x0B,
      PICO_INVALID_HANDLE = 0x0C,
      PICO_INVALID_PARAMETER = 0x0D,
      PICO_INVALID_TIMEBASE = 0x0E,
      PICO_INVALID_VOLTAGE_RANGE = 0x0F,
      PICO_INVALID_CHANNEL = 0x10,
      PICO_INVALID_TRIGGER_CHANNEL = 0x11,
      PICO_INVALID_CONDITION_CHANNEL = 0x12,
      PICO_NO_SIGNAL_GENERATOR = 0x13,
      PICO_STREAMING_FAILED = 0x14,
      PICO_BLOCK_MODE_FAILED = 0x15,
      PICO_NULL_PARAMETER = 0x16,
      PICO_DATA_NOT_AVAILABLE = 0x18,
      PICO_STRING_BUFFER_TOO_SMALL = 0x19,
      PICO_ETS_NOT_SUPPORTED = 0x1A,
      PICO_AUTO_TRIGGER_TIME_TOO_SHORT = 0x1B,
      PICO_BUFFER_STALL = 0x1C,
      PICO_TOO_MANY_SAMPLES = 0x1D,
      PICO_TOO_MANY_SEGMENTS = 0x1E,
      PICO_PULSE_WIDTH_QUALIFIER = 0x1F,
      PICO_DELAY = 0x20,
      PICO_SOURCE_DETAILS = 0x21,
      PICO_CONDITIONS = 0x22,
      PICO_DEVICE_SAMPLING = 0x24,
      PICO_NO_SAMPLES_AVAILABLE = 0x25,
      PICO_SEGMENT_OUT_OF_RANGE = 0x26,
      PICO_BUSY = 0x27,
      PICO_STARTINDEX_INVALID = 0x28,
      PICO_INVALID_INFO = 0x29,
      PICO_INFO_UNAVAILABLE = 0x2A,
      PICO_INVALID_SAMPLE_INTERVAL = 0x2B,
      PICO_TRIGGER_ERROR = 0x2C,
      PICO_MEMORY = 0x2D,
      PICO_DELAY_NULL = 0x36,
      PICO_INVALID_BUFFER = 0x37,
      PICO_CANCELLED = 0x3A,
      PICO_SEGMENT_NOT_USED = 0x3B,
      PICO_NOT_USED = 0x3F,
      PICO_INVALID_STATE = 0x41,
      PICO_DRIVE_FUNCTION = 0x43
    }

    public enum enPL1000Inputs
    {
      PL1000_CHANNEL_1 = 1,
      PL1000_CHANNEL_2,
      PL1000_CHANNEL_3,
      PL1000_CHANNEL_4,
      PL1000_CHANNEL_5,
      PL1000_CHANNEL_6,
      PL1000_CHANNEL_7,
      PL1000_CHANNEL_8,
      PL1000_CHANNEL_9,
      PL1000_CHANNEL_10,
      PL1000_CHANNEL_11,
      PL1000_CHANNEL_12,
      PL1000_CHANNEL_13,
      PL1000_CHANNEL_14,
      PL1000_CHANNEL_15,
      PL1000_CHANNEL_16,
      PL1000_MAX_CHANNELS = PL1000_CHANNEL_16
    }

    public enum enPL1000DO_Channel
    {
      PL1000_DO_CHANNEL_0,
      PL1000_DO_CHANNEL_1,
      PL1000_DO_CHANNEL_2,
      PL1000_DO_CHANNEL_3,
      PL1000_DO_CHANNEL_MAX
    }

    public enum enPL1000OpenProgress
    {
      PL1000_OPEN_PROGRESS_FAIL = -1,
      PL1000_OPEN_PROGRESS_PENDING = 0,
      PL1000_OPEN_PROGRESS_COMPLETE = 1
    }

    public enum enPL1000Method
    {
      SINGLE,
      WINDOW,
      STREAM,
      MAX_METHOD = STREAM
    }

    #endregion

    #region Driver Imports


    [DllImport(_DRIVER_FILENAME, EntryPoint = "pl1000CloseUnit")]
    public static extern PicoErrorCode CloseUnit(short handle);

    [DllImport(_DRIVER_FILENAME, EntryPoint = "pl1000GetUnitInfo")]
    public static extern PicoErrorCode GetUnitInfo(short handle,
                                          StringBuilder infoString,
                                          short strlength,
                                          out short reqSize,
                                          int info);

    [DllImport(_DRIVER_FILENAME, EntryPoint = "pl1000OpenUnit")]
    public static extern PicoErrorCode OpenUnit(out short handle);

    [DllImport(_DRIVER_FILENAME, EntryPoint = "pl1000GetSingle")]
    public static extern PicoErrorCode Enumerate(short handle,
                                          enPL1000Inputs channel,
                                          out ushort value);

    [DllImport(_DRIVER_FILENAME, EntryPoint = "pl1000GetValues")]
    public static extern PicoErrorCode GetValue(short handle,
                                        ushort[] value,
                                        ref uint noOfValues,
                                        out ushort overflow,
                                        out uint triggerIndex);

    [DllImport(_DRIVER_FILENAME, EntryPoint = "pl1000MaxValue")]
    public static extern PicoErrorCode MaxValue(short handle,
                                        out ushort maxvalue);

    [DllImport(_DRIVER_FILENAME, EntryPoint = "pl1000OpenUnitAsync")]
    public static extern PicoErrorCode OpenUnitAsync(out short handle);

    [DllImport(_DRIVER_FILENAME, EntryPoint = "pl1000OpenUnitProgress")]
    public static extern PicoErrorCode pl1000OpenUnitProgress(out short handle,
                                                        out short progress,
                                                        out short complete);

    [DllImport(_DRIVER_FILENAME, EntryPoint = "pl1000Ready")]
    public static extern PicoErrorCode pl1000Ready(short handle,
                                            out short ready);

    [DllImport(_DRIVER_FILENAME, EntryPoint = "pl1000Run")]
    public static extern PicoErrorCode Run(short handle,
                                    uint NoOfValues,
                                    enPL1000Method method);

    [DllImport(_DRIVER_FILENAME, EntryPoint = "pl1000SetDo")]
    public static extern PicoErrorCode SetDo(short handle,
                                        short do_value,
                                        short DOchannel);

    [DllImport(_DRIVER_FILENAME, EntryPoint = "pl1000SetInterval")]
    public static extern PicoErrorCode SetInterval(short handle,
                                            ref uint usforblock,
                                            uint IdealNoOfSamples,
                                            short[] channels,
                                            short NoofChannels);

    [DllImport(_DRIVER_FILENAME, EntryPoint = "pl1000SetTrigger")]
    public static extern PicoErrorCode SetTrigger(short handle,
                                          ushort enabled,
                                          ushort autoTrigger,
                                          ushort autoMS,
                                          enPL1000Inputs channel,
                                          ushort dir,
                                          ushort threshold,
                                          ushort hysteresis,
                                          float delay);

    [DllImport(_DRIVER_FILENAME, EntryPoint = "pl1000Stop")]
    public static extern PicoErrorCode stop(short handle);

    #endregion
  }
}
