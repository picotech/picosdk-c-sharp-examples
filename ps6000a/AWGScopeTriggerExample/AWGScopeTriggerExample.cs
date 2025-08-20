// This is an example of how to setup the AWG Trigger for PicoScope 6000 Series PC Oscilloscope consuming the ps6000a driver
// Input a 10KHz signal onto channel B
// Connect a BNC Cable between the AWG output and Channel A and observe that data captured in Output.csv
// Copyright © 2020-2024 Pico Technology Ltd. See LICENSE file for terms.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using DriverImports;
using PicoPinnedArray;

namespace AWGScopeTriggerExample
{
  class AWGScopeTriggerExample
  {
    /// <summary>
    /// Outputs a 100KHz Sine wave from AWG output after triggering on Channel B. Data captured on Channel A is written to Output.csv
    /// </summary>
    static StandardDriverStatusCode RunAWGTriggerExample(short handle, DeviceResolution resolution, int numChannels)
    {
      var readChannel = Channel.ChannelA;
      var triggerChannel = Channel.ChannelB;
      var channels = new List<Channel>() { readChannel, triggerChannel };

      ulong numSamples = 2000;
      double idealTimeInterval = 0.0000001; //100ns

      var status = ps6000aDevice.InitializeChannels(handle, channels, numChannels);
      if (status != StandardDriverStatusCode.Ok) return status;

      status = ps6000aDevice.EnableAWGOutput(handle, WaveType.Sine, true);
      if (status != StandardDriverStatusCode.Ok) return status;

      status = ps6000aDevice.SetTrigger(handle, triggerChannel);
      if (status != StandardDriverStatusCode.Ok) return status;

      status = ps6000aDevice.RunBlock(handle, resolution, numSamples, readChannel, idealTimeInterval);
      if (status != StandardDriverStatusCode.Ok) return status;

      status = ps6000aDevice.WaitForDataToBeCaptured(handle, readChannel);
      if (status != StandardDriverStatusCode.Ok) return status;

      var data = new short[numSamples];
      var pinned = new PinnedArray<short>(data);

        status = ps6000aDevice.ReadDataFromDevice(handle, readChannel, numSamples, ref pinned);
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
      int numChannels;
      var resolution = DeviceResolution.PICO_DR_8BIT;

      var status = ps6000aDevice.OpenUnit(out handle, resolution, out numChannels);
      if (status == StandardDriverStatusCode.Ok)
      {
        status = RunAWGTriggerExample(handle, resolution, numChannels);
      }

      ps6000aDevice.CloseUnit(handle);
      Console.WriteLine("Application closed with status: " + status);
      Console.ReadLine();
    }
  }
}
