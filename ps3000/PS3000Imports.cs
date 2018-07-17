/*******************************************************************************
*
* Filename: PS3000Imports.cs
*
* Description:
*  This file contains .NET wrapper calls corresponding to function calls 
*  defined in the ps3000Api.h C header file. 
*  It also has the enums required by the (wrapped) function calls.
*   
* Copyright © 2007-2018 Pico Technology Ltd. See LICENSE file for terms. 
*    
*******************************************************************************/

using System;
using System.Runtime.InteropServices;
using System.Text;

namespace PS3000Imports
{
    public class PS3000
    {
        private short _handle = -1;
        private bool _disposed = false;

        #region Constants

        private const string _DRIVER_FILENAME = "ps3000.dll";
        private const int _NUM_UNIT_INFO_CHARS = 256;

        #endregion

        #region Driver Enums

        public enum ErrorCode : short
        {
            Ok,
            MaxUnitsOpened,
            HostRamTooLow,
            DeviceNotFound,
            FirmwareDownloadFail,
            NotResponding,
            ConfigurationFail,
            OSNotSupported,
        }
        public enum Coupling : short
        {
            AC,
            DC,
        }

        public enum Channel : short
        {
            ChannelA,
            ChannelB,
            ChannelC,
            ChannelD,
            External,
            None,
        }

        public enum Range : short
        {
            Range10mV,
            Range20mV,
            Range50mV,
            Range100mV,
            Range200mV,
            Range500mV,
            Range1V,
            Range2V,
            Range5V,
            Range10V,
            Range20V,
            Range50V,
            Range100V,
            Range200V,
            Range400V
        }

        public enum SiggenWaveType : short
        {
            Square,
            Triangle,
            Sine,
        }

        public enum EtsMode : short
        {
            Off,
            Fast,
            Slow,
        }

        public enum TimeUnits : short
        {
            FemtoSeconds,
            PicoSeconds,
            NanoSeconds,
            MicroSeconds,
            MilliSeconds,
            Seconds,
        }

        public enum TriggerThresholdDirection : short
        {
            Rising,
            Falling,
        }

        private enum InfoType : short
        {
            DriverVersion,
            UsbVersion,
            HardwareVersion,
            VariantInfo,
            Serial,
            CalibrationDate,
            ErrorCode,
            KernelDriverVersion,
        }
        
        #endregion

        #region Driver Imports

        [DllImport(_DRIVER_FILENAME)]
        private static extern short ps3000_open_unit();

        [DllImport(_DRIVER_FILENAME)]
        private static extern short ps3000_close_unit(short handle);

        [DllImport(_DRIVER_FILENAME)]
        private static extern short ps3000_get_unit_info(
            short handle,
            StringBuilder info,
            short infoLength,
            InfoType ps3000InfoType);

        [DllImport(_DRIVER_FILENAME)]
        private static extern short ps3000_set_channel(
            short handle,
            Channel channel,
            short enabled,
            Coupling dc,
            Range range);

        [DllImport(_DRIVER_FILENAME)]
        private static extern short ps3000_get_timebase(
            short handle,
            short timebase,
            int numSamples,
            out int nsInterval,
            out TimeUnits timeUnits,
            short oversample,
            out int maxSamples);

        [DllImport(_DRIVER_FILENAME)]
        private static extern int ps3000_set_siggen(
            short handle,
            SiggenWaveType waveType,
            int startFreq,
            int stopFreq,
            float increment,
            short dwellTime,
            bool repeat,
            bool dualSlope);

        [DllImport(_DRIVER_FILENAME)]
        private static extern int ps3000_set_ets(
            short handle,
            EtsMode mode,
            short cycles,
            short interleave);

        [DllImport(_DRIVER_FILENAME)]
        private static extern short ps3000_set_trigger2(
            short handle,
            Channel source,
            short threshold,
            TriggerThresholdDirection direction,
            float delay,
            short autoTriggerMs);

        [DllImport(_DRIVER_FILENAME)]
        private static extern short ps3000_run_block(
            short handle,
            int numSamples,
            short timebase,
            short oversample,
            out int timeIndisposedMs);

        [DllImport(_DRIVER_FILENAME)]
        private static extern short ps3000_run_streaming(
            short handle,
            short timeIntervalMs,
            int maxSamples,
            short windowed);

        [DllImport(_DRIVER_FILENAME)]
        private static extern short ps3000_ready(short handle);

        [DllImport(_DRIVER_FILENAME)]
        private static extern void ps3000_stop(short handle);

        [DllImport(_DRIVER_FILENAME)]
        private static extern int ps3000_get_values(
            short handle,
            short[] bufferA,
            short[] bufferB,
            short[] bufferC,
            short[] bufferD,
            out short overflows,
            int numValues);

        [DllImport(_DRIVER_FILENAME)]
        private static extern int ps3000_get_times_and_values(
            short handle,
            int[] times,
            short[] bufferA,
            short[] bufferB,
            short[] bufferC,
            short[] bufferD,
            out short overflows,
            TimeUnits timeUnits,
            int numValues);

        [DllImport(_DRIVER_FILENAME)]
        private static extern short ps3000_get_mux_offset(
            short handle,
            short channel,
            short time_units,
            ref int offset);

