// This is an example of how to use GetTriggerInfo for PicoScope 6000 Series PC Oscilloscope consuming the ps6000a driver
// This program captures 10 waveforms from the siggen, and uses ps6000aGetTriggerInfo to get 
// information about how much time has passed between a trigger and its subsequent trigger hitting. 
// *** This program requires BNC connection between device's signal generator and Channel A. ***

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using DriverImports;

namespace GetTriggerInfoExample
{
  class GetTriggerInfoExample
  {
    static StandardDriverStatusCode PrintTimeElapsedBetweenTriggers(short handle, ulong numWaveforms,
      double actualTimeInterval)
    {
      for (ulong waveformIndex = 0; waveformIndex < numWaveforms - 1; waveformIndex++)
      {
        var nextWaveformIndex = waveformIndex + 1;

        double timeToTriggerInMilliseconds = 0.0;
        var status = ps6000aDevice.GetTimeToTriggerForWaveformIndex(
          handle, actualTimeInterval, waveformIndex, out timeToTriggerInMilliseconds);
        if (status != StandardDriverStatusCode.Ok) return status;

        double nextTimeToTriggerInMilliseconds = 0.0;
        status = ps6000aDevice.GetTimeToTriggerForWaveformIndex(
          handle, actualTimeInterval, nextWaveformIndex, out nextTimeToTriggerInMilliseconds);
        if (status != StandardDriverStatusCode.Ok) return status;

        // For each of the trigger times, we get the time elapsed between the current trigger and the trigger that follows it. 
        // We expect time periods of approximately 100ms output to the console,
        // as we set our waveform generator output to be 10Hz.
        var deltaTimeBetweenTriggers = nextTimeToTriggerInMilliseconds - timeToTriggerInMilliseconds;

        var message =
          "Time between trigger #" +
          (waveformIndex + 1) +
          " and #" +
          (nextWaveformIndex + 1) +
          ": " +
          deltaTimeBetweenTriggers.ToString("n2") +
          "ms";

        Console.WriteLine(message);
      }

      return StandardDriverStatusCode.Ok;
    }

    static StandardDriverStatusCode RunGetTriggerInfoExample(short handle, DeviceResolution resolution, int numChannels)
    {
      var channel = Channel.ChannelA;
      ulong numSamples = 1000;
      double idealTimeInterval = 1.0e-7; // 100ns
      ulong numWaveforms = 10;

      var status = ps6000aDevice.InitializeChannels(handle, new List<Channel>() { channel }, numChannels);
      if (status != StandardDriverStatusCode.Ok) return status;

      status = ps6000aDevice.SetUpWithNoOfWaveformsToCapture(handle, ref numSamples, numWaveforms);
      if (status != StandardDriverStatusCode.Ok) return status;

      double actualTimeInterval = 0.0;
      uint timebase = 0;
      status = DriverImports.PS6000a.NearestSampleIntervalStateless(handle, EnabledChannelsAndPorts.ChannelA,
        idealTimeInterval, resolution, out timebase, out actualTimeInterval);
      if (status != StandardDriverStatusCode.Ok) return status;

      // We set up our signal generator to output a 10Hz Sine wave. 
      status = ps6000aDevice.EnableAWGOutput(handle, WaveType.Sine, frequency: 10);
      if (status != StandardDriverStatusCode.Ok) return status;

      status = ps6000aDevice.SetTrigger(handle, channel);
      if (status != StandardDriverStatusCode.Ok) return status;

      status = ps6000aDevice.RunBlock(handle, resolution, numSamples, channel, idealTimeInterval);
      if (status != StandardDriverStatusCode.Ok) return status;

      status = ps6000aDevice.WaitForDataToBeCaptured(handle, channel);
      if (status != StandardDriverStatusCode.Ok) return status;

      status = PrintTimeElapsedBetweenTriggers(handle, numWaveforms, actualTimeInterval);
      if (status != StandardDriverStatusCode.Ok) return status;

      Console.WriteLine();

      return StandardDriverStatusCode.Ok;
    }

    static void Main(string[] args)
    {
      short handle = 0;
      var resolution = DeviceResolution.PICO_DR_8BIT;
      int numChannels;

      var status = ps6000aDevice.OpenUnit(out handle, resolution, out numChannels);
      if (status == StandardDriverStatusCode.Ok)
      {
        status = RunGetTriggerInfoExample(handle, resolution, numChannels);
      }

      ps6000aDevice.CloseUnit(handle);
      Console.WriteLine("Application closed with status: " + status);
      Console.ReadLine();
    }
  }
}
