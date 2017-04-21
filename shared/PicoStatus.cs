/**************************************************************************
 *
 *	Filename:			PicoStatus.cs
 *
 *	Description: 
 *	This file defines the status codes returned by a Pico device, a PC Oscilloscope 
 *   or data logger and is based on the PicoStatus.h header file.
 *
 *   In comments, "<API>" is a placeholder for the name of the scope or
 *   data logger API. For example, for the ps5000a API, it stands for
 *   "PS5000A" Or "ps5000a".
 *
 *   Copyright (C) 2017 Pico Technology Ltd. All rights reserved.
 *  
 **************************************************************************/

using System;

public class PicoStatus
{
    // PICO_INFO Values
    // ================

    public const uint PICO_DRIVER_VERSION                         = (uint) 0x00000000UL;
    public const uint PICO_USB_VERSION                            = (uint) 0x00000001UL;
    public const uint PICO_HARDWARE_VERSION                       = (uint) 0x00000002UL;
    public const uint PICO_VARIANT_INFO                           = (uint) 0x00000003UL;
    public const uint PICO_BATCH_AND_SERIAL                       = (uint) 0x00000004UL;
    public const uint PICO_CAL_DATE                               = (uint) 0x00000005UL;
    public const uint PICO_KERNEL_VERSION                         = (uint) 0x00000006UL;

    public const uint PICO_DIGITAL_HARDWARE_VERSION               = (uint) 0x00000007UL;
    public const uint PICO_ANALOGUE_HARDWARE_VERSION              = (uint) 0x00000008UL;

    public const uint PICO_FIRMWARE_VERSION_1                     = (uint) 0x00000009UL;
    public const uint PICO_FIRMWARE_VERSION_2                     = (uint) 0x0000000AUL;

    public const uint PICO_MAC_ADDRESS                            = (uint) 0x0000000BUL;

    public const uint PICO_SHADOW_CAL                             = (uint) 0x0000000CUL;

    public const uint PICO_IPP_VERSION                            = (uint) 0x0000000DUL;

    public const uint PICO_DRIVER_PATH                            = (uint) 0x0000000EUL;


    // PICO_STATUS Values
    // ==================

    // The PicoScope is functioning correctly.
    public const uint PICO_OK                                      = (uint) 0x00000000UL;

    // An attempt has been made to open more than <API>_MAX_UNITS.
    public const uint PICO_MAX_UNITS_OPENED                        = (uint) 0x00000001UL;

    // Not enough memory could be allocated on the host machine.
    public const uint PICO_MEMORY_FAIL                             = (uint) 0x00000002UL;

    // No Pico Technology device could be found.
    public const uint PICO_NOT_FOUND                               = (uint) 0x00000003UL;

    // Unable to download firmware.
    public const uint PICO_FW_FAIL                                 = (uint) 0x00000004UL;

    // The driver is busy opening a device.
    public const uint PICO_OPEN_OPERATION_IN_PROGRESS              = (uint) 0x00000005UL;

    // An unspecified failure occurred.
    public const uint PICO_OPERATION_FAILED                        = (uint) 0x00000006UL;

    // The PicoScope is not responding to commands from the PC.
    public const uint PICO_NOT_RESPONDING                          = (uint) 0x00000007UL;

    // The configuration information in the PicoScope is corrupt or missing.
    public const uint PICO_CONFIG_FAIL                             = (uint) 0x00000008UL;

    // The picopp.sys file is too old to be used with the device driver.
    public const uint PICO_KERNEL_DRIVER_TOO_OLD                   = (uint) 0x00000009UL;

    // The EEPROM has become corrupt, so the device will use a default setting.
    public const uint PICO_EEPROM_CORRUPT                          = (uint) 0x0000000AUL;

    // The operating system on the PC is not supported by this driver.
    public const uint PICO_OS_NOT_SUPPORTED                        = (uint) 0x0000000BUL;

    // There is no device with the handle value passed.
    public const uint PICO_INVALID_HANDLE                          = (uint) 0x0000000CUL;

    // A parameter value is not valid.
    public const uint PICO_INVALID_PARAMETER                       = (uint) 0x0000000DUL;

