using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using PS3000Imports;

namespace PS3000CSStreamingExample
{
  class PS3000CSStreamingExample
  {
    static void Main(string[] args)
    {
      Console.WriteLine("PicoScope 3000 Series (ps3000) Driver C# Console Streaming Example Program");
      
      // Open unit and show splash screen
      Console.WriteLine("\n\nOpening the device...");

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

      Console.WriteLine("Starting Stream");
      ps3000.RunStreaming(10, 1000, 0);
      //ps3000.RunBlock(numberOfSamples, timebase, oversample, out timeIndisposed);
      Thread.Sleep(1000);
      Console.WriteLine("Finished streaming");
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
      /*if ((handle = PS3000.OpenUnit()) <= 0)
      {
        Console.WriteLine("Unable to open device");
        Console.WriteLine("Error code : {0}", handle);
        Console.ReadKey();
      }
      else
      {
        Console.WriteLine("Device opened successfully\n");

        PS3000CSStreamingExample streamingExample = new PS3000CSStreamingExample(handle);
        streamingExample.Run();

        PS3000.CloseUnit(handle);
      }*/
    }
  }
}
