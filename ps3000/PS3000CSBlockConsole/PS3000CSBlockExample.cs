using System;
using PS3000Imports;

namespace PS3000CSBlockExample
{
  class PS3000BlockExample
  {
    static void Main(string[] args)
    {
      var ps3000 = new PS3000();
      ps3000.Open();
      Console.WriteLine(ps3000.Serial);

      ps3000.SetEts(PS3000.EtsMode.Off, 0, 0);
      ps3000.SetChannel(PS3000.Channel.ChannelA, true, PS3000.Coupling.AC, PS3000.Range.Range2V);
      ps3000.SetChannel(PS3000.Channel.ChannelB, false, PS3000.Coupling.AC, PS3000.Range.Range20V);
      ps3000.SetChannel(PS3000.Channel.ChannelC, false, PS3000.Coupling.AC, PS3000.Range.Range20V);
      ps3000.SetChannel(PS3000.Channel.ChannelD, false, PS3000.Coupling.AC, PS3000.Range.Range20V);
      ps3000.SetTrigger(PS3000.Channel.None, 0, PS3000.TriggerThresholdDirection.Falling, 0, 0);

      const int numberOfSamples = 10000;

      int nsInterval;
      PS3000.TimeUnits timeUnits;
      const short oversample = 0;
      int maxSamples;
      short timebase = 0;
      while (!ps3000.GetTimebase(timebase, numberOfSamples, out nsInterval, out timeUnits, oversample, out maxSamples))
        timebase++;
      int timeIndisposed;
      Console.WriteLine("Starting Block");
      ps3000.RunBlock(numberOfSamples, timebase, oversample, out timeIndisposed);
      while (!ps3000.Ready())
      {
      }

      ps3000.Stop();
      Console.WriteLine("Finished Block");
      var bufferA = new short[maxSamples];
      var bufferB = new short[maxSamples];
      var bufferC = new short[maxSamples];
      var bufferD = new short[maxSamples];
      short overflows;
      var samples = ps3000.GetValues(bufferA, bufferB, bufferC, bufferD, out overflows, numberOfSamples);

      ps3000.Close();

      for (var i = 0; i < samples; i++)
        Console.WriteLine($"{bufferA[i]}\t {bufferB[i]}\t {bufferC[i]}\t {bufferD[i]}");

      var resolution = Math.Log(oversample) / Math.Log(4);
      Console.WriteLine($"nsInterval {nsInterval}\t numberOfSamplesRequested {numberOfSamples}\t numberOfSamplesReceived {samples}\t timebase {timebase}\t maxSamples {maxSamples}");
      Console.WriteLine(resolution);
      Console.ReadKey();
    }
  }
}