    // The timebase is not supported or is invalid.
    public const uint PICO_INVALID_TIMEBASE                        = (uint) 0x0000000EUL;

    // The voltage range is not supported or is invalid.
    public const uint PICO_INVALID_VOLTAGE_RANGE                   = (uint) 0x0000000FUL;

    // The channel number is not valid on this device or no channels have been set.
    public const uint PICO_INVALID_CHANNEL                         = (uint) 0x00000010UL;

    // The channel set for a trigger is not available on this device.
    public const uint PICO_INVALID_TRIGGER_CHANNEL                 = (uint) 0x00000011UL;

    // The channel set for a condition is not available on this device.
    public const uint PICO_INVALID_CONDITION_CHANNEL               = (uint) 0x00000012UL;

    // The device does not have a signal generator.
    public const uint PICO_NO_SIGNAL_GENERATOR                     = (uint) 0x00000013UL;

    // Streaming has failed to start or has stopped without user request.
    public const uint PICO_STREAMING_FAILED                        = (uint) 0x00000014UL;

    // Block failed to start - a parameter may have been set wrongly.
    public const uint PICO_BLOCK_MODE_FAILED                       = (uint) 0x00000015UL;

    // A parameter that was required is NULL.
    public const uint PICO_NULL_PARAMETER                          = (uint) 0x00000016UL;

    // The current functionality is not available while using ETS capture mode.
    public const uint PICO_ETS_MODE_SET                            = (uint) 0x00000017UL;

    // No data is available from a run block call.
    public const uint PICO_DATA_NOT_AVAILABLE                      = (uint) 0x00000018UL;

    // The buffer passed for the information was too small.
    public const uint PICO_STRING_BUFFER_TO_SMALL                  = (uint) 0x00000019UL;

    // ETS is not supported on this device.
    public const uint PICO_ETS_NOT_SUPPORTED                       = (uint) 0x0000001AUL;

    // The auto trigger time is less than the time it will take to collect the pre-trigger data.
    public const uint PICO_AUTO_TRIGGER_TIME_TO_SHORT              = (uint) 0x0000001BUL;

    // The collection of data has stalled as unread data would be overwritten.
    public const uint PICO_BUFFER_STALL                            = (uint) 0x0000001CUL;

    // Number of samples requested is more than available in the current memory segment.
    public const uint PICO_TOO_MANY_SAMPLES                        = (uint) 0x0000001DUL;

    // Not possible to create number of segments requested.
    public const uint PICO_TOO_MANY_SEGMENTS                       = (uint) 0x0000001EUL;

    // A null pointer has been passed in the trigger function or one of the parameters is out of range.
    public const uint PICO_PULSE_WIDTH_QUALIFIER                   = (uint) 0x0000001FUL;

    // One or more of the hold-off parameters are out of range.
    public const uint PICO_DELAY                                   = (uint) 0x00000020UL;

    // One or more of the source details are incorrect.
    public const uint PICO_SOURCE_DETAILS                          = (uint) 0x00000021UL;

    // One or more of the conditions are incorrect.
    public const uint PICO_CONDITIONS                              = (uint) 0x00000022UL;

    // The driver's thread is currently in the <API>Ready callback 
    // function and therefore the action cannot be carried out.
    public const uint PICO_USER_CALLBACK                           = (uint) 0x00000023UL;

    // An attempt is being made to get stored data while streaming. Either stop
    // streaming by calling <API>Stop, or use <API>GetStreamingLatestValues.
    public const uint PICO_DEVICE_SAMPLING                         = (uint) 0x00000024UL;

    // Data is unavailable because a run has not been completed.
    public const uint PICO_NO_SAMPLES_AVAILABLE                    = (uint) 0x00000025UL;

    // The memory segment index is out of range.
    public const uint PICO_SEGMENT_OUT_OF_RANGE                    = (uint) 0x00000026UL;

    // The device is busy so data cannot be returned yet.
    public const uint PICO_BUSY                                    = (uint) 0x00000027UL;

    // The start time to get stored data is out of range.
    public const uint PICO_STARTINDEX_INVALID                      = (uint) 0x00000028UL;

    // The information number requested is not a valid number.
    public const uint PICO_INVALID_INFO                            = (uint) 0x00000029UL;

