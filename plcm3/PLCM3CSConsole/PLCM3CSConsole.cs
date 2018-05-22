/*******************************************************************************
 *
 * Filename: PLCM3CSConsole.cs
 *
 * Description:
 *   This is a console-mode program that demonstrates how to use the
 *   plcm3 driver API functions using .NET for the PicoLog CM3 Current 
 *   Data logger.
 *   
 * Examples:
 *    Configure channels
 *    Configure ethernet settings
 *    Collect data
 *    
 * Copyright (C) 2011 - 2017 Pico Technology Ltd. See LICENSE file for terms.    
 *    
 *******************************************************************************/

using System;
using System.Text;
using System.Threading;

using PLCM3Imports;
using PicoStatus;

namespace PLCM3CSConsole
{
    class ConsoleExample
    {
        private readonly short _handle;

        public static uint status;
        public static bool USB = true;

        public static Imports.enPLCM3DataType[] channelSettings = new Imports.enPLCM3DataType[Imports.NUM_CHANNELS];

        /* ********************************************************************************************************************************
        * WaitForKey() 
        * 
        * Wait for user keypress
        *  ********************************************************************************************************************************/
        private static void WaitForKey()
        {
            while (!Console.KeyAvailable)
            {
                Thread.Sleep(100);
            }

            if (Console.KeyAvailable)
            {
                Console.ReadKey(true); // Clear the key
            }
        }


        /* ********************************************************************************************************************************
        * channelSetup()
        * 
        * Set each channel with the type of input it will have. Valid types are from 0 to 4.
        *  ********************************************************************************************************************************/
        void channelSetup()
        {
            int index;
            int channel;
            int type;
            bool validInput;

            Console.WriteLine("Channel Setup\n=============\n");
            Console.WriteLine("0:\tOFF");
            Console.WriteLine("1:\t1mV range (1mV/A)");
            Console.WriteLine("2:\t10mV range (10mV/A)");
            Console.WriteLine("3:\t100mV range (100mV/A)");
            Console.WriteLine("4:\tVoltage input\n");

            for (index = 0; index < Imports.NUM_CHANNELS && status == StatusCodes.PICO_OK; index++)
            {
                channel = 1 + index;
                do
                {
                    Console.WriteLine("Channel {0}:-", channel);

                    try
                    {
                        Console.WriteLine("Enter measurement type: ");
                        type = int.Parse(Console.ReadLine());

                        if (type < (int) Imports.enPLCM3DataType.PLCM3_OFF || type > (int) Imports.enPLCM3DataType.PLCM3_VOLTAGE)
                        {
                            Console.WriteLine("Invalid Input"); 
                            validInput = false;
                        }
                        else
                        {
                            channelSettings[index] = (Imports.enPLCM3DataType)type;
                            Console.WriteLine("Measurement Type: {0}", measurementTypeToString(((Imports.enPLCM3DataType)type)));
                            validInput = true;
                        }

                        Console.WriteLine("\n");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        validInput = false;
                    }
                }
                while (!validInput);
            }
        }


        /* ********************************************************************************************************************************
        * measurementTypeToString()
        * 
        * Routine to allow the selected measurement type into a string to output.
        *  ********************************************************************************************************************************/
        string measurementTypeToString(Imports.enPLCM3DataType measurementType)
        {
            System.Text.StringBuilder type = new System.Text.StringBuilder(80);

            switch (measurementType)
            {
                case Imports.enPLCM3DataType.PLCM3_OFF:
                    return "OFF";

                case Imports.enPLCM3DataType.PLCM3_1_MILLIVOLT:
                    return "PLCM3_1MV";

                case Imports.enPLCM3DataType.PLCM3_10_MILLIVOLTS:
                    return "PLCM3_10MV";

                case Imports.enPLCM3DataType.PLCM3_100_MILLIVOLTS:
                    return "PLCM3_100MV";

                case Imports.enPLCM3DataType.PLCM3_VOLTAGE:
                    return "PLCM3_VOLTAGE";

                default:
                    return "ERROR";
            }
        }


