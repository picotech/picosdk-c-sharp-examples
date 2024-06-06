/*******************************************************************************
 *
 * Filename: PL1000Device.cs
 *
 * Description:
 *   This file contains the PL1000Device class that demonstrates how to use the pl1000
 *    
 * Copyright (C) 2012 - 2024 Pico Technology Ltd. See LICENSE file for terms.    
 *    
 *******************************************************************************/

using System;
using System.Threading;
using System.Linq;

using PL1000Imports;
using System.Collections.Generic;
using DriverImports;

namespace PL1000CSConsole
{
  public class PL1000Device
  {
    private readonly short _handle;
    private readonly Action<string, StandardDriverStatusCode> _checkDriverStatusCodeFunc;
    private readonly Action<string> _logFunc;
    private ushort _maxADCValue;

    public PL1000Device(short handle,
                        Action<string, StandardDriverStatusCode> checkDriverStatusCodeFunc,
                        Action<string> logFunc)
    {
      _handle = handle;
      _checkDriverStatusCodeFunc = checkDriverStatusCodeFunc;
      _logFunc = logFunc;
    }

    /// <summary>
    /// Show information about device
    /// </summary>
    public void GetDeviceInfo()
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
          Imports.GetUnitInfo(_handle, line, 80, out short requiredSize, i);
          _logFunc($"{description[i]}: {line}");
        }
      }
    }

    public void GetMaxAdcValue()
    {
      _checkDriverStatusCodeFunc("\nGetting the maximum ADC value from device...", Imports.MaxValue(_handle, out _maxADCValue));
    }

    public void SetTrigger()
    {
      _checkDriverStatusCodeFunc("\nDisable trigger on device...", Imports.SetTrigger(_handle, 0, 0, 0, 0, 0, 0, 0, 0));
    }

    public void Run(short noOfChannels)
    {
      StandardDriverStatusCode statusCode = 0;

      // setup the p1000 device to sample on all channels at 1kS/s
      const ushort noOfSamplesPerChannel = 1000;
      Imports.enPL1000Method captureMode = Imports.enPL1000Method.STREAM;
      ushort totalSamples = (ushort)(noOfSamplesPerChannel * noOfChannels);
      uint usForBlock = 1000000; // 1s

      List<short> channels = new List<short>();
      for (short i = 1; i <= noOfChannels; i++)
        channels.Add(i);

      // set the sampling interval on the device
      statusCode = Imports.SetInterval(_handle, ref usForBlock, noOfSamplesPerChannel, channels.ToArray(), noOfChannels);
      _checkDriverStatusCodeFunc($"\nSet the device to capture {noOfSamplesPerChannel} samples per channel on {noOfChannels} channels...", statusCode);

      // capture data from the device using the run method
      statusCode = Imports.Run(_handle, noOfSamplesPerChannel, captureMode);
      _checkDriverStatusCodeFunc($"\nStart capturing on device...", statusCode);

      const int msSleepTime = 2000;
      Thread.Sleep(msSleepTime);

      // pull the data back from the device
      ushort overflow = 0;
      ushort[] values = new ushort[noOfSamplesPerChannel * noOfChannels];
      uint numberOfSamples = noOfSamplesPerChannel;
      uint triggerIndex = 0; // the returned value can be ignored as we're capturing with triggering disabled

      statusCode = Imports.GetValue(_handle, values, ref numberOfSamples, out overflow, out triggerIndex);
      _checkDriverStatusCodeFunc("\nGather data from device...", statusCode);
      _logFunc($"\n{numberOfSamples} samples per channel were captured over {msSleepTime}ms\n");

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
          _logFunc($"Channel {i + 1} average voltage: {averageVoltage[i].ToString("F2")}V");
        }
      }
    }
  }
}
