// This is an example of how to setup the Aux Auto Trigger for PicoScope 6000 Series PC Oscilloscope consuming the ps6000a driver
// In this example, the AUX Trigger will trigger on a rising edge of a 10KHz 2Vpp signal or after 5s.
// Copyright © 2020-2024 Pico Technology Ltd. See LICENSE file for terms.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DriverImports;

namespace AuxAutoTriggerExample
{
  class AuxAutoTriggerExample
  {
    /// <summary>
    /// This function sets up and waits for an Aux trigger or 5s.
    /// </summary>
    static StandardDriverStatusCode AuxTriggerDelay(short handle, DeviceResolution resolution, int numChannels)
    {
      var channel = Channel.Aux;
      ulong numSamples = 1000;
      int autoTriggerDelay = 5000000; //5s in μs
      double idealTimeInterval = 0.0000001; //100ns

      var status = ps6000aDevice.SetTrigger(handle, channel, autoTriggerDelay);
      if (status != StandardDriverStatusCode.Ok) return status;

      status = ps6000aDevice.RunBlock(handle, resolution, numSamples, Channel.ChannelA, idealTimeInterval);
      if (status != StandardDriverStatusCode.Ok) return status;

      status = ps6000aDevice.WaitForDataToBeCaptured(handle, channel);
      if (status != StandardDriverStatusCode.Ok) return status;

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
        status = AuxTriggerDelay(handle, resolution, numChannels);
      }

      ps6000aDevice.CloseUnit(handle);
      Console.WriteLine("Application closed with status: " + status);
      Console.ReadLine();
    }
  }
}