        /* ********************************************************************************************************************************
        * applyScaling()
        * 
        * Routine to scale the values returned by the driver.
        *  ********************************************************************************************************************************/
        double applyScaling(long value, int channel, ref String units)
        {
            switch (channelSettings[channel])
            {
                case Imports.enPLCM3DataType.PLCM3_OFF:
                    units = "";
                    return 0;

                case Imports.enPLCM3DataType.PLCM3_1_MILLIVOLT:
                    units = "A";
                    return value / 1000.0;

                case Imports.enPLCM3DataType.PLCM3_10_MILLIVOLTS:
                    units = "A";
                    return value / 1000.0;

                case Imports.enPLCM3DataType.PLCM3_100_MILLIVOLTS:
                    units = "mA";
                    return value;

                case Imports.enPLCM3DataType.PLCM3_VOLTAGE:
                    units = "mV";
                    return value / 1000.0;

                default:
                    return -1;
            }
        }


        /* ********************************************************************************************************************************
        * collectData()
        * 
        * Routine to allow the user to change channel settings
        *  ********************************************************************************************************************************/
        void collectData()
        {
            int channel;
            int index;
            uint[] values = new uint[Imports.NUM_CHANNELS];
            double[] scaledValues = new double[Imports.NUM_CHANNELS];
            string[] units = new string[Imports.NUM_CHANNELS];

            status = StatusCodes.PICO_OK;

	        // Display channel settings
	        Console.WriteLine("Settings:\n");

            for(channel = 0; channel < Imports.NUM_CHANNELS; channel++)
	        {
		        Console.WriteLine("Channel {0}:-", channel + 1);
		        Console.WriteLine("Measurement Type: {0}", measurementTypeToString(channelSettings[channel]));
	        }

            for(index = 0; index < Imports.NUM_CHANNELS && status == StatusCodes.PICO_OK; index++)
            {
                channel = index + 1;
                status = Imports.SetChannel(_handle, (Imports.enPLCM3Channels) channel,
                                            (Imports.enPLCM3DataType) channelSettings[index]);
            }

            if (status != StatusCodes.PICO_OK)
	        {
		        Console.WriteLine("\n\nSetChannel: Status = {0:X}\nPress any key", status);
		   
	        }

            Console.WriteLine();

	        Console.WriteLine("Press any key to start.\n\n");
	        WaitForKey();

	        Console.WriteLine("Press any key to stop...\n\n");

            for (channel = 0; channel < Imports.NUM_CHANNELS; channel++)
            {
                string channelName = String.Format("{0, -13}", "Chan " + (channel + 1).ToString());
                Console.Write("{0}", channelName);
            }

            Console.WriteLine();

            while (!Console.KeyAvailable && (status == StatusCodes.PICO_OK || status == StatusCodes.PICO_NO_SAMPLES_AVAILABLE))
	        {
                for (index = 0; index < Imports.NUM_CHANNELS && (status == StatusCodes.PICO_OK || status == StatusCodes.PICO_NO_SAMPLES_AVAILABLE); index++)
		        {
                    channel = index + 1;
			        status = Imports.GetValue(_handle, (Imports.enPLCM3Channels) channel, out values[index]);

                    if (status == StatusCodes.PICO_NO_SAMPLES_AVAILABLE)
                    {
                        values[index] = 0;
                    }

			        scaledValues[index] = applyScaling(values[index], index, ref units[index]);
		        }

                // Print values for each channel
                for (index = 0; index < Imports.NUM_CHANNELS; index++)
		        {
                    if (scaledValues[index] == 0)
                    {
                        Console.Write("{0}{1, -12}", scaledValues[index], units[index]);
                    }
                    else
                    {
                        Console.Write("{0}{1, -10}", scaledValues[index], units[index]);
                    }

                    if (index == Imports.NUM_CHANNELS - 1)
                    {
                        Console.Write("\n");
                    }
                    else
                    {
                        Console.Write("");
                    }

			        Thread.Sleep(1000);
		        }
	        }

            if (status != StatusCodes.PICO_OK && status != StatusCodes.PICO_NO_SAMPLES_AVAILABLE)
            {
                Console.WriteLine("\n\nGetValue: Status = {0:X}\nPress any key", status);
            }

	        WaitForKey();
        }


