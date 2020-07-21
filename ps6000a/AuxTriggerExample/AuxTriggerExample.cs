// This is an example of how to setup the Aux Trigger for PicoScope 6000 Series PC Oscilloscope consuming the ps6000a driver
// In this example, the AUX Trigger will trigger on a rising edge of a 10KHz 2Vpp signal.

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

      var status = ps6000aDevice.SetTrigger(handle, channel);
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
        status = AuxTrigger(handle, resolution, numChannels);
      }

      ps6000aDevice.CloseUnit(handle);
      Console.WriteLine("Application closed with status: " + status);
      Console.ReadLine();
    }
  }
}
