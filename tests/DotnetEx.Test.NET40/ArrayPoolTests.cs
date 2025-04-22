using NUnit.Framework;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;

namespace DotnetEx.Test
{
    /// <summary>
    /// The tests for the <see cref="ArrayPool{T}"/> class.
    /// </summary>
    [TestFixture]
    public static class ArrayPoolTests
    {
        private struct TestStruct
        {
            internal string InternalRef;
        }

        /*
            NOTE - due to test parallelism and sharing, use an instance pool for testing unless necessary
        */
        [Test]
        public static void SharedInstanceCreatesAnInstanceOnFirstCall()
        {
            Assert.NotNull(ArrayPool<byte>.Shared);
        }

        [Test]
        public static void SharedInstanceOnlyCreatesOneInstanceOfOneTypep()
        {
            ArrayPool<byte> instance = ArrayPool<byte>.Shared;
            Assert.AreSame(instance, ArrayPool<byte>.Shared);
        }

        [Test]
        public static void CreateWillCreateMultipleInstancesOfTheSameType()
        {
            Assert.AreNotSame(ArrayPool<byte>.Create(), ArrayPool<byte>.Create());
        }

        [TestCase(0)]
        [TestCase(-1)]
        public static void CreatingAPoolWithInvalidArrayCountThrows(int length)
        {
            Assert.AreEqual("maxArraysPerBucket", Assert.Throws<ArgumentOutOfRangeException>(() => ArrayPool<byte>.Create(maxArraysPerBucket: length, maxArrayLength: 16)).ParamName);
        }

        [TestCase(0)]
        [TestCase(-1)]
        public static void CreatingAPoolWithInvalidMaximumArraySizeThrows(int length)
        {
            Assert.AreEqual("maxArrayLength", Assert.Throws<ArgumentOutOfRangeException>(() => ArrayPool<byte>.Create(maxArrayLength: length, maxArraysPerBucket: 1)).ParamName);
        }

        [TestCase(1)]
        [TestCase(16)]
        [TestCase(0x40000000)]
        [TestCase(0x7FFFFFFF)]
        public static void CreatingAPoolWithValidMaximumArraySizeSucceeds(int length)
        {
            var pool = ArrayPool<byte>.Create(maxArrayLength: length, maxArraysPerBucket: 1);
            Assert.NotNull(pool);
            Assert.NotNull(pool.Rent(1));
        }

        [TestCaseSource(nameof(BytePoolInstances))]
        public static void RentingWithInvalidLengthThrows(ArrayPool<byte> pool)
        {
            Assert.AreEqual("minimumLength", Assert.Throws<ArgumentOutOfRangeException>(() => pool.Rent(-1)).ParamName);
        }

        [Test]
        public static void RentingGiganticArraySucceedsOrOOMs()
        {
            try
            {
                int len = 0x70000000;
                byte[] buffer = ArrayPool<byte>.Shared.Rent(len);
                Assert.NotNull(buffer);
                Assert.True(buffer.Length >= len);
            }
            catch (OutOfMemoryException) { }
        }

        [TestCaseSource(nameof(BytePoolInstances))]
        public static void Renting0LengthArrayReturnsSingleton(ArrayPool<byte> pool)
        {
            byte[] zero0 = pool.Rent(0);
            byte[] zero1 = pool.Rent(0);
            byte[] zero2 = pool.Rent(0);
            byte[] one = pool.Rent(1);

            Assert.AreSame(zero0, zero1);
            Assert.AreSame(zero1, zero2);
            Assert.AreNotSame(zero2, one);

            pool.Return(zero0);
            pool.Return(zero1);
            pool.Return(zero2);
            pool.Return(one);

            Assert.AreSame(zero0, pool.Rent(0));
        }

        [Test]
        public static void RentingMultipleArraysGivesBackDifferentInstances()
        {
            ArrayPool<byte> instance = ArrayPool<byte>.Create(maxArraysPerBucket: 2, maxArrayLength: 16);
            Assert.AreNotSame(instance.Rent(100), instance.Rent(100));
        }

        [Test]
        public static void RentingMoreArraysThanSpecifiedInCreateWillStillSucceed()
        {
            ArrayPool<byte> instance = ArrayPool<byte>.Create(maxArraysPerBucket: 1, maxArrayLength: 16);
            Assert.NotNull(instance.Rent(100));
            Assert.NotNull(instance.Rent(100));
        }

