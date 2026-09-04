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

        // The cold junction (channel 0) plus the eight thermocouple inputs.
        public const int NUM_TC08_CHANNELS = USBTC08_MAX_CHANNELS + 1;

        // usb_tc08_get_temp never returns more than this many readings in a
        // single call (USBTC08_MAX_SAMPLE_BUFFER in usbtc08.h), so the buffers
        // and the buffer_length argument are both sized from it.
        public const int USBTC08_MAX_SAMPLE_BUFFER = 600;

        // USBTC08_MAX_INFO_CHARS in usbtc08.h.
        public const int USBTC08_MAX_INFO_CHARS = 256;

        // Thermocouple type for channels 1 to 8.
        public const sbyte TC_TYPE_K = (sbyte)'K';

        // Channel 0 is the cold junction and takes this dedicated type, not a
        // thermocouple type.
        public const sbyte TC_TYPE_CJC = (sbyte)'C';

        // The usbtc08 driver reports success as non-zero, unlike the
        // PICO_STATUS drivers where PICO_OK is 0. Do not confuse the two.
        public const short USBTC08_SUCCESS = 1;


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
        System.Text.StringBuilder line = new System.Text.StringBuilder(USBTC08_MAX_INFO_CHARS);

        // A handle of 0 is not a valid unit, so test for greater than zero.
        if (_handle > 0)
        {
            Console.WriteLine("USB TC-08 Device Information:\n");

            // The length passed to the driver is taken from the buffer itself so
            // the two cannot drift apart and let the driver overrun it.
            if (Imports.TC08GetFormattedInfo(_handle, line, (short)line.Capacity) == USBTC08_SUCCESS)
            {
                Console.WriteLine("{0}\n", line);
            }
            else
            {
                Console.WriteLine("Unable to read the device information: {0}\n",
                    Imports.TC08GetLastError(_handle));
            }
        }
    }

    /****************************************************************************
     * Read temperature information from the unit
     ****************************************************************************/
    unsafe void GetValues()
    {
        short status;
        short chan;
        float[] tempbuffer = new float[NUM_TC08_CHANNELS];
        // usb_tc08_get_single's overflow_flags is a single 16-bit field, not an
        // array of per-channel flags. Bit 0 is channel 1: the cold junction has
        // no bit because it cannot go over range.
        short overflow;
        int lines = 0;

        Console.Write("\n");

        Console.WriteLine("Temperatures are in degrees C\n");
        Console.WriteLine("Chan0 is the Cold Junction Temperature\n");

        // Label the columns
        for (chan = 0; chan < NUM_TC08_CHANNELS; chan++)
        {
            Console.Write("Chan{0}:    ", chan);
        }
        Console.Write("\n");

        do
        {
            status = Imports.TC08GetSingle(_handle, tempbuffer, &overflow, Imports.TempUnit.USBTC08_UNITS_CENTIGRADE);

            // Report the failure and stop rather than spinning on a device that
            // is no longer answering (for example one that has been unplugged).
            if (status != USBTC08_SUCCESS)
            {
                Console.WriteLine("\nError reading temperatures: {0}", Imports.TC08GetLastError(_handle));
                break;
            }

            for (chan = 0; chan < NUM_TC08_CHANNELS; chan++)
            {
                // The overflow field carries one bit per THERMOCOUPLE channel,
                // starting at bit 0 for channel 1. The cold junction cannot be
                // over range and has no bit of its own, so channel N is tested
                // at bit N-1 and chan 0 is never marked.
                bool overRange = chan > 0 && (overflow & (1 << (chan - 1))) != 0;

                Console.Write("{0:0.0000}{1}   ", tempbuffer[chan], overRange ? "!" : " ");
            }

            Console.Write("\n");
            Thread.Sleep(1000);

            if (++lines > 9)
            {
                Console.WriteLine("Temperatures are in degrees C  (! marks an over-range channel)\n");
                Console.WriteLine("Chan0 is the Cold Junction Temperature\n");
                Console.WriteLine("Press any key to stop....\n");

                lines = 0;

                for (chan = 0; chan < NUM_TC08_CHANNELS; chan++)
                {
                    Console.Write("Chan{0}:    ", chan);
                }

                Console.Write("\n");
            }
        } while (!Console.KeyAvailable);

        if (Console.KeyAvailable)
        {
            Console.ReadKey(true);       // use up keypress
        }

        Console.WriteLine();
    }

    /****************************************************************************
    * Read temperature information from the unit using streaming
    ****************************************************************************/
    unsafe void GetStreamingValues()
    {
        int chan;
        int lines = 0;

        float[][] tempbuffer = new float[NUM_TC08_CHANNELS][];
        int[] samplesPerChannel = new int[NUM_TC08_CHANNELS];
        short[] overflow = new short[NUM_TC08_CHANNELS];

        // Each channel buffer is pinned for the whole capture. The usbtc08
        // driver is handed the address of these buffers, so they must not be
        // relocated by the garbage collector while it holds them - the
        // marshaller's own pin only lasts for the duration of a single call.
        // One PinnedArray per channel; the array was previously sized
        // buffer_size (1024) while only the first NUM_TC08_CHANNELS entries
        // were ever populated.
        PinnedArray<float>[] pinned = new PinnedArray<float>[NUM_TC08_CHANNELS];

        for (chan = 0; chan < NUM_TC08_CHANNELS; chan++)
        {
            tempbuffer[chan] = new float[USBTC08_MAX_SAMPLE_BUFFER];
            pinned[chan] = new PinnedArray<float>(tempbuffer[chan]);
        }

        int[] times_ms_buffer = new int[USBTC08_MAX_SAMPLE_BUFFER];
        PinnedArray<int> pinnedTimes = new PinnedArray<int>(times_ms_buffer);

        try
        {

        // Find the time interval
        int interval_ms = Imports.TC08GetMinIntervalMS(_handle);

        if (interval_ms <= 0)
        {
            Console.WriteLine("Unable to read the minimum sampling interval: {0}",
                Imports.TC08GetLastError(_handle));
            return;
        }

        Console.Write("\n");

        // TC08Run returns the interval the driver actually applied, or 0 if
        // streaming could not be started.
        int actual_interval_ms = Imports.TC08Run(_handle, interval_ms);

        if (actual_interval_ms <= 0)
        {
            Console.WriteLine("Unable to start streaming: {0}", Imports.TC08GetLastError(_handle));
            return;
        }

        Console.WriteLine("Sampling interval: {0} ms", actual_interval_ms);

        do
        {
            Thread.Sleep(1000);

            // Obtain readings for each channel
            for (chan = 0; chan < NUM_TC08_CHANNELS; chan++)
            {
                samplesPerChannel[chan] = Imports.TC08GetTemp(_handle, tempbuffer[chan], times_ms_buffer,
                    USBTC08_MAX_SAMPLE_BUFFER, out overflow[chan], (short)chan,
                    Imports.TempUnit.USBTC08_UNITS_CENTIGRADE, 0);

                // A negative count means the call failed.
                if (samplesPerChannel[chan] < 0)
                {
                    Console.WriteLine("\nError while streaming: {0}", Imports.TC08GetLastError(_handle));
                    Imports.TC08Stop(_handle);
                    return;
                }

                Console.WriteLine("Channel {0}: {1} reading{2}.\n", chan, samplesPerChannel[chan],
                    samplesPerChannel[chan] == 1 ? "" : "s");
            }

            Console.WriteLine("Temperatures are in degrees C\n");
            Console.Write("Chan0 is the Cold Junction Temperature\n\n");

            // Label the columns
            for (chan = 0; chan < NUM_TC08_CHANNELS; chan++)
            {
                Console.Write("Chan{0}:    ", chan);
            }

            Console.Write("\n");

            // The driver reports a count per channel and those counts can
            // differ, so only print the rows every channel actually returned.
            int rowsToPrint = USBTC08_MAX_SAMPLE_BUFFER;

            for (chan = 0; chan < NUM_TC08_CHANNELS; chan++)
            {
                if (samplesPerChannel[chan] < rowsToPrint)
                {
                    rowsToPrint = samplesPerChannel[chan];
                }
            }

            // Print readings, read back through the pinned view of each buffer.
            for (int i = 0; i < rowsToPrint; i++)
            {
                for (int channel = 0; channel < NUM_TC08_CHANNELS; channel++)
                {
                    Console.Write("{0:0.0000}\t", pinned[channel].Target[i]);
                }

                Console.WriteLine("");
            }

            Console.Write("\n");
            Thread.Sleep(5000);

            if (++lines > 9)
            {
                Console.WriteLine("Press any key to stop....\n");

                lines = 0;
            }

        } while (!Console.KeyAvailable);

        if (Console.KeyAvailable)
        {
            Console.ReadKey(true);       // use up keypress
        }

        Imports.TC08Stop(_handle);

        }
        finally
        {
            // Release the pins on every exit path, including the early returns
            // above. Previously the Dispose loop was only reached on a normal
            // exit, so a driver error left the buffers pinned for the lifetime
            // of the process.
            foreach (PinnedArray<float> p in pinned)
            {
                if (p != null)
                {
                    p.Dispose();
                }
            }

            if (pinnedTimes != null)
            {
                pinnedTimes.Dispose();
            }
        }
    }

    /****************************************************************************
    *  Set channels
    *
    *  Returns false if any channel could not be set up.
    ****************************************************************************/
    bool SetChannels()
    {
	    short channel;
	    short ok;

        // Channel 0 is the cold junction and takes the dedicated 'C' type.
        ok = Imports.TC08SetChannel(_handle, 0, TC_TYPE_CJC);

        // Each result is tested in turn: the driver documents "non-zero means
        // success", so the individual codes must not be combined.
	    for (channel = 1; channel < NUM_TC08_CHANNELS && ok != 0; channel++)
	    {
            ok = Imports.TC08SetChannel(_handle, channel, TC_TYPE_K);
	    }

        if (ok == 0)
        {
            Console.WriteLine("Error setting up channels: {0}", Imports.TC08GetLastError(_handle));
            return false;
        }

        return true;
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

                    // ReadLine returns null at end of input. Without this the
                    // loop would spin forever on a closed or redirected stdin.
                    if (mainsRejectionInput == null)
                    {
                        Console.WriteLine("No more input available. Mains rejection not set.");
                        return;
                    }

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

                if (status == USBTC08_SUCCESS)
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
                    if (SetChannels())
                    {
                        GetValues();
                    }
                    break;

                case 'S':
                    if (SetChannels())
                    {
                        GetStreamingValues();
                    }
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

      // usb_tc08_open_unit returns a positive handle on success, 0 if no unit
      // was found and a negative value on error. Testing only for 0 let a
      // negative handle through as if the unit had opened.
      if (handle <= 0)
      {
        Console.WriteLine("Unable to open device");
        Console.WriteLine("Error code : {0}", Imports.TC08GetLastError(0));
        WaitForKey();
        return;
      }

      Console.WriteLine("Device opened successfully\n");

      try
      {
        ConsoleExample consoleExample = new ConsoleExample(handle);
        consoleExample.Run();
      }
      finally
      {
        // Always release the unit, including when Run throws.
        Imports.TC08CloseUnit(handle);
      }
    }
  }
}
