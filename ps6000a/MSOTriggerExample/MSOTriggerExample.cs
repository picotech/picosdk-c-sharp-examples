// This is an example of how to capture MSO data from Port0 using the latest ps6000a Driver.
// A trigger is setup to trigger on Port 0 Channel 5

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using DriverImports;

namespace MSOTriggerExample
{
  class MSOTriggerExample
  {
    /// <summary>
    /// Captures data on MSO Port0 and exits once data has been fully captured.
    /// </summary>
    static StandardDriverStatusCode RunMSOTriggerExample(short handle, DeviceResolution resolution, int numChannels)
    {
      var channel = Channel.Port0;
      ulong numSamples = 1000;
      double idealTimeInterval = 0.00001; //10us

      //Disable all analogue channels
      //Note: Disabling all analogue channels will result in only the PICO_DR_8BIT resolution being available for captures.
      var status = ps6000aDevice.InitializeChannels(handle, new List<Channel>(), numChannels);
      if (status != StandardDriverStatusCode.Ok) return status;

      status = ps6000aDevice.InitializeDigitalChannels(handle, channel);
      if (status != StandardDriverStatusCode.Ok) return status;

      status = ps6000aDevice.SetDigitalTrigger(handle, channel);
      if (status != StandardDriverStatusCode.Ok) return status;

      status = ps6000aDevice.RunBlock(handle, resolution, numSamples, channel, idealTimeInterval);
      if (status != StandardDriverStatusCode.Ok) return status;

      status = ps6000aDevice.WaitForDataToBeCaptured(handle, channel);
      if (status != StandardDriverStatusCode.Ok) return status;

      var data = new short[numSamples];
      GCHandle gch = GCHandle.Alloc(data);

      try
      {
        status = ps6000aDevice.ReadDataFromDevice(handle, channel, numSamples, ref data);
        if (status != StandardDriverStatusCode.Ok)
          return status;

        ps6000aDevice.WriteDigitalDataToFile(data);
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
      int numChannels;
      var resolution = DeviceResolution.PICO_DR_8BIT;

      var status = ps6000aDevice.OpenUnit(out handle, resolution, out numChannels);
      if (status == StandardDriverStatusCode.Ok)
      {
        status = RunMSOTriggerExample(handle, resolution, numChannels);
      }

      ps6000aDevice.CloseUnit(handle);
      Console.WriteLine("Application closed with status: " + status);
      Console.ReadLine();
    }
  }
}