    // The handle is invalid so no information is available about the device. 
    // Only PICO_DRIVER_VERSION is available.
    public const uint PICO_INFO_UNAVAILABLE                        = (uint) 0x0000002AUL;

    // The sample interval selected for streaming is out of range.
    public const uint PICO_INVALID_SAMPLE_INTERVAL                 = (uint) 0x0000002BUL;

    // ETS is set but no trigger has been set. A trigger setting is required for ETS.
    public const uint PICO_TRIGGER_ERROR                           = (uint) 0x0000002CUL;

    // Driver cannot allocate memory.
    public const uint PICO_MEMORY                                  = (uint) 0x0000002DUL;

    // Incorrect parameter passed to the signal generator.
    public const uint PICO_SIG_GEN_PARAM                           = (uint) 0x0000002EUL;

    // Conflict between the shots and sweeps parameters sent to the signal generator.
    public const uint PICO_SHOTS_SWEEPS_WARNING                    = (uint) 0x0000002FUL;

    // A software trigger has been sent but the trigger source is not a software trigger.
    public const uint PICO_SIGGEN_TRIGGER_SOURCE                   = (uint) 0x00000030UL;

    // An <API>SetTrigger call has found a conflict between the trigger source and the AUX output enable.
    public const uint PICO_AUX_OUTPUT_CONFLICT                     = (uint) 0x00000031UL;

    // ETS mode is being used and AUX is set as an input.
    public const uint PICO_AUX_OUTPUT_ETS_CONFLICT                 = (uint) 0x00000032UL;

    // Attempt to set different EXT input thresholds set for signal generator and oscilloscope trigger.
    public const uint PICO_WARNING_EXT_THRESHOLD_CONFLICT          = (uint) 0x00000033UL;

    // An <API>SetTrigger... function has set AUX as an output and the signal generator is using it as a trigger.
    public const uint PICO_WARNING_AUX_OUTPUT_CONFLICT             = (uint) 0x00000034UL;

    // The combined peak to peak voltage and the analog offset voltage exceed the maximum voltage the signal generator can produce.
    public const uint PICO_SIGGEN_OUTPUT_OVER_VOLTAGE              = (uint) 0x00000035UL;

    // NULL pointer passed as delay parameter.
    public const uint PICO_DELAY_NULL                              = (uint) 0x00000036UL;

    // The buffers for overview data have not been set while streaming.
    public const uint PICO_INVALID_BUFFER                          = (uint) 0x00000037UL;

    // The analog offset voltage is out of range.
    public const uint PICO_SIGGEN_OFFSET_VOLTAGE                   = (uint) 0x00000038UL;

    // The analog peak-to-peak voltage is out of range.
    public const uint PICO_SIGGEN_PK_TO_PK                         = (uint) 0x00000039UL;

    // A block collection has been cancelled.
    public const uint PICO_CANCELLED                               = (uint) 0x0000003AUL;

    // The segment index is not currently being used.
    public const uint PICO_SEGMENT_NOT_USED                        = (uint) 0x0000003BUL;

    // The wrong GetValues function has been called for the collection mode in use.
    public const uint PICO_INVALID_CALL                            = (uint) 0x0000003CUL;

    public const uint PICO_GET_VALUES_INTERRUPTED                  = (uint) 0x0000003DUL;

    // The function is not available.
    public const uint PICO_NOT_USED                                = (uint) 0x0000003FUL;

    // The aggregation ratio requested is out of range.
    public const uint PICO_INVALID_SAMPLERATIO                     = (uint) 0x00000040UL;

    // Device is in an invalid state.
    public const uint PICO_INVALID_STATE                           = (uint) 0x00000041UL;

    // The number of segments allocated is fewer than the number of captures requested.
    public const uint PICO_NOT_ENOUGH_SEGMENTS                     = (uint) 0x00000042UL;

    // A driver function has already been called and not yet finished.
    // Only one call to the driver can be made at any one time.
    public const uint PICO_DRIVER_FUNCTION                         = (uint) 0x00000043UL;

    // Not used
    public const uint PICO_RESERVED                                = (uint) 0x00000044UL;

