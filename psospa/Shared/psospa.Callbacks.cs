// Copyright © 2024 Pico Technology Ltd. See LICENSE file for terms.

using System;

namespace DriverImports
{
  /// <summary>
  /// Callback function registered using RunBlock and the driver calls it back when block mode data is ready.
  /// </summary>
  /// <param name="pVoid">a void pointer passed from RunBlock. Your callback function can write to this location to send any data, such as a status flag, back to your application.</param>
  public delegate void DefinitionBlockReady(short handle, StandardDriverStatusCode status, IntPtr pVoid);

  /// <summary>
  /// Callback function registered using SetDigitalPortInteractionCallback and the driver calls it back when a digital port has been connected or disconnected.
  /// </summary>
  public delegate void DigitalPortCallback(short handle, StandardDriverStatusCode status, IntPtr ports, uint nPorts);
}