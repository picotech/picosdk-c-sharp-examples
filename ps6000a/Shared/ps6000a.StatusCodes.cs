using System.Reflection;

namespace DriverImports
{
  public enum StandardDriverStatusCode : uint
  {
    Ok = 0x00000000,
    MaxUnitsOpened = 0x00000001,
    MemoryFail = 0x00000002,
    NotFound = 0x00000003,
    FirmwareFail = 0x00000004,
    OpenOperationInProgress = 0x00000005,
    OperationFailed = 0x00000006,
    NotResponding = 0x00000007,
    ConfigFail = 0x00000008,
    KernelDriverTooOld = 0x00000009,
    EepromCorrupt = 0x0000000A,
    OSNotSupported = 0x0000000B,
    InvalidHandle = 0x0000000C,
    InvalidParameter = 0x0000000D,
    InvalidTimebase = 0x0000000E,
    InvalidRange = 0x0000000F,
    InvalidChannel = 0x00000010,
    InvalidTriggerChannel = 0x00000011,
    InvalidChannelCondition = 0x00000012,
    NoSignalGenerator = 0x00000013,
    StreamingFailed = 0x00000014,
    BlockModeFailed = 0x00000015,
    NullParameter = 0x00000016,
    EtsModeSet = 0x00000017,
    DataNotAvailable = 0x00000018,
    StringBufferTooSmall = 0x00000019,
    EtsNotSupported = 0x0000001A,
    AutoTriggerTimeToReset = 0x0000001B,
    BufferStall = 0x0000001C,
    TooManySamples = 0x0000001D,
    TooManySegments = 0x0000001E,
    PulseWidthQualifier = 0x0000001F,
    Delay = 0x00000020,
    SourceDetails = 0x00000021,
    Conditions = 0x00000022,
    UserCallback = 0x00000023,
    DeviceSampling = 0x00000024,
    NoSamplesAvailable = 0x00000025,
    SegmentOutOfRange = 0x00000026,
    Busy = 0x00000027,
    StartIndexInvalid = 0x00000028,
    InvalidInfo = 0x00000029,
    InfoUnavailable = 0x0000002A,
    InvalidSampleInterval = 0x0000002B,
    TriggerError = 0x0000002C,
    Memory = 0x0000002D,
    SiggenParam = 0x0000002E,
    ShotsSweepsWarning = 0x0000002F,
    SiggenTriggerSource = 0x00000030,
    AuxOutputConflict = 0x00000031,
    AuxOutputEtsConflict = 0x00000032,
    WarningExtThresholdConflict = 0x00000033,
    WarningAuxOutputConflict = 0x00000034,
    SiggenOutputOverVoltage = 0x00000035,
    DelayNull = 0x00000036,
    InvalidBuffer = 0x00000037,
    SiggenOffsetVoltage = 0x00000038,
    SiggenPeakToPeak = 0x00000039,
    Canceled = 0x0000003A,
    SegmentNotUsed = 0x0000003B,
    InvalidCall = 0x0000003C,
    GetValuesInterrupted = 0x0000003D,
    NotUsed = 0x0000003F,
    InvalidDownsamplingRatio = 0x00000040,
    InvalidState = 0x00000041,
    NotEnoughSegments = 0x00000042,
    DriverFunction = 0x00000043,
    Reserved = 0x00000044,
    InvalidCoupling = 0x00000045,
    BuffersNotSet = 0x00000046,
    RatioModeNotSupported = 0x00000047,
    RapidNotSupportAggregation = 0x00000048,
    InvalidTriggerProperty = 0x00000049,
    InterfaceNotConnected = 0x0000004A,
    ResistanceAndProbeNotAllowed = 0x0000004B,
    PowerFailed = 0x0000004C,
    SiggenWaveformSetupFailed = 0x0000004D,
    FpgaFail = 0x0000004E,
    PowerManager = 0x0000004F,
    InvalidAnalogueOffset = 0x00000050,
    PllLockFailed = 0x00000051,
    AnalogBoard = 0x00000052,
    ConfigFailAwg = 0x00000053,
    InitialiseFpga = 0x00000054,
    ExternalFrequencyInvalid = 0x00000056,
    ClockChangeError = 0x00000057,
    TriggerAndExternalClockClash = 0x00000058,
    PwqAndExternalClockClash = 0x00000059,
    UnableToOpenScalingFile = 0x0000005A,

