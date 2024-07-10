using System;

namespace DriverImports
{
  public enum InfoType
  {
    DriverVersion = 0x00000000,
    UsbVersion = 0x00000001,
    HardwareVersion = 0x00000002,
    VariantInfo = 0x00000003,
    BatchAndSerial = 0x00000004,
    CalibrationDate = 0x00000005,
    KernelDriverVersion = 0x00000006,
    FirmwareVersion = 0x00000009,
    FpgaVersion = 0x0000000A,
    DriverPath = 0x0000000E,
    FpgaVersion2 = 0x0000000F,
    FrontPanelFirmwareVersion = 0x00000010
  }

  public enum DeviceResolution
  {
    PICO_DR_8BIT = 0,
    PICO_DR_12BIT = 1,
    PICO_DR_14BIT = 2,
    PICO_DR_15BIT = 3,
    PICO_DR_16BIT = 4,

    PICO_DR_10BIT = 10,
  }

  public enum TriggerWithinPreTrigger
  {
    Disable,
    Arm
  }

  public enum Channel
  {
    ChannelA,
    ChannelB,
    ChannelC,
    ChannelD,
    ChannelE,
    ChannelF,
    ChannelG,
    ChannelH,

    Port0 = 128,
    Port1 = 129,
    Port2 = 130,
    Port3 = 131,

    External = 1000,
    Aux = 1001,

    PulseWidthSource = 0x10000000,
    DigitalSource = 0x10000001,
  }

  public enum Coupling
  {
    AC = 0,
    DC = 1,

    DC50Ohm = 50,
  }

  public enum ChannelRange
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
    Range_200V,
  }

  public enum BandwidthLimiter
  {
    BW_FULL = 0,
    BW_20MHZ = 20000000,
    BW_25MZ = 25000000,
    BW_250MHZ = 250000000,
    BW_500MHZ = 500000000,
  }

  [Flags]
  public enum EnabledChannelsAndPorts
  {
    None = 0x00000000,
    ChannelA = 0x00000001,
    ChannelB = 0x00000002,
    ChannelC = 0x00000004,
    ChannelD = 0x00000008,

    ChannelE = 0x00000010,
    ChannelF = 0x00000020,
    ChannelG = 0x00000040,
    ChannelH = 0x00000080,

    DigtalPort0 = 0x00010000,
    DigtalPort1 = 0x00020000,
    DigtalPort2 = 0x00040000,
    DigtalPort3 = 0x00080000,
  }

  public enum PicoTimeUnits
  {
    fs = 0,
    ps = 1,
    ns = 2,
    us = 3,
    ms = 4,
    s = 5
  }

  public enum DataType
  {
    PICO_INT8_T,
    PICO_INT16_T,
    PICO_INT32_T,
    PICO_UINT32_T
  }

  [Flags]
  public enum RatioMode : uint
    {
    PICO_RATIO_MODE_AGGREGATE = 1,
    PICO_RATIO_MODE_DECIMATE = 2,
    PICO_RATIO_MODE_AVERAGE = 4,
    PICO_RATIO_MODE_DISTRIBUTION = 8,

    PICO_RATIO_MODE_TRIGGER_DATA_FOR_TIME_CALCULATION = 0x10000000, // buffers cannot be set for this mode
    PICO_RATIO_MODE_SEGMENT_HEADER = 0x20000000, // buffers do not need to be set for this
    PICO_RATIO_MODE_TRIGGER = 0x40000000, // this cannot be combined with any other ratio mode
    PICO_RATIO_MODE_RAW = 0x80000000
    }

  [Flags]
  public enum Action
  {
    PICO_CLEAR_ALL = 0x00000001, //Remove all existing buffers.
    PICO_ADD = 0x00000002,       //Add new buffer for segment channel data type.

    PICO_CLEAR_THIS_DATA_BUFFER = 0x00001000,
    PICO_CLEAR_WAVEFORM_DATA_BUFFERS = 0x00002000,

    PICO_CLEAR_WAVEFORM_READ_DATA_BUFFERS = 0x00004000,
  }

  public enum TriggerState
  {
    DontCare,
    True,
    False,
  }

  public enum TriggerDirection
  {
    // Values for level threshold mode
    Above,
    Below,
    Rising,
    Falling,
    RisingOrFalling,
    AboveLower,
    BelowLower,
    RisingLower,
    FallingLower,

    // Values for window threshold mode
    Inside = Above,
    Outside = Below,
    Enter = Rising,
    Exit = Falling,
    EnterOrExit = RisingOrFalling,
    PositiveRunt = 9,
    NegativeRunt,

    None = Rising,
  }

  public enum DigitalDirection
  {
    DIGITAL_DONT_CARE,
    DIGITAL_DIRECTION_LOW,
    DIGITAL_DIRECTION_HIGH,
    DIGITAL_DIRECTION_RISING,
    DIGITAL_DIRECTION_FALLING,
    DIGITAL_DIRECTION_RISING_OR_FALLING,
  }

  public enum ThresholdMode
  {
    Level,
    Window
  }

  public enum WaveType
  {
    Sine = 0x00000011,
    Square = 0x00000012,
    Triangle = 0x00000013,
    RampUp = 0x00000014,
    RampDown = 0x00000015,
    SinXOverX = 0x00000016,
    Gaussian = 0x00000017,
    HalfSine = 0x00000018,

    DcVoltage = 0x00000400,

    Pwm = 0x00001000,

    WhiteNoise = 0x00002001,
    PseudorandomBitStream = 0x00002002,

    Arbitrary = 0x10000000
  }
    public enum SweepType
    {
        Up = 0,
        Down = 1,
        UpDown = 2,
        DownUp = 3      
    }

    public enum PulseWidthType
  {
    None = 0,
    Less_Than = 1,
    Greater_Than = 2,
    In_Range = 3,
    Out_Of_Range = 4
  }

  public enum SiggenTrigType
  {
    Rising,
    Falling,
    GateHigh,
    GateLow
  }
  public enum SiggenTrigSource
  {
    None,
    ScopeTrigger,
    AuxIn,
    ExtIn,
    SoftwareTrigger
  }

    public enum SigGenFilterState
    {
        FilterAuto = 0,
        FilterOff =1,
        FilterOn =2           
    }
    public enum SigGenParameter
    {
        SigGenParamOutputVolts = 0,
        SigGenParamOutputSample = 1,
        SigGenParamOutputBufferLenght = 2
    }
  public enum DigitalPort
  {
    DigitalNone = 0x00000000,
    DigitalPort0 = 0x00000080,
    DigitalPort1 = 0x00000081,
    DigitalPort2 = 0x00000082,
    DigitalPort3 = 0x00000083,
  }

  public enum DigitalPortName
  {
    PICO_DIGITAL_PORT_NONE = 0,
    PICO_DIGITAL_PORT_MSO_POD = 1000,
    PICO_DIGITAL_PORT_UNKNOWN_DEVICE = -2,
  }

  public enum DigitalChannel
  {
    DIGITAL_CHANNEL0,
    DIGITAL_CHANNEL1,
    DIGITAL_CHANNEL2,
    DIGITAL_CHANNEL3,
    DIGITAL_CHANNEL4,
    DIGITAL_CHANNEL5,
    DIGITAL_CHANNEL6,
    DIGITAL_CHANNEL7,
    DIGITAL_CHANNEL_COUNT
  }

  public enum DigitalPortHysteresis
  {
    VERY_HIGH_400MV,
    HIGH_200MV,
    NORMAL_100MV,
    LOW_50MV
  }
}