        /* ********************************************************************************************************************************
         * checkPort()
         * 
         * Routine to check the port entered is in the correct format
         *  ********************************************************************************************************************************/
        static bool checkPort(string portstr, ref ushort port)
        {
            if (ushort.TryParse(portstr, out port))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /* ********************************************************************************************************************************
        * validateIpAddress()
        * 
        * Routine to check IP address entered is in the correct format
        *  ********************************************************************************************************************************/
        static bool validateIpAddress(string IPAddress)
        {
            // Check input is a valid IPAdress format
            string[] ipparts = IPAddress.Split('.');

            bool result = true;

            if (ipparts.Length == 4)
            {
                foreach (string part in ipparts)
                {
                    int r;

                    if (!int.TryParse(part, out r) || r < 0 || r > 255)
                    {
                        result = false;
                        break;
                    }
                }
            }
            else
            {
                result = false;
            }

            return result;
        }

        /* ********************************************************************************************************************************
        * ethernetSettings()
        * 
        * Routine to update the Ethernet settings on the device
        *  ********************************************************************************************************************************/
        void ethernetSettings()
        {
            short enabled = 1;
            char ch;
            ushort length;
            ushort port = 0;

            System.Text.StringBuilder ipAddress = new System.Text.StringBuilder(20);

            length = (ushort) ipAddress.Capacity;

            //Display current ethernet settings
            status = Imports.IpDetails(_handle, ref enabled, ipAddress, ref length, ref port, Imports.enIpDetailsType.PLCM3_IDT_GET);

            if (status != StatusCodes.PICO_OK)
            {
                Console.WriteLine("IP details: Status = {0:X}", status);
                return;
            }

            Console.WriteLine("\nEthernet Settings\n");
            Console.WriteLine("Enabled:\t{0}", enabled);
            Console.WriteLine("IP Address:\t{0}", ipAddress);
            Console.WriteLine("Port:\t\t{0}", port);

            // Enter new settings
            Console.WriteLine("\nEdit settings? (Y/N)");
            ch = char.ToUpper(Console.ReadKey(true).KeyChar);

            if (ch == 'Y')
            {
                Console.WriteLine("Enable ethernet? (Y/N)\n");
                
                ch = char.ToUpper(Console.ReadKey(true).KeyChar);

                if (ch == 'N')
                {
                    enabled = 0;
                }
                else
                {
                    enabled = 1;

                    Console.WriteLine("Enter IP address: ");
                    string input = Console.ReadLine();

                    if (validateIpAddress(input))
                    {
                        System.Text.StringBuilder IPAddressWrite = new System.Text.StringBuilder(input);

                        Console.WriteLine("Enter port: ");

                        string portstr;
                        portstr = Console.ReadLine();


                        if (checkPort(portstr, ref port))
                        {
                    
                            status = Imports.IpDetails(_handle, ref enabled, IPAddressWrite, ref length, ref port, Imports.enIpDetailsType.PLCM3_IDT_SET);

                            if (status != StatusCodes.PICO_OK)
                            {
                                Console.WriteLine("IP details: Status = (0:X}", status);
                                return;
                            }
                        }
                        else
                        {
                            Console.WriteLine("Invalid Port");
                        }
                  
                    }
                    else
                    {
                        Console.WriteLine("Invalid IP Address");
                    }
                }
            }
        }

        /***********************************************************************************************
         * getDeviceInfo()
         * Show information about device
         * 
        **********************************************************************************************/
        void getDeviceInfo()
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

            StringBuilder line = new StringBuilder(80);

            if (_handle >= 0)
            {

                for (uint i = 0; i < 7; i++)
                {
                    short requiredSize;
                    Imports.GetUnitInfo(_handle, line, 80, out requiredSize, i);

                    Console.WriteLine("{0}: {1}", description[i], line);
                }
            }
        }


