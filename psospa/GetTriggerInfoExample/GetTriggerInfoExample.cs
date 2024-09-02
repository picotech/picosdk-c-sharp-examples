// This is an example of how to use GetTriggerInfo for PicoScope PC Oscilloscope consuming the psospa driver
// This program captures 10 waveforms from the siggen, and uses ps6000aGetTriggerInfo to get 
// information about how much time has passed between a trigger and its subsequent trigger hitting. 
// *** This program requires BNC connection between device's signal generator and Channel A. ***
// This is an example shows how Digital MSO pods are used to report there connection and dissconnection.

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
        var status = psospaDevice.GetTimeToTriggerForWaveformIndex(
          handle, actualTimeInterval, waveformIndex, out timeToTriggerInMilliseconds);
        if (status != StandardDriverStatusCode.Ok) return status;

        double nextTimeToTriggerInMilliseconds = 0.0;
        status = psospaDevice.GetTimeToTriggerForWaveformIndex(
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

      var status = psospaDevice.InitializeChannels(handle, new List<Channel>() { channel }, numChannels);
      if (status != StandardDriverStatusCode.Ok) return status;

      status = psospaDevice.SetUpWithNoOfWaveformsToCapture(handle, ref numSamples, numWaveforms);
      if (status != StandardDriverStatusCode.Ok) return status;

      double actualTimeInterval = 0.0;
      uint timebase = 0;
      byte roundFaster = 0;//if the exact sample interval requested is not available, the function should return the next faster or next slower interval available?

            status = DriverImports.psospa.NearestSampleIntervalStateless(handle, EnabledChannelsAndPorts.ChannelA,
        idealTimeInterval, roundFaster, resolution, out timebase, out actualTimeInterval);
      if (status != StandardDriverStatusCode.Ok) return status;

      // We set up our signal generator to output a 10Hz Sine wave. 
      status = psospaDevice.EnableAWGOutput(handle, WaveType.Sine, frequency: 10);
      if (status != StandardDriverStatusCode.Ok) return status;

      status = psospaDevice.SetTrigger(handle, channel);
      if (status != StandardDriverStatusCode.Ok) return status;

      status = psospaDevice.RunBlock(handle, resolution, numSamples, channel, idealTimeInterval);
      if (status != StandardDriverStatusCode.Ok) return status;

      status = psospaDevice.WaitForDataToBeCaptured(handle, channel);
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

      var status = psospaDevice.OpenUnit(out handle, resolution, out numChannels);
      if (status == StandardDriverStatusCode.Ok)
      {
        status = RunGetTriggerInfoExample(handle, resolution, numChannels);
      }

      psospaDevice.CloseUnit(handle);
      Console.WriteLine("Application closed with status: " + status);
      Console.ReadLine();
    }
  }
}
