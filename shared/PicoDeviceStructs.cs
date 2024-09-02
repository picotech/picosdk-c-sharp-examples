// Copyright © 2020-2024 Pico Technology Ltd. See LICENSE file for terms.

using System.Runtime.InteropServices;
using System.Runtime.Remoting.Channels;

namespace DriverImports
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct TriggerChannelProperties
    {
        public short ThresholdMajor;
        public ushort HysteresisMajor;
        public short ThresholdMinor;
        public ushort HysteresisMinor;
        public Channel Channel;

        public TriggerChannelProperties(
          short thresholdMajor,
          ushort hysteresisMajor,
          short thresholdMinor,
          ushort hysteresisMinor,
          Channel channel)
        {
            ThresholdMajor = thresholdMajor;
            HysteresisMajor = hysteresisMajor;
            ThresholdMinor = thresholdMinor;
            HysteresisMinor = hysteresisMinor;
            Channel = channel;
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct PicoUsbPowerDelivery
    {
        public byte Valid_;
        public uint BusVoltagemV_;
        public uint RpCurrentLimitmA_;
        public byte PartnerConnected_;
        public byte CcPolarity_;
        public enPicoUsbPowerDeliveryDeviceType AttachedDevice_;
        public byte ContractExists_;
        public uint CurrentPdo_;
        public uint CurrentRdo_;

        public PicoUsbPowerDelivery(byte valid_,
                                    uint busVoltagemV_,
                                    uint rpCurrentLimitmA_,
                                    byte partnerConnected_,
                                    byte ccPolarity_,
                                    enPicoUsbPowerDeliveryDeviceType attachedDevice_,
                                    byte contractExists_,
                                    uint currentPdo_,
                                    uint currentRdo_ )
        {
            Valid_ = valid_;
            BusVoltagemV_ = busVoltagemV_;
            RpCurrentLimitmA_ = rpCurrentLimitmA_;
            PartnerConnected_ = partnerConnected_;
            CcPolarity_ = ccPolarity_;
            AttachedDevice_ = attachedDevice_;
            ContractExists_ = contractExists_;
            CurrentPdo_ = currentPdo_;
            CurrentRdo_ = currentRdo_;
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct PicoUsbPowerDetails
    {
        public byte PowerErrorLikely_;
        public PicoUsbPowerDelivery DataPort_;
        public PicoUsbPowerDelivery PowerPort_;
        public PicoUsbPowerDetails(byte powerErrorLikely_, PicoUsbPowerDelivery dataPort_, PicoUsbPowerDelivery powerPort_)
        {
            PowerErrorLikely_ = powerErrorLikely_;
            DataPort_ = dataPort_;
            PowerPort_ = powerPort_;
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct TriggerCondition
    {
        public Channel Channel;
        public TriggerState State;

        public TriggerCondition(Channel channel, TriggerState state)
        {
            Channel = channel;
            State = state;
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct TriggerInfo
    {
        public StandardDriverStatusCode Status;
        public ulong SegmentIndex;
        public ulong TriggerIndex;
        public double TriggerTime;
        public PicoTimeUnits TimeUnits;
        public ulong MissedTriggers;
        public ulong TimeStampCounter;

        public TriggerInfo(
          StandardDriverStatusCode status,
          ulong segmentIndex, ulong triggerIndex, double triggerTime,
          PicoTimeUnits timeUnits, ulong missedTriggers, ulong timeStampCounter)
        {
            Status = status;
            SegmentIndex = segmentIndex;
            TriggerIndex = triggerIndex;
            TriggerTime = triggerTime;
            TimeUnits = timeUnits;
            MissedTriggers = missedTriggers;
            TimeStampCounter = timeStampCounter;
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ThresholdDirection
    {
        public Channel Source;
        public TriggerDirection Direction;
        public ThresholdMode Mode;

        public ThresholdDirection(Channel source, TriggerDirection direction, ThresholdMode mode)
        {
            Source = source;
            Direction = direction;
            Mode = mode;
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct DigitalChannelDirections
    {
        public DigitalChannel Channel;
        public DigitalDirection Direction;

        public DigitalChannelDirections(DigitalChannel channel, DigitalDirection direction)
        {
            Channel = channel;
            Direction = direction;
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct DigitalPortInteractions
    {
        public ushort Connected;
        public Channel Channel;
        public DigitalPortName DigitalPort;
        public StandardDriverStatusCode Status;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
        public char[] serial;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public char[] calibrationDate;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct StreamingDataInfo
    {
        public Channel Channel;
        public RatioMode Mode;
        public DataType Type;
        public int NoOfSamples;
        public ulong BufferIndex;
        public int StartIndex;
        public short Overflow;

        public StreamingDataInfo(Channel channel, RatioMode mode, DataType type, int noOfSamples, ulong bufferIndex, int startIndex, short overflow)
        {
            Channel = channel;
            Mode = mode;
            Type = type;
            NoOfSamples = noOfSamples;
            BufferIndex = bufferIndex;
            StartIndex = startIndex;
            Overflow = overflow;
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct StreamingDataTriggerInfo
    {
        public ulong TriggerAt;
        public short Triggered;
        public short AutoStop;

        public StreamingDataTriggerInfo(ulong triggerAt, short triggered, short autoStop)
        {
            TriggerAt = triggerAt;
            Triggered = triggered;
            AutoStop = autoStop;
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ScalingFactors
    {
        public Channel Channel;
        public PicoConnectProbes.PicoConnectProbes.PicoConnectProbeRange Range;
        public short Offset;
        public double ScalingFactor;

        public ScalingFactors(Channel channel, PicoConnectProbes.PicoConnectProbes.PicoConnectProbeRange range, short offset, double scalingFactor)
        {
            Channel = channel;
            Range = range;
            Offset = offset;
            ScalingFactor = scalingFactor;
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ChannelOvervoltageTripped
    {
        public Channel Channel_;
        public byte Tripped_;

        public ChannelOvervoltageTripped(Channel channel_, byte tripped_)
        {
            Channel_ = channel_;
            Tripped_ = tripped_;
        }
    }

}