using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace DotnetEx.Test
{
    /// <summary>
    /// The tests for the <see cref="Unsafe"/> class.
    /// </summary>
    [TestFixture]
    public static class UnsafeTests
    {
        [Test]
        public static unsafe void ReadInt32()
        {
            int expected = 10;
            void* address = Unsafe.AsPointer(ref expected); // Unsafe.AsPointer is safe since expected is on stack
            int ret = Unsafe.Read<int>(address);
            Assert.AreEqual(expected, ret);
        }

        [Test]
        public static unsafe void WriteInt32()
        {
            int value = 10;
            int* address = (int*)Unsafe.AsPointer(ref value); // Unsafe.AsPointer is safe since value is on stack
            int expected = 20;
            Unsafe.Write(address, expected);

            Assert.AreEqual(expected, value);
            Assert.AreEqual(expected, *address);
            Assert.AreEqual(expected, Unsafe.Read<int>(address));
        }

        [Test]
        public static unsafe void WriteBytesIntoInt32()
        {
            int value = 20;
            int* intAddress = (int*)Unsafe.AsPointer(ref value); // Unsafe.AsPointer is safe since value is on stack
            byte* byteAddress = (byte*)intAddress;
            for (int i = 0; i < 4; i++)
            {
                Unsafe.Write(byteAddress + i, (byte)i);
            }

            Assert.AreEqual(0, Unsafe.Read<byte>(byteAddress));
            Assert.AreEqual(1, Unsafe.Read<byte>(byteAddress + 1));
            Assert.AreEqual(2, Unsafe.Read<byte>(byteAddress + 2));
            Assert.AreEqual(3, Unsafe.Read<byte>(byteAddress + 3));

            Byte4 b4 = Unsafe.Read<Byte4>(byteAddress);
            Assert.AreEqual(0, b4.B0);
            Assert.AreEqual(1, b4.B1);
            Assert.AreEqual(2, b4.B2);
            Assert.AreEqual(3, b4.B3);

            int expected;
            if (BitConverter.IsLittleEndian)
            {
                expected = (b4.B3 << 24) + (b4.B2 << 16) + (b4.B1 << 8) + (b4.B0);
            }
            else
            {
                expected = (b4.B0 << 24) + (b4.B1 << 16) + (b4.B2 << 8) + (b4.B3);
            }
            Assert.AreEqual(expected, value);
        }

        [Test]
        public static unsafe void LongIntoCompoundStruct()
        {
            long value = 1234567891011121314L;
            long* longAddress = (long*)Unsafe.AsPointer(ref value); // Unsafe.AsPointer is safe since value is on stack
            Byte4Short2 b4s2 = Unsafe.Read<Byte4Short2>(longAddress);
            if (BitConverter.IsLittleEndian)
            {
                Assert.AreEqual(162, b4s2.B0);
                Assert.AreEqual(48, b4s2.B1);
                Assert.AreEqual(210, b4s2.B2);
                Assert.AreEqual(178, b4s2.B3);
                Assert.AreEqual(4340, b4s2.S4);
                Assert.AreEqual(4386, b4s2.S6);
            }
            else
            {
                Assert.AreEqual(17, b4s2.B0);
                Assert.AreEqual(34, b4s2.B1);
                Assert.AreEqual(16, b4s2.B2);
                Assert.AreEqual(244, b4s2.B3);
                Assert.AreEqual(-19758, b4s2.S4);
                Assert.AreEqual(12450, b4s2.S6);
            }

            b4s2.B0 = 1;
            b4s2.B1 = 1;
            b4s2.B2 = 1;
            b4s2.B3 = 1;
            b4s2.S4 = 1;
            b4s2.S6 = 1;
            Unsafe.Write(longAddress, b4s2);

            long expected;
            if (BitConverter.IsLittleEndian)
            {
                expected = 281479288520961;
            }
            else
            {
                expected = 72340172821299201;
            }
            Assert.AreEqual(expected, value);
            Assert.AreEqual(expected, Unsafe.Read<long>(longAddress));
        }

        [Test]
        public static unsafe void ReadWriteDoublePointer()
        {
            int value1 = 10;
            int value2 = 20;
            int* valueAddress = (int*)Unsafe.AsPointer(ref value1); // Unsafe.AsPointer is safe since value1 is on stack
            int** valueAddressPtr = &valueAddress;
            Unsafe.Write(valueAddressPtr, new IntPtr(&value2));

            Assert.AreEqual(20, *(*valueAddressPtr));
            Assert.AreEqual(20, Unsafe.Read<int>(valueAddress));
            Assert.AreEqual(new IntPtr(valueAddress), Unsafe.Read<nint>(valueAddressPtr));
            Assert.AreEqual(20, Unsafe.Read<int>(Unsafe.Read<IntPtr>(valueAddressPtr).ToPointer()));
        }

        [Test]
        public static unsafe void CopyToRef()
        {
            int value = 10;
            int destination = -1;
            Unsafe.Copy(ref destination, Unsafe.AsPointer(ref value)); // Unsafe.AsPointer is safe since value is on stack
            Assert.AreEqual(10, destination);
            Assert.AreEqual(10, value);

            int destination2 = -1;
            Unsafe.Copy(ref destination2, &value);
            Assert.AreEqual(10, destination2);
            Assert.AreEqual(10, value);
        }

        [Test]
        public static unsafe void CopyToVoidPtr()
        {
            int value = 10;
            int destination = -1;
            Unsafe.Copy(Unsafe.AsPointer(ref destination), ref value); // Unsafe.AsPointer is safe since destination is on stack
            Assert.AreEqual(10, destination);
            Assert.AreEqual(10, value);

            int destination2 = -1;
            Unsafe.Copy(&destination2, ref value);
            Assert.AreEqual(10, destination2);
            Assert.AreEqual(10, value);
        }

        [Test]
        public static unsafe void CopyToRefGenericStruct()
        {
            Int32Generic<char> destination = default;
            Int32Generic<char> value = new() { Int32 = 5, Value = 'a' };

            Unsafe.Copy(ref destination, Unsafe.AsPointer(ref value)); // Unsafe.AsPointer is safe since value is on stack

            Assert.AreEqual(5, destination.Int32);
            Assert.AreEqual('a', destination.Value);
        }

        [Test]
        public static unsafe void CopyToVoidPtrGenericStruct()
        {
            Int32Generic<char> destination = default;
            Int32Generic<char> value = new() { Int32 = 5, Value = 'a' };

            Unsafe.Copy(Unsafe.AsPointer(ref destination), ref value); // Unsafe.AsPointer is safe since destination is on stack

            Assert.AreEqual(5, destination.Int32);
            Assert.AreEqual('a', destination.Value);
        }

        [Test]
        public static unsafe void SizeOf()
        {
            Assert.AreEqual(1, Unsafe.SizeOf<sbyte>());
            Assert.AreEqual(1, Unsafe.SizeOf<byte>());
            Assert.AreEqual(2, Unsafe.SizeOf<short>());
            Assert.AreEqual(2, Unsafe.SizeOf<ushort>());
            Assert.AreEqual(4, Unsafe.SizeOf<int>());
            Assert.AreEqual(4, Unsafe.SizeOf<uint>());
            Assert.AreEqual(8, Unsafe.SizeOf<long>());
            Assert.AreEqual(8, Unsafe.SizeOf<ulong>());
            Assert.AreEqual(4, Unsafe.SizeOf<float>());
            Assert.AreEqual(8, Unsafe.SizeOf<double>());
            Assert.AreEqual(4, Unsafe.SizeOf<Byte4>());
            Assert.AreEqual(8, Unsafe.SizeOf<Byte4Short2>());
            Assert.AreEqual(512, Unsafe.SizeOf<Byte512>());
        }

        [TestCaseSource(nameof(InitBlockData))]
        public static unsafe void InitBlockStack(int numBytes, byte value)
        {
            byte* stackPtr = stackalloc byte[numBytes];
            Unsafe.InitBlock(stackPtr, value, (uint)numBytes);
            for (int i = 0; i < numBytes; i++)
            {
                Assert.AreEqual(stackPtr[i], value);
            }
        }

        [TestCaseSource(nameof(InitBlockData))]
        public static unsafe void InitBlockUnmanaged(int numBytes, byte value)
        {
            IntPtr allocatedMemory = Marshal.AllocCoTaskMem(numBytes);
            byte* bytePtr = (byte*)allocatedMemory.ToPointer();
            Unsafe.InitBlock(bytePtr, value, (uint)numBytes);
            for (int i = 0; i < numBytes; i++)
            {
                Assert.AreEqual(bytePtr[i], value);
            }
        }

        [TestCaseSource(nameof(InitBlockData))]
        public static unsafe void InitBlockRefStack(int numBytes, byte value)
        {
            byte* stackPtr = stackalloc byte[numBytes];
            Unsafe.InitBlock(ref *stackPtr, value, (uint)numBytes);
            for (int i = 0; i < numBytes; i++)
            {
                Assert.AreEqual(stackPtr[i], value);
            }
        }

        [TestCaseSource(nameof(InitBlockData))]
        public static unsafe void InitBlockRefUnmanaged(int numBytes, byte value)
        {
            IntPtr allocatedMemory = Marshal.AllocCoTaskMem(numBytes);
            byte* bytePtr = (byte*)allocatedMemory.ToPointer();
            Unsafe.InitBlock(ref *bytePtr, value, (uint)numBytes);
            for (int i = 0; i < numBytes; i++)
            {
                Assert.AreEqual(bytePtr[i], value);
            }
        }

        [TestCaseSource(nameof(InitBlockData))]
        public static unsafe void InitBlockUnalignedStack(int numBytes, byte value)
        {
            byte* stackPtr = stackalloc byte[numBytes + 1];
            stackPtr += 1; // +1 = make unaligned
            Unsafe.InitBlockUnaligned(stackPtr, value, (uint)numBytes);
            for (int i = 0; i < numBytes; i++)
            {
                Assert.AreEqual(stackPtr[i], value);
            }
        }

        [TestCaseSource(nameof(InitBlockData))]
        public static unsafe void InitBlockUnalignedUnmanaged(int numBytes, byte value)
        {
            IntPtr allocatedMemory = Marshal.AllocCoTaskMem(numBytes + 1);
            byte* bytePtr = (byte*)allocatedMemory.ToPointer() + 1; // +1 = make unaligned
            Unsafe.InitBlockUnaligned(bytePtr, value, (uint)numBytes);
            for (int i = 0; i < numBytes; i++)
            {
                Assert.AreEqual(bytePtr[i], value);
            }
        }

        [TestCaseSource(nameof(InitBlockData))]
        public static unsafe void InitBlockUnalignedRefStack(int numBytes, byte value)
        {
            byte* stackPtr = stackalloc byte[numBytes + 1];
            stackPtr += 1; // +1 = make unaligned
            Unsafe.InitBlockUnaligned(ref *stackPtr, value, (uint)numBytes);
            for (int i = 0; i < numBytes; i++)
            {
                Assert.AreEqual(stackPtr[i], value);
            }
        }

        [TestCaseSource(nameof(InitBlockData))]
        public static unsafe void InitBlockUnalignedRefUnmanaged(int numBytes, byte value)
        {
            IntPtr allocatedMemory = Marshal.AllocCoTaskMem(numBytes + 1);
            byte* bytePtr = (byte*)allocatedMemory.ToPointer() + 1; // +1 = make unaligned
            Unsafe.InitBlockUnaligned(ref *bytePtr, value, (uint)numBytes);
            for (int i = 0; i < numBytes; i++)
            {
                Assert.AreEqual(bytePtr[i], value);
            }
        }

        public static IEnumerable<object[]> InitBlockData()
        {
            yield return new object[] { 0, (byte)1 };
            yield return new object[] { 1, (byte)1 };
            yield return new object[] { 10, (byte)0 };
            yield return new object[] { 10, (byte)2 };
            yield return new object[] { 10, (byte)255 };
            yield return new object[] { 10000, (byte)255 };
        }

        [TestCaseSource(nameof(CopyBlockData))]
        public static unsafe void CopyBlock(int numBytes)
        {
            byte* source = stackalloc byte[numBytes];
            byte* destination = stackalloc byte[numBytes];

            for (int i = 0; i < numBytes; i++)
            {
                byte value = (byte)(i % 255);
                source[i] = value;
            }

            Unsafe.CopyBlock(destination, source, (uint)numBytes);

            for (int i = 0; i < numBytes; i++)
            {
                byte value = (byte)(i % 255);
                Assert.AreEqual(value, destination[i]);
                Assert.AreEqual(source[i], destination[i]);
            }
        }

        [TestCaseSource(nameof(CopyBlockData))]
        public static unsafe void CopyBlockRef(int numBytes)
        {
            byte* source = stackalloc byte[numBytes];
            byte* destination = stackalloc byte[numBytes];

            for (int i = 0; i < numBytes; i++)
            {
                byte value = (byte)(i % 255);
                source[i] = value;
            }

            Unsafe.CopyBlock(ref destination[0], ref source[0], (uint)numBytes);

            for (int i = 0; i < numBytes; i++)
            {
                byte value = (byte)(i % 255);
                Assert.AreEqual(value, destination[i]);
                Assert.AreEqual(source[i], destination[i]);
            }
        }

        [TestCaseSource(nameof(CopyBlockData))]
        public static unsafe void CopyBlockUnaligned(int numBytes)
        {
            byte* source = stackalloc byte[numBytes + 1];
            byte* destination = stackalloc byte[numBytes + 1];
            source += 1;      // +1 = make unaligned
            destination += 1; // +1 = make unaligned

            for (int i = 0; i < numBytes; i++)
            {
                byte value = (byte)(i % 255);
                source[i] = value;
            }

            Unsafe.CopyBlockUnaligned(destination, source, (uint)numBytes);

            for (int i = 0; i < numBytes; i++)
            {
                byte value = (byte)(i % 255);
                Assert.AreEqual(value, destination[i]);
                Assert.AreEqual(source[i], destination[i]);
            }
        }

        [TestCaseSource(nameof(CopyBlockData))]
        public static unsafe void CopyBlockUnalignedRef(int numBytes)
        {
            byte* source = stackalloc byte[numBytes + 1];
            byte* destination = stackalloc byte[numBytes + 1];
            source += 1;      // +1 = make unaligned
            destination += 1; // +1 = make unaligned

            for (int i = 0; i < numBytes; i++)
            {
                byte value = (byte)(i % 255);
                source[i] = value;
            }

            Unsafe.CopyBlockUnaligned(ref destination[0], ref source[0], (uint)numBytes);

            for (int i = 0; i < numBytes; i++)
            {
                byte value = (byte)(i % 255);
                Assert.AreEqual(value, destination[i]);
                Assert.AreEqual(source[i], destination[i]);
            }
        }

        public static IEnumerable<object[]> CopyBlockData()
        {
            yield return new object[] { 0 };
            yield return new object[] { 1 };
            yield return new object[] { 10 };
            yield return new object[] { 100 };
            yield return new object[] { 100000 };
        }

        [Test]
        public static void As()
        {
            object o = "Hello";
            Assert.AreEqual("Hello", Unsafe.As<string>(o));
        }

        [Test]
        public static void ByteOffsetArray()
        {
            byte[] a = [0, 1, 2, 3, 4, 5, 6, 7];

            Assert.AreEqual(new IntPtr(0), Unsafe.ByteOffset(ref a[0], ref a[0]));
            Assert.AreEqual(new IntPtr(1), Unsafe.ByteOffset(ref a[0], ref a[1]));
            Assert.AreEqual(new IntPtr(-1), Unsafe.ByteOffset(ref a[1], ref a[0]));
            Assert.AreEqual(new IntPtr(2), Unsafe.ByteOffset(ref a[0], ref a[2]));
            Assert.AreEqual(new IntPtr(-2), Unsafe.ByteOffset(ref a[2], ref a[0]));
            Assert.AreEqual(new IntPtr(3), Unsafe.ByteOffset(ref a[0], ref a[3]));
            Assert.AreEqual(new IntPtr(4), Unsafe.ByteOffset(ref a[0], ref a[4]));
            Assert.AreEqual(new IntPtr(5), Unsafe.ByteOffset(ref a[0], ref a[5]));
            Assert.AreEqual(new IntPtr(6), Unsafe.ByteOffset(ref a[0], ref a[6]));
            Assert.AreEqual(new IntPtr(7), Unsafe.ByteOffset(ref a[0], ref a[7]));
        }

        [Test]
        public static void ByteOffsetStackByte4()
        {
            Byte4 byte4 = new();

            Assert.AreEqual(new IntPtr(0), Unsafe.ByteOffset(ref byte4.B0, ref byte4.B0));
            Assert.AreEqual(new IntPtr(1), Unsafe.ByteOffset(ref byte4.B0, ref byte4.B1));
            Assert.AreEqual(new IntPtr(-1), Unsafe.ByteOffset(ref byte4.B1, ref byte4.B0));
            Assert.AreEqual(new IntPtr(2), Unsafe.ByteOffset(ref byte4.B0, ref byte4.B2));
            Assert.AreEqual(new IntPtr(-2), Unsafe.ByteOffset(ref byte4.B2, ref byte4.B0));
            Assert.AreEqual(new IntPtr(3), Unsafe.ByteOffset(ref byte4.B0, ref byte4.B3));
            Assert.AreEqual(new IntPtr(-3), Unsafe.ByteOffset(ref byte4.B3, ref byte4.B0));
        }

        private static unsafe class StaticReadonlyHolder
        {
            public static readonly void* Pointer;
            static unsafe StaticReadonlyHolder()
            {
                byte* ptr = stackalloc byte[1];
                Pointer = ptr;
            }
        }

        [Test]
        public static unsafe void ByteOffsetConstantRef()
        {
            // https://github.com/dotnet/runtime/pull/99019
            [MethodImpl(MethodImplOptions.NoInlining)]
            static nint NullTest(ref byte origin) => Unsafe.ByteOffset(ref origin, ref Unsafe.NullRef<byte>());
            Assert.AreEqual((nint)0, NullTest(ref Unsafe.NullRef<byte>()));

            [MethodImpl((MethodImplOptions)0x100)]
            static ref byte GetStatic(ref byte x) => ref x;
            [MethodImpl(MethodImplOptions.NoInlining)]
            static nint StaticReadonlyTest(ref byte x) => Unsafe.ByteOffset(ref GetStatic(ref Unsafe.AsRef<byte>(StaticReadonlyHolder.Pointer)), ref x);
            Assert.AreEqual((nint)0, StaticReadonlyTest(ref Unsafe.AsRef<byte>(StaticReadonlyHolder.Pointer)));
        }

        [Test]
        public static unsafe void AsRef()
        {
            byte[] b = [0x42, 0x42, 0x42, 0x42];
            fixed (byte* p = b)
            {
                ref int r = ref Unsafe.AsRef<int>(p);
                Assert.AreEqual(0x42424242, r);

                r = 0x0EF00EF0;
                Assert.AreEqual(0xFE, b[0] | b[1] | b[2] | b[3]);
            }
        }

        [Test]
        public static void InAsRef()
        {
            int[] a = [0x123, 0x234, 0x345, 0x456];

            ref int r = ref Unsafe.AsRef<int>(in a[0]);
            Assert.AreEqual(0x123, r);

            r = 0x42;
            Assert.AreEqual(0x42, a[0]);
        }

        [Test]
        public static void RefAs()
        {
            byte[] b = [0x42, 0x42, 0x42, 0x42];

            ref int r = ref Unsafe.As<byte, int>(ref b[0]);
            Assert.AreEqual(0x42424242, r);

            r = 0x0EF00EF0;
            Assert.AreEqual(0xFE, b[0] | b[1] | b[2] | b[3]);
        }

        [Test]
        public static void RefAdd()
        {
            int[] a = [0x123, 0x234, 0x345, 0x456];

            ref int r1 = ref Unsafe.Add(ref a[0], 1);
            Assert.AreEqual(0x234, r1);

            ref int r2 = ref Unsafe.Add(ref r1, 2);
            Assert.AreEqual(0x456, r2);

            ref int r3 = ref Unsafe.Add(ref r2, -3);
            Assert.AreEqual(0x123, r3);
        }

        [Test]
        public static unsafe void VoidPointerAdd()
        {
            int[] a = [0x123, 0x234, 0x345, 0x456];

            fixed (void* ptr = a)
            {
                void* r1 = Unsafe.Add<int>(ptr, 1);
                Assert.AreEqual(0x234, *(int*)r1);

                void* r2 = Unsafe.Add<int>(r1, 2);
                Assert.AreEqual(0x456, *(int*)r2);

                void* r3 = Unsafe.Add<int>(r2, -3);
                Assert.AreEqual(0x123, *(int*)r3);
            }

            fixed (void* ptr = &a[1])
            {
                void* r0 = Unsafe.Add<int>(ptr, -1);
                Assert.AreEqual(0x123, *(int*)r0);

                void* r3 = Unsafe.Add<int>(ptr, 2);
                Assert.AreEqual(0x456, *(int*)r3);
            }
        }

        [Test]
        public static void RefAddIntPtr()
        {
            int[] a = [0x123, 0x234, 0x345, 0x456];

            ref int r1 = ref Unsafe.Add(ref a[0], (nint)1);
            Assert.AreEqual(0x234, r1);

            ref int r2 = ref Unsafe.Add(ref r1, (nint)2);
            Assert.AreEqual(0x456, r2);

            ref int r3 = ref Unsafe.Add(ref r2, (nint)(-3));
            Assert.AreEqual(0x123, r3);
        }

        [Test]
        public static void RefAddNuint()
        {
            int[] a = [0x123, 0x234, 0x345, 0x456];

            ref int r1 = ref Unsafe.Add(ref a[0], (nuint)1);
            Assert.AreEqual(0x234, r1);

            ref int r2 = ref Unsafe.Add(ref r1, (nuint)2);
            Assert.AreEqual(0x456, r2);
        }

        [Test]
        public static void RefAddByteOffset()
        {
            byte[] a = [0x12, 0x34, 0x56, 0x78];

            ref byte r1 = ref Unsafe.AddByteOffset(ref a[0], (nint)1);
            Assert.AreEqual(0x34, r1);

            ref byte r2 = ref Unsafe.AddByteOffset(ref r1, (nint)2);
            Assert.AreEqual(0x78, r2);

            ref byte r3 = ref Unsafe.AddByteOffset(ref r2, (nint)(-3));
            Assert.AreEqual(0x12, r3);
        }

        [Test]
        public static void RefAddNuintByteOffset()
        {
            byte[] a = [0x12, 0x34, 0x56, 0x78];

            ref byte r1 = ref Unsafe.AddByteOffset(ref a[0], (nuint)1);
            Assert.AreEqual(0x34, r1);

            ref byte r2 = ref Unsafe.AddByteOffset(ref r1, (nuint)2);
            Assert.AreEqual(0x78, r2);
        }

        [Test]
        public static unsafe void RefSubtract()
        {
            int[] a = [0x123, 0x234, 0x345, 0x456];

            ref int r1 = ref Unsafe.Subtract(ref a[0], -2);
            Assert.AreEqual(0x345, r1);

            ref int r2 = ref Unsafe.Subtract(ref r1, -1);
            Assert.AreEqual(0x456, r2);

            ref int r3 = ref Unsafe.Subtract(ref r2, 3);
            Assert.AreEqual(0x123, r3);

            // https://github.com/dotnet/runtime/pull/99019
            [MethodImpl(MethodImplOptions.NoInlining)]
            static ref byte NullTest(nuint offset) => ref Unsafe.Subtract(ref Unsafe.NullRef<byte>(), offset);
            Assert.True(Unsafe.IsNullRef(ref NullTest(0)));
        }

        [Test]
        public static unsafe void VoidPointerSubtract()
        {
            int[] a = [0x123, 0x234, 0x345, 0x456];

            fixed (void* ptr = a)
            {
                void* r1 = Unsafe.Subtract<int>(ptr, -2);
                Assert.AreEqual(0x345, *(int*)r1);

                void* r2 = Unsafe.Subtract<int>(r1, -1);
                Assert.AreEqual(0x456, *(int*)r2);

                void* r3 = Unsafe.Subtract<int>(r2, 3);
                Assert.AreEqual(0x123, *(int*)r3);
            }

            fixed (void* ptr = &a[1])
            {
                void* r0 = Unsafe.Subtract<int>(ptr, 1);
                Assert.AreEqual(0x123, *(int*)r0);

                void* r3 = Unsafe.Subtract<int>(ptr, -2);
                Assert.AreEqual(0x456, *(int*)r3);
            }
        }

        [Test]
        public static void RefSubtractIntPtr()
        {
            int[] a = [0x123, 0x234, 0x345, 0x456];

            ref int r1 = ref Unsafe.Subtract(ref a[0], (nint)(-2));
            Assert.AreEqual(0x345, r1);

            ref int r2 = ref Unsafe.Subtract(ref r1, (nint)(-1));
            Assert.AreEqual(0x456, r2);

            ref int r3 = ref Unsafe.Subtract(ref r2, (nint)3);
            Assert.AreEqual(0x123, r3);
        }

        [Test]
        public static void RefSubtractNuint()
        {
            int[] a = [0x123, 0x234, 0x345, 0x456];

            ref int r3 = ref Unsafe.Subtract(ref a[3], (nuint)3);
            Assert.AreEqual(0x123, r3);
        }

        [Test]
        public static void RefSubtractByteOffset()
        {
            byte[] a = [0x12, 0x34, 0x56, 0x78];

            ref byte r1 = ref Unsafe.SubtractByteOffset(ref a[0], (nint)(-1));
            Assert.AreEqual(0x34, r1);

            ref byte r2 = ref Unsafe.SubtractByteOffset(ref r1, (nint)(-2));
            Assert.AreEqual(0x78, r2);

            ref byte r3 = ref Unsafe.SubtractByteOffset(ref r2, (nint)3);
            Assert.AreEqual(0x12, r3);
        }

        [Test]
        public static void RefSubtractNuintByteOffset()
        {
            byte[] a = [0x12, 0x34, 0x56, 0x78];

            ref byte r3 = ref Unsafe.SubtractByteOffset(ref a[3], (nuint)3);
            Assert.AreEqual(0x12, r3);
        }

        [Test]
        public static void RefAreSame()
        {
            long[] a = new long[2];

            Assert.True(Unsafe.AreSame(ref a[0], ref a[0]));
            Assert.False(Unsafe.AreSame(ref a[0], ref a[1]));
        }

        [Test]
        public static unsafe void RefIsAddressGreaterThan()
        {
            int[] a = new int[2];

            Assert.False(Unsafe.IsAddressGreaterThan(ref a[0], ref a[0]));
            Assert.False(Unsafe.IsAddressGreaterThan(ref a[0], ref a[1]));
            Assert.True(Unsafe.IsAddressGreaterThan(ref a[1], ref a[0]));
            Assert.False(Unsafe.IsAddressGreaterThan(ref a[1], ref a[1]));

            // The following tests ensure that we're using unsigned comparison logic

            Assert.False(Unsafe.IsAddressGreaterThan(ref Unsafe.AsRef<byte>((void*)1), ref Unsafe.AsRef<byte>((void*)-1)));
            Assert.True(Unsafe.IsAddressGreaterThan(ref Unsafe.AsRef<byte>((void*)-1), ref Unsafe.AsRef<byte>((void*)1)));
            Assert.True(Unsafe.IsAddressGreaterThan(ref Unsafe.AsRef<byte>((void*)int.MinValue), ref Unsafe.AsRef<byte>((void*)int.MaxValue)));
            Assert.False(Unsafe.IsAddressGreaterThan(ref Unsafe.AsRef<byte>((void*)int.MaxValue), ref Unsafe.AsRef<byte>((void*)int.MinValue)));
            Assert.False(Unsafe.IsAddressGreaterThan(ref Unsafe.AsRef<byte>(null), ref Unsafe.AsRef<byte>(null)));
        }

        [Test]
        public static unsafe void RefIsAddressLessThan()
        {
            int[] a = new int[2];

            Assert.False(Unsafe.IsAddressLessThan(ref a[0], ref a[0]));
            Assert.True(Unsafe.IsAddressLessThan(ref a[0], ref a[1]));
            Assert.False(Unsafe.IsAddressLessThan(ref a[1], ref a[0]));
            Assert.False(Unsafe.IsAddressLessThan(ref a[1], ref a[1]));

            // The following tests ensure that we're using unsigned comparison logic

            Assert.True(Unsafe.IsAddressLessThan(ref Unsafe.AsRef<byte>((void*)1), ref Unsafe.AsRef<byte>((void*)-1)));
            Assert.False(Unsafe.IsAddressLessThan(ref Unsafe.AsRef<byte>((void*)-1), ref Unsafe.AsRef<byte>((void*)1)));
            Assert.False(Unsafe.IsAddressLessThan(ref Unsafe.AsRef<byte>((void*)int.MinValue), ref Unsafe.AsRef<byte>((void*)int.MaxValue)));
            Assert.True(Unsafe.IsAddressLessThan(ref Unsafe.AsRef<byte>((void*)int.MaxValue), ref Unsafe.AsRef<byte>((void*)int.MinValue)));
            Assert.False(Unsafe.IsAddressLessThan(ref Unsafe.AsRef<byte>(null), ref Unsafe.AsRef<byte>(null)));
        }

        [Test]
        public static unsafe void ReadUnaligned_ByRef_Int32()
        {
            byte[] unaligned = Int32Double.Unaligned(123456789, 3.42);

            int actual = Unsafe.ReadUnaligned<int>(ref unaligned[1]);

            Assert.AreEqual(123456789, actual);
        }

        [Test]
        public static unsafe void ReadUnaligned_ByRef_Double()
        {
            byte[] unaligned = Int32Double.Unaligned(123456789, 3.42);

            double actual = Unsafe.ReadUnaligned<double>(ref unaligned[9]);

            Assert.AreEqual(3.42, actual);
        }

        [Test]
        public static unsafe void ReadUnaligned_ByRef_Struct()
        {
            byte[] unaligned = Int32Double.Unaligned(123456789, 3.42);

            Int32Double actual = Unsafe.ReadUnaligned<Int32Double>(ref unaligned[1]);

            Assert.AreEqual(123456789, actual.Int32);
            Assert.AreEqual(3.42, actual.Double);
        }

        [Test]
        public static unsafe void ReadUnaligned_ByRef_StructManaged()
        {
            Int32Generic<char> s = new() { Int32 = 5, Value = 'a' };

            Int32Generic<char> actual = Read<Int32Generic<char>>(ref Unsafe.As<Int32Generic<char>, byte>(ref s));

            Assert.AreEqual(5, actual.Int32);
            Assert.AreEqual('a', actual.Value);

            [MethodImpl(MethodImplOptions.NoInlining)]
            static T Read<T>(ref byte b) where T : unmanaged => Unsafe.ReadUnaligned<T>(ref b);
        }

        [Test]
        public static unsafe void ReadUnaligned_Ptr_Int32()
        {
            byte[] unaligned = Int32Double.Unaligned(123456789, 3.42);

            fixed (byte* p = unaligned)
            {
                int actual = Unsafe.ReadUnaligned<int>(p + 1);

                Assert.AreEqual(123456789, actual);
            }
        }

        [Test]
        public static unsafe void ReadUnaligned_Ptr_Double()
        {
            byte[] unaligned = Int32Double.Unaligned(123456789, 3.42);

            fixed (byte* p = unaligned)
            {
                double actual = Unsafe.ReadUnaligned<double>(p + 9);

                Assert.AreEqual(3.42, actual);
            }
        }

        [Test]
        public static unsafe void ReadUnaligned_Ptr_Struct()
        {
            byte[] unaligned = Int32Double.Unaligned(123456789, 3.42);

            fixed (byte* p = unaligned)
            {
                Int32Double actual = Unsafe.ReadUnaligned<Int32Double>(p + 1);

                Assert.AreEqual(123456789, actual.Int32);
                Assert.AreEqual(3.42, actual.Double);
            }
        }

        [Test]
        public static unsafe void WriteUnaligned_ByRef_Int32()
        {
            byte[] unaligned = new byte[sizeof(Int32Double) + 1];

            Unsafe.WriteUnaligned(ref unaligned[1], 123456789);

            int actual = Int32Double.Aligned(unaligned).Int32;
            Assert.AreEqual(123456789, actual);
        }

        [Test]
        public static unsafe void WriteUnaligned_ByRef_Double()
        {
            byte[] unaligned = new byte[sizeof(Int32Double) + 1];

            Unsafe.WriteUnaligned(ref unaligned[9], 3.42);

            double actual = Int32Double.Aligned(unaligned).Double;
            Assert.AreEqual(3.42, actual);
        }

        [Test]
        public static unsafe void WriteUnaligned_ByRef_Struct()
        {
            byte[] unaligned = new byte[sizeof(Int32Double) + 1];

            Unsafe.WriteUnaligned(ref unaligned[1], new Int32Double { Int32 = 123456789, Double = 3.42 });

            Int32Double actual = Int32Double.Aligned(unaligned);
            Assert.AreEqual(123456789, actual.Int32);
            Assert.AreEqual(3.42, actual.Double);
        }

        [Test]
        public static unsafe void WriteUnaligned_ByRef_StructManaged()
        {
            Int32Generic<char> actual = default;

            Write(ref Unsafe.As<Int32Generic<char>, byte>(ref actual), new Int32Generic<char>() { Int32 = 5, Value = 'a' });

            Assert.AreEqual(5, actual.Int32);
            Assert.AreEqual('a', actual.Value);

            [MethodImpl(MethodImplOptions.NoInlining)]
            static void Write<T>(ref byte b, T value) where T : unmanaged => Unsafe.WriteUnaligned<T>(ref b, value);
        }

        [Test]
        public static unsafe void WriteUnaligned_Ptr_Int32()
        {
            byte[] unaligned = new byte[sizeof(Int32Double) + 1];

            fixed (byte* p = unaligned)
            {
                Unsafe.WriteUnaligned(p + 1, 123456789);
            }

            int actual = Int32Double.Aligned(unaligned).Int32;
            Assert.AreEqual(123456789, actual);
        }

        [Test]
        public static unsafe void WriteUnaligned_Ptr_Double()
        {
            byte[] unaligned = new byte[sizeof(Int32Double) + 1];

            fixed (byte* p = unaligned)
            {
                Unsafe.WriteUnaligned(p + 9, 3.42);
            }

            double actual = Int32Double.Aligned(unaligned).Double;
            Assert.AreEqual(3.42, actual);
        }

        [Test]
        public static unsafe void WriteUnaligned_Ptr_Struct()
        {
            byte[] unaligned = new byte[sizeof(Int32Double) + 1];

            fixed (byte* p = unaligned)
            {
                Unsafe.WriteUnaligned(p + 1, new Int32Double { Int32 = 123456789, Double = 3.42 });
            }

            Int32Double actual = Int32Double.Aligned(unaligned);
            Assert.AreEqual(123456789, actual.Int32);
            Assert.AreEqual(3.42, actual.Double);
        }

        [Test]
        public static void SkipInit()
        {
            // Validate that calling with primitive types works.

            Unsafe.SkipInit(out sbyte _);
            Unsafe.SkipInit(out byte _);
            Unsafe.SkipInit(out short _);
            Unsafe.SkipInit(out ushort _);
            Unsafe.SkipInit(out int _);
            Unsafe.SkipInit(out uint _);
            Unsafe.SkipInit(out long _);
            Unsafe.SkipInit(out ulong _);
            Unsafe.SkipInit(out float _);
            Unsafe.SkipInit(out double _);

            // Validate that calling on user-defined unmanaged structs works.

            Unsafe.SkipInit(out Byte4 _);
            Unsafe.SkipInit(out Byte4Short2 _);
            Unsafe.SkipInit(out Byte512 _);
            Unsafe.SkipInit(out Int32Double _);
        }

        [Test]
        public static void SkipInit_PreservesPrevious()
        {
            // Validate that calling on already initialized types preserves the previous value.
#pragma warning disable IDE0018, IDE0059
            sbyte sbyteValue = 1;
            Unsafe.SkipInit(out sbyteValue);
            Assert.AreEqual((sbyte)1, sbyteValue);

            byte byteValue = 2;
            Unsafe.SkipInit(out byteValue);
            Assert.AreEqual((byte)2, byteValue);

            short shortValue = 3;
            Unsafe.SkipInit(out shortValue);
            Assert.AreEqual((short)3, shortValue);

            ushort ushortValue = 4;
            Unsafe.SkipInit(out ushortValue);
            Assert.AreEqual((ushort)4, ushortValue);

            int intValue = 5;
            Unsafe.SkipInit(out intValue);
            Assert.AreEqual(5, intValue);

            uint uintValue = 6;
            Unsafe.SkipInit(out uintValue);
            Assert.AreEqual(6u, uintValue);

            long longValue = 7;
            Unsafe.SkipInit(out longValue);
            Assert.AreEqual(7L, longValue);

            ulong ulongValue = 8;
            Unsafe.SkipInit(out ulongValue);
            Assert.AreEqual(8ul, ulongValue);

            float floatValue = 9;
            Unsafe.SkipInit(out floatValue);
            Assert.AreEqual(9f, floatValue);

            double doubleValue = 10;
            Unsafe.SkipInit(out doubleValue);
            Assert.AreEqual(10d, doubleValue);

            Byte4 byte4Value = new() { B0 = 11, B1 = 12, B2 = 13, B3 = 14 };
            Unsafe.SkipInit(out byte4Value);
            Assert.AreEqual((byte)11, byte4Value.B0);
            Assert.AreEqual((byte)12, byte4Value.B1);
            Assert.AreEqual((byte)13, byte4Value.B2);
            Assert.AreEqual((byte)14, byte4Value.B3);

            Byte4Short2 byte4Short2Value = new() { B0 = 15, B1 = 16, B2 = 17, B3 = 18, S4 = 19, S6 = 20 };
            Unsafe.SkipInit(out byte4Short2Value);
            Assert.AreEqual((byte)15, byte4Short2Value.B0);
            Assert.AreEqual((byte)16, byte4Short2Value.B1);
            Assert.AreEqual((byte)17, byte4Short2Value.B2);
            Assert.AreEqual((byte)18, byte4Short2Value.B3);
            Assert.AreEqual((short)19, byte4Short2Value.S4);
            Assert.AreEqual((short)20, byte4Short2Value.S6);

            Int32Double int32DoubleValue = new() { Int32 = 21, Double = 22 };
            Unsafe.SkipInit(out int32DoubleValue);
            Assert.AreEqual(21, int32DoubleValue.Int32);
            Assert.AreEqual(22d, int32DoubleValue.Double);
#pragma warning restore IDE0018, IDE0059
        }

        [Test]
        public static unsafe void IsNullRef_NotNull()
        {
            // Validate that calling with a primitive type works.

            int intValue = 5;
            Assert.False(Unsafe.IsNullRef(ref intValue));

            // Validate that calling on user-defined unmanaged structs works.

            Int32Double int32DoubleValue = default;
            Assert.False(Unsafe.IsNullRef(ref int32DoubleValue));

            // Validate on ref created from a pointer

            int* p = (int*)1;
            Assert.False(Unsafe.IsNullRef(ref Unsafe.AsRef<int>(p)));
        }

        [Test]
        public static unsafe void IsNullRef_Null()
        {
            // Validate that calling with a primitive type works.

            Assert.True(Unsafe.IsNullRef(ref Unsafe.AsRef<int>(null)));

            // Validate that calling on user-defined unmanaged structs works.

            Assert.True(Unsafe.IsNullRef(ref Unsafe.AsRef<Int32Double>(null)));

            // Validate on ref created from a pointer

            int* p = (int*)0;
            Assert.True(Unsafe.IsNullRef(ref Unsafe.AsRef<int>(p)));
        }

        [Test]
        public static unsafe void NullRef()
        {
            // Validate that calling with a primitive type works.

            Assert.True(Unsafe.IsNullRef(ref Unsafe.NullRef<int>()));

            // Validate that calling on user-defined unmanaged structs works.

            Assert.True(Unsafe.IsNullRef(ref Unsafe.NullRef<Int32Double>()));

            // Validate that pinning results in a null pointer

            fixed (void* p = &Unsafe.NullRef<int>())
            {
                Assert.True(p == (void*)0);
            }

            // Validate that dereferencing a null ref throws a NullReferenceException

            _ = Assert.Throws<NullReferenceException>(() => Unsafe.NullRef<int>() = 42);
        }

        [Test]
        public static unsafe void BitCast()
        {
            // Conversion between differently sized types should fail

            Assert.Throws<NotSupportedException>(() => Unsafe.BitCast<int, long>(5));
            Assert.Throws<NotSupportedException>(() => Unsafe.BitCast<long, int>(5));

            // Conversion between floating-point and same sized integral should succeed

            Assert.AreEqual(0x8000_0000u, Unsafe.BitCast<float, uint>(-0.0f));
            Assert.AreEqual(float.PositiveInfinity, Unsafe.BitCast<uint, float>(0x7F80_0000u));

            // Conversion between same sized integers should succeed

            Assert.AreEqual(int.MinValue, Unsafe.BitCast<uint, int>(0x8000_0000u));
            Assert.AreEqual(0x8000_0000u, Unsafe.BitCast<int, uint>(int.MinValue));

            // Runtime requires that all types be at least 1-byte, so empty to empty should succeed

            EmptyA empty1 = new EmptyA();
            EmptyB empty2 = Unsafe.BitCast<EmptyA, EmptyB>(empty1);

            // ..., likewise, empty to/from byte should succeed

            byte empty3 = Unsafe.BitCast<EmptyA, byte>(empty1);
            EmptyA empty4 = Unsafe.BitCast<byte, EmptyA>(1);

            // ..., however, empty to/from a larger type should fail

            Assert.Throws<NotSupportedException>(() => Unsafe.BitCast<int, EmptyA>(5));
            Assert.Throws<NotSupportedException>(() => Unsafe.BitCast<EmptyA, int>(empty1));

            Assert.AreEqual(uint.MaxValue, (long)Unsafe.BitCast<int, uint>(-1));
            Assert.AreEqual(uint.MaxValue, (ulong)Unsafe.BitCast<int, uint>(-1));

            byte b = 255;
            sbyte sb = -1;

            Assert.AreEqual(255L, (long)Unsafe.BitCast<sbyte, byte>(sb));
            Assert.AreEqual(-1L, (long)Unsafe.BitCast<byte, sbyte>(b));

            Assert.AreEqual(255L, (long)Unsafe.BitCast<short, ushort>(b));
            Assert.AreEqual(ushort.MaxValue, (long)Unsafe.BitCast<short, ushort>(sb));
            Assert.AreEqual(255L, (long)Unsafe.BitCast<ushort, short>(b));

            Assert.AreEqual(255L, (long)Unsafe.BitCast<int, uint>(b));
            Assert.AreEqual(uint.MaxValue, (long)Unsafe.BitCast<int, uint>(sb));
            Assert.AreEqual(255L, (long)Unsafe.BitCast<uint, int>(b));

            Assert.AreEqual(255UL, Unsafe.BitCast<long, ulong>(b));
            Assert.AreEqual(ulong.MaxValue, Unsafe.BitCast<long, ulong>(sb));
            Assert.AreEqual(255L, Unsafe.BitCast<ulong, long>(b));

            S2 s2 = BitConverter.IsLittleEndian ? new S2(255, 0) : new S2(0, 255);
            S4 s4 = BitConverter.IsLittleEndian ? new S4(255, 0, 0, 0) : new S4(0, 0, 0, 255);
            S8 s8 = BitConverter.IsLittleEndian ? new S8(255, 0, 0, 0, 0, 0, 0, 0) : new S8(0, 0, 0, 0, 0, 0, 0, 255);

            Assert.AreEqual(s2, Unsafe.BitCast<ushort, S2>(b));
            Assert.AreEqual(s2, Unsafe.BitCast<short, S2>(b));
            Assert.AreEqual(new S2(255, 255), Unsafe.BitCast<short, S2>(sb));

            Assert.AreEqual(s4, Unsafe.BitCast<uint, S4>(b));
            Assert.AreEqual(s4, Unsafe.BitCast<int, S4>(b));
            Assert.AreEqual(new S4(255, 255, 255, 255), Unsafe.BitCast<int, S4>(sb));

            Assert.AreEqual(s8, Unsafe.BitCast<ulong, S8>(b));
            Assert.AreEqual(s8, Unsafe.BitCast<long, S8>(b));
            Assert.AreEqual(new S8(255, 255, 255, 255, 255, 255, 255, 255), Unsafe.BitCast<long, S8>(sb));

            Assert.AreEqual((ushort)255, Unsafe.BitCast<S2, ushort>(s2));
            Assert.AreEqual((short)255, Unsafe.BitCast<S2, short>(s2));
            Assert.AreEqual(255U, Unsafe.BitCast<S4, uint>(s4));
            Assert.AreEqual(255, Unsafe.BitCast<S4, int>(s4));
            Assert.AreEqual(255UL, Unsafe.BitCast<S8, ulong>(s8));
            Assert.AreEqual(255L, Unsafe.BitCast<S8, long>(s8));

            byte* misalignedPtr = stackalloc byte[9];
            misalignedPtr += 1;

            *misalignedPtr = 255;

            Assert.AreEqual(s2, Unsafe.BitCast<ushort, S2>(*misalignedPtr));
            Assert.AreEqual(s2, Unsafe.BitCast<short, S2>(*misalignedPtr));
            Assert.AreEqual(new S2(255, 255), Unsafe.BitCast<short, S2>(*(sbyte*)misalignedPtr));

            Assert.AreEqual(s4, Unsafe.BitCast<uint, S4>(*misalignedPtr));
            Assert.AreEqual(s4, Unsafe.BitCast<int, S4>(*misalignedPtr));
            Assert.AreEqual(new S4(255, 255, 255, 255), Unsafe.BitCast<int, S4>(*(sbyte*)misalignedPtr));

            Assert.AreEqual(s8, Unsafe.BitCast<ulong, S8>(*misalignedPtr));
            Assert.AreEqual(s8, Unsafe.BitCast<long, S8>(*misalignedPtr));
            Assert.AreEqual(new S8(255, 255, 255, 255, 255, 255, 255, 255), Unsafe.BitCast<long, S8>(*(sbyte*)misalignedPtr));

            *(S2*)misalignedPtr = s2;
            Assert.AreEqual((ushort)255, Unsafe.BitCast<S2, ushort>(*(S2*)misalignedPtr));
            Assert.AreEqual((short)255, Unsafe.BitCast<S2, short>(*(S2*)misalignedPtr));
            *(S4*)misalignedPtr = s4;
            Assert.AreEqual(255U, Unsafe.BitCast<S4, uint>(*(S4*)misalignedPtr));
            Assert.AreEqual(255, Unsafe.BitCast<S4, int>(*(S4*)misalignedPtr));
            *(S8*)misalignedPtr = s8;
            Assert.AreEqual(255UL, Unsafe.BitCast<S8, ulong>(*(S8*)misalignedPtr));
            Assert.AreEqual(255L, Unsafe.BitCast<S8, long>(*(S8*)misalignedPtr));

            float s = Unsafe.ReadUnaligned<float>(ref Unsafe.As<S4, byte>(ref s4));
            double d = Unsafe.ReadUnaligned<double>(ref Unsafe.As<S8, byte>(ref s8));

            Assert.AreEqual(s, Unsafe.BitCast<S4, float>(s4));
            Assert.AreEqual(d, Unsafe.BitCast<S8, double>(s8));

            *(S2*)misalignedPtr = s2;
            *(S4*)misalignedPtr = s4;
            Assert.AreEqual(s, Unsafe.BitCast<S4, float>(*(S4*)misalignedPtr));
            *(S8*)misalignedPtr = s8;
            Assert.AreEqual(d, Unsafe.BitCast<S8, double>(*(S8*)misalignedPtr));
        }
    }

    [StructLayout(LayoutKind.Sequential)] public record struct S2(byte a, byte b);
    [StructLayout(LayoutKind.Sequential)] public record struct S4(byte a, byte b, byte c, byte d);
    [StructLayout(LayoutKind.Sequential)] public record struct S8(byte a, byte b, byte c, byte d, byte e, byte f, byte g, byte h);

    [StructLayout(LayoutKind.Explicit)]
    public struct Byte4
    {
        [FieldOffset(0)]
        public byte B0;
        [FieldOffset(1)]
        public byte B1;
        [FieldOffset(2)]
        public byte B2;
        [FieldOffset(3)]
        public byte B3;
    }

    [StructLayout(LayoutKind.Explicit)]
    public struct Byte4Short2
    {
        [FieldOffset(0)]
        public byte B0;
        [FieldOffset(1)]
        public byte B1;
        [FieldOffset(2)]
        public byte B2;
        [FieldOffset(3)]
        public byte B3;
        [FieldOffset(4)]
        public short S4;
        [FieldOffset(6)]
        public short S6;
    }

    public unsafe struct Byte512
    {
        public fixed byte Bytes[512];
    }

    [StructLayout(LayoutKind.Explicit, Size = 16)]
    public unsafe struct Int32Double
    {
        [FieldOffset(0)]
        public int Int32;
        [FieldOffset(8)]
        public double Double;

        public static unsafe byte[] Unaligned(int i, double d)
        {
            Int32Double aligned = new() { Int32 = i, Double = d };
            byte[] unaligned = new byte[sizeof(Int32Double) + 1];
            Int32Double* ptr = &aligned;
            Marshal.Copy((nint)ptr, unaligned, 1, sizeof(Int32Double));
            return unaligned;
        }

        public static unsafe Int32Double Aligned(byte[] unaligned)
        {
            fixed (byte* p = unaligned)
            {
                return *(Int32Double*)(p + 1);
            }
        }
    }

    public struct StringInt32
    {
        public string String;
        public int Int32;
    }

    public struct Int32Generic<T> where T : unmanaged
    {
        public int Int32;
        public T Value;
    }

    public struct Single4
    {
        public float X;
        public float Y;
        public float Z;
        public float W;
    }

    public struct EmptyA;

    public struct EmptyB;
}
