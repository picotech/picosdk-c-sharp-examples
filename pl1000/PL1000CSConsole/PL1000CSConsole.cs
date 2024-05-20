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
using System.Linq;

using PL1000Imports;
using PicoErrorCode = PL1000Imports.Imports.PicoErrorCode;
using System.Collections.Generic;

namespace PL1000CSConsole
{
  class PL1000CSConsole
  {
    private short _handle;
    private ushort _maxADCValue;

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
      string[] description =
        {
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
    * Capture streamed data for a given number of channels and display the averaged voltages
    * 
    **********************************************************************************************/
    public void Run(short noOfChannels)
    {
      PicoErrorCode errorCode = 0;

      // setup the p1000 device to sample on all channels at 1kS/s
      const ushort noOfSamplesPerChannel = 1000;
      Imports.enPL1000Method captureMode = Imports.enPL1000Method.STREAM;
      ushort totalSamples = (ushort)(noOfSamplesPerChannel * noOfChannels);
      uint usForBlock = 1000000; // 1s

      Console.WriteLine($"\nTest on {noOfChannels} channels...");
      Console.WriteLine("----------------------------------");

      List<short> channels = new List<short>();
      for (short i = 1; i <= noOfChannels; i++)
        channels.Add(i);

      Console.WriteLine("\nSet the sampling interval on device...");
      errorCode = Imports.SetInterval(_handle, ref usForBlock, noOfSamplesPerChannel, channels.ToArray(), noOfChannels);
      CheckDriverErrorCode(errorCode);
      // write out to console the usForBlock, noOfSamplesPerChannel and noOfChannels
      Console.WriteLine($"\nusForBlock: {usForBlock}\nnoOfSamplesPerChannel: {noOfSamplesPerChannel}\nnoOfChannels: {noOfChannels}");

      // capture data from the device using the run method
      Console.WriteLine($"\nStart capturing on device...");
      errorCode = Imports.Run(_handle, noOfSamplesPerChannel, captureMode);
      CheckDriverErrorCode(errorCode);

      const int msSleepTime = 2000;
      Thread.Sleep(msSleepTime);

      // Call GetValue method
      ushort overflow = 0;
      ushort[] values = new ushort[noOfSamplesPerChannel * noOfChannels];
      uint numberOfSamples = noOfSamplesPerChannel;
      uint triggerIndex = 0; // the returned value can be ignored as we're capturing with triggering disabled

      Console.WriteLine("\nGather data from device...");
      errorCode = Imports.GetValue(_handle, values, ref numberOfSamples, out overflow, out triggerIndex);
      CheckDriverErrorCode(errorCode);
      Console.WriteLine($"\n{numberOfSamples} samples per channel were captured over {msSleepTime}ms");

      if (numberOfSamples > 0)
      {
        // Average the samples per channel back from the device
        ushort[][] channelValues = new ushort[noOfChannels][];
        double[] averageVoltage = new double[noOfChannels];
        for (int i = 0; i < noOfChannels; i++)
        {
          channelValues[i] = new ushort[numberOfSamples];

          int index = 0;
          for (int j = i; j < (numberOfSamples * noOfChannels); j += noOfChannels)
          {
            channelValues[i][index++] = values[j];
          }

          averageVoltage[i] = channelValues[i].Average(e => e * 2.5f / _maxADCValue);

          // write the average voltage (2dp precision) to the console
          Console.WriteLine($"Channel {i + 1} average voltage: {averageVoltage[i].ToString("F2")}V");
        }
      }
    }

    private static void CheckDriverErrorCode(PicoErrorCode errorCode)
    {
      if (errorCode != PicoErrorCode.PICO_OK)
        throw new PicoErrorCodeException(errorCode);

      Console.WriteLine("Success");
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
      const short noOfChannels = 1;
      short handle = -1;
      PicoErrorCode errorCode = PicoErrorCode.PICO_OK;

      Console.WriteLine("PicoLog 1000 Series (pl1000) Driver Example Program");
      Console.WriteLine("Version 1.1\n");
      try
      {
        // Open unit 
        Console.WriteLine("\nAttempting to open a device...");
        errorCode = Imports.OpenUnit(out handle);
        CheckDriverErrorCode(errorCode);

        // we only get here if the device opened successfully
        Console.WriteLine("Handle: {0}", handle);
        Console.WriteLine("Device opened successfully:\n");

        PL1000CSConsole consoleExample = new PL1000CSConsole(handle);

        // Display unit information
        consoleExample.GetDeviceInfo();

        // disable the trigger on the pl1000 device
        consoleExample.SetTrigger();

        // get the max adc value
        consoleExample.GetMaxAdcValue();

        for (short i = 1; i <= 16; i++)
          consoleExample.Run(i);
      }
      catch (Exception ex)
      {
        Console.WriteLine($"Error found: {ex.Message}");
      }
      finally
      {
        WaitForUserToContinue();

        if (handle > 0)
          Imports.CloseUnit(handle);
      }
    }

    private void GetMaxAdcValue()
    {
      // get the maximum ADC value
      Console.WriteLine("\nGetting the maximum ADC value from device...");
      CheckDriverErrorCode(Imports.MaxValue(_handle, out _maxADCValue));
    }

    private void SetTrigger()
    {
      Console.WriteLine("\nDisable trigger on device...");
      CheckDriverErrorCode(Imports.SetTrigger(_handle, 0, 0, 0, 0, 0, 0, 0, 0));
    }

    private static void WaitForUserToContinue()
    {
      Console.WriteLine();
      Console.WriteLine("Press any key to exit the application.");
      WaitForKey();
    }
  }
}
