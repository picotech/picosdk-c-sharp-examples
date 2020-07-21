// This is an example of how to use TriggerWithinPreTrigger for PicoScope 6000 Series PC Oscilloscope consuming the ps6000a driver
// This program outputs a pulse from the siggen, which is then captured on channel A and stored using this pre-trigger functionality. 
// *** This program requires BNC connection between device's signal generator and Channel A. ***

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

namespace TriggerWithinPreTriggerExample
{
  class TriggerWithinPreTriggerExample
  {
    static StandardDriverStatusCode RunTriggerWithinPreTriggerExample(short handle, DeviceResolution resolution, int numChannels)
    {
      var channel = Channel.ChannelA;
      ulong numSamples = 2000000; // 1 million pre-trigger samples (and 1 million post-trigger samples)
      double idealTimeInterval = 1e-7; // 100ns

      var status = ps6000aDevice.InitializeChannels(handle, new List<Channel>() { channel }, numChannels);
      if (status != StandardDriverStatusCode.Ok)
        return status;

      status = ps6000aDevice.EnableAWGOutput(handle, WaveType.Sine, softwareTrigger: true);
      if (status != StandardDriverStatusCode.Ok)
        return status;

      status = DriverImports.PS6000a.TriggerWithinPreTriggerSamples(handle, TriggerWithinPreTrigger.Arm);
      if (status != StandardDriverStatusCode.Ok)
        return status;

      status = ps6000aDevice.SetTrigger(handle, channel);
      if (status != StandardDriverStatusCode.Ok)
        return status;

      status = ps6000aDevice.RunBlock(handle, resolution, numSamples, channel, idealTimeInterval, null);
      if (status != StandardDriverStatusCode.Ok)
        return status;

      // Wait three milliseconds for AWG
      // (This is due to the device having a post sample latency after these previous API calls.
      // This latency can be reduced by enabling extra channels and/or increasing bit resolution.)
      Thread.Sleep(3);

      // The rising trigger we're passing in for this api function is dummy data
      status = DriverImports.PS6000a.SigGenSoftwareTriggerControl(handle, SiggenTrigType.Rising);
      if (status != StandardDriverStatusCode.Ok)
        return status;

      status = ps6000aDevice.WaitForDataToBeCaptured(handle, channel);
      if (status != StandardDriverStatusCode.Ok)
        return status;

      var data = new short[numSamples];
      GCHandle gch = GCHandle.Alloc(data);

      try
      {
        status = ps6000aDevice.ReadDataFromDevice(handle, channel, numSamples, ref data);
        if (status != StandardDriverStatusCode.Ok)
          return status;

        ps6000aDevice.WriteDataToFile(data);
      }
      finally
      {
        gch.Free();
      }

      return status;
    }

    static void Main(string[] args)
    {
      short handle = 0;
      var resolution = DeviceResolution.PICO_DR_8BIT;
      int numChannels;

      var status = ps6000aDevice.OpenUnit(out handle, resolution, out numChannels);
      if (status == StandardDriverStatusCode.Ok)
      {
        status = RunTriggerWithinPreTriggerExample(handle, resolution, numChannels);
      }

      ps6000aDevice.CloseUnit(handle);
      Console.WriteLine("Application closed with status: " + status);
      Console.ReadLine();
    }
  }
}
