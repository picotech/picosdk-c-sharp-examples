using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DriverImports;

namespace MSOConnectDisconnectExample
{
  class DigitalPortCallbackExample : IDisposable
  {
    private DigitalPortCallback _callback; //Strong reference so the callback isn't garbage collected.
    private readonly ManualResetEvent _callbackEvent = new ManualResetEvent(false);
    private StandardDriverStatusCode _callbackStatus = StandardDriverStatusCode.Ok;

    /// <summary>
    /// Callback event for when a device has been connected or disconnected to a digital port.
    /// </summary>
    private void DigitalPortCallback(short handle, StandardDriverStatusCode status, IntPtr ports, uint nPorts)
    {
      IntPtr ptr = ports;
      _callbackStatus = status;

      List<DigitalPortInteractions> portInteractions = new List<DigitalPortInteractions>();

      for (int port = 0; port < nPorts; port++)
      {
        var portInteraction = (DigitalPortInteractions)Marshal.PtrToStructure(ptr, typeof(DigitalPortInteractions));
        portInteractions.Add(portInteraction);
        ptr += Marshal.SizeOf(typeof(DigitalPortInteractions));
      }

      foreach (var portInteraction in portInteractions)
      {
        string connected = (portInteraction.Connected > 0) ? "connected" : "disconnected";
        Console.WriteLine("Port: " + portInteraction.Channel + " has " + connected + ": " + portInteraction.DigitalPort);
      }
    }

    /// <summary>
    /// This function initialize the digital port callback and reports any disconnections and connections on the digital port.
    /// </summary>
    public StandardDriverStatusCode RunDigitalPortCallbackExample(short handle)
    {
      _callback = DigitalPortCallback;
      var status = DriverImports.PS6000a.SetDigitalPortInteractionCallback(handle, _callback);
      if (status != StandardDriverStatusCode.Ok) return status;

      bool exit = false;
      Console.WriteLine("PRESS ANY KEY TO EXIT LOOP.");
      Thread cancelWaitForCapture = new Thread(() => ps6000aDevice.CancelDataCapture(ref exit));
      cancelWaitForCapture.Start();

      while (_callbackStatus == StandardDriverStatusCode.Ok && !exit)
      {
        Thread.Sleep(100);
      }

      return _callbackStatus;
    }

    public void Dispose()
    {
      _callbackEvent.Dispose();
    }
  }

  class MSOConnectDisconnectExample
  {
    static void Main(string[] args)
    {
      short handle = 0;
      int numChannels;
      var resolution = DeviceResolution.PICO_DR_8BIT;

      var status = ps6000aDevice.OpenUnit(out handle, resolution, out numChannels);
      if (status == StandardDriverStatusCode.Ok)
      {
        using (DigitalPortCallbackExample bmCallback = new DigitalPortCallbackExample())
        {
          status = bmCallback.RunDigitalPortCallbackExample(handle);
        }
      }

      ps6000aDevice.CloseUnit(handle);
      Console.WriteLine("Application closed with status: " + status);
      Console.ReadLine();
    }
  }
}
