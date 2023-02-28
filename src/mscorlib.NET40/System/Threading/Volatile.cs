// ==++==
// 
//   Copyright (c) Microsoft Corporation.  All rights reserved.
// 
// ==--==
//
// <OWNER>Microsoft</OWNER>
namespace System.Threading
{
    /// <summary>
	/// Contains methods for performing volatile memory operations.
	/// </summary>
	public static class Volatile
    {
        /// <summary>
        /// Reads the value of the specified field. On systems that require it, inserts a memory barrier
        /// that prevents the processor from reordering memory operations as follows: If a read or write
        /// appears after this method in the code, the processor cannot move it before this method.
        /// </summary>
        /// <param name="location">The field to read.</param>
        /// <returns>The value that was read. This value is the latest written by any processor in
        /// the computer, regardless of the number of processors or the state of processor cache.</returns>
        public static bool Read(ref bool location)
        {
            bool result = location;
            Thread.MemoryBarrier();

            return result;
        }

        /// <summary>
        /// Reads the value of the specified field. On systems that require it, inserts a memory barrier
        /// that prevents the processor from reordering memory operations as follows: If a read or write
        /// appears after this method in the code, the processor cannot move it before this method.
        /// </summary>
        /// <param name="location">The field to read.</param>
        /// <returns>The value that was read. This value is the latest written by any processor in
        /// the computer, regardless of the number of processors or the state of processor cache.</returns>
        public static byte Read(ref byte location)
        {
            byte result = location;
            Thread.MemoryBarrier();

            return result;
        }

        /// <summary>
        /// Reads the value of the specified field. On systems that require it, inserts a memory barrier
        /// that prevents the processor from reordering memory operations as follows: If a read or write
        /// appears after this method in the code, the processor cannot move it before this method.
        /// </summary>
        /// <param name="location">The field to read.</param>
        /// <returns>The value that was read. This value is the latest written by any processor in
        /// the computer, regardless of the number of processors or the state of processor cache.</returns>
        public static double Read(ref double location)
        {
            return Interlocked.CompareExchange(ref location, 0.0, 0.0);
        }

        /// <summary>
        /// Reads the value of the specified field. On systems that require it, inserts a memory barrier
        /// that prevents the processor from reordering memory operations as follows: If a read or write
        /// appears after this method in the code, the processor cannot move it before this method.
        /// </summary>
        /// <param name="location">The field to read.</param>
        /// <returns>The value that was read. This value is the latest written by any processor in
        /// the computer, regardless of the number of processors or the state of processor cache.</returns>
        public static short Read(ref short location)
        {
            short result = location;
            Thread.MemoryBarrier();

            return result;
        }

        /// <summary>
        /// Reads the value of the specified field. On systems that require it, inserts a memory barrier
        /// that prevents the processor from reordering memory operations as follows: If a read or write
        /// appears after this method in the code, the processor cannot move it before this method.
        /// </summary>
        /// <param name="location">The field to read.</param>
        /// <returns>The value that was read. This value is the latest written by any processor in
        /// the computer, regardless of the number of processors or the state of processor cache.</returns>
        public static int Read(ref int location)
        {
            int result = location;
            Thread.MemoryBarrier();

            return result;
        }

        /// <summary>
        /// Reads the value of the specified field. On systems that require it, inserts a memory barrier
        /// that prevents the processor from reordering memory operations as follows: If a read or write
        /// appears after this method in the code, the processor cannot move it before this method.
        /// </summary>
        /// <param name="location">The field to read.</param>
        /// <returns>The value that was read. This value is the latest written by any processor in
        /// the computer, regardless of the number of processors or the state of processor cache.</returns>
        public static long Read(ref long location)
        {
            return Interlocked.CompareExchange(ref location, 0L, 0L);
        }

        /// <summary>
        /// Reads the value of the specified field. On systems that require it, inserts a memory barrier
        /// that prevents the processor from reordering memory operations as follows: If a read or write
        /// appears after this method in the code, the processor cannot move it before this method.
        /// </summary>
        /// <param name="location">The field to read.</param>
        /// <returns>The value that was read. This value is the latest written by any processor in
        /// the computer, regardless of the number of processors or the state of processor cache.</returns>
        public static IntPtr Read(ref IntPtr location)
        {
            IntPtr result = location;
            Thread.MemoryBarrier();

            return result;
        }