    MemoryClockFrequency = 0x0000005B,
    I2CNotResponding = 0x0000005C,

    NoCapturesAvailable = 0x0000005D,
    NotUsedInThisCaptureMode = 0x0000005E,

    GetDataActive = 0x00000103,

    IpNetworked = 0x00000104,
    InvalidIpAddress = 0x00000105,
    IpsocketFailed = 0x00000106,
    IpsocketTimedout = 0x00000107,
    SettingsFailed = 0x00000108,
    NetworkFailed = 0x00000109,
    Ws232DllNotLoaded = 0x0000010A,
    InvalidIpPort = 0x0000010B,

    CouplingNotSupported = 0x0000010C,
    BandwidthNotSupported = 0x0000010D,
    InvalidBandwidth = 0x0000010E,

    AwgNotSupported = 0x0000010F,
    EtsNotRunning = 0x00000110,

    WhiteNoiseNotSupported = 0x00000111,
    WaveTypeNotSupported = 0x00000112,

    InvalidDigitalPort = 0x00000113,
    InvalidDigitalChannel = 0x00000114,
    InvalidDigitalTriggerDirection = 0x00000115,

    PRBSNotSupported = 0x00000116,

    EtsNotAvailableWithLogicChannels = 0x00000117,

    WarningRepeatValue = 0x00000118,

    PowerSupplyConnected = 0x00000119,
    PowerSupplyNotConnected = 0x0000011A,
    PowerSupplyRequestInvalid = 0x0000011B,
    PowerSupplyUnderVoltage = 0x0000011C,

    DeviceCapturing = 0x11D,

    USB3DeviceOnNonUSB3Port = 0x0000011E,
    NotSupportedByDevice = 0x0000011F,
    InvalidDeviceResolution = 0x00000120,
    InvalidChannelCountForResolution = 0x00000121,

    ChannelDisabledOnUSBPower = 0x00000122,

    SiggenDcVoltageNotConfigurable = 0x00000123,
    NoTriggerEnabledForTriggerInPreTrig = 0x00000124,
    TriggerWithinPreTrigNotArmed = 0x00000125,
    TriggerWithinPreNotAllowedWithDelay = 0x00000126,
    TriggerIndexUnavailable = 0x00000127,
    AwgClockFrequency = 0x00000128,
    TooManyChannelsInUse = 0x00000129,
    NullConditions = 0x0000012A,
    DuplicateConditionSource = 0x0000012B,
    InvalidConditionInfo = 0x0000012C,
    SettingsReadFailed = 0x0000012D,
    SettingsWriteFailed = 0x0000012E,
    ArgumentOutOfRange = 0x0000012F,

    HardwareVersionNotSupported = 0x00000130,
    DigitalHardwareVersionNotSupported = 0x00000131,
    AnalogueHardwareVersionNotSupported = 0x00000132,

    UnableToConvertToResistance = 0x00000133,
    DuplicatedChannel = 0x00000134,

    InvalidResistanceConversion = 0x00000135,
    InvalidValueInMaxBuffer = 0x00000136,
    InvalidValueInMinBuffer = 0x00000137,

    SiggenFrequencyOutOfRange = 0x00000138,
    Eeprom2Corrupt = 0x00000139,
    Eeprom2Fail = 0x0000013A,
    SerialBufferTooSmall = 0x0000013B,
    SiggenTriggerAndExternalClockClash = 0x0000013C,
    WarningSiggenAuxioTriggerDisabled = 0x0000013D,
    SiggenGatingAuxioNotAvailable = 0x00000013E,
    SiggenGatingAuxioEnabled = 0x00000013F,
    ResourceError = 0x00000140,
    TemperatureTypeInvalid = 0x000000141,
    TemperatureTypeNotSupported = 0x000000142,
    Timeout = 0x00000143,
    DeviceNotFunctioning = 0x00000144,
    InternalError = 0x00000145,
    MultipleDevicesFound = 0x00000146,
    WarningNumberOfSegmentsReduced = 0x00000147,