    // An invalid coupling type was specified in <API>SetChannel.
    public const uint PICO_INVALID_COUPLING                        = (uint) 0x00000045UL;

    // An attempt was made to get data before a data buffer was defined.
    public const uint PICO_BUFFERS_NOT_SET                         = (uint) 0x00000046UL;

    // The selected downsampling mode (used for data reduction) is not allowed.
    public const uint PICO_RATIO_MODE_NOT_SUPPORTED                = (uint) 0x00000047UL;

    // Aggregation was requested in rapid block mode.
    public const uint PICO_RAPID_NOT_SUPPORT_AGGREGATION           = (uint) 0x00000048UL;

    // An invalid parameter was passed to <API>SetTriggerChannelProperties.
    public const uint PICO_INVALID_TRIGGER_PROPERTY                = (uint) 0x00000049UL;

    // The driver was unable to contact the oscilloscope.
    public const uint PICO_INTERFACE_NOT_CONNECTED                 = (uint) 0x0000004AUL;

    // Resistance-measuring mode is not allowed in conjunction with the specified probe.
    public const uint PICO_RESISTANCE_AND_PROBE_NOT_ALLOWED        = (uint) 0x0000004BUL;

    // The device was unexpectedly powered down.
    public const uint PICO_POWER_FAILED                            = (uint) 0x0000004CUL;

    // A problem occurred in <API>SetSigGenBuiltIn or <API>SetSigGenArbitrary.
    public const uint PICO_SIGGEN_WAVEFORM_SETUP_FAILED            = (uint) 0x0000004DUL;

    // FPGA not successfully set up.
    public const uint PICO_FPGA_FAIL                               = (uint) 0x0000004EUL;

    public const uint PICO_POWER_MANAGER                           = (uint) 0x0000004FUL;

    // An impossible analog offset value was specified in <API>SetChannel.
    public const uint PICO_INVALID_ANALOGUE_OFFSET                 = (uint) 0x00000050UL;

    // There is an error within the device hardware.
    public const uint PICO_PLL_LOCK_FAILED                         = (uint) 0x00000051UL;

    // There is an error within the device hardware.
    public const uint PICO_ANALOG_BOARD                            = (uint) 0x00000052UL;

    // Unable to configure the signal generator.
    public const uint PICO_CONFIG_FAIL_AWG                         = (uint) 0x00000053UL;

    // The FPGA cannot be initialized, so unit cannot be opened.
    public const uint PICO_INITIALISE_FPGA                         = (uint) 0x00000054UL;

    // The frequency for the external clock is not within 15% of the nominal value.
    public const uint PICO_EXTERNAL_FREQUENCY_INVALID              = (uint) 0x00000056UL;

    // The FPGA could not lock the clock signal.
    public const uint PICO_CLOCK_CHANGE_ERROR                      = (uint) 0x00000057UL;

    // You are trying to configure the AUX input as both a trigger and a reference clock.
    public const uint PICO_TRIGGER_AND_EXTERNAL_CLOCK_CLASH        = (uint) 0x00000058UL;

    // You are trying to congfigure the AUX input as both a pulse width qualifier and a reference clock.
    public const uint PICO_PWQ_AND_EXTERNAL_CLOCK_CLASH            = (uint) 0x00000059UL;

    // The requested scaling file cannot be opened.
    public const uint PICO_UNABLE_TO_OPEN_SCALING_FILE             = (uint) 0x0000005AUL;

    // The frequency of the memory is reporting incorrectly.
    public const uint PICO_MEMORY_CLOCK_FREQUENCY                  = (uint) 0x0000005BUL;

    // The I2C that is being actioned is not responding to requests.
    public const uint PICO_I2C_NOT_RESPONDING                      = (uint) 0x0000005CUL;

    // There are no captures available and therefore no data can be returned.
    public const uint PICO_NO_CAPTURES_AVAILABLE                   = (uint) 0x0000005DUL;

    // The number of trigger channels is greater than 4,
    // except for a PS4824 where 8 channels are allowed for rising/falling/rising_or_falling trigger directions.
    public const uint PICO_TOO_MANY_TRIGGER_CHANNELS_IN_USE				 = (uint) 0x0000005FUL;