        /// <summary>
        /// Reads the value of the specified field. On systems that require it, inserts a memory barrier
        /// that prevents the processor from reordering memory operations as follows: If a read or write
        /// appears after this method in the code, the processor cannot move it before this method.
        /// </summary>
        /// <param name="location">The field to read.</param>
        /// <returns>The value that was read. This value is the latest written by any processor in
        /// the computer, regardless of the number of processors or the state of processor cache.</returns>
        public static sbyte Read(ref sbyte location)
        {
            sbyte result = location;
            Thread.MemoryBarrier();

            return result;
        }

        /// <summary>
        /// Reads the value of the specified field. On systems that require it, inserts a memory barrier
        /// that prevents the processor from reordering memory operations as follows: If a read or write
        /// appears after this method in the code, the processor cannot move it before this method.
        /// </summary>
        /// <param name="location">The field to read.</param>
        /// <returns>The value that was read. This value is the latest written by any processor in
        /// the computer, regardless of the number of processors or the state of processor cache.</returns>
        public static float Read(ref float location)
        {
            float result = location;
            Thread.MemoryBarrier();

            return result;
        }

        /// <summary>
        /// Reads the value of the specified field. On systems that require it, inserts a memory barrier
        /// that prevents the processor from reordering memory operations as follows: If a read or write
        /// appears after this method in the code, the processor cannot move it before this method.
        /// </summary>
        /// <param name="location">The field to read.</param>
        /// <returns>The value that was read. This value is the latest written by any processor in
        /// the computer, regardless of the number of processors or the state of processor cache.</returns>
        public static ushort Read(ref ushort location)
        {
            ushort result = location;
            Thread.MemoryBarrier();

            return result;
        }

        /// <summary>
        /// Reads the value of the specified field. On systems that require it, inserts a memory barrier
        /// that prevents the processor from reordering memory operations as follows: If a read or write
        /// appears after this method in the code, the processor cannot move it before this method.
        /// </summary>
        /// <param name="location">The field to read.</param>
        /// <returns>The value that was read. This value is the latest written by any processor in
        /// the computer, regardless of the number of processors or the state of processor cache.</returns>
        public static uint Read(ref uint location)
        {
            uint result = location;
            Thread.MemoryBarrier();

            return result;
        }

        /// <summary>
        /// Reads the value of the specified field. On systems that require it, inserts a memory barrier
        /// that prevents the processor from reordering memory operations as follows: If a read or write
        /// appears after this method in the code, the processor cannot move it before this method.
        /// </summary>
        /// <param name="location">The field to read.</param>
        /// <returns>The value that was read. This value is the latest written by any processor in
        /// the computer, regardless of the number of processors or the state of processor cache.</returns>
        public static unsafe ulong Read(ref ulong location)
        {
            fixed (ulong* pLocation = &location)
            {
                return (ulong)Interlocked.CompareExchange(ref *(long*)pLocation, 0L, 0L);
            }
        }

        /// <summary>
        /// Reads the value of the specified field. On systems that require it, inserts a memory barrier
        /// that prevents the processor from reordering memory operations as follows: If a read or write
        /// appears after this method in the code, the processor cannot move it before this method.
        /// </summary>
        /// <param name="location">The field to read.</param>
        /// <returns>The value that was read. This value is the latest written by any processor in
        /// the computer, regardless of the number of processors or the state of processor cache.</returns>
        public static UIntPtr Read(ref UIntPtr location)
        {
            UIntPtr result = location;
            Thread.MemoryBarrier();

            return result;
        }