    // the calibration pin states argument is out of range
    PICO_CAL_PINS_STATES = 0x00000148,
    // the calibration pin frequency argument is out of range
    PICO_CAL_PINS_FREQUENCY = 0x00000149,
    // the calibration pin amplitude argument is out of range
    PICO_CAL_PINS_AMPLITUDE = 0x0000014A,
    // the calibration pin wavetype argument is out of range
    PICO_CAL_PINS_WAVETYPE = 0x0000014B,
    // the calibration pin offset argument is out of range
    PICO_CAL_PINS_OFFSET = 0x0000014C,
    // the probe's identity has a problem
    PICO_PROBE_FAULT = 0x0000014D,
    // the probe has not been identified
    PICO_PROBE_IDENTITY_UNKNOWN = 0x0000014E,
    // enabling the probe would cause the device to exceed the allowable current limits
    PICO_PROBE_POWER_DC_POWER_SUPPLY_REQUIRED = 0x0000014F,
    // the DC power supply is connected; enabling the probe would cause the device to exceed the
    // allowable current limit
    PICO_PROBE_NOT_POWERED_WITH_DC_POWER_SUPPLY = 0x00000150,
    // failed to complete probe configuration
    PICO_PROBE_CONFIG_FAILURE = 0x00000151,
    // failed to set the callback function, as currently in current callback function
    PICO_PROBE_INTERACTION_CALLBACK = 0x00000152,
    // the probe has been verified but not know on this driver
    PICO_UNKNOWN_INTELLIGENT_PROBE = 0x00000153,
    // the intelligent probe cannot be verified
    PICO_INTELLIGENT_PROBE_CORRUPT = 0x00000154,
    // the callback is null, probe collection will only start when 
    // first callback is a none null pointer
    PICO_PROBE_COLLECTION_NOT_STARTED = 0x00000155,
    // a current drawn by probes have exceeded that allowed
    PICO_PROBE_POWER_CONSUMPTION_EXCEEDED = 0x00000156,

    PICO_WARNING_PROBE_CHANNEL_OUT_OF_SYNC = 0x00000157,

    PICO_ENDPOINT_MISSING = 0x00000158,

    PICO_UNKNOWN_ENDPOINT_REQUEST = 0x00000159,

    // The adc on board the device has not been correctly identified
    PICO_ADC_TYPE_ERROR = 0x0000015A,

    PICO_FPGA2_FAILED = 0x0000015B,

    PICO_FPGA2_DEVICE_STATUS = 0x0000015C,

    PICO_ENABLE_PROGRAM_FPGA2_FAILED = 0x0000015D,
    PICO_NO_CHANNELS_OR_PORTS_ENABLED = 0x0000015E,

    PICO_INVALID_RATIO_MODE = 0x0000015F,

    PICO_READS_NOT_SUPPORTED_IN_CURRENT_CAPTURE_MODE = 0x00000160,

    // these selection tests can be masked together to show that mode than one read selection has failed the tests,
    // therefore theses error codes cover 0x00000161UL to 0x0000016FUL
    PICO_READ1_SELECTION_CHECK_FAILED = 0x00000161,
    PICO_READ2_SELECTION_CHECK_FAILED = 0x00000162,
    PICO_READ3_SELECTION_CHECK_FAILED = 0x00000164,
    PICO_READ4_SELECTION_CHECK_FAILED = 0x00000168,

    PICO_READ_SELECTION_OUT_OF_RANGE = 0x00000170,

    PICO_MULTIPLE_RATIO_MODES = 0x00000171,


    PICO_NO_SAMPLES_READ = 0x00000172,

    PICO_RATIO_MODE_NOT_REQUESTED = 0x00000173,

    PICO_NO_USER_READ_REQUESTS_SET = 0x00000174,

    PICO_ZERO_SAMPLES_INVALID = 0x00000175,

    PICO_ANALOGUE_HARDWARE_MISSING = 0x00000176,

    PICO_ANALOGUE_HARDWARE_PINS = 0x00000177,

    PICO_ANALOGUE_HARDWARE_SMPS_FAULT = 0x00000178,

    PICO_DIGITAL_ANALOGUE_HARDWARE_CONFLICT = 0x00000179,

    PICO_RATIO_MODE_BUFFER_NOT_SET = 0x0000017A,

    // The resolution is valid but not suppoprted by the opened device.
    PICO_RESOLUTION_NOT_SUPPORTED_BY_VARIANT = 0x0000017B,
    // The requested trigger threshold is out of range for the current device resolution
    PICO_THRESHOLD_OUT_OF_RANGE = 0x0000017C,

    // The simple trigger only supports upper edge dirction options
    PICO_INVALID_SIMPLE_TRIGGER_DIRECTION = 0x0000017D,

