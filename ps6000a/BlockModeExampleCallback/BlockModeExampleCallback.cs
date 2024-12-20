﻿// This is an example of how to setup and capture in block mode with a callback for PicoScope 6000 Series PC Oscilloscope consuming the ps6000a driver
// Copyright © 2020-2024 Pico Technology Ltd. See LICENSE file for terms.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DriverImports;
using PicoPinnedArray;

namespace BlockModeExampleCallback
{
  class BlockModeCallback : IDisposable
  {
    private DefinitionBlockReady _callback; //Strong reference so the callback isn't garbage collected.
    private readonly ManualResetEvent _callbackEvent = new ManualResetEvent(false);
    private StandardDriverStatusCode _callbackStatus = StandardDriverStatusCode.Ok;

    /// <summary>
    /// Callback event for when data has been fully captured.
    /// </summary>
    private void BlockReadyCallback(short handle, StandardDriverStatusCode status, IntPtr pVoid)
    {
      _callbackStatus = status;
      _callbackEvent.Set();
    }

    /// <summary>
    /// Calls Runblock and exits when data has been fully captured.
    /// </summary>
    public StandardDriverStatusCode WaitForDataToBeCaptured(short handle, Channel channel)
    {
      //Wait for the data to be fully captured and ready to read.
      _callbackEvent.WaitOne();
      var status = _callbackStatus;

      if (status == StandardDriverStatusCode.Ok)
        Console.WriteLine("Data captured on " + channel);

      return status;
    }

    /// <summary>
    /// Captures 1000 samples on Channel A and the data is written to Output.csv
    /// </summary>
    public StandardDriverStatusCode RunBlockModeCallbackExample(short handle, DeviceResolution resolution, int numChannels)
    {
      var channel = Channel.ChannelA;
      ulong numSamples = 1000;
      double idealTimeInterval = 0.0000001; //100ns

      var status = ps6000aDevice.InitializeChannels(handle, new List<Channel>() { channel }, numChannels);
      if (status != StandardDriverStatusCode.Ok) return status;

      _callback = BlockReadyCallback;
      status = ps6000aDevice.RunBlock(handle, resolution, numSamples, channel, idealTimeInterval, _callback);
      if (status != StandardDriverStatusCode.Ok) return status;

      status = WaitForDataToBeCaptured(handle, channel);
      if (status != StandardDriverStatusCode.Ok) return status;

      var data = new short[numSamples];
      var pinned = new PinnedArray<short>(data);

      
      
        status = ps6000aDevice.ReadDataFromDevice(handle, channel, numSamples, ref data);
        if (status != StandardDriverStatusCode.Ok)
          return status;

            PicoFileFunctions.WriteDataToFile(data);
     

      return status;
    }

    public void Dispose()
    {
      _callbackEvent.Dispose();
    }
  }

  class BlockModeExampleCallback
  {
    static void Main(string[] args)
    {
      short handle = 0;
      int numChannels;
      var resolution = DeviceResolution.PICO_DR_8BIT;

      var status = ps6000aDevice.OpenUnit(out handle, resolution, out numChannels);
      if (status == StandardDriverStatusCode.Ok)
      {
        using (BlockModeCallback bmCallback = new BlockModeCallback())
        {
          status = bmCallback.RunBlockModeCallbackExample(handle, resolution, numChannels);
        }
      }

      ps6000aDevice.CloseUnit(handle);
      Console.WriteLine("Application closed with status: " + status);
      Console.ReadLine();
    }
  }
}
