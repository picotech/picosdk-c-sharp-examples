/**************************************************************************
*
* Filename:    PicoConnectProbes.cs
*
* Description:
*
* This file is based on the C header file PicoConnectProbes.h and 
* defines the enumerations and functions relating to PicoConnect (TM)
* probes, along with the input ranges for current clamps and a high
* voltage probe.
* 
* Copyright (C) 2017 Pico Technology Ltd. See LICENSE file for terms.
*
*************************************************************************/

using System;

namespace PicoConnectProbes
{ 
    public static class PicoConnectProbes
    {
        #region PicoConnectProbe constants

        // Input ranges (amps) when using 200 A - 2 kA current clamp
        public static readonly ushort[] inputRanges2kA = {1, 2, 5, 10, 20, 50, 100, 200, 500, 1000, 2000};

        // Input ranges (milliamperes) when using 40 A current clamp
        public static readonly ushort[] inputRanges40A = {100, 200, 500, 1000, 2000, 5000, 10000, 20000, 40000};

        // Input ranges (volts) when using CAT III High Voltage probe
        public static readonly float[] inputRanges1kV = {2.5F, 5, 12.5F, 25, 50, 125, 250, 500, 1000};

        #endregion

        #region PicoConnectProbe enums

        public enum PicoConnectProbe : uint
        {
            PICO_CONNECT_PROBE_NONE = 0,

            PICO_CONNECT_PROBE_D9_BNC = 4000,
            PICO_CONNECT_PROBE_D9_2X_BNC,
            PICO_CONNECT_PROBE_DIFFERENTIAL,
            PICO_CONNECT_PROBE_CURRENT_CLAMP_200_2KA,
            PICO_CONNECT_PROBE_CURRENT_CLAMP_40A,
            PICO_CONNECT_PROBE_CAT3_HV_1KV,

            PICO_CONNECT_PROBE_INTELLIGENT = (uint) 0xFFFFFFFD,

            PICO_CONNECT_PROBE_UNKNOWN_PROBE = (uint) 0xFFFFFFFE,
            PICO_CONNECT_PROBE_FAULT_PROBE = (uint) 0xFFFFFFFF
        }


        public enum PicoConnectProbeRange : int
        {
            PICO_X1_PROBE_10MV,
            PICO_X1_PROBE_20MV,
            PICO_X1_PROBE_50MV,
            PICO_X1_PROBE_100MV,
            PICO_X1_PROBE_200MV,
            PICO_X1_PROBE_500MV,
            PICO_X1_PROBE_1V,
            PICO_X1_PROBE_2V,
            PICO_X1_PROBE_5V,
            PICO_X1_PROBE_10V,
            PICO_X1_PROBE_20V,
            PICO_X1_PROBE_50V,
            PICO_X1_PROBE_100V,
            PICO_X1_PROBE_200V,
            PICO_X1_PROBE_RANGES = (PICO_X1_PROBE_200V + 1) - PICO_X1_PROBE_10MV,

            PICO_PS4000A_RESISTANCE_315K = 0x00000200,
            PICO_PS4000A_RESISTANCE_1100K,
            PICO_PS4000A_RESISTANCE_10M,
            PICO_PS4000A_MAX_RESISTANCE_RANGES = (PICO_PS4000A_RESISTANCE_10M + 1) - PICO_PS4000A_RESISTANCE_315K,
            PICO_PS4000A_RESISTANCE_ADCV_FLAG = 0x10000000,

            PICO_CONNECT_PROBE_OFF = 1024,

            PICO_D9_BNC_10MV = 0,
            PICO_D9_BNC_20MV,
            PICO_D9_BNC_50MV,
            PICO_D9_BNC_100MV,
            PICO_D9_BNC_200MV,
            PICO_D9_BNC_500MV,
            PICO_D9_BNC_1V,
            PICO_D9_BNC_2V,
            PICO_D9_BNC_5V,
            PICO_D9_BNC_10V,
            PICO_D9_BNC_20V,
            PICO_D9_BNC_50V,
            PICO_D9_BNC_100V,
            PICO_D9_BNC_200V,
            PICO_MAX_D9_BNC_RANGES = (PICO_D9_BNC_200V + 1) - PICO_D9_BNC_10MV,


            PICO_D9_2X_BNC_10MV = PICO_D9_BNC_10MV,
            PICO_D9_2X_BNC_20MV = PICO_D9_BNC_20MV,
            PICO_D9_2X_BNC_50MV = PICO_D9_BNC_50MV,
            PICO_D9_2X_BNC_100MV = PICO_D9_BNC_100MV,
            PICO_D9_2X_BNC_200MV = PICO_D9_BNC_200MV,
            PICO_D9_2X_BNC_500MV = PICO_D9_BNC_500MV,
            PICO_D9_2X_BNC_1V = PICO_D9_BNC_1V,
            PICO_D9_2X_BNC_2V = PICO_D9_BNC_2V,
            PICO_D9_2X_BNC_5V = PICO_D9_BNC_5V,
            PICO_D9_2X_BNC_10V = PICO_D9_BNC_10V,
            PICO_D9_2X_BNC_20V = PICO_D9_BNC_20V,
            PICO_D9_2X_BNC_50V = PICO_D9_BNC_50V,
            PICO_D9_2X_BNC_100V = PICO_D9_BNC_100V,
            PICO_D9_2X_BNC_200V = PICO_D9_BNC_200V,
            PICO_MAX_D9_2X_BNC_RANGES = (PICO_D9_2X_BNC_200V + 1) - PICO_D9_2X_BNC_10MV,


