/****************************************************************************
 *
 * Filename:    PicoScaling.cs
 * Copyright:   Pico Technology Limited 2023 - 2024
 * Description:
 *
 * This header defines scaling related to all channel and probe ranges
 * with corresponding units.
 * For example - voltage/current/resistance/pressure/temperature etc.
 *
 ****************************************************************************/

using System;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;

namespace ProbeScaling
{
    public struct PicoProbeScaling
	{
        public PicoConnectProbes.PicoConnectProbes.PicoConnectProbeRange ProbeEnum;
        public string Probe_Range_text;
        public double MinScale;
        public double MaxScale;
        public string Unit_text;
        public PicoProbeScaling(PicoConnectProbes.PicoConnectProbes.PicoConnectProbeRange probeenum,
                                string probe_range_text,
                                double minscale,
                                double maxscale,
                                string unit_text)
        {
            ProbeEnum = probeenum;
            Probe_Range_text = probe_range_text;
            MinScale = minscale;
            MaxScale = maxscale;
            Unit_text = unit_text;
        }
    }

    class Scaling
    {
        public static readonly uint[] inputRanges = { 10, 20, 50, 100, 200, 500, 1000, 2000, 5000, 10000, 20000, 50000, 100000, 200000 };
        public enum Std_Voltage_Range : int
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
            Range_200V
        }

