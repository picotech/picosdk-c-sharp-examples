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
 * Copyright © 2015-2017 Pico Technology Ltd. See LICENSE file for terms.
 *      
 ******************************************************************************/

using System;
using System.Threading;

using PicoHRDLImports;

namespace PicoHRDL
{
    struct ChannelSettings
    {
        public bool enabled;
        public Imports.HRDLRange range;
        public short singleEnded;
    }

    class PicoHRDLCSConsole
    {
        private readonly short _handle;
        private bool hasDigitalIO = false;
        private short numberOfAnalogChannels = 0;

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

            Console.WriteLine("\nUnit opened - handle {0}:-\n", handle);

            PicoHRDLCSConsole picoHRDLConsole = new PicoHRDLCSConsole(handle);

            picoHRDLConsole.GetDeviceInfo(handle);

            // Set Mains Rejection

            short setMainsStatus = Imports.SetMains(handle, Imports.HRDLMainsRejection.HRDL_FIFTY_HERTZ);   //Set noise rejection for 50 Hz  

            // Array to store channel settings

            ChannelSettings[] picoHRDLChannelSettings = new ChannelSettings[(int) Imports.HRDLInputs.HRDL_MAX_ANALOG_CHANNELS + 1];

            // Set Input channel

            short analogChannelStatus = Imports.SetAnalogInChannel(handle, (short) Imports.HRDLInputs.HRDL_ANALOG_IN_CHANNEL_1, 1, 
                                            (short) Imports.HRDLRange.HRDL_2500_MV, (short) 1); // Set channel 1, enabled, range = 2500 mV, single ended

            picoHRDLChannelSettings[1].enabled = true;
            picoHRDLChannelSettings[1].range = Imports.HRDLRange.HRDL_2500_MV;
            picoHRDLChannelSettings[1].singleEnded = 1;

            // Turn off all other channels

            for (int channel = (int) Imports.HRDLInputs.HRDL_ANALOG_IN_CHANNEL_2; channel <= (int) picoHRDLConsole.numberOfAnalogChannels; channel++ )
            {
                analogChannelStatus = Imports.SetAnalogInChannel(handle, (short) channel, 0,
                                            (short)Imports.HRDLRange.HRDL_2500_MV, (short) 1);
            }

            // Set digital I/O (ADC-24 only)

            if (picoHRDLConsole.hasDigitalIO == true)
            {
                short directionOut = (short) Imports.HRDLDigitalIOChannel.HRDL_DIGITAL_IO_CHANNEL_2 + (short) Imports.HRDLDigitalIOChannel.HRDL_DIGITAL_IO_CHANNEL_3 + (short) Imports.HRDLDigitalIOChannel.HRDL_DIGITAL_IO_CHANNEL_4;
                short digitalPinOutState = 0; // All digital I/O pins that are outputs are set to low
                short enabledDigitalIn = 0;

                // If at least one I/O pin is set as an input
                if (directionOut < 15)
                {
                    enabledDigitalIn = 1;
                }

                picoHRDLChannelSettings[(int)Imports.HRDLInputs.HRDL_DIGITAL_CHANNELS].enabled = true;
                picoHRDLChannelSettings[(int)Imports.HRDLInputs.HRDL_DIGITAL_CHANNELS].range = Imports.HRDLRange.HRDL_2500_MV;
                picoHRDLChannelSettings[(int)Imports.HRDLInputs.HRDL_DIGITAL_CHANNELS].singleEnded = 1;

                short setDigitalIOStatus = Imports.SetDigitalIOChannel(handle, directionOut, digitalPinOutState, enabledDigitalIn);
            }
            else
            {
                picoHRDLChannelSettings[(int)Imports.HRDLInputs.HRDL_DIGITAL_CHANNELS].enabled = false;
                picoHRDLChannelSettings[(int)Imports.HRDLInputs.HRDL_DIGITAL_CHANNELS].range = Imports.HRDLRange.HRDL_2500_MV;
                picoHRDLChannelSettings[(int)Imports.HRDLInputs.HRDL_DIGITAL_CHANNELS].singleEnded = 1;
            }

            // Set Interval
            short returnIntervalStatus = Imports.SetInterval(handle, 80, (short) Imports.HRDLConversionTime.HRDL_60MS); // sample interval time= 80 ms, conversion time = 60ms

            // Specify number of values to collect and capture block of data

            int numberOfValues = 100;
            
            Console.WriteLine();
            Console.WriteLine("Collecting block of data...");

            short status = Imports.HRDLRun(handle, numberOfValues, (short) Imports.BlockMethod.HRDL_BM_BLOCK);

            short ready = Imports.HRDLReady(handle);

            while (ready != 1)
            {
                ready = Imports.HRDLReady(handle);
                Thread.Sleep(100);
            }

