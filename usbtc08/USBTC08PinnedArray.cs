/**************************************************************************
 *
 * Filename: USBTC08PinnedArray.cs
 * 
 * Description:
 *  This file defines an object to hold an array in memory when 
 *   registering a data buffer with the usbtc08 driver.
 *
 * Copyright (C) 2011 - 2017 Pico Technology Ltd. See LICENSE file for terms. 
 * 
 **************************************************************************/

using System;
using System.Runtime.InteropServices;

namespace USBTC08PinnedArray
{
  public class PinnedArray<T>
  {
    GCHandle _pinnedHandle;
    private bool _disposed;

    public PinnedArray(int arraySize) : this(new T[arraySize]) { }

    public PinnedArray(T[] array)
    {
      _pinnedHandle = GCHandle.Alloc(array, GCHandleType.Pinned);
    }

    ~PinnedArray()
    {
      Dispose();
    }

    public T[] Target
    {
      get { return (T[])_pinnedHandle.Target; }
    }

    public static implicit operator T[](PinnedArray<T> a)
    {
      if (a == null)
        return null;
      else
        return (T[])a._pinnedHandle.Target;
    }

    public void Dispose()
    {
      if (!_disposed)
      {
        _pinnedHandle.Free();
        _disposed = true;

        GC.SuppressFinalize(this);
      }
    }
  }
}