    // When more than 4 trigger channels are set on a PS4824 and the direction is out of range.
    public const uint PICO_INVALID_TRIGGER_DIRECTION					= (uint) 0x00000060UL;

    //  When more than 4 trigger channels are set and their trigger condition states are not <API>_CONDITION_TRUE.
    public const uint PICO_INVALID_TRIGGER_STATES						 = (uint) 0x00000061UL;

    // The capture mode the device is currently running in does not support the current request.
    public const uint PICO_NOT_USED_IN_THIS_CAPTURE_MODE           = (uint) 0x0000005EUL;

    public const uint PICO_GET_DATA_ACTIVE                         = (uint) 0x00000103UL;

    // Codes 104 to 10B are used by the PT104 (USB) when connected via the Network Socket.

    // The device is currently connected via the IP Network socket and thus the call made is not supported.
    public const uint PICO_IP_NETWORKED                            = (uint) 0x00000104UL;

    // An incorrect IP address has been passed to the driver.
    public const uint PICO_INVALID_IP_ADDRESS                      = (uint) 0x00000105UL;

    // The IP socket has failed.
    public const uint PICO_IPSOCKET_FAILED                         = (uint) 0x00000106UL;

    // The IP socket has timed out.
    public const uint PICO_IPSOCKET_TIMEDOUT                       = (uint) 0x00000107UL;

    // Failed to apply the requested settings.
    public const uint PICO_SETTINGS_FAILED                         = (uint) 0x00000108UL;

    // The network connection has failed.
    public const uint PICO_NETWORK_FAILED                          = (uint) 0x00000109UL;

    // Unable to load the WS2 DLL.
    public const uint PICO_WS2_32_DLL_NOT_LOADED                   = (uint) 0x0000010AUL;

    // The specified IP port is invalid.
    public const uint PICO_INVALID_IP_PORT                         = (uint) 0x0000010BUL;

    // The type of coupling requested is not supported on the opened device.
    public const uint PICO_COUPLING_NOT_SUPPORTED                  = (uint) 0x0000010CUL;

    // Bandwidth limiting is not supported on the opened device.
    public const uint PICO_BANDWIDTH_NOT_SUPPORTED                 = (uint) 0x0000010DUL;

    // The value requested for the bandwidth limit is out of range.
    public const uint PICO_INVALID_BANDWIDTH                       = (uint) 0x0000010EUL;

    // The arbitrary waveform generator is not supported by the opened device.
    public const uint PICO_AWG_NOT_SUPPORTED                       = (uint) 0x0000010FUL;

    // Data has been requested with ETS mode set but run block has not been called, 
    // or stop has been called.
    public const uint PICO_ETS_NOT_RUNNING                         = (uint) 0x00000110UL;

    // White noise output is not supported on the opened device.
    public const uint PICO_SIG_GEN_WHITENOISE_NOT_SUPPORTED        = (uint) 0x00000111UL;

    // The wave type requested is not supported by the opened device.
    public const uint PICO_SIG_GEN_WAVETYPE_NOT_SUPPORTED          = (uint) 0x00000112UL;

    // The requested digital port number is out of range (MSOs only).
    public const uint PICO_INVALID_DIGITAL_PORT                    = (uint) 0x00000113UL;

    // The digital channel is not in the range <API>_DIGITAL_CHANNEL0 to
    // <API>_DIGITAL_CHANNEL15, the digital channels that are supported.
    public const uint PICO_INVALID_DIGITAL_CHANNEL                 = (uint) 0x00000114UL;

    // The digital trigger direction is not a valid trigger direction and should be equal
    // in value to one of the <API>_DIGITAL_DIRECTION enumerations.
    public const uint PICO_INVALID_DIGITAL_TRIGGER_DIRECTION       = (uint) 0x00000115UL;

    // Signal generator does not generate pseudo-random binary sequence.
    public const uint PICO_SIG_GEN_PRBS_NOT_SUPPORTED              = (uint) 0x00000116UL;

    // When a digital port is enabled, ETS sample mode is not available for use.
    public const uint PICO_ETS_NOT_AVAILABLE_WITH_LOGIC_CHANNELS   = (uint) 0x00000117UL;

