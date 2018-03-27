/**************************************************************************
 *
 * Filename: USBTC08CSConsole.cs
 * 
 * Description:
 *   This is a console-mode program that demonstrates how to use the
 *   USBTC08 driver using .NET
 *   
 * Copyright (C) 2011-2018 Pico Technology Ltd. See LICENSE file for terms. 
 *   
 **************************************************************************/

using System;
using System.Threading;

using USBTC08Imports;
using PicoPinnedArray;

namespace USBTC08ConsoleExample
{
    class ConsoleExample
    {
        private readonly short _handle;
        public const int USBTC08_MAX_CHANNELS = 8;
        public const char TC_TYPE_K = 'K';
        public const int PICO_OK = 1;
       

    private static void WaitForKey()
    {
        while (!Console.KeyAvailable)
        {
            Thread.Sleep(100);
        }

        if (Console.KeyAvailable)
        {
            Console.ReadKey(true); // clear the key
        }
    }

    /****************************************************************************
     * Read the device information
     ****************************************************************************/
    void GetDeviceInfo()
    {
        System.Text.StringBuilder line = new System.Text.StringBuilder(256);

        if (_handle >= 0)
        {
            Console.WriteLine("USB TC-08 Device Information:\n");
            Imports.TC08GetFormattedInfo(_handle, line, 256);
            Console.WriteLine("{0}\n", line);
        }
    }

    /****************************************************************************
     * Read temperature information from the unit
     ****************************************************************************/
    unsafe void GetValues()
    {
        short status;
        short chan;
        float[] tempbuffer = new float[9]; 
        short overflow;
        int lines=0;

        Console.Write("\n");

        Console.WriteLine("Temperatures are in °C\n");
        Console.WriteLine("Chan0 is the Cold Junction Temperature\n");

        // Label the columns
        for (chan = 0; chan <= USBTC08_MAX_CHANNELS; chan++)
        {
            Console.Write("Chan{0}:    ", chan);
        }
        Console.Write("\n");

        do
        { 
            status = Imports.TC08GetSingle(_handle, tempbuffer, &overflow, Imports.TempUnit.USBTC08_UNITS_CENTIGRADE);
            
            if (status == PICO_OK)
            {
                for (chan = 0; chan <= USBTC08_MAX_CHANNELS; chan++)
                {
                    Console.Write("{0:0.0000}   ", tempbuffer[chan]);
                }

                Console.Write("\n");
                Thread.Sleep(1000);
            }

            if (++lines > 9)
            {
                Console.WriteLine("Temperatures are in °C\n");
                Console.WriteLine("Chan0 is the Cold Junction Temperature\n");
                Console.WriteLine("Press any key to stop....\n");

                lines = 0;

                for (chan = 0; chan <= USBTC08_MAX_CHANNELS; chan++)
                {
                    Console.Write("Chan{0}:    ", chan);
                }

                Console.Write("\n");
            }
        } while (!Console.KeyAvailable);

        char ch = (Console.ReadKey().KeyChar);       // use up keypress
        status = Imports.TC08Stop(_handle);

        Console.WriteLine();
    }

    /****************************************************************************
    * Read temperature information from the unit using streaming
    ****************************************************************************/
    unsafe void GetStreamingValues()
    {
        short status;
        short chan;
        int interval_ms;
        int buffer_size = 1024;

        float[][] tempbuffer = new float[9][];

        PinnedArray<float>[] pinned = new PinnedArray<float>[buffer_size];

        for (short channel = 0; channel <= USBTC08_MAX_CHANNELS; channel++)
        {
            tempbuffer[channel] = new float[buffer_size];
            pinned[channel] = new PinnedArray<float>(tempbuffer[channel]);
        }

        int[] times_ms_buffer = new int[buffer_size];
        short[] overflow = new short[9];
        int lines = 0;
        int numberOfSamples = 0;

        // Find the time interval
        interval_ms = Imports.TC08GetMinIntervalMS(_handle);

        Console.Write("\n");

        int actual_interval_ms = Imports.TC08Run(_handle, interval_ms);

        do
        {
            Thread.Sleep(1000);

            if (actual_interval_ms > 0)
            {
                // Obtain readings for each channel
                for (chan = 0; chan <= USBTC08_MAX_CHANNELS; chan++)
                {
                    numberOfSamples = Imports.TC08GetTemp(_handle, tempbuffer[chan], times_ms_buffer, buffer_size,
                        out overflow[chan], chan, Imports.TempUnit.USBTC08_UNITS_CENTIGRADE, 0);

                    if (numberOfSamples == 1)
                    {
                        Console.WriteLine("Channel {0}: {1} reading.\n", chan, numberOfSamples);
                    }
                    else
                    {
                        Console.WriteLine("Channel {0}: {1} readings.\n", chan, numberOfSamples);
                    }

                    lines++;
                }

                Console.WriteLine("Temperatures are in °C\n");
                Console.Write("Chan0 is the Cold Junction Temperature\n\n");

                // Label the columns
                for (chan = 0; chan <= USBTC08_MAX_CHANNELS; chan++)
                {
                    Console.Write("Chan{0}:    ", chan);
                }

                Console.Write("\n");

                // Print readings
                for (int i = 0; i < numberOfSamples; i++)
                {

                    for (int channel = 0; channel <= USBTC08_MAX_CHANNELS; channel++)
                    {
                        Console.Write("{0:0.0000}\t", pinned[channel].Target[i]);
                    }

                    Console.WriteLine("");

                }

                Console.Write("\n");
                Thread.Sleep(5000);
            }

            if (++lines > 9)
            {
                Console.WriteLine("Press any key to stop....\n");

                lines = 0;
            }

        } while (!Console.KeyAvailable);

        char ch = (Console.ReadKey().KeyChar);       // use up keypress
        status = Imports.TC08Stop(_handle);

        // Un-pin the arrays
        foreach (PinnedArray<float> p in pinned)
        {
            if (p != null)
            {
                p.Dispose();
            }

        }
    }

