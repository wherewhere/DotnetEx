using NUnit.Compatibility;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace DotnetEx.Test
{
    /// <summary>
    /// The tests for the <see cref="EnumEx"/> class.
    /// </summary>
    [TestFixture]
    public static class EnumTests
    {
        #region EnumTypes

        public enum SimpleEnum
        {
            Red = 1,
            Blue = 2,
            Green = 3,
            Green_a = 3,
            Green_b = 3,
            B = 4
        }

        public enum ByteEnum : byte
        {
            Min = byte.MinValue,
            One = 1,
            Two = 2,
            Max = byte.MaxValue,
        }

        public enum SByteEnum : sbyte
        {
            Min = sbyte.MinValue,
            One = 1,
            Two = 2,
            Max = sbyte.MaxValue,
        }

        public enum UInt16Enum : ushort
        {
            Min = ushort.MinValue,
            One = 1,
            Two = 2,
            Max = ushort.MaxValue,
        }

        public enum Int16Enum : short
        {
            Min = short.MinValue,
            One = 1,
            Two = 2,
            Max = short.MaxValue,
        }

        public enum UInt32Enum : uint
        {
            Min = uint.MinValue,
            One = 1,
            Two = 2,
            Max = uint.MaxValue,
        }

        public enum Int32Enum : int
        {
            Min = int.MinValue,
            One = 1,
            Two = 2,
            Max = int.MaxValue,
        }

        public enum UInt64Enum : ulong
        {
            Min = ulong.MinValue,
            One = 1,
            Two = 2,
            Max = ulong.MaxValue,
        }

        public enum Int64Enum : long
        {
            Min = long.MinValue,
            One = 1,
            Two = 2,
            Max = long.MaxValue,
        }

        #endregion

        public static IEnumerable<TestCaseData> Parse_TestData
        {
            get
            {
                // SByte
                yield return new TestCaseData("Min", false, SByteEnum.Min);
                yield return new TestCaseData("mAx", true, SByteEnum.Max);
                yield return new TestCaseData("1", false, SByteEnum.One);
                yield return new TestCaseData("5", false, (SByteEnum)5);
                yield return new TestCaseData(sbyte.MinValue.ToString(), false, (SByteEnum)sbyte.MinValue);
                yield return new TestCaseData(sbyte.MaxValue.ToString(), false, (SByteEnum)sbyte.MaxValue);

                // Byte
                yield return new TestCaseData("Min", false, ByteEnum.Min);
                yield return new TestCaseData("mAx", true, ByteEnum.Max);
                yield return new TestCaseData("1", false, ByteEnum.One);
                yield return new TestCaseData("5", false, (ByteEnum)5);
                yield return new TestCaseData(byte.MinValue.ToString(), false, (ByteEnum)byte.MinValue);
                yield return new TestCaseData(byte.MaxValue.ToString(), false, (ByteEnum)byte.MaxValue);

                // Int16
                yield return new TestCaseData("Min", false, Int16Enum.Min);
                yield return new TestCaseData("mAx", true, Int16Enum.Max);
                yield return new TestCaseData("1", false, Int16Enum.One);
                yield return new TestCaseData("5", false, (Int16Enum)5);
                yield return new TestCaseData(short.MinValue.ToString(), false, (Int16Enum)short.MinValue);
                yield return new TestCaseData(short.MaxValue.ToString(), false, (Int16Enum)short.MaxValue);

                // UInt16
                yield return new TestCaseData("Min", false, UInt16Enum.Min);
                yield return new TestCaseData("mAx", true, UInt16Enum.Max);
                yield return new TestCaseData("1", false, UInt16Enum.One);
                yield return new TestCaseData("5", false, (UInt16Enum)5);
                yield return new TestCaseData(ushort.MinValue.ToString(), false, (UInt16Enum)ushort.MinValue);
                yield return new TestCaseData(ushort.MaxValue.ToString(), false, (UInt16Enum)ushort.MaxValue);

                // Int32
                yield return new TestCaseData("Min", false, Int32Enum.Min);
                yield return new TestCaseData("mAx", true, Int32Enum.Max);
                yield return new TestCaseData("1", false, Int32Enum.One);
                yield return new TestCaseData("5", false, (Int32Enum)5);
                yield return new TestCaseData(int.MinValue.ToString(), false, (Int32Enum)int.MinValue);
                yield return new TestCaseData(int.MaxValue.ToString(), false, (Int32Enum)int.MaxValue);

                // UInt32
                yield return new TestCaseData("Min", false, UInt32Enum.Min);
                yield return new TestCaseData("mAx", true, UInt32Enum.Max);
                yield return new TestCaseData("1", false, UInt32Enum.One);
                yield return new TestCaseData("5", false, (UInt32Enum)5);
                yield return new TestCaseData(uint.MinValue.ToString(), false, (UInt32Enum)uint.MinValue);
                yield return new TestCaseData(uint.MaxValue.ToString(), false, (UInt32Enum)uint.MaxValue);

                // Int64
                yield return new TestCaseData("Min", false, Int64Enum.Min);
                yield return new TestCaseData("mAx", true, Int64Enum.Max);
                yield return new TestCaseData("1", false, Int64Enum.One);
                yield return new TestCaseData("5", false, (Int64Enum)5);
                yield return new TestCaseData(long.MinValue.ToString(), false, (Int64Enum)long.MinValue);
                yield return new TestCaseData(long.MaxValue.ToString(), false, (Int64Enum)long.MaxValue);

                // UInt64
                yield return new TestCaseData("Min", false, UInt64Enum.Min);
                yield return new TestCaseData("mAx", true, UInt64Enum.Max);
                yield return new TestCaseData("1", false, UInt64Enum.One);
                yield return new TestCaseData("5", false, (UInt64Enum)5);
                yield return new TestCaseData(ulong.MinValue.ToString(), false, (UInt64Enum)ulong.MinValue);
                yield return new TestCaseData(ulong.MaxValue.ToString(), false, (UInt64Enum)ulong.MaxValue);

                // SimpleEnum
                yield return new TestCaseData("Red", false, SimpleEnum.Red);
                yield return new TestCaseData(" Red", false, SimpleEnum.Red);
                yield return new TestCaseData("Red ", false, SimpleEnum.Red);
                yield return new TestCaseData(" red ", true, SimpleEnum.Red);
                yield return new TestCaseData("B", false, SimpleEnum.B);
                yield return new TestCaseData("B,B", false, SimpleEnum.B);
                yield return new TestCaseData(" Red , Blue ", false, SimpleEnum.Red | SimpleEnum.Blue);
                yield return new TestCaseData("Blue,Red,Green", false, SimpleEnum.Red | SimpleEnum.Blue | SimpleEnum.Green);
                yield return new TestCaseData("Blue,Red,Red,Red,Green", false, SimpleEnum.Red | SimpleEnum.Blue | SimpleEnum.Green);
                yield return new TestCaseData("Red,Blue,   Green", false, SimpleEnum.Red | SimpleEnum.Blue | SimpleEnum.Green);
                yield return new TestCaseData("1", false, SimpleEnum.Red);
                yield return new TestCaseData(" 1 ", false, SimpleEnum.Red);
                yield return new TestCaseData("2", false, SimpleEnum.Blue);
                yield return new TestCaseData("99", false, (SimpleEnum)99);
                yield return new TestCaseData("-42", false, (SimpleEnum)(-42));
                yield return new TestCaseData("   -42", false, (SimpleEnum)(-42));
                yield return new TestCaseData("   -42 ", false, (SimpleEnum)(-42));
            }
        }

        public static IEnumerable<TestCaseData> Parse_Invalid_TestData
        {
            get
            {
                yield return new TestCaseData(null, "", false, typeof(ArgumentNullException));
                yield return new TestCaseData(typeof(object), "", false, typeof(ArgumentException));
                yield return new TestCaseData(typeof(int), "", false, typeof(ArgumentException));

                yield return new TestCaseData(typeof(SimpleEnum), null, false, typeof(ArgumentNullException));
                yield return new TestCaseData(typeof(SimpleEnum), "", false, typeof(ArgumentException));
                yield return new TestCaseData(typeof(SimpleEnum), "    \t", false, typeof(ArgumentException));
                yield return new TestCaseData(typeof(SimpleEnum), " red ", false, typeof(ArgumentException));
                yield return new TestCaseData(typeof(SimpleEnum), "Purple", false, typeof(ArgumentException));
                yield return new TestCaseData(typeof(SimpleEnum), ",Red", false, typeof(ArgumentException));
                yield return new TestCaseData(typeof(SimpleEnum), "Red,", false, typeof(ArgumentException));
                yield return new TestCaseData(typeof(SimpleEnum), "B,", false, typeof(ArgumentException));
                yield return new TestCaseData(typeof(SimpleEnum), " , , ,", false, typeof(ArgumentException));
                yield return new TestCaseData(typeof(SimpleEnum), "Red,Blue,", false, typeof(ArgumentException));
                yield return new TestCaseData(typeof(SimpleEnum), "Red,,Blue", false, typeof(ArgumentException));
                yield return new TestCaseData(typeof(SimpleEnum), "Red,Blue, ", false, typeof(ArgumentException));
                yield return new TestCaseData(typeof(SimpleEnum), "Red Blue", false, typeof(ArgumentException));
                yield return new TestCaseData(typeof(SimpleEnum), "1,Blue", false, typeof(ArgumentException));
                yield return new TestCaseData(typeof(SimpleEnum), "1,1", false, typeof(ArgumentException));
                yield return new TestCaseData(typeof(SimpleEnum), "Blue,1", false, typeof(ArgumentException));
                yield return new TestCaseData(typeof(SimpleEnum), "Blue, 1", false, typeof(ArgumentException));

                yield return new TestCaseData(typeof(ByteEnum), "-1", false, typeof(OverflowException));
                yield return new TestCaseData(typeof(ByteEnum), "256", false, typeof(OverflowException));

                yield return new TestCaseData(typeof(SByteEnum), "-129", false, typeof(OverflowException));
                yield return new TestCaseData(typeof(SByteEnum), "128", false, typeof(OverflowException));

                yield return new TestCaseData(typeof(Int16Enum), "-32769", false, typeof(OverflowException));
                yield return new TestCaseData(typeof(Int16Enum), "32768", false, typeof(OverflowException));

                yield return new TestCaseData(typeof(UInt16Enum), "-1", false, typeof(OverflowException));
                yield return new TestCaseData(typeof(UInt16Enum), "65536", false, typeof(OverflowException));

                yield return new TestCaseData(typeof(Int32Enum), "-2147483649", false, typeof(OverflowException));
                yield return new TestCaseData(typeof(Int32Enum), "2147483648", false, typeof(OverflowException));

                yield return new TestCaseData(typeof(UInt32Enum), "-1", false, typeof(OverflowException));
                yield return new TestCaseData(typeof(UInt32Enum), "4294967296", false, typeof(OverflowException));

                yield return new TestCaseData(typeof(Int64Enum), "-9223372036854775809", false, typeof(OverflowException));
                yield return new TestCaseData(typeof(Int64Enum), "9223372036854775808", false, typeof(OverflowException));

                yield return new TestCaseData(typeof(UInt64Enum), "-1", false, typeof(OverflowException));
                yield return new TestCaseData(typeof(UInt64Enum), "18446744073709551616", false, typeof(OverflowException));
            }
        }

        [TestCaseSource(typeof(EnumTests), nameof(Parse_TestData))]
        public static void Parse<T>(string value, bool ignoreCase, T expected) where T : struct
        {
            T result;
            if (!ignoreCase)
            {
                Assert.True(Enum.TryParse(value, out result));
                Assert.AreEqual(expected, result);

                Assert.AreEqual(expected, Enum.Parse(expected.GetType(), value));
                Assert.AreEqual(expected, Enum.Parse<T>(value));
            }

            Assert.True(Enum.TryParse(value, ignoreCase, out result));
            Assert.AreEqual(expected, result);

            Assert.AreEqual(expected, Enum.Parse(expected.GetType(), value, ignoreCase));
            Assert.AreEqual(expected, Enum.Parse<T>(value, ignoreCase));
        }

        [TestCaseSource(typeof(EnumTests), nameof(Parse_Invalid_TestData))]
        public static void Parse_Invalid(Type enumType, string value, bool ignoreCase, Type exceptionType)
        {
            Type typeArgument = enumType == null || !enumType.GetTypeInfo().IsEnum ? typeof(SimpleEnum) : enumType;
            MethodInfo parseMethod = typeof(EnumTests).GetTypeInfo().GetMethod(nameof(Parse_Generic_Invalid), BindingFlags.Static | BindingFlags.NonPublic).MakeGenericMethod(typeArgument);
            parseMethod.Invoke(null, [enumType, value, ignoreCase, exceptionType]);
        }

        private static void Parse_Generic_Invalid<T>(Type enumType, string value, bool ignoreCase, Type exceptionType) where T : struct
        {
            object result = null;
            if (!ignoreCase)
            {
                if (enumType != null && enumType.IsEnum)
                {
                    Assert.False(Enum.TryParse(enumType, value, out result));
                    Assert.AreEqual(default, result);

                    Assert.Throws(exceptionType, () => Enum.Parse<T>(value));
                }
                else
                {
                    Assert.Throws(exceptionType, () => Enum.TryParse(enumType, value, out result));
                    Assert.AreEqual(default, result);
                }
            }

            if (enumType != null && enumType.IsEnum)
            {
                Assert.False(Enum.TryParse(enumType, value, ignoreCase, out result));
                Assert.AreEqual(default, result);

                Assert.Throws(exceptionType, () => Enum.Parse<T>(value, ignoreCase));
            }
            else
            {
                Assert.Throws(exceptionType, () => Enum.TryParse(enumType, value, ignoreCase, out result));
                Assert.AreEqual(default, result);
            }
        }
    }
}