    public const uint PICO_WARNING_REPEAT_VALUE                    = (uint) 0x00000118UL;

    // 4-channel scopes only: The DC power supply is connected.
    public const uint PICO_POWER_SUPPLY_CONNECTED                  = (uint) 0x00000119UL;

    // 4-channel scopes only: The DC power supply is not connected.
    public const uint PICO_POWER_SUPPLY_NOT_CONNECTED              = (uint) 0x0000011AUL;

    // Incorrect power mode passed for current power source.
    public const uint PICO_POWER_SUPPLY_REQUEST_INVALID            = (uint) 0x0000011BUL;

    // The supply voltage from the USB source is too low.
    public const uint PICO_POWER_SUPPLY_UNDERVOLTAGE               = (uint) 0x0000011CUL;

    // The oscilloscope is in the process of capturing data.
    public const uint PICO_CAPTURING_DATA                          = (uint) 0x0000011DUL;

    // A USB 3.0 device is connected to a non-USB 3.0 port.
    public const uint PICO_USB3_0_DEVICE_NON_USB3_0_PORT           = (uint) 0x0000011EUL;

    // A function has been called that is not supported by the current device.
    public const uint PICO_NOT_SUPPORTED_BY_THIS_DEVICE            = (uint) 0x0000011FUL;

    // The device resolution is invalid (out of range).
    public const uint PICO_INVALID_DEVICE_RESOLUTION               = (uint) 0x00000120UL;

    // The number of channels that can be enabled is limited in 15 and 16-bit modes.
    // (Flexible Resolution Oscilloscopes only)
    public const uint PICO_INVALID_NUMBER_CHANNELS_FOR_RESOLUTION  = (uint) 0x00000121UL;

    // USB power not sufficient for all requested channels.
    public const uint PICO_CHANNEL_DISABLED_DUE_TO_USB_POWERED     = (uint) 0x00000122UL;

    // The signal generator does not have a configurable DC offset.
    public const uint PICO_SIGGEN_DC_VOLTAGE_NOT_CONFIGURABLE      = (uint) 0x00000123UL;

    // An attempt has been made to define pre-trigger delay without first enabling a trigger.
    public const uint PICO_NO_TRIGGER_ENABLED_FOR_TRIGGER_IN_PRE_TRIG   = (uint) 0x00000124UL;

    // An attempt has been made to define pre-trigger delay without first arming a trigger.
    public const uint PICO_TRIGGER_WITHIN_PRE_TRIG_NOT_ARMED            = (uint) 0x00000125UL;

    // Pre-trigger delay and post-trigger delay cannot be used at the same time.
    public const uint PICO_TRIGGER_WITHIN_PRE_NOT_ALLOWED_WITH_DELAY    = (uint) 0x00000126UL;

    // The array index points to a nonexistent trigger.
    public const uint PICO_TRIGGER_INDEX_UNAVAILABLE                    = (uint) 0x00000127UL;

    public const uint PICO_AWG_CLOCK_FREQUENCY									 = (uint) 0x00000128UL;

    // There are more 4 analog channels with a trigger condition set.
    public const uint PICO_TOO_MANY_CHANNELS_IN_USE							 = (uint) 0x00000129UL;

    // The condition parameter is a null pointer.
    public const uint PICO_NULL_CONDITIONS											 = (uint) 0x0000012AUL;

    // There is more than one condition pertaining to the same channel.
    public const uint PICO_DUPLICATE_CONDITION_SOURCE						 = (uint) 0x0000012BUL;	

    // The parameter relating to condition information is out of range.
    public const uint PICO_INVALID_CONDITION_INFO								 = (uint) 0x0000012CUL;	

    // Reading the metadata has failed.
    public const uint PICO_SETTINGS_READ_FAILED									 = (uint) 0x0000012DUL;

    // Writing the metadata has failed.
    public const uint PICO_SETTINGS_WRITE_FAILED								 = (uint) 0x0000012EUL;

    // A parameter has a value out of the expected range.
    public const uint PICO_ARGUMENT_OUT_OF_RANGE								 = (uint) 0x0000012FUL;

    // The driver does not support the hardware variant connected.
    public const uint PICO_HARDWARE_VERSION_NOT_SUPPORTED				 = (uint) 0x00000130UL;