        /***********************************************************************************************
        * run()
        * Show information about device
        * 
        **********************************************************************************************/
        public void run()
        {
            char ch;
          
            getDeviceInfo();

            do
            {
                Console.WriteLine("\nSelect an operation:\n");
                Console.WriteLine("S:\tStart acquisition");
                Console.WriteLine("C:\tChannel Settings");
                Console.WriteLine("E:\tEthernet Settings");
                Console.WriteLine("X:\tExit");
                Console.WriteLine("\n");

                ch = char.ToUpper(Console.ReadKey(true).KeyChar);

                switch (ch)
                {
                    case 'S':
                       collectData();
                        break;

                    case 'C':
                       channelSetup();
                        break;

                    case 'E':
                        if (!USB)
                        {
                            Console.WriteLine("Connect via USB to set up Ethernet.");
                            break;
                        }
                        ethernetSettings();
                        break;

                    case 'X':
                        break;

                    default:
                        Console.WriteLine("\nInvalid selection.");
                        break;
                }

            } while (ch != 'X');
        }

        private ConsoleExample(short handle)
        {
            _handle = handle;
        }


        /***********************************************************************************************
        * Main()
        * 
        * 
        **********************************************************************************************/
        static void Main(string[] args)
        {
            short handle;
            char ch ;
            bool validSelection = false;
            string ipAddress = "";
            uint status = StatusCodes.PICO_OK;
            
            StringBuilder detailsStr = new StringBuilder(256);
            uint enumerateLength = (uint) detailsStr.Capacity;

            StringBuilder str = new StringBuilder(80);
            str = null;

            for (int chan = 0; chan < Imports.NUM_CHANNELS; chan++)
            {
                channelSettings[chan] = Imports.enPLCM3DataType.PLCM3_1_MILLIVOLT;
            }

            Console.WriteLine("PicoLog CM3 (plcm3) Driver C# Example Program");
            Console.WriteLine("Version 1.1\n");

            Console.WriteLine("Enumerating devices...");

            // Enumerate all USB and Ethernet devices
            status = Imports.Enumerate(detailsStr, ref enumerateLength, Imports.enCommunicationType.PLCM3_CT_ALL);

            if (status == StatusCodes.PICO_OK)
            {
                Console.WriteLine("PLCM3 devices found: {0}\n", detailsStr);
            }
            else
            {
                Console.WriteLine("No PLCM3 devices found.");
            }

            do
            {
                Console.WriteLine("");
                Console.WriteLine("Select connection:");
                Console.WriteLine("U:\tUSB");
                Console.WriteLine("E:\tEthernet");

                ch = char.ToUpper(Console.ReadKey(true).KeyChar);

                switch (ch)
                {
                    case 'U':
                        USB = true;
                        validSelection = true;
                        break;

                    case 'E':
                        USB = false;
                        validSelection = true;
                       
                        bool isValidIpAddress = true;

                        do
                        {
                            Console.WriteLine("\nEnter IP Address of PLCM3 unit:");
                            ipAddress = Console.ReadLine();
                            isValidIpAddress = validateIpAddress(ipAddress);
                            {
                                if (!isValidIpAddress)
                                {
                                    Console.WriteLine("Invalid IP address format.");
                                }
                            }
                        }
                        while(!isValidIpAddress);
                        break;
                      

                    default:
                        Console.WriteLine("\nInvalid selection");
                        break;
                }

            } while (!validSelection);


            // Open unit 
            Console.WriteLine("\n\nOpening the device...");

            if (USB)
            {
                Imports.OpenUnit(out handle, str);
            }
            else
            {
                status = Imports.OpenUnitViaIp(out handle, str, ipAddress);
            }

            Console.WriteLine("Handle: {0}", handle);

            if (handle == 0)
            {
                Console.WriteLine("Unable to open device.");
                Console.WriteLine("Error code : {0}", handle);
                WaitForKey();
            }
            else
            {
                Console.WriteLine("\nDevice opened successfully:-\n");

                ConsoleExample consoleExample = new ConsoleExample(handle);
                consoleExample.run();

                Imports.CloseUnit(handle);
            }
        }
    }
}

