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

      // A handle of 0 is not a valid unit, so test for greater than zero.
      if (_handle > 0)
      {
        // Driven by the length of the description table, so the two cannot fall
        // out of step if a line is added or removed.
        for (uint i = 0; i < description.Length; i++)
        {
          // Cleared each time round: if a call fails, the buffer would
          // otherwise still hold the previous line and it would be printed
          // again as though it belonged to this one.
          line.Clear();

          // The length passed to the driver comes from the buffer itself, so
          // the two cannot drift apart and let the driver overrun it.
          StandardDriverStatusCode statusCode =
              Imports.GetUnitInfo(_handle, line, (short)line.Capacity, out short requiredSize, i);

          if (statusCode == StandardDriverStatusCode.Ok)
          {
            _logFunc($"{description[i]}: {line}");
          }
          else
          {
            _logFunc($"{description[i]}: unavailable ({statusCode})");
          }
        }
      }
    }

    public void GetMaxAdcValue()
    {
      _checkDriverStatusCodeFunc("\nGetting the maximum ADC value from device...", Imports.MaxValue(_handle, out _maxADCValue));
    }

    public void SetTrigger()
    {
      // With enabled set to 0 the remaining trigger arguments, including the
      // channel, are ignored by the driver.
      _checkDriverStatusCodeFunc("\nDisable trigger on device...", Imports.SetTrigger(_handle, 0, 0, 0, 0, 0, 0, 0, 0));
    }

    public void Run(short noOfChannels)
    {
      StandardDriverStatusCode statusCode = StandardDriverStatusCode.Ok;

      // setup the p1000 device to sample on all channels at 1kS/s
      const ushort noOfSamplesPerChannel = 1000;
      const int msSleepTime = 2000;
      Imports.enPL1000Method captureMode = Imports.enPL1000Method.STREAM;
      uint usForBlock = 1000000; // 1s

      if (noOfChannels < 1 || noOfChannels > (short)Imports.enPL1000Inputs.PL1000_MAX_CHANNELS)
      {
        _logFunc($"noOfChannels must be between 1 and {(short)Imports.enPL1000Inputs.PL1000_MAX_CHANNELS}.");
        return;
      }

      List<short> channels = new List<short>();
      for (short i = 1; i <= noOfChannels; i++)
        channels.Add(i);

      // set the sampling interval on the device
      statusCode = Imports.SetInterval(_handle, ref usForBlock, noOfSamplesPerChannel, channels.ToArray(), noOfChannels);
      _checkDriverStatusCodeFunc($"\nSet the device to capture {noOfSamplesPerChannel} samples per channel on {noOfChannels} channels...", statusCode);

      // In STREAM mode the count passed to Run is the size of the driver's
      // circular buffer in samples per channel, not the number of samples to
      // collect. Sizing it to a single collection period lets the buffer wrap
      // during the sleep below, so readings are overwritten before GetValue
      // reads them. The pl1000Con C example uses a factor of ten for the same
      // reason.
      const uint circularBufferFactor = 10;

      // capture data from the device using the run method
      statusCode = Imports.Run(_handle, noOfSamplesPerChannel * circularBufferFactor, captureMode);
      _checkDriverStatusCodeFunc($"\nStart capturing on device...", statusCode);

      try
      {
        Thread.Sleep(msSleepTime);

        // pull the data back from the device
        ushort overflow = 0;
        ushort[] values = new ushort[noOfSamplesPerChannel * noOfChannels];
        uint numberOfSamples = noOfSamplesPerChannel;
        uint triggerIndex = 0; // the returned value can be ignored as we're capturing with triggering disabled

        statusCode = Imports.GetValue(_handle, values, ref numberOfSamples, out overflow, out triggerIndex);
        _checkDriverStatusCodeFunc("\nGather data from device...", statusCode);

        // numberOfSamples is an in/out argument: the driver reports back how
        // many samples per channel it actually returned. Clamp it to what the
        // buffer holds before it is used to index into that buffer.
        if (numberOfSamples > noOfSamplesPerChannel)
        {
          numberOfSamples = noOfSamplesPerChannel;
        }

        _logFunc($"\n{numberOfSamples} samples per channel were captured over {msSleepTime}ms\n");

        if (overflow != 0)
        {
          _logFunc("Warning: one or more channels went over range during this capture.\n");
        }

        if (numberOfSamples > 0)
        {
          if (_maxADCValue == 0)
          {
            _logFunc("The maximum ADC value is zero, so readings cannot be converted to volts.");
            return;
          }

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

            averageVoltage[i] = channelValues[i].Average(e => e * Imports.PL1000_FULL_SCALE_VOLTS / _maxADCValue);

            // write the average voltage (2dp precision) to the console
            _logFunc($"Channel {i + 1} average voltage: {averageVoltage[i].ToString("F2")}V");
          }
        }
      }
      finally
      {
        // Stop the device whatever happened above, so it is not left converting
        // after this method returns. The unchecked call is deliberate: throwing
        // from a finally block would hide the original failure.
        Imports.Stop(_handle);
      }
    }
  }
}
