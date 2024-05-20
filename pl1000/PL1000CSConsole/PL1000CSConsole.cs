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
 * Copyright (C) 2012 - 2024 Pico Technology Ltd. See LICENSE file for terms.    
 *    
 *******************************************************************************/

using System;
using System.Threading;
using DriverImports;
using PL1000Imports;

namespace PL1000CSConsole
{
  class PL1000CSConsole
  {
    static void Main(string[] args)
    {
      const short noOfChannels = 4;
      short handle = -1;
      StandardDriverStatusCode statusCode = StandardDriverStatusCode.Ok;

      Log("PicoLog 1000 Series (pl1000) Driver Example Program");
      Log("Version 1.2\n");
      try
      {
        // Open unit 
        statusCode = Imports.OpenUnit(out handle);
        CheckDriverStatus("\nAttempting to open a device...", statusCode);

        // we only get here if the device opened successfully
        Log($"Handle: {handle}");
        Log("Device opened successfully:\n");

        PL1000Device device = new PL1000Device(handle, CheckDriverStatus, Log);

        // Display unit information
        device.GetDeviceInfo();

        // disable the trigger on the pl1000 device
        device.SetTrigger();

        // get the max adc value
        device.GetMaxAdcValue();

        // capture the data from the device
        device.Run(noOfChannels);
      }
      catch (Exception ex)
      {
        Log($"Error found: {ex.Message}");
      }
      finally
      {
        WaitForUserToContinue();

        if (handle > 0)
          Imports.CloseUnit(handle);
      }
    }

    private static void WaitForUserToContinue()
    {
      Log("");
      Log("Press any key to exit the application.");
      WaitForKey();
    }

    /// <summary>
    /// Wait for user keypress
    /// </summary>
    private static void WaitForKey()
    {
      while (!Console.KeyAvailable) 
        Thread.Sleep(100);
      
      if (Console.KeyAvailable) 
        Console.ReadKey(true);
    }

    /// <summary>
    /// Log the given action
    /// </summary>
    /// <param name="action"></param>
    private static void Log(string action)
    {
      Console.WriteLine(action);
    }

    /// <summary>
    /// Logs a device interaction and checks the returned status code
    /// </summary>
    /// <param name="action">The performed driver interaction</param>
    /// <param name="returnedStatusCode">The driver return code</param>
    /// <exception cref="StandardDriverStatusCodeException"></exception>
    private static void CheckDriverStatus(string action, StandardDriverStatusCode returnedStatusCode)
    {
      Log(action);

      if (returnedStatusCode != StandardDriverStatusCode.Ok)
        throw new StandardDriverStatusCodeException(returnedStatusCode);

      Log("Success");
    }
  }
}
