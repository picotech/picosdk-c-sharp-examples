/**************************************************************************
 *
 * Filename: PS3000ACSBlockConsole.cs
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
 *   Collect a block of samples immediately
 *   
 * Copyright © 2018 Pico Technology Ltd. See LICENSE file for terms.
 *  
 **************************************************************************/
using PS3000Imports;
using System;

namespace PS3000CSBlockExample
{
    class PS3000BlockExample
    {
        private const int DualScope = 2;
        private const int QuadScope = 4;
        private const int NumberOfSamples = 10;
        private const short OverSample = 1;

        private static short _timeBase = 0;
        private static int _maxSamples = 0;
        private static int _timeInterval = 0;
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
            Console.WriteLine("PicoScope 3000 Series (ps3000) Driver C# Block Example Program\n");

            if (SetUpDevice())
            {
                CollectData();
                DisplayData();
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
                ps3000.SetTrigger(PS3000.Channel.None, 0, PS3000.TriggerThresholdDirection.Falling, 0, 0);
                ps3000.SetEts(PS3000.EtsMode.Off, 0, 0);
                GetTimeBase();
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
        /// Determine the timebase
        /// </summary>
        private static void GetTimeBase()
        {
            PS3000.TimeUnits timeUnits;

            while (!ps3000.GetTimebase(_timeBase,
                                       NumberOfSamples,
                                       out _timeInterval,
                                       out timeUnits,
                                       OverSample,
                                       out _maxSamples))
            {
                _timeBase++;
            }
        }

        /// <summary>
        /// Collect data from device
        /// </summary>
        private static void CollectData()
        {
            Console.WriteLine("Collecting Device Data...\n");

            int timeIndisposed;

            Console.WriteLine("Starting Block Capture");

            ps3000.RunBlock(NumberOfSamples, _timeBase, OverSample, out timeIndisposed);

            while (!ps3000.Ready())
            {
                // Wait
            }

            ps3000.Stop();

            Console.WriteLine("Finished Block Capture\n");

            _bufferA = new short[_maxSamples];
            _bufferB = new short[_maxSamples];
            _bufferC = new short[_maxSamples];
            _bufferD = new short[_maxSamples];

            _samples = ps3000.GetValues(_bufferA,
                                        _bufferB,
                                        _bufferC,
                                        _bufferD,
                                        out short overflows,
                                        NumberOfSamples);

            ps3000.Close();
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

            Console.WriteLine($"\nBlock Mode Statistics");
            Console.WriteLine($"---------------------");

            Console.WriteLine($"Time Base:\t\t { _timeBase }");
            Console.WriteLine($"Time Interval (ns):\t { _timeInterval }");
            Console.WriteLine($"Requested Samples:\t { NumberOfSamples }");
            Console.WriteLine($"Received Samples:\t { _samples }");
            Console.WriteLine($"Available Samples:\t { _maxSamples }");
        }
    }
}