    // The aux trigger is not supported on this variant
    PICO_AUX_NOT_SUPPORTED = 0x0000017E,

    // The trigger directions pointer may not be null
    PICO_NULL_DIRECTIONS = 0x0000017F,

    // The trigger channel properties pointer may not be null
    PICO_NULL_CHANNEL_PROPERTIES = 0x00000180,

    // A trigger is set on a channel that has not been enabled
    PICO_TRIGGER_CHANNEL_NOT_ENABLED = 0x00000181,

    // A trigger condition has been set but a trigger property not set
    PICO_CONDITION_HAS_NO_TRIGGER_PROPERTY = 0x00000182,

    PICO_RATIO_MODE_TRIGGER_MASKING_INVALID = 0x00000183,

    PICO_TRIGGER_DATA_REQUIRES_MIN_BUFFER_SIZE_OF_40_SAMPLES = 0x00000184,

    PICO_NO_OF_CAPTURES_OUT_OF_RANGE = 0x00000185,

    PICO_RATIO_MODE_SEGMENT_HEADER_DOES_NOT_REQUIRE_BUFFERS = 0x00000186,

    PICO_FOR_SEGMENT_HEADER_USE_GETTRIGGERINFO = 0x00000187,

    PICO_READ_NOT_SET = 0x00000188,

    PICO_ADC_SETTING_MISMATCH = 0x00000189,

    PICO_DATATYPE_INVALID = 0x0000018A,

    PICO_RATIO_MODE_DOES_NOT_SUPPORT_DATATYPE = 0x0000018B,

    PICO_CHANNEL_COMBINATION_NOT_VALID_IN_THIS_RESOLUTION = 0x0000018C,

    PICO_USE_8BIT_RESOLUTION = 0x0000018D,

    PICO_AGGREGATE_BUFFERS_SAME_POINTER = 0x0000018E,

    PICO_OVERLAPPED_READ_VALUES_OUT_OF_RANGE = 0x0000018F,

    PICO_OVERLAPPED_READ_SEGMENTS_OUT_OF_RANGE = 0x00000190,

    PICO_CHANNELFLAGSCOMBINATIONS_ARRAY_SIZE_TOO_SMALL = 0x00000191,

    PICO_CAPTURES_EXCEEDS_NO_OF_SUPPORTED_SEGMENTS = 0x00000192,

    PICO_TIME_UNITS_OUT_OF_RANGE = 0x00000193,

    PICO_NO_SAMPLES_REQUESTED = 0x00000194,

    PICO_INVALID_ACTION = 0x00000195,

    PICO_NO_OF_SAMPLES_NEED_TO_BE_EQUAL_WHEN_ADDING_BUFFERS = 0x00000196,

    PICO_WAITING_FOR_DATA_BUFFERS = 0x00000197,

    PICO_STREAMING_ONLY_SUPPORTS_ONE_READ = 0x00000198,

    PICO_CLEAR_DATA_BUFFER_INVALID = 0x00000199,

    PICO_INVALID_ACTION_FLAGS_COMBINATION = 0x0000019A,

    PICO_BOTH_MIN_AND_MAX_NULL_BUFFERS_CANNOT_BE_ADDED = 0x0000019B,

    PICO_CONFLICT_IN_SET_DATA_BUFFERS_CALL_REMOVE_DATA_BUFFER_TO_RESET = 0x0000019C,

    PICO_REMOVING_DATA_BUFFER_ENTRIES_NOT_ALLOWED_WHILE_DATA_PROCESSING = 0x0000019D,

    PICO_CYUSB_REQUEST_FAILED = 0x00000200,

    PICO_STREAMING_DATA_REQUIRED = 0x00000201,

    PICO_NO_TRIGGER_CONDITIONS_SET = 0x0000020C,

    PICO_PROBE_COMPONENT_ERROR = 0x0000020E,

    PICO_INVALID_VARIANT = 0x00001000,

    // A null pointer has been passed in the trigger function or one of the parameters is out of range.
    PICO_PULSE_WIDTH_QUALIFIER_LOWER_UPPER_CONFILCT = 0x00002000,
    PICO_PULSE_WIDTH_QUALIFIER_TYPE = 0x00002001,
    PICO_PULSE_WIDTH_QUALIFIER_DIRECTION = 0x00002002,