        public static readonly ReadOnlyCollection<PicoProbeScaling> ProbeArray = new ReadOnlyCollection<PicoProbeScaling>(new[]
        {
            // x1
        new PicoProbeScaling(){ ProbeEnum = PicoConnectProbes.PicoConnectProbes.PicoConnectProbeRange.PICO_X1_PROBE_10MV, Probe_Range_text = "10mV",   MinScale = -0.01,MaxScale = 0.01,   Unit_text =  "V"},
        new PicoProbeScaling(){ ProbeEnum = PicoConnectProbes.PicoConnectProbes.PicoConnectProbeRange.PICO_X1_PROBE_20MV, Probe_Range_text = "20mV",   MinScale = -0.02,MaxScale = 0.02,   Unit_text =  "V"},
        new PicoProbeScaling(){ ProbeEnum = PicoConnectProbes.PicoConnectProbes.PicoConnectProbeRange.PICO_X1_PROBE_50MV, Probe_Range_text = "50mV",   MinScale = -0.05,MaxScale = 0.05,   Unit_text =  "V"},
        new PicoProbeScaling(){ ProbeEnum = PicoConnectProbes.PicoConnectProbes.PicoConnectProbeRange.PICO_X1_PROBE_100MV, Probe_Range_text = "100mV",  MinScale = -0.1, MaxScale = 0.1,   Unit_text =  "V"},
        new PicoProbeScaling(){ ProbeEnum = PicoConnectProbes.PicoConnectProbes.PicoConnectProbeRange.PICO_X1_PROBE_200MV, Probe_Range_text = "200mV",  MinScale = -0.2, MaxScale = 0.2,   Unit_text =  "V"},
        new PicoProbeScaling(){ ProbeEnum = PicoConnectProbes.PicoConnectProbes.PicoConnectProbeRange.PICO_X1_PROBE_500MV, Probe_Range_text = "500mV",  MinScale = -0.5, MaxScale = 0.5,   Unit_text =  "V"},
        new PicoProbeScaling(){ ProbeEnum = PicoConnectProbes.PicoConnectProbes.PicoConnectProbeRange.PICO_X1_PROBE_1V, Probe_Range_text = "1V",     MinScale = -1,   MaxScale = 1,   Unit_text =  "V"},
        new PicoProbeScaling(){ ProbeEnum = PicoConnectProbes.PicoConnectProbes.PicoConnectProbeRange.PICO_X1_PROBE_2V, Probe_Range_text = "2V",     MinScale = -2,   MaxScale = 2,   Unit_text =  "V"},
        new PicoProbeScaling(){ ProbeEnum = PicoConnectProbes.PicoConnectProbes.PicoConnectProbeRange.PICO_X1_PROBE_5V, Probe_Range_text = "5V",     MinScale = -5,   MaxScale = 5,   Unit_text =  "V"},
        new PicoProbeScaling(){ ProbeEnum = PicoConnectProbes.PicoConnectProbes.PicoConnectProbeRange.PICO_X1_PROBE_10V, Probe_Range_text = "10V",    MinScale = -10,   MaxScale = 10,   Unit_text =  "V"},
        new PicoProbeScaling(){ ProbeEnum = PicoConnectProbes.PicoConnectProbes.PicoConnectProbeRange.PICO_X1_PROBE_20MV, Probe_Range_text = "20V",    MinScale = -20,   MaxScale = 20,   Unit_text =  "V"},
        new PicoProbeScaling(){ ProbeEnum = PicoConnectProbes.PicoConnectProbes.PicoConnectProbeRange.PICO_X1_PROBE_50V,
            Probe_Range_text = "50V", MinScale = -50, MaxScale = 50, Unit_text = "V"},
        new PicoProbeScaling(){ ProbeEnum = PicoConnectProbes.PicoConnectProbes.PicoConnectProbeRange.PICO_X1_PROBE_100V,
            Probe_Range_text = "100V", MinScale = -100, MaxScale = 100, Unit_text = "V"},
        new PicoProbeScaling(){ ProbeEnum = PicoConnectProbes.PicoConnectProbes.PicoConnectProbeRange.PICO_X1_PROBE_200V,
            Probe_Range_text = "200V", MinScale = -200, MaxScale = 200, Unit_text = "V"},
        // x10
        new PicoProbeScaling(){ ProbeEnum = PicoConnectProbes.PicoConnectProbes.PicoConnectProbeRange.PICO_X10_PROBE_100MV, Probe_Range_text = "x10_100mV",  MinScale = -0.1, MaxScale = 0.1,   Unit_text =  "V"},
        new PicoProbeScaling(){ ProbeEnum = PicoConnectProbes.PicoConnectProbes.PicoConnectProbeRange.PICO_X10_PROBE_200MV, Probe_Range_text = "x10_200mV",  MinScale = -0.2, MaxScale = 0.2,   Unit_text =  "V"},
        new PicoProbeScaling(){ ProbeEnum = PicoConnectProbes.PicoConnectProbes.PicoConnectProbeRange.PICO_X10_PROBE_500MV, Probe_Range_text = "x10_500mV",  MinScale = -0.5, MaxScale = 0.5,   Unit_text =  "V"},
        new PicoProbeScaling(){ ProbeEnum = PicoConnectProbes.PicoConnectProbes.PicoConnectProbeRange.PICO_X10_PROBE_1V, Probe_Range_text = "x10_1V",     MinScale = -1,   MaxScale = 1,   Unit_text =  "V"},
        new PicoProbeScaling(){ ProbeEnum = PicoConnectProbes.PicoConnectProbes.PicoConnectProbeRange.PICO_X10_PROBE_2V, Probe_Range_text = "x10_2V",     MinScale = -2,   MaxScale = 2,   Unit_text =  "V"},
        new PicoProbeScaling(){ ProbeEnum = PicoConnectProbes.PicoConnectProbes.PicoConnectProbeRange.PICO_X10_PROBE_5V, Probe_Range_text = "x10_5V",     MinScale = -5,   MaxScale = 5,   Unit_text =  "V"},
        new PicoProbeScaling(){ ProbeEnum = PicoConnectProbes.PicoConnectProbes.PicoConnectProbeRange.PICO_X10_PROBE_10V, Probe_Range_text = "x10_10V",    MinScale = -10,   MaxScale = 10,   Unit_text =  "V"},
        new PicoProbeScaling(){ ProbeEnum = PicoConnectProbes.PicoConnectProbes.PicoConnectProbeRange.PICO_X10_PROBE_20V, Probe_Range_text = "x10_20V",    MinScale = -20,   MaxScale = 20,   Unit_text =  "V"},
        new PicoProbeScaling(){ ProbeEnum = PicoConnectProbes.PicoConnectProbes.PicoConnectProbeRange.PICO_X10_PROBE_50V,
            Probe_Range_text = "x10_50V", MinScale = -50, MaxScale = 50, Unit_text = "V"},
        new PicoProbeScaling(){ ProbeEnum = PicoConnectProbes.PicoConnectProbes.PicoConnectProbeRange.PICO_X10_PROBE_100V,
            Probe_Range_text = "x10_100V", MinScale = -100, MaxScale = 100, Unit_text = "V"},
        new PicoProbeScaling(){ ProbeEnum = PicoConnectProbes.PicoConnectProbes.PicoConnectProbeRange.PICO_X10_PROBE_200V,
            Probe_Range_text = "x10_200V", MinScale = -200, MaxScale = 200, Unit_text = "V"},
        new PicoProbeScaling(){ ProbeEnum = PicoConnectProbes.PicoConnectProbes.PicoConnectProbeRange.PICO_X10_PROBE_500V,
            Probe_Range_text = "x10_500V", MinScale = -500, MaxScale = 500, Unit_text = "V"},
        // D9_BNC
        new PicoProbeScaling(){ ProbeEnum = PicoConnectProbes.PicoConnectProbes.PicoConnectProbeRange.PICO_D9_BNC_10MV, Probe_Range_text = "D9_BNC_10mV",  MinScale = -0.01, MaxScale = 0.01,   Unit_text =  "V"},
        new PicoProbeScaling(){ ProbeEnum = PicoConnectProbes.PicoConnectProbes.PicoConnectProbeRange.PICO_D9_BNC_20MV, Probe_Range_text = "D9_BNC_20mV",  MinScale = -0.02, MaxScale = 0.02,   Unit_text =  "V"},
        new PicoProbeScaling(){ ProbeEnum = PicoConnectProbes.PicoConnectProbes.PicoConnectProbeRange.PICO_D9_BNC_50MV, Probe_Range_text = "D9_BNC_50mV",  MinScale = -0.05, MaxScale = 0.05,   Unit_text =  "V"},
        new PicoProbeScaling(){ ProbeEnum = PicoConnectProbes.PicoConnectProbes.PicoConnectProbeRange.PICO_D9_BNC_100MV, Probe_Range_text = "D9_BNC_100mV",  MinScale = -0.1, MaxScale = 0.1,   Unit_text =  "V"},
        new PicoProbeScaling(){ ProbeEnum = PicoConnectProbes.PicoConnectProbes.PicoConnectProbeRange.PICO_D9_BNC_200MV, Probe_Range_text = "D9_BNC_200mV",  MinScale = -0.2, MaxScale = 0.2,   Unit_text =  "V"},
        new PicoProbeScaling(){ ProbeEnum = PicoConnectProbes.PicoConnectProbes.PicoConnectProbeRange.PICO_D9_BNC_500MV, Probe_Range_text = "D9_BNC_500mV",  MinScale = -0.5, MaxScale = 0.5,   Unit_text =  "V"},
        new PicoProbeScaling(){ ProbeEnum = PicoConnectProbes.PicoConnectProbes.PicoConnectProbeRange.PICO_D9_BNC_1V, Probe_Range_text = "D9_BNC_1V",     MinScale = -1,   MaxScale = 1,   Unit_text =  "V"},
        new PicoProbeScaling(){ ProbeEnum = PicoConnectProbes.PicoConnectProbes.PicoConnectProbeRange.PICO_D9_BNC_2V, Probe_Range_text = "D9_BNC_2V",     MinScale = -2,   MaxScale = 2,   Unit_text =  "V"},
        new PicoProbeScaling(){ ProbeEnum = PicoConnectProbes.PicoConnectProbes.PicoConnectProbeRange.PICO_D9_BNC_5V, Probe_Range_text = "D9_BNC_5V",     MinScale = -5,   MaxScale = 5,   Unit_text =  "V"},
        new PicoProbeScaling(){ ProbeEnum = PicoConnectProbes.PicoConnectProbes.PicoConnectProbeRange.PICO_D9_BNC_10V, Probe_Range_text = "D9_BNC_10V",    MinScale = -10,   MaxScale = 10,   Unit_text =  "V"},
        new PicoProbeScaling(){ ProbeEnum = PicoConnectProbes.PicoConnectProbes.PicoConnectProbeRange.PICO_D9_BNC_20MV, Probe_Range_text = "D9_BNC_20V",    MinScale = -20,   MaxScale = 20,   Unit_text =  "V"},
        new PicoProbeScaling(){ ProbeEnum = PicoConnectProbes.PicoConnectProbes.PicoConnectProbeRange.PICO_D9_BNC_50V,
            Probe_Range_text = "D9_BNC_50V", MinScale = -50, MaxScale = 50, Unit_text = "V"},
        new PicoProbeScaling(){ ProbeEnum = PicoConnectProbes.PicoConnectProbes.PicoConnectProbeRange.PICO_D9_BNC_100V,
            Probe_Range_text = "D9_BNC_100V", MinScale = -100, MaxScale = 100, Unit_text = "V"},
        new PicoProbeScaling(){ ProbeEnum = PicoConnectProbes.PicoConnectProbes.PicoConnectProbeRange.PICO_D9_BNC_200V,
            Probe_Range_text = "D9_BNC_200V", MinScale = -200, MaxScale = 200, Unit_text = "V"},
        // D9_2X_BNC
        new PicoProbeScaling(){ ProbeEnum = PicoConnectProbes.PicoConnectProbes.PicoConnectProbeRange.PICO_D9_2X_BNC_10MV, Probe_Range_text = "D9_2X_BNC_10mV",  MinScale = -0.01, MaxScale = 0.01,   Unit_text =  "V"},
        new PicoProbeScaling(){ ProbeEnum = PicoConnectProbes.PicoConnectProbes.PicoConnectProbeRange.PICO_D9_2X_BNC_20MV, Probe_Range_text = "D9_2X_BNC_20mV",  MinScale = -0.02, MaxScale = 0.02,   Unit_text =  "V"},
        new PicoProbeScaling(){ ProbeEnum = PicoConnectProbes.PicoConnectProbes.PicoConnectProbeRange.PICO_D9_2X_BNC_50MV, Probe_Range_text = "D9_2X_BNC_50mV",  MinScale = -0.05, MaxScale = 0.05,   Unit_text =  "V"},
        new PicoProbeScaling(){ ProbeEnum = PicoConnectProbes.PicoConnectProbes.PicoConnectProbeRange.PICO_D9_2X_BNC_100MV, Probe_Range_text = "D9_2X_BNC_100mV",  MinScale = -0.1, MaxScale = 0.1,   Unit_text =  "V"},
        new PicoProbeScaling(){ ProbeEnum = PicoConnectProbes.PicoConnectProbes.PicoConnectProbeRange.PICO_D9_2X_BNC_200MV, Probe_Range_text = "D9_2X_BNC_200mV",  MinScale = -0.2, MaxScale = 0.2,   Unit_text =  "V"},
        new PicoProbeScaling(){ ProbeEnum = PicoConnectProbes.PicoConnectProbes.PicoConnectProbeRange.PICO_D9_2X_BNC_500MV, Probe_Range_text = "D9_2X_BNC_500mV",  MinScale = -0.5, MaxScale = 0.5,   Unit_text =  "V"},
        new PicoProbeScaling(){ ProbeEnum = PicoConnectProbes.PicoConnectProbes.PicoConnectProbeRange.PICO_D9_2X_BNC_1V, Probe_Range_text = "D9_2X_BNC_1V",     MinScale = -1,   MaxScale = 1,   Unit_text =  "V"},
        new PicoProbeScaling(){ ProbeEnum = PicoConnectProbes.PicoConnectProbes.PicoConnectProbeRange.PICO_D9_2X_BNC_2V, Probe_Range_text = "D9_2X_BNC_2V",     MinScale = -2,   MaxScale = 2,   Unit_text =  "V"},
        new PicoProbeScaling(){ ProbeEnum = PicoConnectProbes.PicoConnectProbes.PicoConnectProbeRange.PICO_D9_2X_BNC_5V, Probe_Range_text = "D9_2X_BNC_5V",     MinScale = -5,   MaxScale = 5,   Unit_text =  "V"},
        new PicoProbeScaling(){ ProbeEnum = PicoConnectProbes.PicoConnectProbes.PicoConnectProbeRange.PICO_D9_2X_BNC_10V, Probe_Range_text = "D9_2X_BNC_10V",    MinScale = -10,   MaxScale = 10,   Unit_text =  "V"},
        new PicoProbeScaling(){ ProbeEnum = PicoConnectProbes.PicoConnectProbes.PicoConnectProbeRange.PICO_D9_2X_BNC_20MV, Probe_Range_text = "D9_2X_BNC_20V",    MinScale = -20,   MaxScale = 20,   Unit_text =  "V"},
        new PicoProbeScaling(){ ProbeEnum = PicoConnectProbes.PicoConnectProbes.PicoConnectProbeRange.PICO_D9_2X_BNC_50V,
            Probe_Range_text = "D9_2X_BNC_50V", MinScale = -50, MaxScale = 50, Unit_text = "V"},
        new PicoProbeScaling(){ ProbeEnum = PicoConnectProbes.PicoConnectProbes.PicoConnectProbeRange.PICO_D9_2X_BNC_100V,
            Probe_Range_text = "D9_2X_BNC_100V", MinScale = -100, MaxScale = 100, Unit_text = "V"},
        new PicoProbeScaling(){ ProbeEnum = PicoConnectProbes.PicoConnectProbes.PicoConnectProbeRange.PICO_D9_2X_BNC_200V,
            Probe_Range_text = "D9_2X_BNC_200V", MinScale = -200, MaxScale = 200, Unit_text = "V"},
        // DIFFERENTIAL
        new PicoProbeScaling(){ ProbeEnum = PicoConnectProbes.PicoConnectProbes.PicoConnectProbeRange.PICO_DIFFERENTIAL_10MV, Probe_Range_text = "DIFFERENTIAL_10mV",  MinScale = -0.01, MaxScale = 0.01,   Unit_text =  "V"},
        new PicoProbeScaling(){ ProbeEnum = PicoConnectProbes.PicoConnectProbes.PicoConnectProbeRange.PICO_DIFFERENTIAL_20MV, Probe_Range_text = "DIFFERENTIAL_20mV",  MinScale = -0.02, MaxScale = 0.02,   Unit_text =  "V"},
        new PicoProbeScaling(){ ProbeEnum = PicoConnectProbes.PicoConnectProbes.PicoConnectProbeRange.PICO_DIFFERENTIAL_50MV, Probe_Range_text = "DIFFERENTIAL_50mV",  MinScale = -0.05, MaxScale = 0.05,   Unit_text =  "V"},
        new PicoProbeScaling(){ ProbeEnum = PicoConnectProbes.PicoConnectProbes.PicoConnectProbeRange.PICO_DIFFERENTIAL_100MV, Probe_Range_text = "DIFFERENTIAL_100mV",  MinScale = -0.1, MaxScale = 0.1,   Unit_text =  "V"},
        new PicoProbeScaling(){ ProbeEnum = PicoConnectProbes.PicoConnectProbes.PicoConnectProbeRange.PICO_DIFFERENTIAL_200MV, Probe_Range_text = "DIFFERENTIAL_200mV",  MinScale = -0.2, MaxScale = 0.2,   Unit_text =  "V"},
        new PicoProbeScaling(){ ProbeEnum = PicoConnectProbes.PicoConnectProbes.PicoConnectProbeRange.PICO_DIFFERENTIAL_500MV, Probe_Range_text = "DIFFERENTIAL_500mV",  MinScale = -0.5, MaxScale = 0.5,   Unit_text =  "V"},
        new PicoProbeScaling(){ ProbeEnum = PicoConnectProbes.PicoConnectProbes.PicoConnectProbeRange.PICO_DIFFERENTIAL_1V, Probe_Range_text = "DIFFERENTIAL_1V",     MinScale = -1,   MaxScale = 1,   Unit_text =  "V"},
        new PicoProbeScaling(){ ProbeEnum = PicoConnectProbes.PicoConnectProbes.PicoConnectProbeRange.PICO_DIFFERENTIAL_2V, Probe_Range_text = "DIFFERENTIAL_2V",     MinScale = -2,   MaxScale = 2,   Unit_text =  "V"},
        new PicoProbeScaling(){ ProbeEnum = PicoConnectProbes.PicoConnectProbes.PicoConnectProbeRange.PICO_DIFFERENTIAL_5V, Probe_Range_text = "DIFFERENTIAL_5V",     MinScale = -5,   MaxScale = 5,   Unit_text =  "V"},
        new PicoProbeScaling(){ ProbeEnum = PicoConnectProbes.PicoConnectProbes.PicoConnectProbeRange.PICO_DIFFERENTIAL_10V, Probe_Range_text = "DIFFERENTIAL_10V",    MinScale = -10,   MaxScale = 10,   Unit_text =  "V"},
        new PicoProbeScaling(){ ProbeEnum = PicoConnectProbes.PicoConnectProbes.PicoConnectProbeRange.PICO_DIFFERENTIAL_20MV, Probe_Range_text = "DIFFERENTIAL_20V",    MinScale = -20,   MaxScale = 20,   Unit_text =  "V"},
        new PicoProbeScaling(){ ProbeEnum = PicoConnectProbes.PicoConnectProbes.PicoConnectProbeRange.PICO_DIFFERENTIAL_50V,
            Probe_Range_text = "DIFFERENTIAL_50V", MinScale = -50, MaxScale = 50, Unit_text = "V"},
        new PicoProbeScaling(){ ProbeEnum = PicoConnectProbes.PicoConnectProbes.PicoConnectProbeRange.PICO_DIFFERENTIAL_100V,
            Probe_Range_text = "DIFFERENTIAL_100V", MinScale = -100, MaxScale = 100, Unit_text = "V"},
        new PicoProbeScaling(){ ProbeEnum = PicoConnectProbes.PicoConnectProbes.PicoConnectProbeRange.PICO_DIFFERENTIAL_200V,
            Probe_Range_text = "DIFFERENTIAL_200V", MinScale = -200, MaxScale = 200, Unit_text = "V"},
        // Resistance Probe
         new PicoProbeScaling(){ ProbeEnum = PicoConnectProbes.PicoConnectProbes.PicoConnectProbeRange.PICO_PS4000A_RESISTANCE_315K,
            Probe_Range_text = "315kOhm", MinScale = 0, MaxScale = 315000, Unit_text = "Ohms"},
         new PicoProbeScaling(){ ProbeEnum = PicoConnectProbes.PicoConnectProbes.PicoConnectProbeRange.PICO_PS4000A_RESISTANCE_1100K,
            Probe_Range_text = "1.1MOhm", MinScale = 0, MaxScale = 1100000, Unit_text = "Ohms"},   
         new PicoProbeScaling(){ ProbeEnum = PicoConnectProbes.PicoConnectProbes.PicoConnectProbeRange.PICO_PS4000A_RESISTANCE_10M,
            Probe_Range_text = "10MOhm", MinScale = 0, MaxScale = 10000000, Unit_text = "Ohms"},
        // PICO_CURRENT_CLAMP_200A
        new PicoProbeScaling(){ ProbeEnum = PicoConnectProbes.PicoConnectProbes.PicoConnectProbeRange.PICO_CURRENT_CLAMP_200A_2kA_1A, Probe_Range_text = "CURRENT_CLAMP_200A_2kA_1A",    MinScale = -1,   MaxScale = 1,   Unit_text =  "A"},
        new PicoProbeScaling(){ ProbeEnum = PicoConnectProbes.PicoConnectProbes.PicoConnectProbeRange.PICO_CURRENT_CLAMP_200A_2kA_2A, Probe_Range_text = "CURRENT_CLAMP_200A_2kA_2A",    MinScale = -2,   MaxScale = 2,   Unit_text =  "A"},
        new PicoProbeScaling(){ ProbeEnum = PicoConnectProbes.PicoConnectProbes.PicoConnectProbeRange.PICO_CURRENT_CLAMP_200A_2kA_5A, Probe_Range_text = "CURRENT_CLAMP_200A_2kA_5A",    MinScale = -5,   MaxScale = 5,   Unit_text =  "A"},
        new PicoProbeScaling(){ ProbeEnum = PicoConnectProbes.PicoConnectProbes.PicoConnectProbeRange.PICO_CURRENT_CLAMP_200A_2kA_10A, Probe_Range_text = "CURRENT_CLAMP_200A_2kA_10A",    MinScale = -10,   MaxScale = 10,   Unit_text =  "A"},
        new PicoProbeScaling(){ ProbeEnum = PicoConnectProbes.PicoConnectProbes.PicoConnectProbeRange.PICO_CURRENT_CLAMP_200A_2kA_20A, Probe_Range_text = "CURRENT_CLAMP_200A_2kA_20A",    MinScale = -20,   MaxScale = 20,   Unit_text =  "A"},
        new PicoProbeScaling(){ ProbeEnum = PicoConnectProbes.PicoConnectProbes.PicoConnectProbeRange.PICO_CURRENT_CLAMP_200A_2kA_50A, Probe_Range_text = "CURRENT_CLAMP_200A_2kA_50A",    MinScale = -50,   MaxScale = 50,   Unit_text =  "A"},
        new PicoProbeScaling(){ ProbeEnum = PicoConnectProbes.PicoConnectProbes.PicoConnectProbeRange.PICO_CURRENT_CLAMP_200A_2kA_100A, Probe_Range_text = "CURRENT_CLAMP_200A_2kA_100A",    MinScale = -100,   MaxScale = 100,   Unit_text =  "A"},
        new PicoProbeScaling(){ ProbeEnum = PicoConnectProbes.PicoConnectProbes.PicoConnectProbeRange.PICO_CURRENT_CLAMP_200A_2kA_200A, Probe_Range_text = "CURRENT_CLAMP_200A_2kA_200A",    MinScale = -200,   MaxScale = 200,   Unit_text =  "A"},
        new PicoProbeScaling(){ ProbeEnum = PicoConnectProbes.PicoConnectProbes.PicoConnectProbeRange.PICO_CURRENT_CLAMP_200A_2kA_500A, Probe_Range_text = "CURRENT_CLAMP_200A_2kA_500A",    MinScale = -500,   MaxScale = 500,   Unit_text =  "A"},
        new PicoProbeScaling(){ ProbeEnum = PicoConnectProbes.PicoConnectProbes.PicoConnectProbeRange.PICO_CURRENT_CLAMP_200A_2kA_1000A, Probe_Range_text = "CURRENT_CLAMP_200A_2kA_1000A",    MinScale = -1000,   MaxScale = 1000,   Unit_text =  "A"},
        new PicoProbeScaling(){ ProbeEnum = PicoConnectProbes.PicoConnectProbes.PicoConnectProbeRange.PICO_CURRENT_CLAMP_200A_2kA_2000A, Probe_Range_text = "CURRENT_CLAMP_200A_2kA_2000A",    MinScale = -2000,   MaxScale = 2000,   Unit_text =  "A"},
        // CURRENT_CLAMP_40A
        new PicoProbeScaling(){ ProbeEnum = PicoConnectProbes.PicoConnectProbes.PicoConnectProbeRange.PICO_CURRENT_CLAMP_40A_100mA,
            Probe_Range_text = "CURRENT_CLAMP_40A_100mA", MinScale = -0.1, MaxScale = 0.1, Unit_text = "A"},    
        new PicoProbeScaling(){ ProbeEnum = PicoConnectProbes.PicoConnectProbes.PicoConnectProbeRange.PICO_CURRENT_CLAMP_40A_200mA,
            Probe_Range_text = "CURRENT_CLAMP_40A_200mA", MinScale = -0.2, MaxScale = 0.2, Unit_text = "A"},
        new PicoProbeScaling(){ ProbeEnum = PicoConnectProbes.PicoConnectProbes.PicoConnectProbeRange.PICO_CURRENT_CLAMP_40A_500mA,
            Probe_Range_text = "CURRENT_CLAMP_40A_500mA", MinScale = -0.5, MaxScale = 0.5, Unit_text = "A"},
        new PicoProbeScaling(){ ProbeEnum = PicoConnectProbes.PicoConnectProbes.PicoConnectProbeRange.PICO_CURRENT_CLAMP_40A_1A,
            Probe_Range_text = "CURRENT_CLAMP_40A_1A", MinScale = -1, MaxScale = 1, Unit_text = "A"},
        new PicoProbeScaling(){ ProbeEnum = PicoConnectProbes.PicoConnectProbes.PicoConnectProbeRange.PICO_CURRENT_CLAMP_40A_2A,
            Probe_Range_text = "CURRENT_CLAMP_40A_2A", MinScale = -2, MaxScale = 2, Unit_text = "A"},
        new PicoProbeScaling(){ ProbeEnum = PicoConnectProbes.PicoConnectProbes.PicoConnectProbeRange.PICO_CURRENT_CLAMP_40A_5A,
            Probe_Range_text = "CURRENT_CLAMP_40A_5A", MinScale = -5, MaxScale = 5, Unit_text = "A"},
        new PicoProbeScaling(){ ProbeEnum = PicoConnectProbes.PicoConnectProbes.PicoConnectProbeRange.PICO_CURRENT_CLAMP_40A_10A,
            Probe_Range_text = "CURRENT_CLAMP_40A_10A", MinScale = -10, MaxScale = 10, Unit_text = "A"},
        new PicoProbeScaling(){ ProbeEnum = PicoConnectProbes.PicoConnectProbes.PicoConnectProbeRange.PICO_CURRENT_CLAMP_40A_20A,
            Probe_Range_text = "CURRENT_CLAMP_40A_20A", MinScale = -20, MaxScale = 20, Unit_text = "A"},
        new PicoProbeScaling(){ ProbeEnum = PicoConnectProbes.PicoConnectProbes.PicoConnectProbeRange.PICO_CURRENT_CLAMP_40A_40A,
            Probe_Range_text = "CURRENT_CLAMP_40A_40A", MinScale = -40, MaxScale = 40, Unit_text = "A"},
        // 1kV CAT III probe
        new PicoProbeScaling(){ ProbeEnum = PicoConnectProbes.PicoConnectProbes.PicoConnectProbeRange.PICO_1KV_2_5V,
            Probe_Range_text = "1KV_2.5V", MinScale = -2.5, MaxScale = 2.5, Unit_text = "V"},
        new PicoProbeScaling(){ ProbeEnum = PicoConnectProbes.PicoConnectProbes.PicoConnectProbeRange.PICO_1KV_5V,
            Probe_Range_text = "1KV_5V", MinScale = -5, MaxScale = 5, Unit_text = "V"},
      new PicoProbeScaling(){ ProbeEnum = PicoConnectProbes.PicoConnectProbes.PicoConnectProbeRange.PICO_1KV_12_5V,
            Probe_Range_text = "1KV_12.5V", MinScale = -12.5, MaxScale = 12.5, Unit_text = "V"},
        new PicoProbeScaling(){ ProbeEnum = PicoConnectProbes.PicoConnectProbes.PicoConnectProbeRange.PICO_1KV_25V,
            Probe_Range_text = "1KV_25V", MinScale = -25, MaxScale = 25, Unit_text = "V"},
        new PicoProbeScaling(){ ProbeEnum = PicoConnectProbes.PicoConnectProbes.PicoConnectProbeRange.PICO_1KV_50V,
            Probe_Range_text = "1KV_50V", MinScale = -50, MaxScale = 50, Unit_text = "V"},
        new PicoProbeScaling(){ ProbeEnum = PicoConnectProbes.PicoConnectProbes.PicoConnectProbeRange.PICO_1KV_125V,
            Probe_Range_text = "1KV_125V", MinScale = -125, MaxScale = 125, Unit_text = "V"},
        new PicoProbeScaling(){ ProbeEnum = PicoConnectProbes.PicoConnectProbes.PicoConnectProbeRange.PICO_1KV_250V,
            Probe_Range_text = "1KV_250V", MinScale = -250, MaxScale = 250, Unit_text = "V"},
        new PicoProbeScaling(){ ProbeEnum = PicoConnectProbes.PicoConnectProbes.PicoConnectProbeRange.PICO_1KV_500V,
            Probe_Range_text = "1KV_500V", MinScale = -500, MaxScale = 500, Unit_text = "V"},
        new PicoProbeScaling(){ ProbeEnum = PicoConnectProbes.PicoConnectProbes.PicoConnectProbeRange.PICO_1KV_1000V,
            Probe_Range_text = "1KV_1000V", MinScale = -1000, MaxScale = 1000, Unit_text = "V"},
        // CURRENT_CLAMP_2000ARMS
       new PicoProbeScaling(){ ProbeEnum = PicoConnectProbes.PicoConnectProbes.PicoConnectProbeRange.PICO_CURRENT_CLAMP_2000ARMS_10A,
            Probe_Range_text = "CURRENT_CLAMP_2000ARMS_10A", MinScale = -10, MaxScale = 10, Unit_text = "A"},
        new PicoProbeScaling(){ ProbeEnum = PicoConnectProbes.PicoConnectProbes.PicoConnectProbeRange.PICO_CURRENT_CLAMP_2000ARMS_20A,
            Probe_Range_text = "CURRENT_CLAMP_2000ARMS_20A", MinScale = -20, MaxScale = 20, Unit_text = "A"},
        new PicoProbeScaling(){ ProbeEnum = PicoConnectProbes.PicoConnectProbes.PicoConnectProbeRange.PICO_CURRENT_CLAMP_2000ARMS_50A,
            Probe_Range_text = "CURRENT_CLAMP_2000ARMS_50A", MinScale = -50, MaxScale = 50, Unit_text = "A"},
       new PicoProbeScaling(){ ProbeEnum = PicoConnectProbes.PicoConnectProbes.PicoConnectProbeRange.PICO_CURRENT_CLAMP_2000ARMS_100A,
            Probe_Range_text = "CURRENT_CLAMP_2000ARMS_100A", MinScale = -100, MaxScale = 100, Unit_text = "A"},
        new PicoProbeScaling(){ ProbeEnum = PicoConnectProbes.PicoConnectProbes.PicoConnectProbeRange.PICO_CURRENT_CLAMP_2000ARMS_200A,
            Probe_Range_text = "CURRENT_CLAMP_2000ARMS_200A", MinScale = -200, MaxScale = 200, Unit_text = "A"},
        new PicoProbeScaling(){ ProbeEnum = PicoConnectProbes.PicoConnectProbes.PicoConnectProbeRange.PICO_CURRENT_CLAMP_2000ARMS_500A,
            Probe_Range_text = "CURRENT_CLAMP_2000ARMS_500A", MinScale = -500, MaxScale = 500, Unit_text = "A"},
       new PicoProbeScaling(){ ProbeEnum = PicoConnectProbes.PicoConnectProbes.PicoConnectProbeRange.PICO_CURRENT_CLAMP_2000ARMS_1000A,
            Probe_Range_text = "CURRENT_CLAMP_2000ARMS_1000A", MinScale = -1000, MaxScale = 1000, Unit_text = "A"},
        new PicoProbeScaling(){ ProbeEnum = PicoConnectProbes.PicoConnectProbes.PicoConnectProbeRange.PICO_CURRENT_CLAMP_2000ARMS_2000A,
            Probe_Range_text = "CURRENT_CLAMP_2000ARMS_2000A", MinScale = -2000, MaxScale = 2000, Unit_text = "A"},
        new PicoProbeScaling(){ ProbeEnum = PicoConnectProbes.PicoConnectProbes.PicoConnectProbeRange.PICO_CURRENT_CLAMP_2000ARMS_5000A,
            Probe_Range_text = "CURRENT_CLAMP_2000ARMS_5000A", MinScale = -5000, MaxScale = 5000, Unit_text = "A"},
        // CURRENT_CLAMP_100A
        new PicoProbeScaling(){ ProbeEnum = PicoConnectProbes.PicoConnectProbes.PicoConnectProbeRange.PICO_CURRENT_CLAMP_100A_2_5A,
            Probe_Range_text = "PICO_CURRENT_CLAMP_100A_2_5A", MinScale = -2.5, MaxScale = 2.5, Unit_text = "A"},
        new PicoProbeScaling(){ ProbeEnum = PicoConnectProbes.PicoConnectProbes.PicoConnectProbeRange.PICO_CURRENT_CLAMP_100A_5A,
            Probe_Range_text = "PICO_CURRENT_CLAMP_100A_5A", MinScale = -5, MaxScale = 5, Unit_text = "A"},
        new PicoProbeScaling(){ ProbeEnum = PicoConnectProbes.PicoConnectProbes.PicoConnectProbeRange.PICO_CURRENT_CLAMP_100A_10A,
            Probe_Range_text = "PICO_CURRENT_CLAMP_100A_10A", MinScale = -10, MaxScale = 10, Unit_text = "A"},
        new PicoProbeScaling(){ ProbeEnum = PicoConnectProbes.PicoConnectProbes.PicoConnectProbeRange.PICO_CURRENT_CLAMP_100A_25A,
            Probe_Range_text = "PICO_CURRENT_CLAMP_100A_25A", MinScale = -25, MaxScale = 25, Unit_text = "A"},
        new PicoProbeScaling(){ ProbeEnum = PicoConnectProbes.PicoConnectProbes.PicoConnectProbeRange.PICO_CURRENT_CLAMP_100A_50A,
            Probe_Range_text = "PICO_CURRENT_CLAMP_100A_50A", MinScale = -50, MaxScale = 50, Unit_text = "A"},
        new PicoProbeScaling(){ ProbeEnum = PicoConnectProbes.PicoConnectProbes.PicoConnectProbeRange.PICO_CURRENT_CLAMP_100A_100A,
            Probe_Range_text = "PICO_CURRENT_CLAMP_100A_100A", MinScale = -100, MaxScale = 100, Unit_text = "A"},
        // CURRENT_CLAMP_60A
        new PicoProbeScaling(){ ProbeEnum = PicoConnectProbes.PicoConnectProbes.PicoConnectProbeRange.PICO_CURRENT_CLAMP_60A_2A,
            Probe_Range_text = "CURRENT_CLAMP_60A_2A", MinScale = -2, MaxScale = 2, Unit_text = "A"},
        new PicoProbeScaling(){ ProbeEnum = PicoConnectProbes.PicoConnectProbes.PicoConnectProbeRange.PICO_CURRENT_CLAMP_60A_5A,
            Probe_Range_text = "CURRENT_CLAMP_60A_5A", MinScale = -5, MaxScale = 5, Unit_text = "A"},
        new PicoProbeScaling(){ ProbeEnum = PicoConnectProbes.PicoConnectProbes.PicoConnectProbeRange.PICO_CURRENT_CLAMP_60A_10A,
            Probe_Range_text = "CURRENT_CLAMP_60A_10A", MinScale = -10, MaxScale = 10, Unit_text = "A"},
        new PicoProbeScaling(){ ProbeEnum = PicoConnectProbes.PicoConnectProbes.PicoConnectProbeRange.PICO_CURRENT_CLAMP_60A_20A,
            Probe_Range_text = "CURRENT_CLAMP_60A_20A", MinScale = -20, MaxScale = 20, Unit_text = "A"},
        new PicoProbeScaling(){ ProbeEnum = PicoConnectProbes.PicoConnectProbes.PicoConnectProbeRange.PICO_CURRENT_CLAMP_60A_50A,
            Probe_Range_text = "CURRENT_CLAMP_60A_50A", MinScale = -50, MaxScale = 50, Unit_text = "A"},
        new PicoProbeScaling(){ ProbeEnum = PicoConnectProbes.PicoConnectProbes.PicoConnectProbeRange.PICO_CURRENT_CLAMP_60A_60A,
            Probe_Range_text = "CURRENT_CLAMP_60A_60A", MinScale = -60, MaxScale = 60, Unit_text = "A"},
        // CURRENT_CLAMP_60A_V2
        new PicoProbeScaling(){ ProbeEnum = PicoConnectProbes.PicoConnectProbes.PicoConnectProbeRange.PICO_CURRENT_CLAMP_60A_V2_0_5A,
            Probe_Range_text = "CURRENT_CLAMP_60A_V2_0_5A", MinScale = -0.5, MaxScale = 0.5, Unit_text = "A"},
        new PicoProbeScaling(){ ProbeEnum = PicoConnectProbes.PicoConnectProbes.PicoConnectProbeRange.PICO_CURRENT_CLAMP_60A_V2_1A,
            Probe_Range_text = "CURRENT_CLAMP_60A_V2_1A", MinScale = -1, MaxScale = 1, Unit_text = "A"},
        new PicoProbeScaling(){ ProbeEnum = PicoConnectProbes.PicoConnectProbes.PicoConnectProbeRange.PICO_CURRENT_CLAMP_60A_V2_2A,
            Probe_Range_text = "CURRENT_CLAMP_60A_V2_2A", MinScale = -2, MaxScale = 2, Unit_text = "A"},
        new PicoProbeScaling(){ ProbeEnum = PicoConnectProbes.PicoConnectProbes.PicoConnectProbeRange.PICO_CURRENT_CLAMP_60A_V2_5A,
            Probe_Range_text = "CURRENT_CLAMP_60A_V2_5A", MinScale = -5, MaxScale = 5, Unit_text = "A"},
        new PicoProbeScaling(){ ProbeEnum = PicoConnectProbes.PicoConnectProbes.PicoConnectProbeRange.PICO_CURRENT_CLAMP_60A_V2_10A,
            Probe_Range_text = "CURRENT_CLAMP_60A_V2_10A", MinScale = -10, MaxScale = 10, Unit_text = "A"},
        new PicoProbeScaling(){ ProbeEnum = PicoConnectProbes.PicoConnectProbes.PicoConnectProbeRange.PICO_CURRENT_CLAMP_60A_V2_20A,
            Probe_Range_text = "CURRENT_CLAMP_60A_V2_20A", MinScale = -20, MaxScale = 20, Unit_text = "A"},
        new PicoProbeScaling(){ ProbeEnum = PicoConnectProbes.PicoConnectProbes.PicoConnectProbeRange.PICO_CURRENT_CLAMP_60A_V2_50A,
            Probe_Range_text = "CURRENT_CLAMP_60A_V2_50A", MinScale = -50, MaxScale = 50, Unit_text = "A"},
        new PicoProbeScaling(){ ProbeEnum = PicoConnectProbes.PicoConnectProbes.PicoConnectProbeRange.PICO_CURRENT_CLAMP_60A_V2_60A,
            Probe_Range_text = "CURRENT_CLAMP_60A_V2_60A", MinScale = -60, MaxScale = 60, Unit_text = "A"},
        // X10_ACTIVE_PROBE
        new PicoProbeScaling(){ ProbeEnum = PicoConnectProbes.PicoConnectProbes.PicoConnectProbeRange.PICO_X10_ACTIVE_PROBE_100MV,
            Probe_Range_text = "X10_ACTIVE_PROBE_100MV", MinScale = -0.1, MaxScale = 0.1, Unit_text = "V"},
        new PicoProbeScaling(){ ProbeEnum = PicoConnectProbes.PicoConnectProbes.PicoConnectProbeRange.PICO_X10_ACTIVE_PROBE_200MV,
            Probe_Range_text = "X10_ACTIVE_PROBE_200MV", MinScale = -0.2, MaxScale = 0.2, Unit_text = "V"},
        new PicoProbeScaling(){ ProbeEnum = PicoConnectProbes.PicoConnectProbes.PicoConnectProbeRange.PICO_X10_ACTIVE_PROBE_500MV,
            Probe_Range_text = "X10_ACTIVE_PROBE_500MV", MinScale = -0.5, MaxScale = 0.5, Unit_text = "V"},
        new PicoProbeScaling(){ ProbeEnum = PicoConnectProbes.PicoConnectProbes.PicoConnectProbeRange.PICO_X10_ACTIVE_PROBE_1V,
            Probe_Range_text = "X10_ACTIVE_PROBE_1V", MinScale = -1, MaxScale = 1, Unit_text = "V"},
        new PicoProbeScaling(){ ProbeEnum = PicoConnectProbes.PicoConnectProbes.PicoConnectProbeRange.PICO_X10_ACTIVE_PROBE_2V,
            Probe_Range_text = "X10_ACTIVE_PROBE_2V", MinScale = -2, MaxScale = 2, Unit_text = "V"},
        new PicoProbeScaling(){ ProbeEnum = PicoConnectProbes.PicoConnectProbes.PicoConnectProbeRange.PICO_X10_ACTIVE_PROBE_5V,
            Probe_Range_text = "X10_ACTIVE_PROBE_5V", MinScale = -5, MaxScale = 5, Unit_text = "V"},
        // Probe Off
        new PicoProbeScaling(){ ProbeEnum = PicoConnectProbes.PicoConnectProbes.PicoConnectProbeRange.PICO_CONNECT_PROBE_OFF, Probe_Range_text = "Probe Off",  MinScale = 0, MaxScale = 0, Unit_text =  "NA"}
        });