        /// <summary>
        /// Reads the object reference from the specified field. On systems that require it, inserts a memory
        /// barrier that prevents the processor from reordering memory operations as follows: If a read
        /// or write appears after this method in the code, the processor cannot move it before this method.
        /// </summary>
        /// <param name="location">The field to read.</param>
        /// <typeparam name="T">The type of field to read. This must be a reference type, not a value type.</typeparam>
        /// <returns>The reference to <typeparamref name="T"/> that was read. This reference is the latest
        /// written by any processor in the computer, regardless of the number of processors or the state of
        /// processor cache.</returns>
        public static T Read<T>(ref T location) where T : class
        {
            T result = location;
            Thread.MemoryBarrier();

            return result;
        }

        /// <summary>
        /// Writes the specified value to the specified field. On systems that require it, inserts a memory
        /// barrier that prevents the processor from reordering memory operations as follows: If a read or
        /// write appears before this method in the code, the processor cannot move it after this method.
        /// </summary>
        /// <param name="location">The field where the value is written.</param>
        /// <param name="value">The value to write. The value is written immediately so that it is visible
        /// to all processors in the computer.</param>
        public static void Write(ref bool location, bool value)
        {
            Thread.MemoryBarrier();
            location = value;
        }

        /// <summary>
        /// Writes the specified value to the specified field. On systems that require it, inserts a memory
        /// barrier that prevents the processor from reordering memory operations as follows: If a read or
        /// write appears before this method in the code, the processor cannot move it after this method.
        /// </summary>
        /// <param name="location">The field where the value is written.</param>
        /// <param name="value">The value to write. The value is written immediately so that it is visible
        /// to all processors in the computer.</param>
        public static void Write(ref byte location, byte value)
        {
            Thread.MemoryBarrier();
            location = value;
        }

        /// <summary>
        /// Writes the specified value to the specified field. On systems that require it, inserts a memory
        /// barrier that prevents the processor from reordering memory operations as follows: If a read or
        /// write appears before this method in the code, the processor cannot move it after this method.
        /// </summary>
        /// <param name="location">The field where the value is written.</param>
        /// <param name="value">The value to write. The value is written immediately so that it is visible
        /// to all processors in the computer.</param>
        public static void Write(ref double location, double value)
        {
            Interlocked.Exchange(ref location, value);
        }

        /// <summary>
        /// Writes the specified value to the specified field. On systems that require it, inserts a memory
        /// barrier that prevents the processor from reordering memory operations as follows: If a read or
        /// write appears before this method in the code, the processor cannot move it after this method.
        /// </summary>
        /// <param name="location">The field where the value is written.</param>
        /// <param name="value">The value to write. The value is written immediately so that it is visible
        /// to all processors in the computer.</param>
        public static void Write(ref short location, short value)
        {
            Thread.MemoryBarrier();
            location = value;
        }

        /// <summary>
        /// Writes the specified value to the specified field. On systems that require it, inserts a memory
        /// barrier that prevents the processor from reordering memory operations as follows: If a read or
        /// write appears before this method in the code, the processor cannot move it after this method.
        /// </summary>
        /// <param name="location">The field where the value is written.</param>
        /// <param name="value">The value to write. The value is written immediately so that it is visible
        /// to all processors in the computer.</param>
        public static void Write(ref int location, int value)
        {
            Thread.MemoryBarrier();
            location = value;
        }

        /// <summary>
        /// Writes the specified value to the specified field. On systems that require it, inserts a memory
        /// barrier that prevents the processor from reordering memory operations as follows: If a memory
        /// operation appears before this method in the code, the processor cannot move it after this method.
        /// </summary>
        /// <param name="location">The field where the value is written.</param>
        /// <param name="value">The value to write. The value is written immediately so that it is visible
        /// to all processors in the computer.</param>
        public static void Write(ref long location, long value)
        {
            Interlocked.Exchange(ref location, value);
        }

        /// <summary>
        /// Writes the specified value to the specified field. On systems that require it, inserts a memory
        /// barrier that prevents the processor from reordering memory operations as follows: If a read or
        /// write appears before this method in the code, the processor cannot move it after this method.
        /// </summary>
        /// <param name="location">The field where the value is written.</param>
        /// <param name="value">The value to write. The value is written immediately so that it is visible
        /// to all processors in the computer.</param>
        public static void Write(ref IntPtr location, IntPtr value)
        {
            Thread.MemoryBarrier();
            location = value;
        }