        [Test]
        public static void RentCanReturnBiggerArraySizeThanRequested()
        {
            ArrayPool<byte> pool = ArrayPool<byte>.Create(maxArraysPerBucket: 1, maxArrayLength: 32);
            byte[] rented = pool.Rent(27);
            Assert.NotNull(rented);
            Assert.AreEqual(32, rented.Length);
        }

        [Test]
        public static void RentingAnArrayWithLengthGreaterThanSpecifiedInCreateStillSucceeds()
        {
            Assert.NotNull(ArrayPool<byte>.Create(maxArrayLength: 100, maxArraysPerBucket: 1).Rent(200));
        }

        [TestCaseSource(nameof(BytePoolInstances))]
        public static void CallingReturnBufferWithNullBufferThrows(ArrayPool<byte> pool)
        {
            Assert.AreEqual("array", Assert.Throws<ArgumentNullException>(() => pool.Return(null)).ParamName);
        }

        private static void FillArray(byte[] buffer)
        {
            for (byte i = 0; i < buffer.Length; i++)
                buffer[i] = i;
        }

        private static void CheckFilledArray(byte[] buffer, Action<byte, byte> assert)
        {
            for (byte i = 0; i < buffer.Length; i++)
            {
                assert(buffer[i], i);
            }
        }

        [TestCaseSource(nameof(BytePoolInstances))]
        public static void CallingReturnWithoutClearingDoesNotClearTheBuffer(ArrayPool<byte> pool)
        {
            byte[] buffer = pool.Rent(4);
            FillArray(buffer);
            pool.Return(buffer, clearArray: false);
            CheckFilledArray(buffer, (b1, b2) => Assert.AreEqual(b1, b2));
        }

        [TestCaseSource(nameof(BytePoolInstances))]
        public static void CallingReturnWithClearingDoesClearTheBuffer(ArrayPool<byte> pool)
        {
            byte[] buffer = pool.Rent(4);
            FillArray(buffer);

            // Note - yes this is bad to hold on to the old instance but we need to validate the contract
            pool.Return(buffer, clearArray: true);
            CheckFilledArray(buffer, (b1, b2) => Assert.AreEqual(default(byte), b1));
        }

        [Test]
        public static void CallingReturnOnReferenceTypeArrayDoesNotClearTheArray()
        {
            ArrayPool<string> pool = ArrayPool<string>.Create();
            string[] array = pool.Rent(2);
            array[0] = "foo";
            array[1] = "bar";
            pool.Return(array, clearArray: false);
            Assert.NotNull(array[0]);
            Assert.NotNull(array[1]);
        }

        [Test]
        public static void CallingReturnOnReferenceTypeArrayAndClearingSetsTypesToNull()
        {
            ArrayPool<string> pool = ArrayPool<string>.Create();
            string[] array = pool.Rent(2);
            array[0] = "foo";
            array[1] = "bar";
            pool.Return(array, clearArray: true);
            Assert.Null(array[0]);
            Assert.Null(array[1]);
        }

        [Test]
        public static void CallingReturnOnValueTypeWithInternalReferenceTypesAndClearingSetsValueTypeToDefault()
        {
            ArrayPool<TestStruct> pool = ArrayPool<TestStruct>.Create();
            TestStruct[] array = pool.Rent(2);
            array[0].InternalRef = "foo";
            array[1].InternalRef = "bar";
            pool.Return(array, clearArray: true);
            Assert.AreEqual(default(TestStruct), array[0]);
            Assert.AreEqual(default(TestStruct), array[1]);
        }

        [Test]
        public static void TakingAllBuffersFromABucketPlusAnAllocatedOneShouldAllowReturningAllBuffers()
        {
            ArrayPool<byte> pool = ArrayPool<byte>.Create(maxArrayLength: 16, maxArraysPerBucket: 1);
            byte[] rented = pool.Rent(16);
            byte[] allocated = pool.Rent(16);
            pool.Return(rented);
            pool.Return(allocated);
        }

        [Test]
        public static void NewDefaultArrayPoolWithSmallBufferSizeRoundsToOurSmallestSupportedSize()
        {
            ArrayPool<byte> pool = ArrayPool<byte>.Create(maxArrayLength: 8, maxArraysPerBucket: 1);
            byte[] rented = pool.Rent(8);
            Assert.True(rented.Length == 16);
        }