        // Probe and Scaling functions //

        /****************************************************************************
        * getRangeScaling
        *
        * Gets the ChannelRangeInfo (Scaling, Units etc) for a given input "ChannelRange" (enum)
        * Returns false if not found, with default scale to use.
        ****************************************************************************/
        public static bool getRangeScaling(PicoConnectProbes.PicoConnectProbes.PicoConnectProbeRange ChannelRange, out PicoProbeScaling ChannelRangeInfo)
        {          
            uint max_index = ( (uint)ProbeScaling.Scaling.ProbeArray.Count ) - 1;
            //Create Unknown_Range and set default to return if not found
            PicoProbeScaling Unknown_UnitLess = new PicoProbeScaling()
            { ProbeEnum = PicoConnectProbes.PicoConnectProbes.PicoConnectProbeRange.PICO_X1_PROBE_1V,
              Probe_Range_text = "Unknown_Range_Normailising_to_+/-1", MinScale = -1, MaxScale = 1, Unit_text = "UnitLess" };
            ChannelRangeInfo = Unknown_UnitLess; //ProbeArray[6];//1V x1 range

            // search
            //return probeScaling.Find(x => x.ProbeEnum == probe);
            // returns -1 if not found, else the found index 
            long pos = -1;
            for (uint i = 0; i < max_index; i++)
            {
                if (ProbeScaling.Scaling.ProbeArray[(int)i].ProbeEnum == ChannelRange)
                {
                    pos = i;
                    break;
                }
            }
            if (pos == -1)//range not found
            {
                return false;       
            }
            else
            {
                ChannelRangeInfo = ProbeArray[(int)pos]; //return pointer to ProbeArray
                return true;
            }
        }