    // The driver does not support the digital hardware variant connected.
    public const uint PICO_DIGITAL_HARDWARE_VERSION_NOT_SUPPORTED				 = (uint) 0x00000131UL;

    // The driver does not support the analog hardware variant connected.
    public const uint PICO_ANALOGUE_HARDWARE_VERSION_NOT_SUPPORTED				 = (uint) 0x00000132UL;

    // Converting a channel's ADC value to resistance has failed.
    public const uint PICO_UNABLE_TO_CONVERT_TO_RESISTANCE			 = (uint) 0x00000133UL;

    // The channel is listed more than once in the function call.
    public const uint PICO_DUPLICATED_CHANNEL										 = (uint) 0x00000134UL;

    // The range cannot have resistance conversion applied.
    public const uint PICO_INVALID_RESISTANCE_CONVERSION				 = (uint) 0x00000135UL;

    // An invalid value is in the max buffer.
    public const uint PICO_INVALID_VALUE_IN_MAX_BUFFER					 = (uint) 0x00000136UL;

    // An invalid value is in the min buffer.
    public const uint PICO_INVALID_VALUE_IN_MIN_BUFFER					 = (uint) 0x00000137UL;

    // When calculating the frequency for phase conversion,  
    // the frequency is greater than that supported by the current variant.
    public const uint PICO_SIGGEN_FREQUENCY_OUT_OF_RANGE				 = (uint) 0x00000138UL;

    // The device's EEPROM is corrupt. Contact Pico Technology support: https://www.picotech.com/tech-support.
    public const uint PICO_EEPROM2_CORRUPT											 = (uint) 0x00000139UL;

    // The EEPROM has failed.
    public const uint PICO_EEPROM2_FAIL													 = (uint) 0x0000013AUL;

    // The serial buffer is too small for the required information.
    public const uint PICO_SERIAL_BUFFER_TOO_SMALL							 = (uint) 0x0000013BUL;

    // The signal generator trigger and the external clock have both been set.
    // This is not allowed.
    public const uint PICO_SIGGEN_TRIGGER_AND_EXTERNAL_CLOCK_CLASH  = (uint) 0x0000013CUL;

    // The AUX trigger was enabled and the external clock has been enabled, 
    // so the AUX has been automatically disabled.
    public const uint PICO_WARNING_SIGGEN_AUXIO_TRIGGER_DISABLED  = (uint) 0x0000013DUL;

    // The AUX I/O was set as a scope trigger and is now being set as a signal generator
    // gating trigger. This is not allowed.
    public const uint PICO_SIGGEN_GATING_AUXIO_NOT_AVAILABLE	 	  = (uint) 0x00000013EUL;

    // The AUX I/O was set by the signal generator as a gating trigger and is now being set 
    // as a scope trigger. This is not allowed.
    public const uint PICO_SIGGEN_GATING_AUXIO_ENABLED				 	  = (uint) 0x00000013FUL;

    // A resource has failed to initialise 
    public const uint PICO_RESOURCE_ERROR = (uint) 0x00000140UL;

    // The temperature type is out of range
    public const uint PICO_TEMPERATURE_TYPE_INVALID							  = (uint) 0x000000141UL;

    // A requested temperature type is not supported on this device
    public const uint PICO_TEMPERATURE_TYPE_NOT_SUPPORTED				  = (uint) 0x000000142UL;

    // A read/write to the device has timed out
    public const uint PICO_TIMEOUT										= (uint) 0x00000143UL;

    // The device cannot be connected correctly
    public const uint PICO_DEVICE_NOT_FUNCTIONING							    = (uint) 0x00000144UL;

    // The driver has experienced an unknown error and is unable to recover from this error
    public const uint PICO_INTERNAL_ERROR												  = (uint) 0x00000145UL;

    // Used when opening units via IP and more than multiple units have the same ip address
    public const uint PICO_MULTIPLE_DEVICES_FOUND								  = (uint) 0x00000146UL;

    public const uint PICO_WARNING_NUMBER_OF_SEGMENTS_REDUCED 	 					 = (uint) 0x00000147UL;

    // the calibration pin states argument is out of range
    public const uint PICO_CAL_PINS_STATES											 = (uint) 0x00000148UL;