        [Test]
        public static void ReturningToCreatePoolABufferGreaterThanMaxSizeDoesNotThrow()
        {
            ArrayPool<byte> pool = ArrayPool<byte>.Create(maxArrayLength: 16, maxArraysPerBucket: 1);
            byte[] rented = pool.Rent(32);
            pool.Return(rented);
        }

        [Test]
        public static void RentingAllBuffersAndCallingRentAgainWillAllocateBufferAndReturnIt()
        {
            ArrayPool<byte> pool = ArrayPool<byte>.Create(maxArrayLength: 16, maxArraysPerBucket: 1);
            byte[] rented1 = pool.Rent(16);
            byte[] rented2 = pool.Rent(16);
            Assert.NotNull(rented1);
            Assert.NotNull(rented2);
        }

        [Test]
        public static void RentingReturningThenRentingABufferShouldNotAllocate()
        {
            ArrayPool<byte> pool = ArrayPool<byte>.Create(maxArrayLength: 16, maxArraysPerBucket: 1);
            byte[] bt = pool.Rent(16);
            int id = bt.GetHashCode();
            pool.Return(bt);
            bt = pool.Rent(16);
            Assert.AreEqual(id, bt.GetHashCode());
        }

        [TestCaseSource(nameof(BytePoolInstances))]
        public static void CanRentManySizedBuffers(ArrayPool<byte> pool)
        {
            for (int i = 1; i < 10000; i++)
            {
                byte[] buffer = pool.Rent(i);
                Assert.AreEqual(i <= 16 ? 16 : (int)BitOperations.RoundUpToPowerOf2((uint)i), buffer.Length);
                pool.Return(buffer);
            }
        }

        [TestCase(1, 16)]
        [TestCase(15, 16)]
        [TestCase(16, 16)]
        [TestCase(1023, 1024)]
        [TestCase(1024, 1024)]
        [TestCase(4096, 4096)]
        [TestCase(1024 * 1024, 1024 * 1024)]
        [TestCase(1024 * 1024 * 2, 1024 * 1024 * 2)]
        public static void RentingSpecificLengthsYieldsExpectedLengths(int requestedMinimum, int expectedLength)
        {
            foreach (ArrayPool<byte> pool in new[] { ArrayPool<byte>.Create((int)BitOperations.RoundUpToPowerOf2((uint)requestedMinimum), 1), ArrayPool<byte>.Shared })
            {
                byte[] buffer1 = pool.Rent(requestedMinimum);
                byte[] buffer2 = pool.Rent(requestedMinimum);

                Assert.NotNull(buffer1);
                Assert.AreEqual(expectedLength, buffer1.Length);

                Assert.NotNull(buffer2);
                Assert.AreEqual(expectedLength, buffer2.Length);

                Assert.AreNotSame(buffer1, buffer2);

                pool.Return(buffer2);
                pool.Return(buffer1);
            }

            foreach (ArrayPool<char> pool in new[] { ArrayPool<char>.Create((int)BitOperations.RoundUpToPowerOf2((uint)requestedMinimum), 1), ArrayPool<char>.Shared })
            {
                char[] buffer1 = pool.Rent(requestedMinimum);
                char[] buffer2 = pool.Rent(requestedMinimum);

                Assert.NotNull(buffer1);
                Assert.AreEqual(expectedLength, buffer1.Length);

                Assert.NotNull(buffer2);
                Assert.AreEqual(expectedLength, buffer2.Length);

                Assert.AreNotSame(buffer1, buffer2);

                pool.Return(buffer2);
                pool.Return(buffer1);
            }

            foreach (ArrayPool<string> pool in new[] { ArrayPool<string>.Create((int)BitOperations.RoundUpToPowerOf2((uint)requestedMinimum), 1), ArrayPool<string>.Shared })
            {
                string[] buffer1 = pool.Rent(requestedMinimum);
                string[] buffer2 = pool.Rent(requestedMinimum);

                Assert.NotNull(buffer1);
                Assert.AreEqual(expectedLength, buffer1.Length);

                Assert.NotNull(buffer2);
                Assert.AreEqual(expectedLength, buffer2.Length);

                Assert.AreNotSame(buffer1, buffer2);

                pool.Return(buffer2);
                pool.Return(buffer1);
            }
        }

