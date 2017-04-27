/**************************************************************************
 *
 * Filename: USBPT104PinnedArray.cs
 * 
 * Description:
 *  This file defines an object to hold an array in memory when 
 *   registering a data buffer with the usbpt104 driver.
 *
 * Copyright (C) 2015 - 2017 Pico Technology Ltd. See LICENSE file for terms. 
 * 
 **************************************************************************/

using System;
using System.Runtime.InteropServices;

namespace USBPT104CSConsole
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