/**************************************************************************
*
* Filename:    PicoFileFunctions.cs
*
* Description:
*
* This file includes functions help write data arrays and aquisition details to files.
* 
* Copyright (C) 2024 Pico Technology Ltd. See LICENSE file for terms.
*
*************************************************************************/
using System;
using System.IO;
using DriverImports;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using ProbeScaling;
using System.Globalization;

public static class PicoFileFunctions
    {
    #region PicoFileFunctions constants

    #endregion

    #region PicoFileFunctions enums

    #endregion

    #region Functions
    /*
      * Functions X
      * 
      */
    /// <summary>
    /// Writes 2D array to Output.txt file
    /// </summary>
    internal static void WriteArrayToFile(short[][] ArrayData,
                                        ps6000aDevice.ChannelSettings[] _channelSettings,
                                        double actualTimeInterval = 1,
                                        string startOfFileName = "Output",
                                        short Triggersample = 0,
                                        short maxADCValue = 0)
    {
        string filename;
        short numChannels = (short)ArrayData.Length;//Equal to Number of Channels
        //short memorysegments = (short)ArrayData.GetLength(0);
        ulong numSamples = (ulong)ArrayData[0].Length;

            filename = startOfFileName + ".txt";//next file name
            try
            {
                using (var writer = new StreamWriter(filename))
                {
                    //Write 2 header lines (one for Info, one for Channels)
                    writer.WriteLine("SampleRate " + actualTimeInterval + " SamplesPerBlock " + numSamples + " Trigger@Sample " + Triggersample);
                    short channel;
                    for (channel = 0; channel < numChannels; channel++)
                    {
                        if (_channelSettings[channel].enabled)
                            writer.Write((Channel)channel + "-ADC mV ");
                }
                    writer.Write("\n");
                    //Write array data to file
                    for (ulong sample = 0; sample < numSamples; sample++)
                    {
                        for (channel = 0; channel < numChannels; channel++)
                        {
                            if (_channelSettings[channel].enabled)
                            {
                                writer.Write(ArrayData[channel][sample] + " ");

                                //Write scaled data here
                                double value = ProbeScaling.Scaling.adc_to_mv(ArrayData[channel][sample],
                                    (uint)_channelSettings[channel].range,
                                    maxADCValue);
                                value.ToString("###.0##e+00");
                                writer.Write(String.Format("{0:###.0##e+00} ", value));
                        }
                        }
                        writer.Write("\n");
                    }
                    writer.Close();
                }
                //Console.WriteLine("The captured data has been written to " + filename + ".");
            }
            catch (Exception e)
            {
                Console.WriteLine("Failed to write data to " + filename + ". Please check that there are no other instances of " + filename + " being used.");
                Console.WriteLine(e);
            }
    }

    /// <summary>
    /// Writes 3D array to OutputN.txt files
    /// </summary>
    internal static void WriteArrayToFiles(short[][][] ArrayData,
                                            ps6000aDevice.ChannelSettings[] _channelSettings,
                                            double actualTimeInterval = 1,
                                            string startOfFileName = "Output",
                                            short Triggersample = 0,
                                            short maxADCValue = 0)
        {
            string filename;
            short numChannels = (short)ArrayData[0].Length;//Equal to Number of Channels
            short memorysegments = (short)ArrayData.GetLength(0);
            ulong numSamples = (ulong)ArrayData[0][0].Length;

            for (short segments = 0; segments < memorysegments; segments++)
            {
                numSamples = (ulong)ArrayData[segments][0].Length;

                filename = startOfFileName + segments + ".txt";//next file name
                try
                {
                    using (var writer = new StreamWriter(filename))
                    {
                        //Write 2 header lines (one for Info, one for Channels)
                        writer.WriteLine("Segment " + segments + " SampleRate " + actualTimeInterval + " SamplesPerBlock " + numSamples + " Trigger@Sample " + Triggersample);
                        short channel;
                        for (channel = 0; channel < numChannels; channel++)
                        {
                            if (_channelSettings[channel].enabled)
                                writer.Write((Channel)channel + "-ADC mV ");
                        }
                        writer.Write("\n");
                        //Write array data to file
                        for (ulong sample = 0; sample < numSamples; sample++)
                        {
                            for (channel = 0; channel < numChannels; channel++)
                            {
                                if (_channelSettings[channel].enabled)
                                {
                                    writer.Write(ArrayData[segments][channel][sample] + " ");

                                    double value = ProbeScaling.Scaling.adc_to_mv(ArrayData[segments][channel][sample],
                                        (uint)_channelSettings[channel].range,
                                        maxADCValue);
                                    value.ToString("###.0##e+00");
                                    writer.Write(String.Format("{0:###.0##e+00} ", value));

                            }
                        }
                            writer.Write("\n");
                        }
                        writer.Close();
                    }
                    //Console.WriteLine("The captured data has been written to " + filename + ".");
                }
                catch (Exception e)
                {
                    Console.WriteLine("Failed to write data to " + filename + ". Please check that there are no other instances of " + filename + " being used.");
                    Console.WriteLine(e);
                }
            }
        }

    internal static void WriteDataToFile(short[] data, string filename = "Output.csv")
    {
        try
        {
            //Record data in a .csv document.
            using (var writer = new StreamWriter(filename))
            {
                foreach (var reading in data)
                {
                    writer.WriteLine(reading);
                }

                writer.Close();
            }

            Console.WriteLine("The captured data has been written to " + filename + ".");
        }
        catch (Exception e)
        {
            Console.WriteLine("Failed to write data to " + filename + ". Please check that there are no other instances of " + filename + " being used.");
            Console.WriteLine(e);
        }
    }

    /// <summary>
    /// Writes digital data to Output.csv
    /// </summary>
    internal static void WriteDigitalDataToFile(short[] data)
    {
        var DigitalData = new List<string>();

        //Order the data into a dictionary
        for (int index = 0; index < data.Length; index++)
        {
            string samples = string.Empty;
            for (int digitalChannel = (int)DigitalChannel.DIGITAL_CHANNEL0; digitalChannel < (int)DigitalChannel.DIGITAL_CHANNEL_COUNT; digitalChannel++)
            {
                samples += ((data[index] >> digitalChannel) & 0x0001) + ",";
            }
            DigitalData.Add(samples);
        }

        try
        {
            //Record data in a .csv document.
            using (var writer = new StreamWriter("Output.csv"))
            {
                foreach (var reading in DigitalData)
                {
                    writer.WriteLine(reading);
                }

                writer.Close();
            }

            Console.WriteLine("The captured data has been written to Output.csv.");
        }
        catch (Exception e)
        {
            Console.WriteLine("Failed to write data to Output.csv. Please check that there are no other instances of Output.csv being used.");
            Console.WriteLine(e);
        }
    }

    #endregion
    }