        [Test]
        public static void RentingAfterPoolExhaustionReturnsSizeForCorrespondingBucket_SmallerThanLimit()
        {
            ArrayPool<byte> pool = ArrayPool<byte>.Create(maxArrayLength: 64, maxArraysPerBucket: 2);

            Assert.AreEqual(16, pool.Rent(15).Length); // try initial bucket
            Assert.AreEqual(16, pool.Rent(15).Length);

            Assert.AreEqual(32, pool.Rent(15).Length); // try one more level
            Assert.AreEqual(32, pool.Rent(15).Length);

            Assert.AreEqual(16, pool.Rent(15).Length); // fall back to original size
        }

        [Test]
        public static void RentingAfterPoolExhaustionReturnsSizeForCorrespondingBucket_JustBelowLimit()
        {
            ArrayPool<byte> pool = ArrayPool<byte>.Create(maxArrayLength: 64, maxArraysPerBucket: 2);

            Assert.AreEqual(32, pool.Rent(31).Length); // try initial bucket
            Assert.AreEqual(32, pool.Rent(31).Length);

            Assert.AreEqual(64, pool.Rent(31).Length); // try one more level
            Assert.AreEqual(64, pool.Rent(31).Length);

            Assert.AreEqual(32, pool.Rent(31).Length); // fall back to original size
        }

        [Test]
        public static void RentingAfterPoolExhaustionReturnsSizeForCorrespondingBucket_AtLimit()
        {
            ArrayPool<byte> pool = ArrayPool<byte>.Create(maxArrayLength: 64, maxArraysPerBucket: 2);

            Assert.AreEqual(64, pool.Rent(63).Length); // try initial bucket
            Assert.AreEqual(64, pool.Rent(63).Length);

            Assert.AreEqual(64, pool.Rent(63).Length); // still get original size
        }

        [TestCaseSource(nameof(BytePoolInstances))]
        public static void ReturningANonPooledBufferOfDifferentSizeToThePoolThrows(ArrayPool<byte> pool)
        {
            Assert.AreEqual("array", Assert.Throws<ArgumentException>(() => pool.Return(new byte[1])).ParamName);
        }

        [TestCaseSource(nameof(BytePoolInstances))]
        public static void RentAndReturnManyOfTheSameSize_NoneAreSame(ArrayPool<byte> pool)
        {
            foreach (int length in new[] { 1, 16, 32, 64, 127, 4096, 4097 })
            {
                for (int iter = 0; iter < 2; iter++)
                {
                    var buffers = new HashSet<byte[]>();
                    for (int i = 0; i < 100; i++)
                    {
                        buffers.Add(pool.Rent(length));
                    }

                    Assert.AreEqual(100, buffers.Count);

                    foreach (byte[] buffer in buffers)
                    {
                        pool.Return(buffer);
                    }
                }
            }
        }

        [TestCaseSource(nameof(BytePoolInstances))]
        public static void UsePoolInParallel(ArrayPool<byte> pool)
        {
            int[] sizes = [16, 32, 64, 128];
            Parallel.For(0, 250000, i =>
            {
                foreach (int size in sizes)
                {
                    byte[] array = pool.Rent(size);
                    Assert.NotNull(array);
                    Assert.GreaterOrEqual(array.Length, size);
                    Assert.LessOrEqual(array.Length, int.MaxValue);
                    pool.Return(array);
                }
            });
        }

        [Test]
        public static void ConfigurablePool_AllocatedArraysAreCleared_string() => ConfigurablePool_AllocatedArraysAreCleared<string>();

        [Test]
        public static void ConfigurablePool_AllocatedArraysAreCleared_byte() => ConfigurablePool_AllocatedArraysAreCleared<byte>();

        [Test]
        public static void ConfigurablePool_AllocatedArraysAreCleared_DateTime() => ConfigurablePool_AllocatedArraysAreCleared<DateTime>();

        private static void ConfigurablePool_AllocatedArraysAreCleared<T>()
        {
            ArrayPool<T> pool = ArrayPool<T>.Create();
            for (int size = 1; size <= 1000; size++)
            {
                T[] arr = pool.Rent(size);
                for (int i = 0; i < arr.Length; i++)
                {
                    Assert.AreEqual(default(T), arr[i]);
                }
            }
        }

        public static IEnumerable<object[]> BytePoolInstances()
        {
            yield return new object[] { ArrayPool<byte>.Create() };
            yield return new object[] { ArrayPool<byte>.Create(1024 * 1024, 50) };
            yield return new object[] { ArrayPool<byte>.Create(1024 * 1024, 1) };
            yield return new object[] { ArrayPool<byte>.Shared };
        }
    }
}