        [DllImport(_DRIVER_FILENAME)]
        private static extern bool ps3000_run_streaming_ns(
            short handle,
            uint sampleInterval,
            TimeUnits timeUnits,
            uint maxSamples,
            bool autoStop,
            uint nSamplePerAggregate,
            uint overviewBufferSize);

        #endregion

        #region Public Definitions

        public class PicoException : ApplicationException
        {
          public PicoException() : base() { }

          public PicoException(String message) : base(message) { }

          public PicoException(String message, Exception innerException) : base(message, innerException) { }
        }

        #endregion

        #region Private Members

        private string GetInfo(InfoType info)
        {
          StringBuilder s = new StringBuilder(_NUM_UNIT_INFO_CHARS);

          CheckRet(ps3000_get_unit_info(_handle, s, _NUM_UNIT_INFO_CHARS, info), "ps3000_get_unit_info");

          return s.ToString();
        }

        private void CheckOpened()
        {
          if (!Opened)
            throw new PicoException("PS3000 unit is not open.");
        }
        private void CheckRet(short ret, string func)
        {
          if (ret < 1)
            throw new PicoException(string.Format("The call to {0} returned an error", func));
        }

        #endregion

        #region Public Members

        public bool Open()
        {
            _handle = ps3000_open_unit();

            return Opened;
        }

        public void Close()
        {
            if (Opened)
                ps3000_close_unit(_handle);

            _handle = -1;
        }

        public bool Opened
        {
            get { return _handle > 0; }
        }

        public string DriverVersion
        {
            get { return GetInfo(InfoType.DriverVersion); }
        }

        public string UsbVersion
        {
            get { return GetInfo(InfoType.UsbVersion); }
        }

        public string HardwareVersion
        {
            get { return GetInfo(InfoType.HardwareVersion); }
        }

        public string VariantInfo
        {
            get { return GetInfo(InfoType.VariantInfo); }
        }

        public string Serial
        {
            get { return GetInfo(InfoType.Serial); }
        }

        public string CalibrationDate
        {
            get { return GetInfo(InfoType.CalibrationDate); }
        }

        public ErrorCode Error
        {
            get { return (ErrorCode) Enum.Parse(typeof(ErrorCode), GetInfo(InfoType.ErrorCode)); }
        }

        public string KernelDriverVersion
        {
            get { return GetInfo(InfoType.KernelDriverVersion); }
        }

        public void SetChannel(Channel channel, short enabled, Coupling dc, Range range)
        {
            CheckOpened();
            CheckRet(ps3000_set_channel(_handle, channel, enabled, dc, range), "ps3000_set_channel");
        }

        public bool GetTimebase(short timebase, int numSamples, out int nsInterval, out TimeUnits timeUnits, short oversample, out int maxSamples)
        {
            CheckOpened();
            short ret = ps3000_get_timebase(_handle, timebase, numSamples, out nsInterval, out timeUnits, oversample, out maxSamples);

            return ret > 0;
        }

        public int SetSiggen(SiggenWaveType waveType, int startFreq, int stopFreq, float increment, short dwellTime, bool repeat, bool dualSlope)
        {
            CheckOpened();
            return ps3000_set_siggen(_handle, waveType, startFreq, stopFreq, increment, dwellTime, repeat, dualSlope);
        }

        public int SetEts(EtsMode mode, short cycles, short interleave)
        {
            CheckOpened();
            return ps3000_set_ets(_handle, mode, cycles, interleave);
        }

        public void SetTrigger(Channel source, short threshold, TriggerThresholdDirection direction, float delay, short autoTriggerMs)
        {
            CheckOpened();
            CheckRet(ps3000_set_trigger2(_handle, source, threshold, direction, delay, autoTriggerMs), "ps3000_set_trigger2");
        }

        public void RunBlock(int numSamples, short timebase, short oversample, out int timeIndisposedMs)
        {
            CheckOpened();
            CheckRet(ps3000_run_block(_handle, numSamples, timebase, oversample, out timeIndisposedMs), "ps3000_run_block");
        }

        public void RunStreaming(short timeIntervalMs, int maxSamples, short windowed)
        {
            CheckOpened();
            CheckRet(ps3000_run_streaming(_handle, timeIntervalMs, maxSamples, windowed), "ps3000_run_streaming");
        }

        public bool Ready()
        {
            CheckOpened();
            short ready = ps3000_ready(_handle);

            return ready > 0;
        }

        public void Stop()
        {
            CheckOpened();
            ps3000_stop(_handle);
        }

        public int GetValues(short[] bufferA, short[] bufferB, short[] bufferC, short[] bufferD, out short overflows, int numValues)
        {
            CheckOpened();
            return ps3000_get_values(_handle, bufferA, bufferB, bufferC, bufferD, out overflows, numValues);
        }

        public int GetValues(int[] times, short[] bufferA, short[] bufferB, short[] bufferC, short[] bufferD, out short overflows, TimeUnits timeUnits, int numValues)
        {
            CheckOpened();
            return ps3000_get_times_and_values(_handle, times, bufferA, bufferB, bufferC, bufferD, out overflows, timeUnits, numValues);
        }

        #endregion

        #region Destructor and Disposal

        ~PS3000()
        {
            Dispose();
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                Close();
                _disposed = true;

                GC.SuppressFinalize(this);
            }
        }

        #endregion
    }
}