/******************************************************************************
 *
 * Filename: PicoHRDLConsole.cs
 *
 * Description:
 *     This is a console-mode program that demonstrates how to use the
 *     PicoLog High Resolution Data Logger (picohrdl) driver functions 
 *     using .NET to collect data on Channel 1 of an ADC-20/24 device.
 *      
 * Supported PicoLog models:
 *  
 *     ADC-20
 *     ADC-24
 *      
 * Copyright (C) 2015 - 2017 Pico Technology Ltd. See LICENSE file for terms.
 *      
 ******************************************************************************/

using System;
using System.Threading;

using PicoHRDLImports;

namespace PicoHRDL
{
    class PicoHRDLCSConsole
    {
        private readonly short _handle;
        

        public PicoHRDLCSConsole(short handle)
        {
        
            _handle = handle;
        }

        static void Main(string[] args)
        {

            Console.WriteLine("PicoLog High Resolution Data Logger (picohrdl) Driver C# Example Program");

            short handle = Imports.HRDLOpenUnit();

            if (handle == 0)
            {
                Console.WriteLine("No devices found. Exiting application...\n");
                Thread.Sleep(2000);
                System.Environment.Exit(1);
            }

            Console.WriteLine("\nUnit opened - handle {0}", handle);

            PicoHRDLCSConsole picoHRDLConsole = new PicoHRDLCSConsole(handle);

            picoHRDLConsole.GetDeviceInfo(handle);

            // Set Mains Rejection

            short setMainsStatus = Imports.SetMains(handle, Imports.HRDLMainsRejection.HRDL_FIFTY_HERTZ);   //Set noise rejection for 50Hz  
            
            // Set Input channel

            short analogChannelStatus = Imports.SetAnalogInChannel(handle, (short) Imports.HRDLInputs.HRDL_ANALOG_IN_CHANNEL_1, 1, 
                                            (short) Imports.HRDLRange.HRDL_2500_MV, (short) 1); //set channel 1, enabled, range = 2500mV, single ended
            
            // Set Interval
            short returnIntervalStatus = Imports.SetInterval(handle, 80, (short) Imports.HRDLConversionTime.HRDL_60MS); //sample interval time= 80ms, conversion time = 60ms

            // Specify number of values to collect and capture block of data

            int values = 10;
            
            Console.WriteLine();
            Console.WriteLine("Collecting block of data on Channel 1:");

            short status = Imports.HRDLRun(handle, values, (short) Imports.BlockMethod.HRDL_BM_BLOCK);

            short ready = Imports.HRDLReady(handle);

            while (ready != 1)
            {
                ready = Imports.HRDLReady(handle);
                Thread.Sleep(100);
            }

            // Set up data array to retrieve values
            int[] data = new int[100];
            short[] overflow = new short[1];

            int numValues = Imports.GetValues(handle, data, overflow, values);

            int minAdc = 0;
            int maxAdc = 0;

            // Obtain Max Min ADC Count values for Channel 1
            short returnAdcMaxMin = Imports.GetMinMaxAdcCounts(handle, out minAdc, out maxAdc, (short) Imports.HRDLInputs.HRDL_ANALOG_IN_CHANNEL_1);

            Console.WriteLine();
            Console.WriteLine("Max/Min ADC Values for Channel 1:");
            Console.WriteLine("Max ADC: {0}", maxAdc);
            Console.WriteLine("Min ADC: {0}", minAdc);

            Console.WriteLine();
            Console.WriteLine("Collected {0} values:\n", numValues);

            float[] scaledData = new float[numValues];

            for (int n = 0; n < numValues; n++)
            {
                scaledData[n] = picoHRDLConsole.adcToMv(data[n], (short) Imports.HRDLRange.HRDL_2500_MV, maxAdc);
                Console.WriteLine("Raw: {0} \tScaled: {1}", data[n], scaledData[n]);
            }

            Console.WriteLine();
            status = Imports.HRDLCloseUnit(handle);

            char ch;

            do
            {
                Console.WriteLine("Press X to exit.");
                ch = char.ToUpper(Console.ReadKey(true).KeyChar);
            }

            while (ch != 'X');

            System.Environment.Exit(0);

        }

        /**
         * GetDeviceInfo 
         * 
         * Prints information about the device to the console window.
         * 
         * Inputs:
         *      handle - the handle to the device
         */
        public void GetDeviceInfo(short handle)
        {
            string[] description = {
                           "Driver Version    ",
                           "USB Version       ",
                           "Hardware Version  ",
                           "Variant Info      ",
                           "Serial            ",
                           "Cal Date          ",
                           "Kernel Ver        "
                         };
            
            System.Text.StringBuilder line = new System.Text.StringBuilder(80);

            if (handle >= 0)
            {
                for (short i = 0; i < 6; i++)
                {
                    
                    Imports.GetUnitInfo(handle, line, 80, i);
                    
                    Console.WriteLine("{0}: {1}", description[i], line);
                }
            }
          
        }

        /**
         * adcToMv 
         * 
         * Converts ADC counts to millivolt values
         * 
         * Inputs:
         *      value - the raw ADC count
         *      range - the voltage range set for the channel
         *      maxValue - the maximum ADC count value
         *      
         * Outputs:
         *      mvValue - the value in millivolts
         * 
         */
        public float adcToMv(int value, short range, int maxValue) 
        {
            float mvValue = 0.0f;

            float vMax = (float) (Imports.MAX_VOLTAGE_RANGE / Math.Pow(2, range)); // Find voltage scaling factor

            mvValue = ((float) value / maxValue) * vMax;

            return mvValue;
        }

    }
}
