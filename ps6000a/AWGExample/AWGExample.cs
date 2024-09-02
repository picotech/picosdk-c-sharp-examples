﻿// This is an example of how to setup the AWG for PicoScope 6000 Series PC Oscilloscope consuming the ps6000a driver
// Connect a BNC Cable between the AWG output and Channel A and observe that data captured in Output.csv
// Copyright © 2020-2024 Pico Technology Ltd. See LICENSE file for terms.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DriverImports;
using PicoPinnedArray;

namespace AWGExample
{
  class AWGExample
  {
    /// <summary>
    /// Outputs a 100KHz Sine wave from AWG output, captures on Channel A and the data is written to Output.csv
    /// </summary>
    static StandardDriverStatusCode RunAWGExample(short handle, DeviceResolution resolution, int numChannels)
    {
      var channel = Channel.ChannelA;
      ulong numSamples = 1000;
      double idealTimeInterval = 0.0000001; //100ns

      var status = ps6000aDevice.InitializeChannels(handle, new List<Channel>() { channel }, numChannels);
      if (status != StandardDriverStatusCode.Ok) return status;

      status = ps6000aDevice.EnableAWGOutput(handle, WaveType.Sine);
      if (status != StandardDriverStatusCode.Ok) return status;

      status = ps6000aDevice.RunBlock(handle, resolution, numSamples, channel, idealTimeInterval);
      if (status != StandardDriverStatusCode.Ok) return status;

      status = ps6000aDevice.WaitForDataToBeCaptured(handle, channel);
      if (status != StandardDriverStatusCode.Ok) return status;

      var data = new short[numSamples];
      var pinned = new PinnedArray<short>(data);

           
      
        status = ps6000aDevice.ReadDataFromDevice(handle, channel, numSamples, ref data);
        if (status != StandardDriverStatusCode.Ok)
          return status;

        PicoFileFunctions.WriteDataToFile(data);
     
      return status;
    }

    static void Main(string[] args)
    {
      short handle = 0;
      int numChannels;
      var resolution = DeviceResolution.PICO_DR_8BIT;

      var status = ps6000aDevice.OpenUnit(out handle, resolution, out numChannels);
      if (status == StandardDriverStatusCode.Ok)
      {
        status = RunAWGExample(handle, resolution, numChannels);
      }

      ps6000aDevice.CloseUnit(handle);
      Console.WriteLine("Application closed with status: " + status);
      Console.ReadLine();
    }
  }
}
