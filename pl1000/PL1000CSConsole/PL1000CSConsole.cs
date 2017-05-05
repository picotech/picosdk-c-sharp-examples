/*******************************************************************************
 *
 * Filename: PL1000CSConsole.cs
 *
 * Description:
 *   This is a console-mode program that demonstrates how to use the
 *   pl1000 driver using .NET
 *
 * Supported PicoLog models:
 *
 *		PicoLog 1012 & 1216
 *		
 * Examples:
 *    Open a connection and display unit information before closing the 
 *    connection to the device.
 *    
 * Copyright (C) 2012 - 2017 Pico Technology Ltd. See LICENSE file for terms.    
 *    
 *******************************************************************************/

using System;
using System.Threading;

using PL1000Imports;

namespace PL1000CSConsole
{
    class PL1000CSConsole
    {
        private short _handle;

        /* ********************************************************************************************************************************
        *  WaitForKey() 
        * 
        *  Wait for user keypress
        *  ********************************************************************************************************************************/

        private static void WaitForKey()
        {
            while (!Console.KeyAvailable) Thread.Sleep(100);
            if (Console.KeyAvailable) Console.ReadKey(true); // clear the key
        }

        /***********************************************************************************************
         * GetDeviceInfo()
         * Show information about device
         * 
        **********************************************************************************************/
        void GetDeviceInfo()
        {
            string[] description = {
                                       "Driver Version    ",
                                       "USB Version       ",
                                       "Hardware Version  ",
                                       "Variant Info      ",
                                       "Batch and Serial  ", 
						               "Calibration Date  ", 
						               "Kernel Driver Ver "
                                    };
            
            System.Text.StringBuilder line = new System.Text.StringBuilder(80);

            if (_handle >= 0)
            {

                for (int i = 0; i < 7; i++)
                {
                    short requiredSize;
                    Imports.GetUnitInfo(_handle, line, 80, out requiredSize, i);

                    Console.WriteLine("{0}: {1}", description[i], line);
                }
            }
        }


        /***********************************************************************************************
        * Run()
        * Show information about device
        * 
        **********************************************************************************************/
        public void Run()
        {
            // Display unit information
            GetDeviceInfo();
        }

        /***********************************************************************************************
        * PL1000ConsoleExample()
        * Show information about device
        * 
        **********************************************************************************************/
        PL1000CSConsole(short handle)
        {
            _handle = handle;
        }

        static void Main(string[] args)
        {
            short handle;

            System.Text.StringBuilder str = new System.Text.StringBuilder(80);
            str = null;

            Console.WriteLine("PicoLog 1000 Series (pl1000) Driver Example Program");
            Console.WriteLine("Version 1.1\n");

            // Open unit 
            Console.WriteLine("\nAttempting to open a device...");

            Imports.OpenUnit(out handle);

            Console.WriteLine("Handle: {0}", handle);

            if (handle == 0)
            {
                Console.WriteLine("Unable to open device");
                Console.WriteLine("Error code : {0}", handle);
            }
            else
            {
                Console.WriteLine("Device opened successfully:\n");

                PL1000CSConsole consoleExample = new PL1000CSConsole(handle);
                consoleExample.Run();

                Console.WriteLine();
                Console.WriteLine("Press any key to exit the application.");
                WaitForKey();

                Imports.CloseUnit(handle);
            }

            

        }
    }
}
