// <copyright file="PicoPinnedArray.cs" company="Pico Technology Ltd.">
// Copyright (C) 2009 - 2017 Pico Technology Ltd. See LICENSE file for terms.
// </copyright>

namespace PicoPinnedArray
{
    using System;
    using System.Runtime.InteropServices;

    /// <summary>
    /// This class defines an object to hold an array in memory when registering a data buffer with a driver.
    /// </summary>
    /// <typeparam name="T">Type of array</typeparam>
    public sealed class PinnedArray<T> : IDisposable
    {
        private readonly GCHandle pinnedHandle;
        private bool disposed = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="PinnedArray{T}"/> class.
        /// </summary>
        /// <param name="arraySize">Size of the array</param>
        public PinnedArray(int arraySize)
            : this(new T[arraySize])
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PinnedArray{T}"/> class.
        /// </summary>
        /// <param name="array">Existing array to protect from garbage collection</param>
        public PinnedArray(T[] array)
        {
            this.pinnedHandle = GCHandle.Alloc(array, GCHandleType.Pinned);
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="PinnedArray{T}"/> class.
        /// </summary>
        ~PinnedArray() => this.Dispose(false);

        /// <summary>
        /// Gets the pinned array
        /// </summary>
        public T[] Target => (T[])this.pinnedHandle.Target;

        /// <summary>
        /// Extension for direct access to array elements
        /// </summary>
        /// <param name="array">Pinned array to access</param>
        public static implicit operator T[](PinnedArray<T> array) => array?.Target;

        /// <summary>
        /// Implement the IDisposable pattern.
        /// </summary>
        /// <remarks>
        /// No virtual Dispose() necessary as this class is sealed.
        /// </remarks>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (this.disposed)
            {
                return;
            }

            if (disposing)
            {
                // Dispose managed resources (IDisposable objects)
            }

            this.pinnedHandle.Free();
            this.disposed = true;
        }
    }
}