        /****************************************************************************
        * scaled_value_to_adc
        *
        * Convert Scaled value (voltage or Probe units) to ADC value 
        * Inputs:
        * - double - "scaled" value
        * - ChannelRangeInfo (Used to scale the raw data)
        * - Scopes "maxADCValue" used
        ****************************************************************************/
        public static short scaled_value_to_adc(double scaled, PicoProbeScaling ChannelRangeInfo, short maxADCValue)
        {
            return (short) ((scaled / (double)(ChannelRangeInfo.MaxScale)) * maxADCValue);
        }

        /****************************************************************************
        * adc_to_scaled_value
        *
        * Convert an 16-bit ADC count into Scaled data - voltage or Probe units
        * Inputs:
        * - int - raw ADC value
        * - ChannelRangeInfo (Used to scale the raw data)
        * - Scopes "maxADCValue" used
        ****************************************************************************/
        public static double adc_to_scaled_value(short raw, PicoProbeScaling ChannelRangeInfo, short maxADCValue)
        {
                return ((raw * (ChannelRangeInfo.MaxScale)) / (double)maxADCValue);
        }

        /****************************************************************************
        * adc_to_scaled_values
        *
        * Convert an Array of 16-bit ADC count into Scaled data Array - voltage or Probe units
        * Inputs:
        * - int - raw ADC data Array
        * - ChannelRangeInfo (Used to scale the raw data)
        * - Scopes "maxADCValue" used
        ****************************************************************************/
        public static void adc_to_scaled_values(short[] raw, PicoProbeScaling ChannelRangeInfo, short maxADCValue, out double[] adc_scaled_values)
        {
            adc_scaled_values = new double[raw.Length];
            foreach (short i in raw)
                adc_scaled_values[i] = ((raw[i] * (ChannelRangeInfo.MaxScale)) / (double)maxADCValue);
            //returns - "adc_scaled_values" - array
        }

        /****************************************************************************
        * adc_to_mv
        *
        * Convert an 16-bit ADC count into Scaled data - voltage or Probe units
        * Inputs:
        * - int - raw ADC value
        * - ChannelRange
        * - Scopes "maxADCValue" used
        ****************************************************************************/
        public static double adc_to_mv(short raw, uint ChannelRange, short maxADCValue)
        {
            return ((raw * (inputRanges[ChannelRange])) / (double)maxADCValue);
        }

        /****************************************************************************
        * mv_to_adc
        *
        * Convert Scaled value (voltage or Probe units) to ADC value 
        * Inputs:
        * - double - "scaled" value
        * - ChannelRangeInfo (Used to scale the raw data)
        * - Scopes "maxADCValue" used
        ****************************************************************************/
        public static short mv_to_adc(double scaled, uint ChannelRange, short maxADCValue)
        {
            return (short)( (scaled / (double)(inputRanges[ChannelRange]) ) * maxADCValue);
        }
    }
}