    /****************************************************************************
    *  Set channels 
    ****************************************************************************/
    void SetChannels()
    {
	    short channel;
	    short ok;

	    for (channel = 0; channel <= USBTC08_MAX_CHANNELS; channel++)
	    {
            ok = Imports.TC08SetChannel(_handle, channel, TC_TYPE_K);

	    }
    }

    /****************************************************************************
    *  Run
    ****************************************************************************/
    public void Run()
    {
            short status = 0;
            short errorCode = 0;

            Console.WriteLine("Set mains rejection frequency? (Y/N)");

            char input = char.ToUpper(Console.ReadKey().KeyChar);
            Console.WriteLine();

            if (input.Equals('Y'))
            {
                Console.WriteLine("Select mains frequency to reject:");
                Console.WriteLine("0 - 50 Hz");
                Console.WriteLine("1 - 60 Hz");

                short mainsRejectionFrequency = 0;
                string mainsRejectionInput = "";
                bool validInput = false;

                do
                {
                    mainsRejectionInput =  Console.ReadLine();

                    validInput = Int16.TryParse(mainsRejectionInput, out mainsRejectionFrequency);

                    if (validInput == true)
                    {
                        if (mainsRejectionFrequency != (short)Imports.MainsFrequency.USBTC08_MAINS_FIFTY_HERTZ &&
                        mainsRejectionFrequency != (short)Imports.MainsFrequency.USBTC08_MAINS_SIXTY_HERTZ)
                        {
                            validInput = false;
                        }
                    }
                }
                while (validInput == false);

                status = Imports.TC08SetMains(_handle, (Imports.MainsFrequency)mainsRejectionFrequency);

                if (status == 1)
                {
                    Console.WriteLine("Mains rejection set successfully.");
                }
                else
                {
                    errorCode = Imports.TC08GetLastError(_handle);

                    Console.WriteLine("Error calling TCO8SetMains: {0}", errorCode);
                    Imports.TC08CloseUnit(_handle);
                    WaitForKey();
                    Environment.Exit(-1);
                }
            }
            else
            {
                Console.WriteLine("Mains rejection not set.");
            }

            Console.WriteLine();

            //// main loop - read key and call routine
            char ch = ' ';

        while (ch != 'X')
        {
            Console.WriteLine("Please select an operation:\n");
            Console.WriteLine("I - View Device Info");
            Console.WriteLine("G - Get Temperatures");
            Console.WriteLine("S - Get Temperatures - Streaming");
            Console.WriteLine("X - Exit\n");
            Console.WriteLine("Operation:");

            ch = char.ToUpper(Console.ReadKey().KeyChar);

            Console.WriteLine("\n");
            switch (ch)
            {
                case 'I':
                    GetDeviceInfo();
                    break;

                case 'G':
                    SetChannels();
                    GetValues();
                    break;

                case 'S':
                    SetChannels();
                    GetStreamingValues();
                    break;

                case 'X':
                    /* Handled by outer loop */
                    break;

                default:
                    Console.WriteLine("Invalid operation");
                    break;
            }
        }
    }


    private ConsoleExample(short handle)
    {
        _handle = handle;
    }


    static void Main()
    {
      Console.WriteLine("USB TC-08 Driver Example Program");
      Console.WriteLine("Version 1.2\n");

      // Open connection to device
      Console.WriteLine("\nOpening the device...");

      short handle = Imports.TC08OpenUnit();
      Console.WriteLine("Handle: {0}", handle);

      if (handle == 0)
      {
        Console.WriteLine("Unable to open device");
        Console.WriteLine("Error code : {0}", handle);
        WaitForKey();
      }
      else
      {
        Console.WriteLine("Device opened successfully\n");

        ConsoleExample consoleExample = new ConsoleExample(handle);
        consoleExample.Run();

        Imports.TC08CloseUnit(handle);
      }
    }
  }
}  