    // the calibration pin frequency argument is out of range
    public const uint PICO_CAL_PINS_FREQUENCY										 = (uint) 0x00000149UL;

    // the calibration pin amplitude argument is out of range
    public const uint PICO_CAL_PINS_AMPLITUDE										 = (uint) 0x0000014AUL;

    // the calibration pin wavetype argument is out of range
    public const uint PICO_CAL_PINS_WAVETYPE										 = (uint) 0x0000014BUL;

    // the calibration pin offset argument is out of range
    public const uint PICO_CAL_PINS_OFFSET											 = (uint) 0x0000014CUL;

    // the probe's identity has a problem
    public const uint PICO_PROBE_FAULT												= (uint) 0x0000014DUL;

    // the probe has not been identified
    public const uint PICO_PROBE_IDENTITY_UNKNOWN								 = (uint) 0x0000014EUL;

    // enabling the probe would cause the device to exceed the allowable current limit
    public const uint PICO_PROBE_POWER_DC_POWER_SUPPLY_REQUIRED  = (uint) 0x0000014FUL;

    // the DC power supply is connected; enabling the probe would cause the device to exceed the
    // allowable current limit
    public const uint PICO_PROBE_NOT_POWERED_WITH_DC_POWER_SUPPLY  = (uint) 0x00000150UL;

    // failed to complete probe configuration
    public const uint PICO_PROBE_CONFIG_FAILURE									 = (uint) 0x00000151UL;

    // failed to set the callback function, as currently in current callback function
    public const uint PICO_PROBE_INTERACTION_CALLBACK						 = (uint) 0x00000152UL;

    // the probe has been verified but not know on this driver
    public const uint PICO_UNKNOWN_INTELLIGENT_PROBE						 = (uint) 0x00000153UL;

    // the intelligent probe cannot be verified
    public const uint PICO_INTELLIGENT_PROBE_CORRUPT						 = (uint) 0x00000154UL;

    // the callback is null, probe collection will only start when 
    // first callback is a none null pointer
    public const uint PICO_PROBE_COLLECTION_NOT_STARTED					 = (uint) 0x00000155UL;

    // the current drawn by the probe(s) has exceeded the allowed limit
    public const uint PICO_PROBE_POWER_CONSUMPTION_EXCEEDED     = (uint) 0x00000156UL;

    // the channel range limits have changed due to connecting or disconnecting a probe
    // the channel has been enabled
    public const uint PICO_WARNING_PROBE_CHANNEL_OUT_OF_SYNC  = (uint) 0x00000157UL;

    // The time stamp per waveform segment has been reset.
    public const uint PICO_DEVICE_TIME_STAMP_RESET				= (uint) 0x01000000UL;

    // An internal erorr has occurred and a watchdog timer has been called.
    public const uint PICO_WATCHDOGTIMER                         = (uint) 0x10000000UL;

    // The picoipp.dll has not been found.
    public const uint PICO_IPP_NOT_FOUND                		= (uint) 0x10000001UL;

    // A function in the picoipp.dll does not exist.
    public const uint PICO_IPP_NO_FUNCTION					    = (uint) 0x10000002UL;

    // The Pico IPP call has failed.
    public const uint PICO_IPP_ERROR					        = (uint) 0x10000003UL;

    // Shadow calibration is not available on this device.
    public const uint PICO_SHADOW_CAL_NOT_AVAILABLE              = (uint) 0x10000004UL;

    // Shadow calibration is currently disabled.
    public const uint PICO_SHADOW_CAL_DISABLED                   = (uint) 0x10000005UL;

    // Shadow calibration error has occurred.
    public const uint PICO_SHADOW_CAL_ERROR                      = (uint) 0x10000006UL;

    // The shadow calibration is corrupt.
    public const uint PICO_SHADOW_CAL_CORRUPT                    = (uint) 0x10000007UL;

    // the memory onboard the device has overflowed
    public const uint PICO_DEVICE_MEMORY_OVERFLOW                = (uint) 0x10000008UL;

    public const uint PICO_RESERVED_1							= (uint) 0x11000000UL;

    public PicoStatus()
	{
	}
}