        /// <summary>
        /// Writes the specified value to the specified field. On systems that require it, inserts a memory
        /// barrier that prevents the processor from reordering memory operations as follows: If a read or
        /// write appears before this method in the code, the processor cannot move it after this method.
        /// </summary>
        /// <param name="location">The field where the value is written.</param>
        /// <param name="value">The value to write. The value is written immediately so that it is visible
        /// to all processors in the computer.</param>
        public static void Write(ref sbyte location, sbyte value)
        {
            Thread.MemoryBarrier();
            location = value;
        }

        /// <summary>
        /// Writes the specified value to the specified field. On systems that require it, inserts a memory
        /// barrier that prevents the processor from reordering memory operations as follows: If a read or
        /// write appears before this method in the code, the processor cannot move it after this method.
        /// </summary>
        /// <param name="location">The field where the value is written.</param>
        /// <param name="value">The value to write. The value is written immediately so that it is visible
        /// to all processors in the computer.</param>
        public static void Write(ref float location, float value)
        {
            Thread.MemoryBarrier();
            location = value;
        }

        /// <summary>
        /// Writes the specified value to the specified field. On systems that require it, inserts a memory
        /// barrier that prevents the processor from reordering memory operations as follows: If a read or
        /// write appears before this method in the code, the processor cannot move it after this method.
        /// </summary>
        /// <param name="location">The field where the value is written.</param>
        /// <param name="value">The value to write. The value is written immediately so that it is visible
        /// to all processors in the computer.</param>
        public static void Write(ref ushort location, ushort value)
        {
            Thread.MemoryBarrier();
            location = value;
        }

        /// <summary>
        /// Writes the specified value to the specified field. On systems that require it, inserts a memory
        /// barrier that prevents the processor from reordering memory operations as follows: If a read or
        /// write appears before this method in the code, the processor cannot move it after this method.
        /// </summary>
        /// <param name="location">The field where the value is written.</param>
        /// <param name="value">The value to write. The value is written immediately so that it is visible
        /// to all processors in the computer.</param>
        public static void Write(ref uint location, uint value)
        {
            Thread.MemoryBarrier();
            location = value;
        }

        /// <summary>
        /// Writes the specified value to the specified field. On systems that require it, inserts a memory
        /// barrier that prevents the processor from reordering memory operations as follows: If a read or
        /// write appears before this method in the code, the processor cannot move it after this method.
        /// </summary>
        /// <param name="location">The field where the value is written.</param>
        /// <param name="value">The value to write. The value is written immediately so that it is visible
        /// to all processors in the computer.</param>
        public static unsafe void Write(ref ulong location, ulong value)
        {
            fixed (ulong* pLocation = &location)
            {
                Interlocked.Exchange(ref *(long*)pLocation, (long)value);
            }
        }

        /// <summary>
        /// Writes the specified value to the specified field. On systems that require it, inserts a memory
        /// barrier that prevents the processor from reordering memory operations as follows: If a read or
        /// write appears before this method in the code, the processor cannot move it after this method.
        /// </summary>
        /// <param name="location">The field where the value is written.</param>
        /// <param name="value">The value to write. The value is written immediately so that it is visible
        /// to all processors in the computer.</param>
        public static void Write(ref UIntPtr location, UIntPtr value)
        {
            Thread.MemoryBarrier();
            location = value;
        }

        /// <summary>
        /// Writes the specified object reference to the specified field. On systems that require it, inserts
        /// a memory barrier that prevents the processor from reordering memory operations as follows: If a
        /// read or write appears before this method in the code, the processor cannot move it after this method.
        /// </summary>
        /// <param name="location">The field where the object reference is written.</param>
        /// <param name="value">The object reference to write. The reference is written immediately so that it
        /// is visible to all processors in the computer.</param>
        /// <typeparam name="T">The type of field to write. This must be a reference type, not a value type.</typeparam>
        public static void Write<T>(ref T location, T value) where T : class
        {
            Thread.MemoryBarrier();
            location = value;
        }
    }
}
