#if !COMP_NETSTANDARD1_1
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers.Resources;
using System.Diagnostics;

namespace System.Buffers
{
    internal sealed partial class DefaultArrayPool<T> : ArrayPool<T>
    {
        /// <summary>
        /// Provides a thread-safe bucket containing buffers that can be Rent'd and Return'd.
        /// </summary>
        private sealed class Bucket
        {
            internal readonly int _bufferLength;
            private readonly T[][] _buffers;
            private readonly int _poolId;
            private readonly object _lock = new();
            private int _index;

            /// <summary>
            /// Gets an ID for the bucket to use with events.
            /// </summary>
            internal int Id => GetHashCode();


            /// <summary>
            /// Creates the pool with numberOfBuffers arrays where each buffer is of bufferLength length.
            /// </summary>
            internal Bucket(int bufferLength, int numberOfBuffers, int poolId)
            {
                _buffers = new T[numberOfBuffers][];
                _bufferLength = bufferLength;
                _poolId = poolId;
            }


            /// <summary>
            /// Takes an array from the bucket. If the bucket is empty, returns null.
            /// </summary>
            internal T[]? Rent()
            {
                T[]?[] buffers = _buffers;
                T[]? buffer = null;

                bool allocateBuffer = false;

                if (Debugger.IsAttached)
                {
                    lock (_lock)
                    {
                        if (_index < buffers.Length)
                        {
                            buffer = buffers[_index];
                            buffers[_index++] = null;
                            allocateBuffer = buffer == null;
                        }
                    }
                }
                else
                {
                    if (_index < buffers.Length)
                    {
                        buffer = buffers[_index];
                        buffers[_index++] = null;
                        allocateBuffer = buffer == null;
                    }
                }

                // While we were holding the lock, we grabbed whatever was at the next available index, if
                // there was one. If we tried and if we got back null, that means we hadn't yet allocated
                // for that slot, in which case we should do so now.
                if (allocateBuffer)
                {
                    buffer = new T[_bufferLength];
                }

                return buffer;
            }

            /// <summary>
            /// Attempts to return the buffer to the bucket. If successful, the buffer will be stored
            /// in the bucket and true will be returned; otherwise, the buffer won't be stored, and false
            /// will be returned.
            /// </summary>
            internal void Return(T[] array)
            {
                // Check to see if the buffer is the correct size for this bucket
                if (array.Length != _bufferLength)
                {
                    throw new ArgumentException(Strings.ArgumentException_BufferNotFromPool, nameof(array));
                }

                if (Debugger.IsAttached)
                {
                    lock (_lock)
                    {
                        if (_index != 0)
                        {
                            _buffers[--_index] = array;
                        }
                    }
                }
                else
                {
                    if (_index != 0)
                    {
                        _buffers[--_index] = array;
                    }
                }
            }
        }
    }
}
#endif