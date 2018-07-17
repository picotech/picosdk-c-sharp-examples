/**************************************************************************
 *
 * Filename: PS3000ACSStreamingExample.cs
 *
 * Description:
 *  This is a console-mode program that demonstrates how to use the
 *  PicoScope 3000 Series (ps3000) driver functions. using .NET
 *
 * Supported PicoScope models:
 *
 *	PicoScope 3204
 *	PicoScope 3205
 *	PicoScope 3206
 *	PicoScope 3224
 *	PicoScope 3424
 *	PicoScope 3425
 *		
 * Examples:
 *   Collect a stream of data
 *   
 * Copyright © 2018 Pico Technology Ltd. See LICENSE file for terms.
 *  
 **************************************************************************/
using System;
using System.Threading;
using PS3000Imports;

namespace PS3000CSStreamingExample
{
    class PS3000CSStreamingExample
    {
        private const int DualScope = 2;
        private const int QuadScope = 4;
        private const int MaxSamples = 1024;
        private const int NumberOfSamples = 1000;
        private const short OverSample = 1;

        private static int _channelCount = 0;
        private static int _samples = 0;

        private static short[] _bufferA;
        private static short[] _bufferB;
        private static short[] _bufferC;
        private static short[] _bufferD;

        private static PS3000 ps3000 = new PS3000();


        /// <summary>
        /// Application entry point
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            Console.WriteLine("PicoScope 3000 Series (ps3000) Driver C# Streaming Example Program\n");

            if (SetUpDevice())
            {
                CollectData();
                DisplayData();
                CloseDevice();
            }

            Console.ReadKey();
        }

        /// <summary>
        /// Intialise and setup device
        /// </summary>
        private static bool SetUpDevice()
        {
            bool isDeviceOpened = OpenDevice();

            if (isDeviceOpened)
            {               
                SetChannels();
                ps3000.SetTrigger(PS3000.Channel.None, 0, 0, 0, 0);
                ps3000.SetEts(PS3000.EtsMode.Off, 0, 0);
            }

            return isDeviceOpened;
        }

        /// <summary>
        /// Open the device
        /// </summary>
        /// <returns></returns>
        private static bool OpenDevice()
        {
            bool isDeviceOpened = false;

            Console.WriteLine("Opening the device...\n");

            isDeviceOpened = ps3000.Open();

            if (isDeviceOpened)
            {
                Console.WriteLine("Device opened successfully\n");

                // Display device info
                Console.WriteLine($"Driver Version:   { ps3000.DriverVersion }");
                Console.WriteLine($"USB Version:      { ps3000.UsbVersion }");
                Console.WriteLine($"Hardware Version: { ps3000.HardwareVersion }");
                Console.WriteLine($"Variant Info:     { ps3000.VariantInfo }");
                Console.WriteLine($"Serial:           { ps3000.Serial }");
                Console.WriteLine($"Cal Date:         { ps3000.KernelDriverVersion }");
                Console.WriteLine($"Hardware Version: { ps3000.HardwareVersion} \n");
            }
            else
            {
                Console.WriteLine("Unable to open device\n");
            }

            return isDeviceOpened;
        }

        /// <summary>
        /// Initialise device input channels
        /// </summary>
        private static void SetChannels()
        {
            _channelCount = int.Parse(ps3000.VariantInfo[1].ToString());

            if (_channelCount == DualScope)
            {
                ps3000.SetChannel(PS3000.Channel.ChannelA, 1, PS3000.Coupling.DC, PS3000.Range.Range2V);
                ps3000.SetChannel(PS3000.Channel.ChannelB, 0, PS3000.Coupling.DC, PS3000.Range.Range20V);                
            }
            else if (_channelCount == QuadScope)
            {
                ps3000.SetChannel(PS3000.Channel.ChannelA, 1, PS3000.Coupling.DC, PS3000.Range.Range2V);
                ps3000.SetChannel(PS3000.Channel.ChannelB, 0, PS3000.Coupling.DC, PS3000.Range.Range20V);
                ps3000.SetChannel(PS3000.Channel.ChannelC, 0, PS3000.Coupling.DC, PS3000.Range.Range20V);
                ps3000.SetChannel(PS3000.Channel.ChannelD, 0, PS3000.Coupling.DC, PS3000.Range.Range20V);
            }
            else
            {
                Console.WriteLine("Error: Invalid Number of Channels\n");
            }
        }        

        /// <summary>
        /// Collect data from device
        /// </summary>
        private static void CollectData()
        {
            Console.WriteLine("Collecting Device Data...\n");

            Console.WriteLine("Starting Streaming Capture");

            ps3000.RunStreaming(10, NumberOfSamples, 0);            

            Thread.Sleep(1000);

            Console.WriteLine("Finished Streaming Capture\n");

            _bufferA = new short[MaxSamples];
            _bufferB = new short[MaxSamples];
            _bufferC = new short[MaxSamples];
            _bufferD = new short[MaxSamples];

            _samples = ps3000.GetValues(_bufferA,
                                        _bufferB,
                                        _bufferC,
                                        _bufferD,
                                        out short overflows,
                                        MaxSamples);            
        }

        /// <summary>
        /// Display collected data
        /// </summary>
        private static void DisplayData()
        {
            if (_channelCount == DualScope)
            {
                Console.WriteLine($"ChA\tChB");
                Console.WriteLine($"---\t---");


                for (var i = 0; i < _samples; i++)
                {
                    Console.WriteLine($"{ _bufferA[i] }\t{ _bufferB[i] }");
                }

            }
            else if (_channelCount == QuadScope)
            {
                Console.WriteLine($"ChA\tChB\tChC\tChD");

                for (var i = 0; i < _samples; i++)
                {
                    Console.WriteLine($"{ _bufferA[i] }\t{ _bufferB[i] }\t{ _bufferC[i] }\t{ _bufferD[i] }");
                }
            }

            Console.WriteLine($"\nStreaming Mode Statistics");
            Console.WriteLine($"---------------------");

            Console.WriteLine($"Requested Samples:\t { NumberOfSamples }");
            Console.WriteLine($"Received Samples:\t { _samples }");
            Console.WriteLine($"Available Samples:\t { MaxSamples }");
        }

        /// <summary>
        /// Stop and Close the device
        /// </summary>
        private static void CloseDevice()
        {
            ps3000.Stop();
            ps3000.Close();
        }
    }
}