            PICO_DIFFERENTIAL_10MV = PICO_D9_BNC_10MV,
            PICO_DIFFERENTIAL_20MV = PICO_D9_BNC_20MV,
            PICO_DIFFERENTIAL_50MV = PICO_D9_BNC_50MV,
            PICO_DIFFERENTIAL_100MV = PICO_D9_BNC_100MV,
            PICO_DIFFERENTIAL_200MV = PICO_D9_BNC_200MV,
            PICO_DIFFERENTIAL_500MV = PICO_D9_BNC_500MV,
            PICO_DIFFERENTIAL_1V = PICO_D9_BNC_1V,
            PICO_DIFFERENTIAL_2V = PICO_D9_BNC_2V,
            PICO_DIFFERENTIAL_5V = PICO_D9_BNC_5V,
            PICO_DIFFERENTIAL_10V = PICO_D9_BNC_10V,
            PICO_DIFFERENTIAL_20V = PICO_D9_BNC_20V,
            PICO_DIFFERENTIAL_50V = PICO_D9_BNC_50V,
            PICO_DIFFERENTIAL_100V = PICO_D9_BNC_100V,
            PICO_DIFFERENTIAL_200V = PICO_D9_BNC_200V,
            PICO_MAX_DIFFERENTIAL_RANGES = (PICO_DIFFERENTIAL_200V + 1) - PICO_DIFFERENTIAL_10MV,


            PICO_CURRENT_CLAMP_200A_2kA_1A = 4000,
            PICO_CURRENT_CLAMP_200A_2kA_2A,
            PICO_CURRENT_CLAMP_200A_2kA_5A,
            PICO_CURRENT_CLAMP_200A_2kA_10A,
            PICO_CURRENT_CLAMP_200A_2kA_20A,
            PICO_CURRENT_CLAMP_200A_2kA_50A,
            PICO_CURRENT_CLAMP_200A_2kA_100A,
            PICO_CURRENT_CLAMP_200A_2kA_200A,
            PICO_CURRENT_CLAMP_200A_2kA_500A,
            PICO_CURRENT_CLAMP_200A_2kA_1000A,
            PICO_CURRENT_CLAMP_200A_2kA_2000A,
            PICO_MAX_CURRENT_CLAMP_200A_2kA_RANGES = (PICO_CURRENT_CLAMP_200A_2kA_2000A + 1) - PICO_CURRENT_CLAMP_200A_2kA_1A,


            PICO_CURRENT_CLAMP_40A_100mA = 5000,
            PICO_CURRENT_CLAMP_40A_200mA,
            PICO_CURRENT_CLAMP_40A_500mA,
            PICO_CURRENT_CLAMP_40A_1A,
            PICO_CURRENT_CLAMP_40A_2A,
            PICO_CURRENT_CLAMP_40A_5A,
            PICO_CURRENT_CLAMP_40A_10A,
            PICO_CURRENT_CLAMP_40A_20A,
            PICO_CURRENT_CLAMP_40A_40A,
            PICO_MAX_CURRENT_CLAMP_40A_RANGES = (PICO_CURRENT_CLAMP_40A_40A + 1) - PICO_CURRENT_CLAMP_40A_100mA,

            PICO_1KV_2_5V = 6003,
            PICO_1KV_5V,
            PICO_1KV_12_5V,
            PICO_1KV_25V,
            PICO_1KV_50V,
            PICO_1KV_125V,
            PICO_1KV_250V,
            PICO_1KV_500V,
            PICO_1KV_1000V,
            PICO_MAX_1KV_RANGES = (PICO_1KV_1000V + 1) - PICO_1KV_2_5V

        }

        #endregion

        #region Functions

        /*
         * getPicoConnectProbeString 
         * 
         * Returns the name of the PicoConnect (TM) Probe corresponding to
         * the PicoConnectProbe enumeration.
         */
        public static String getPicoConnectProbeString(PicoConnectProbe name)
        {
            switch (name)
            {
                case PicoConnectProbe.PICO_CONNECT_PROBE_NONE:
                    return "None";

                case PicoConnectProbe.PICO_CONNECT_PROBE_D9_BNC:
                    return "D9 BNC";

                case PicoConnectProbe.PICO_CONNECT_PROBE_D9_2X_BNC:
                    return "D9 2x BNC";

                case PicoConnectProbe.PICO_CONNECT_PROBE_DIFFERENTIAL:
                    return "Differential";

                case PicoConnectProbe.PICO_CONNECT_PROBE_CURRENT_CLAMP_200_2KA:
                    return "Current Clamp 200Amp and 2000Amp";

                case PicoConnectProbe.PICO_CONNECT_PROBE_CURRENT_CLAMP_40A:
                    return "Current Clamp 40Amp";

                case PicoConnectProbe.PICO_CONNECT_PROBE_CAT3_HV_1KV:
                    return "Probe CatIII HV 1kV";

                case PicoConnectProbe.PICO_CONNECT_PROBE_INTELLIGENT:
                    return "Intelligent";

                case PicoConnectProbe.PICO_CONNECT_PROBE_UNKNOWN_PROBE:
                    return "Unknown";

                case PicoConnectProbe.PICO_CONNECT_PROBE_FAULT_PROBE:
                    return "Fault";
            }

            return "Not Implemented Probe Name";
        }

        #endregion
    }
}

