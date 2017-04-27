/**************************************************************************
*
* Filename: USBPT104CSConsole.cs
* 
* Description:
*   This is a console-mode program that demonstrates how to call the
*   usbpt104 driver API functions using .NET.
*   
* Copyright (C) 2015 - 2017 Pico Technology Ltd. See LICENSE file for terms.
*
***************************************************************************/

using System;
using System.Text;
using System.Threading;

using USBPT104Imports;
using PicoStatus;

namespace USBPT104CSConsole
{
    class ConsoleExample
    {
        private readonly short _handle;
        public const uint USBPT104_MAX_CHANNELS = 4;
        public bool[] enabledChannels = new bool[] { false, false, false, false };
        private static void WaitForKey()
        {
            while(!Console.KeyAvailable)
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
            short required_size = 0;
            StringBuilder line = new System.Text.StringBuilder(256);
            short lineLength = (short) line.Capacity;

            if (_handle >= 0)
            {
                Console.WriteLine("Device Info:\n");
                
                Imports.UsbPt104GetUnitInfo(_handle, line, lineLength, ref required_size, PicoInfo.PICO_BATCH_AND_SERIAL);
                Console.WriteLine("Batch and serial: {0}", line);
                
                Imports.UsbPt104GetUnitInfo(_handle, line, lineLength, ref required_size, PicoInfo.PICO_DRIVER_VERSION);
                Console.WriteLine("Driver version: {0}", line);
                
                Imports.UsbPt104GetUnitInfo(_handle, line, lineLength, ref required_size, PicoInfo.PICO_CAL_DATE);
                Console.WriteLine("Calibration date: {0}", line);
            }
            else
            {
                Console.WriteLine("Error: Failed to get Device Info\n");
            }
        }

   
        /****************************************************************************
         * Read temperature information from the unit
         ****************************************************************************/
        unsafe void GetValues()
        {
            uint status = StatusCodes.PICO_OK;
            uint chan;
            int []tempbuffer = new int[4]; // Single ended measurements; use 8 for differential and update for loop below

            bool channelSet = false;

            foreach (bool b in enabledChannels)
            {
                if (b == true)
                {
                    channelSet = true;
                    break;
                }
            }

            if (!channelSet)
            {
                Console.WriteLine("Please enable at least one channel");
                return;
            }

            Console.Write("\n");
            Console.WriteLine("Press any key to stop....\n");
            Console.Write("\n");

            // Label the columns
            for (chan = 0; chan < USBPT104_MAX_CHANNELS; chan++)
            {   
                if (enabledChannels[chan] == false)
                {
                    continue;
                }
                Console.Write("Chan{0}:\t\t", chan+1);
            }
            Console.Write("\n");

            do
            {
                    for (chan = 0; chan < USBPT104_MAX_CHANNELS; chan++)
                    {
                        if (enabledChannels[chan] == false)
                        {
                            continue;
                        }

                        status = Imports.UsbPt104GetValue( _handle, (Imports.UsbPt104Channels) (chan + 1), out tempbuffer[chan], 0);

                        if (status == StatusCodes.PICO_OK)
                        {
                            // Divide by 1000 for PT100 and PT1000 sensors         
                            Console.Write("{0:000.0000}\t", tempbuffer[chan] / 1000.0000);
                        }
                        else
                        {
                            Console.Write("{0:000.0000}\t", 0);
                        }
                             
                        

                    }
                    Console.Write("\n");
                    Thread.Sleep(2880); // Delay for min. hardware channel reading time, to get new reading from all channels

            } while (!Console.KeyAvailable);

            char ch = (Console.ReadKey().KeyChar);       // Use up keypress
            status = Imports.UsbPt104CloseUnit(_handle);
        }

        /****************************************************************************
        *  Set channels 
        ****************************************************************************/
        
        void SetChannels()
        {
            SetChannels:
            Console.WriteLine("Press 1-4 to carry out the actions:");
            for (int i = 0 ; i < enabledChannels.Length; i++) {
                string channelStatus = enabledChannels[i] ? "Disable" : "Enable";
                Console.WriteLine("[" + i + "]: " + channelStatus +" channel "+i);
            }

            Console.WriteLine("[4]: Exit");

            int clicked = -1;
            Console.Write("Input: ");
            int.TryParse(Console.ReadLine(), out clicked);
            if(clicked == 4)
            {
                return;
            }
            if(clicked == -1 || clicked < 0 || clicked > 3)
            {
                Console.WriteLine("Error setting channels.");
                goto SetChannels;
            }

            short enabled = enabledChannels[clicked] ? Imports.USBPT104_OFF : Imports.USBPT104_MAX_WIRES;
            uint status = Imports.UsbPt104SetChannel(_handle, (Imports.UsbPt104Channels)(clicked + 1), Imports.UsbPt104DataType.USBPT104_PT100, enabled);
            
            if (status == 0)
            {
                enabledChannels[clicked] = !enabledChannels[clicked];
            }
            string action = enabledChannels[clicked] ? "Enabled" : "Disabled";
            Console.WriteLine("Channel " + clicked + " is " + action);

        }

        /****************************************************************************
        *  Run
        ****************************************************************************/
        
        public void Run()
        {
            // Main loop - read key and call routine
            char ch = ' ';
            while (ch != 'X')
            {
                Console.WriteLine("");
                Console.WriteLine("I - Device Info");
                Console.WriteLine("S - Set Channels");
                Console.WriteLine("G - Get Temperatures");
                //Console.WriteLine("N - Setup IP network");
                //Console.WriteLine("P - Get Temperatures via IP network");
                Console.WriteLine("X - Exit");
                Console.Write("\nOperation:");

                ch = char.ToUpper(Console.ReadKey().KeyChar);

                Console.WriteLine("\n");
                switch (ch)
                {
                    case 'I':
                        GetDeviceInfo();
                        break;

                    case 'S':
                        SetChannels();
                        break;

                    case 'G':
                        GetValues();
                        break;

                    case 'X':
                        // Handled by outer loop 
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
            short handle;
            uint status = StatusCodes.PICO_OK;

            StringBuilder detailsStr = new StringBuilder(256);
            uint enumerateLength = (uint) detailsStr.Capacity;

            Console.WriteLine("USB PT-104 (usbpt104) Driver Example Program");
            Console.WriteLine("Version 1.3\n");

            Console.WriteLine("Enumerating devices...\n");

            // Enumerate all USB and Ethernet devices
            status = Imports.UsbPt104Enumerate(detailsStr, ref enumerateLength, Imports.CommunicationType.CT_ALL);

            if (status == StatusCodes.PICO_OK)
            {
                Console.WriteLine("USB PT-104 devices found: {0}\n", detailsStr);
            }
            else
            {
                Console.WriteLine("No USB PT-104 devices found.");
            }

            Console.WriteLine("\nOpening the device...");

            status = Imports.UsbPt104OpenUnit(out handle, null);
            Console.WriteLine("Handle: {0}", handle);
      
            if (handle == 0)
            {
                Console.WriteLine("Unable to open device");
                Console.WriteLine("Error code : {0}", status);
                WaitForKey();
            }
            else
            {
                Console.WriteLine("Device opened successfully\n");

                // WaitForKey();
                ConsoleExample consoleExample = new ConsoleExample(handle);
                consoleExample.Run();

                Imports.UsbPt104CloseUnit(handle);
            }
        }
    }
}  
