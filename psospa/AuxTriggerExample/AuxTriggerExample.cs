// This is an example of how to setup the Aux Trigger for PicoScope PC Oscilloscope consuming the psospa driver
// In this example, the AUX Trigger will trigger on a rising edge of a 10KHz 2Vpp signal.
// Copyright © 2024 Pico Technology Ltd. See LICENSE file for terms.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DriverImports;

namespace ExternalTriggerExample
{
  class AuxTriggerExample
  {
    /// <summary>
    /// This function sets up and waits for an Aux trigger.
    /// </summary>
    static StandardDriverStatusCode AuxTrigger(short handle, DeviceResolution resolution, int numChannels)
    {
      var channel = Channel.Aux;
      ulong numSamples = 1000;
      double idealTimeInterval = 0.0000001; //100ns

      var status = psospaDevice.SetTrigger(handle, channel);
      if (status != StandardDriverStatusCode.Ok) return status;

      status = psospaDevice.RunBlock(handle, resolution, numSamples, Channel.ChannelA, idealTimeInterval);
      if (status != StandardDriverStatusCode.Ok) return status;

      status = psospaDevice.WaitForDataToBeCaptured(handle, channel);
      if (status != StandardDriverStatusCode.Ok) return status;

      return status;
    }

    static void Main(string[] args)
    {
      short handle = 0;
      int numChannels;
      var resolution = DeviceResolution.PICO_DR_8BIT;

      var status = psospaDevice.OpenUnit(out handle, resolution, out numChannels);
      if (status == StandardDriverStatusCode.Ok)
      {
        status = AuxTrigger(handle, resolution, numChannels);
      }

      psospaDevice.CloseUnit(handle);
      Console.WriteLine("Application closed with status: " + status);
      Console.ReadLine();
    }
  }
}