    PICO_THRESHOLD_MODE_OUT_OF_RANGE = 0x00002003,

    PICO_TRIGGER_AND_PULSEWIDTH_DIRECTION_IN_CONFLICT = 0x00002004,

    PICO_THRESHOLD_UPPER_LOWER_MISMATCH = 0x00002005,

    PICO_PULSE_WIDTH_LOWER_OUT_OF_RANGE = 0x00002006,
    PICO_PULSE_WIDTH_UPPER_OUT_OF_RANGE = 0x00002007,

    // The devices front panel has caused an error.
    PICO_FRONT_PANEL_ERROR = 0x00002008,

    // The actual and expected mode of the front panel do not match.
    PICO_FRONT_PANEL_MODE = 0x0000200B,

    // A front panel feature is not available or failed to configure.
    PICO_FRONT_PANEL_FEATURE = 0x0000200C,

    // When setting the pulse width conditions either the pointer is null or the number of conditions is set to zero.
    PICO_NO_PULSE_WIDTH_CONDITIONS_SET = 0x0000200D,

    // a trigger condition exists for a port, but the port has not been enabled
    PICO_TRIGGER_PORT_NOT_ENABLED = 0x0000200E,

    // a trigger condition exists for a port, but no digital channel directions have been set
    PICO_DIGITAL_DIRECTION_NOT_SET = 0x0000200F,

    PICO_I2C_DEVICE_INVALID_READ_COMMAND = 0x00002010,

    PICO_I2C_DEVICE_INVALID_RESPONSE = 0x00002011,

    PICO_I2C_DEVICE_INVALID_WRITE_COMMAND = 0x00002012,

    PICO_I2C_DEVICE_ARGUMENT_OUT_OF_RANGE = 0x00002013,

    // The actual and expected mode do not match.
    PICO_I2C_DEVICE_MODE = 0x00002014,

    // While trying to configure the device, set up failed.
    PICO_I2C_DEVICE_SETUP_FAILED = 0x00002015,

    // A feature is not available or failed to configure.
    PICO_I2C_DEVICE_FEATURE = 0x00002016,

    // The device did not pass the validation checks.
    PICO_I2C_DEVICE_VALIDATION_FAILED = 0x00002017,

    // The number of MSO's edge transitions being set is not supported by this device (RISING, FALLING, or RISING_OR_FALLING).
    PICO_MSO_TOO_MANY_EDGE_TRANSITIONS_WHEN_USING_PULSE_WIDTH = 0x00003000,

    // A probe LED position requested is not one of the available probe positions in the ProbeLedPosition enum.
    PICO_INVALID_PROBE_LED_POSITION = 0x00003001,

    // The LED position is not supported by the selected variant.
    PICO_PROBE_LED_POSITION_NOT_SUPPORTED = 0x00003002,

    // A channel has more than one of the same LED position in the ProbeChannelLedSetting struct.
    PICO_DUPLICATE_PROBE_CHANNEL_LED_POSITION = 0x00003003,

    // Setting the probes LED has failed.
    PICO_PROBE_LED_FAILURE = 0x00003004,

    // Probe is not supported by the selected variant.
    PICO_PROBE_NOT_SUPPORTED_BY_THIS_DEVICE = 0x00003005,

    // The probe name is not in the list of enPicoConnectProbe enums.
    PICO_INVALID_PROBE_NAME = 0x00003006,

    // The number of colour settings are zero or a null pointer passed to the function.
    PICO_NO_PROBE_COLOUR_SETTINGS = 0x00003007,

    // Channel has no probe connected to it.
    PICO_NO_PROBE_CONNECTED_ON_REQUESTED_CHANNEL = 0x00003008,

    // Connected probe does not require calibration.
    PICO_PROBE_DOES_NOT_REQUIRE_CALIBRATION = 0x00003009,

    // Connected probe could not be calibrated - hardware fault is a possible cause.
    PICO_PROBE_CALIBRATION_FAILED = 0x0000300A,

    // A probe has been connected, but the version is not recognised.
    PICO_PROBE_VERSION_ERROR = 0x0000300B,

    // The requested trigger time is to long for the selected variant.
    PICO_AUTO_TRIGGER_TIME_TOO_LONG = 0x00004000,

    // The MSO pod did not pass the validation checks.
    PICO_MSO_POD_VALIDATION_FAILED = 0x00005000,

