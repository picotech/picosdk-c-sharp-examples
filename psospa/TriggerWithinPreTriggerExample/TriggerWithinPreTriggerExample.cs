// This is an example of how to use TriggerWithinPreTrigger for PicoScope PC Oscilloscope consuming the psospa driver
// This program outputs a pulse from the siggen, which is then captured on channel A and stored using this pre-trigger functionality. 
// *** This program requires BNC connection between device's signal generator and Channel A. ***
// Copyright © 2024 Pico Technology Ltd. See LICENSE file for terms.

using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DriverImports;
using PicoPinnedArray;

namespace TriggerWithinPreTriggerExample
{
  class TriggerWithinPreTriggerExample
  {
    static StandardDriverStatusCode RunTriggerWithinPreTriggerExample(short handle, DeviceResolution resolution, int numChannels)
    {
      var channel = Channel.ChannelA;
      ulong numSamples = 2000000; // 1 million pre-trigger samples (and 1 million post-trigger samples)
      double idealTimeInterval = 1e-7; // 100ns

      var status = psospaDevice.InitializeChannels(handle, new List<Channel>() { channel }, numChannels);
      if (status != StandardDriverStatusCode.Ok)
        return status;

      status = psospaDevice.EnableAWGOutput(handle, WaveType.Sine, softwareTrigger: true);
      if (status != StandardDriverStatusCode.Ok)
        return status;

      status = DriverImports.psospa.TriggerWithinPreTriggerSamples(handle, TriggerWithinPreTrigger.Arm);
      if (status != StandardDriverStatusCode.Ok)
        return status;

      status = psospaDevice.SetTrigger(handle, channel);
      if (status != StandardDriverStatusCode.Ok)
        return status;

      status = psospaDevice.RunBlock(handle, resolution, numSamples, channel, idealTimeInterval, null);
      if (status != StandardDriverStatusCode.Ok)
        return status;

      // Wait three milliseconds for AWG
      // (This is due to the device having a post sample latency after these previous API calls.
      // This latency can be reduced by enabling extra channels and/or increasing bit resolution.)
      Thread.Sleep(3);

      // The rising trigger we're passing in for this api function is dummy data
      status = DriverImports.psospa.SigGenSoftwareTriggerControl(handle, SiggenTrigType.Rising);
      if (status != StandardDriverStatusCode.Ok)
        return status;

      status = psospaDevice.WaitForDataToBeCaptured(handle, channel);
      if (status != StandardDriverStatusCode.Ok)
        return status;

      var data = new short[numSamples];
      var pinned = new PinnedArray<short>(data);

      status = psospaDevice.ReadDataFromDevice(handle, channel, numSamples, ref pinned);
      if (status != StandardDriverStatusCode.Ok)
        return status;

      // Un-pin the arrays
      if (pinned != null)
      {
        pinned.Dispose();
      }

      // Work with the arrays, Write the data to a file, etc
      PicoFileFunctions.WriteDataToFile(data);

      //Allow the GC to free the buffers
      data = null; // Now the array is ready for Garbage Collection.

      return status;
    }

    static void Main(string[] args)
    {
      short handle = 0;
      var resolution = DeviceResolution.PICO_DR_8BIT;
      int numChannels;

      var status = psospaDevice.OpenUnit(out handle, resolution, out numChannels);
      if (status == StandardDriverStatusCode.Ok)
      {
        status = RunTriggerWithinPreTriggerExample(handle, resolution, numChannels);
      }

      psospaDevice.CloseUnit(handle);
      Console.WriteLine("Application closed with status: " + status);
      Console.ReadLine();
    }
  }
}