            // Find the number of enabled channels (including digital for the ADC-24)
            short numberOfEnabledChannels = 0;
            status = Imports.GetNumberOfEnabledChannels(handle, out numberOfEnabledChannels);

            if (picoHRDLChannelSettings[(int)Imports.HRDLInputs.HRDL_DIGITAL_CHANNELS].enabled == true)
            {
                numberOfEnabledChannels = (short)(numberOfEnabledChannels + 1);
            }

            int numberOfValuesPerChannel = numberOfValues / numberOfEnabledChannels;

            // Set up data array to retrieve values
            int[] times = new int[numberOfValues];
            int[] data = new int[numberOfValues];

            short overflow = 0;

            int numValues = 0;

            numValues = Imports.GetTimesAndValues(handle, times, data, out overflow, numberOfValuesPerChannel);

            // Check to see if an overflow occured during the last data collection
            if (overflow > 0)
            {
                Console.WriteLine("An over voltage occured during the last data run.\n\n");
            }
            
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

            Console.WriteLine("Time shown in each row is for first reading in set.\n");
            Console.Write("Time\t");

            for (int ch = (int) Imports.HRDLInputs.HRDL_DIGITAL_CHANNELS; ch <= (int) Imports.HRDLInputs.HRDL_MAX_ANALOG_CHANNELS; ch++)
            {
                if (picoHRDLChannelSettings[ch].enabled == true && ch == (int) Imports.HRDLInputs.HRDL_DIGITAL_CHANNELS)
                {
                    Console.Write("1234\t");
                }
                else if (picoHRDLChannelSettings[ch].enabled == true)
                {
                    Console.Write("Ch{0}\t", ch);
                }
            }

            Console.WriteLine();

            Console.Write("(ms)\t");

            for (int ch = (int)Imports.HRDLInputs.HRDL_DIGITAL_CHANNELS; ch <= (int)Imports.HRDLInputs.HRDL_MAX_ANALOG_CHANNELS; ch++)
            {
                if (picoHRDLChannelSettings[ch].enabled == true && ch == (int)Imports.HRDLInputs.HRDL_DIGITAL_CHANNELS)
                {
                    Console.Write(" DO \t");
                }
                else if (picoHRDLChannelSettings[ch].enabled == true)
                {
                    Console.Write("(mV)\t");
                }
            }

            Console.WriteLine("\n");
            
            float[] scaledData = new float[numValues];

            //for (int n = 0; n < numValues; n++)
            //{
            //    scaledData[n] = picoHRDLConsole.adcToMv(data[n], (short) Imports.HRDLRange.HRDL_2500_MV, maxAdc);
            //    Console.WriteLine("Raw: {0} \tScaled: {1}", data[n], scaledData[n]);
            //}

            int timeCount = 0;

            // Display the 10 readings for each active channel
            // The time displayed will be for the first reading in each row
            for (int n = 0; n < (numValues / numberOfEnabledChannels); n++)
            {
                Console.Write("{0}\t", times[timeCount * numberOfEnabledChannels]);

                for (int channel = (int) Imports.HRDLInputs.HRDL_DIGITAL_CHANNELS; channel <= (int)Imports.HRDLInputs.HRDL_MAX_ANALOG_CHANNELS; channel++)
                {
                    if (picoHRDLChannelSettings[channel].enabled == true)
                    {
                        if (channel == (int) Imports.HRDLInputs.HRDL_DIGITAL_CHANNELS)
                        {
                            Console.Write("{0}{1}{2}{3}\t", 0x01 & (data[n]), 0x01 & (data[n] >> 0x1), 0x01 & (data[n] >> 0x2), 0x01 & (data[n] >> 0x3));
                            n++;
                        }
                        else
                        {
                            scaledData[n] = picoHRDLConsole.adcToMv(data[n], (short) picoHRDLChannelSettings[channel].range, maxAdc);
                            Console.Write("{0}\t", scaledData[n]);
                        }
                    }
                }
                Console.WriteLine();
                timeCount++;
            }

            Imports.HRDLStop(handle);

            Console.WriteLine();
            status = Imports.HRDLCloseUnit(handle);

            char input;

            do
            {
                Console.WriteLine("Press X to exit.");
                input = char.ToUpper(Console.ReadKey(true).KeyChar);
            }

            while (input != 'X');

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

                    if (i == 3)
                    {
                        short variant = short.Parse(line.ToString());

                        if (variant == 24)
                        {
                            hasDigitalIO = true;
                            numberOfAnalogChannels = (short) Imports.HRDLInputs.HRDL_MAX_ANALOG_CHANNELS;
                        }
                        else if(variant == 20)
                        {
                            hasDigitalIO = false;
                            numberOfAnalogChannels = (short) 8;
                        }
                        else
                        {
                            hasDigitalIO = false;
                            numberOfAnalogChannels = (short) 0;
                        }

                    }
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