    // No MSO pod found on the requested digital port.
    PICO_NO_MSO_POD_CONNECTED = 0x00005001,

    // the digital port enum value is not in the enPicoDigitalPortHysteresis declaration
    PICO_DIGITAL_PORT_HYSTERESIS_OUT_OF_RANGE = 0x00005002,

    // Status error for when the device has overheated.
    NotRespondingOverheated = 0x00005010,

    // waiting for the device to capture timed out 
    PICO_HARDWARE_CAPTURE_TIMEOUT = 0x00006000,

    // waiting for the device be ready for capture timed out
    PICO_HARDWARE_READY_TIMEOUT = 0x00006001,

    DeviceTimeStampReset = 0x01000000,

    PICO_TRIGGER_TIME_NOT_REQUESTED = 0x02000001,
    PICO_TRIGGER_TIME_BUFFER_NOT_SET = 0x02000002,
    PICO_TRIGGER_TIME_FAILED_TO_CALCULATE = 0x02000004,

    PICO_TRIGGER_TIME_STAMP_NOT_REQUESTED = 0x02000100,

    SiggenSettingsMismatch = 0x03000010,
    SiggenSettingsChangedCallApply = 0x03000011,
    SiggenWavetypeNotSupported = 0x03000012,
    SiggenTriggerTypeNotSupported = 0x03000013,
    SiggenTriggerSourceNotSupported = 0x03000014,
    SiggenFilterStateNotSupported = 0x03000015,

    SiggenNullParameter = 0x03000020,
    SiggenEmptyBufferSupplied = 0x03000021,
    SiggenRangeNotSupplied = 0x03000022,
    SiggenBufferNotSupplied = 0x03000023,
    SiggenFrequencyNotSupplied = 0x03000024,
    SiggenSweepInfoNotSupplied = 0x03000025,
    SiggenTriggerInfoNotSupplied = 0x03000026,
    SiggenClockFreqNotSupplied = 0x03000027,

    SigGenTooManySamples = 0x03000030,
    SigGenDutyCycleOutOfRange = 0x03000031,
    SigGenCyclesOutOfRange = 0x03000032,
    SigGenPrescaleOutOfRange = 0x03000033,
    SigGenSweepTypeInvalid = 0x03000034,
    SigGenSweepWaveTypeMismatch = 0x03000035,
    SigGenInvalidSweepParameters = 0x03000036,
    SigGenSweepPrescaleNotSupported = 0x03000037,

    UsbPermissionsError = 0x03000040,

    PICO_PORTS_WITHOUT_ANALOGUE_CHANNELS_ONLY_ALLOWED_IN_8BIT_RESOLUTION = 0x03001000,

    // checking if the firmware needs updating the updateRequired parameter is null
    PICO_FIRMWARE_UPDATE_REQUIRED_TO_USE_DEVICE_WITH_THIS_DRIVER = 0x03004000,
    PICO_UPDATE_REQUIRED_NULL = 0x03004001,
    PICO_FIRMWARE_UP_TO_DATE = 0x03004002,
    PICO_FLASH_FAIL = 0x03004003,
    PICO_INTERNAL_ERROR_FIRMWARE_LENGTH_INVALID = 0x03004004,
    PICO_INTERNAL_ERROR_FIRMWARE_NULL = 0x03004005,
    PICO_FIRMWARE_FAILED_TO_BE_CHANGED = 0x03004006,
    PICO_FIRMWARE_FAILED_TO_RELOAD = 0x03004007,
    PICO_FIRMWARE_FAILED_TO_BE_UPDATE = 0x03004008,
    PICO_FIRMWARE_VERSION_OUT_OF_RANGE = 0x03004009,
    PICO_FRONTPANEL_FIRMWARE_UPDATE_REQUIRED_TO_USE_DEVICE_WITH_THIS_DRIVER = 0x0300400A,

    // the adc is powered down when trying to capture data
    PICO_ADC_POWERED_DOWN = 0x03002000,

    WatchdogTimer = 0x10000000,
    IppNotFound = 0x10000001,
    IppNoFunction = 0x10000002,
    IppError = 0x10000003,
    ShadowCalNotAvailable = 0x10000004,
    ShadowCalDisabled = 0x10000005,
    ShadowCalError = 0x10000006,
    ShadowCalCorrupt = 0x10000007,

    Reserved1 = 0x11000000,
  }
}
