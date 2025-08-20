// This is an example of how to use GetTriggerInfo for PicoScope PC Oscilloscope consuming the psospa driver
// This program captures twice on one waveform, one capture using a pulse width 
// trigger expecting a waveform with a pulse width greater than the trigger, and 
// the other expecting a waveform with a pulse width less than the trigger.
// *** This program requires BNC connection between device's signal generator and Channel A. ***
// Copyright © 2024 Pico Technology Ltd. See LICENSE file for terms.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using DriverImports;
using PicoPinnedArray;

namespace PulseWidthTriggerExample
{
  class PulseWidthTriggerExample
  {
    static StandardDriverStatusCode RunPulseWidthTriggerExample(
      short handle, DeviceResolution resolution, int numChannels,
      double awgFrequency, double pulseWidthFrequency, PulseWidthType pulseWidthType, string filename)
    {
      double cycleTime = 1.0 / pulseWidthFrequency;

      // The minimum time in seconds required for the pulse width trigger to hit.
      double pulseWidthPeriodSeconds = 0.5 * cycleTime;

      Console.WriteLine($"Running a pulse width qualifier test with a period of {(float)(1000000.0 * pulseWidthPeriodSeconds) }us.");

      switch (pulseWidthType)
      {
        case PulseWidthType.Greater_Than:
          Console.WriteLine("The AWG frequency is less than the pulse width period.");
          break;
        case PulseWidthType.Less_Than:
          Console.WriteLine("The AWG frequency is greater than the pulse width period.");
          break;
      }

      Console.WriteLine();

      var channel = Channel.ChannelA;
      ulong numSamples = 1000;
      double idealTimeInterval = 0.0000001; //100ns

      var status = psospaDevice.EnableAWGOutput(handle, WaveType.Square, frequency: awgFrequency);

      if (status != StandardDriverStatusCode.Ok)
        return status;

      status = psospaDevice.InitializeChannels(handle, new List<Channel>() {channel}, numChannels);

      if (status != StandardDriverStatusCode.Ok)
        return status;

      double actualTimeInterval = 0.0;
      uint timebase = 0;
      byte roundFaster = 0;//if the exact sample interval requested is not available, the function should return the next faster or next slower interval available?

            status = DriverImports.psospa.NearestSampleIntervalStateless(handle, EnabledChannelsAndPorts.ChannelA,
        idealTimeInterval, roundFaster, resolution, out timebase, out actualTimeInterval);

      if (status != StandardDriverStatusCode.Ok)
        return status;

      double timeIntervalNanoseconds = 0;
      ulong maxSamples = 0;
      status = DriverImports.psospa.GetTimebase(handle, timebase, 1, out timeIntervalNanoseconds, out maxSamples, 0);

      if (status != StandardDriverStatusCode.Ok)
        return status;

      var pulseWidthSampleCount = (uint) Math.Floor(pulseWidthPeriodSeconds / actualTimeInterval);


      status = psospaDevice.SetTriggerAndPulseWidth(handle, channel, pulseWidthSampleCount, pulseWidthType);

      if (status != StandardDriverStatusCode.Ok)
        return status;

      psospaDevice.RunBlock(handle, resolution, numSamples, channel, idealTimeInterval);

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
      int numChannels;
      var resolution = DeviceResolution.PICO_DR_8BIT;

      var status = psospaDevice.OpenUnit(out handle, resolution, out numChannels);

      // We set up our siggen to output at 10kHz for both captures.
      double awgFrequency = 10000.0;

      Console.WriteLine();

      if (status == StandardDriverStatusCode.Ok)
      {
        // For the first one of these captures, we set up a pulse width frequency of 10.5kHz, which applies to an actual pulse width of about 47.6us.
        // We set up our AWG at a frequency of 10kHz, which applies to a pulse width of 50us. Since this is larger, we set up our pulse width trigger
        // to expect a waveform with a pulse width greater than what we set up for the trigger.
        status = RunPulseWidthTriggerExample(handle, resolution, numChannels, awgFrequency, 10500.0, PulseWidthType.Greater_Than, "Output_A.csv");
        Console.WriteLine();
      }

      if (status == StandardDriverStatusCode.Ok)
      {
        // For the second one of these captures, we set up a pulse width frequency of 9.5kHz, which applies to an actual pulse width of about 52.6us.
        // We set up our AWG at a frequency of 10kHz, which applies to a pulse width of 50us. Since this is smaller, we set up our pulse width trigger
        // to expect a waveform with a pulse width less than what we set up for the trigger.
        status = RunPulseWidthTriggerExample(handle, resolution, numChannels, awgFrequency, 9500.0, PulseWidthType.Less_Than, "Output_B.csv");
        Console.WriteLine();
      }

      psospaDevice.CloseUnit(handle);
      Console.WriteLine("Application closed with status: " + status);
      Console.ReadLine();
    }
  }
}
