using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DotnetEx.Test
{
    /// <summary>
    /// The tests for the <see cref="StringEx"/> class.
    /// </summary>
    [TestFixture]
    public static class StringTests
    {
        public static IEnumerable<object[]> IsNullOrWhitespace_TestData()
        {
            for (int i = 0; i < char.MaxValue; i++)
            {
                if (char.IsWhiteSpace((char)i))
                {
                    yield return new object[] { new string((char)i, 3), true };
                    yield return new object[] { new string((char)i, 3) + "x", false };
                }
            }

            yield return new object[] { null, true };
            yield return new object[] { "", true };
            yield return new object[] { "foo", false };
        }

        [TestCase("$$", new string[] { }, 0, 0, "")]
        [TestCase("$$", new string[] { null }, 0, 1, "")]
        [TestCase("$$", new string[] { null, "Bar", null }, 0, 3, "$$Bar$$")]
        [TestCase("$$", new string[] { "", "", "" }, 0, 3, "$$$$")]
        [TestCase("", new string[] { "", "", "" }, 0, 3, "")]
        [TestCase(null, new string[] { "Foo", "Bar", "Baz" }, 0, 3, "FooBarBaz")]
        [TestCase("$$", new string[] { "Foo", "Bar", "Baz" }, 0, 3, "Foo$$Bar$$Baz")]
        [TestCase("$$", new string[] { "Foo", "Bar", "Baz" }, 3, 0, "")]
        [TestCase("$$", new string[] { "Foo", "Bar", "Baz" }, 1, 1, "Bar")]
        public static void Join_StringArray(string separator, string[] values, int startIndex, int count, string expected)
        {
            if (startIndex + count == values.Length && count != 0)
            {
                Assert.AreEqual(expected, string.Join(separator, values));

                List<string> iEnumerableStringOptimized = [.. values];
                Assert.AreEqual(expected, string.Join(separator, iEnumerableStringOptimized));
                Assert.AreEqual(expected, string.Join<string>(separator, iEnumerableStringOptimized)); // Call the generic IEnumerable<T>-based overload

                Queue<string> iEnumerableStringNotOptimized = new(values);
                Assert.AreEqual(expected, string.Join(separator, iEnumerableStringNotOptimized));
                Assert.AreEqual(expected, string.Join<string>(separator, iEnumerableStringNotOptimized));

                List<object> iEnumerableObject = [.. values];
                Assert.AreEqual(expected, string.Join(separator, iEnumerableObject));

                // Bug/Documented behavior: Join(string, object[]) returns "" when the first item in the array is null
                if (values.Length == 0 || values[0] != null)
                {
                    object[] arrayOfObjects = values;
                    Assert.AreEqual(expected, string.Join(separator, arrayOfObjects));
                }
            }
            Assert.AreEqual(expected, string.Join(separator, values, startIndex, count));
        }

        [TestCase('$', new string[] { }, 0, 0, "")]
        [TestCase('$', new string[] { null }, 0, 1, "")]
        [TestCase('$', new string[] { null, "Bar", null }, 0, 3, "$Bar$")]
        [TestCase('$', new string[] { "", "", "" }, 0, 3, "$$")]
        [TestCase('$', new string[] { "Foo", "Bar", "Baz" }, 0, 3, "Foo$Bar$Baz")]
        [TestCase('$', new string[] { "Foo", "Bar", "Baz" }, 3, 0, "")]
        [TestCase('$', new string[] { "Foo", "Bar", "Baz" }, 1, 1, "Bar")]
        public static void Join_CharSeparator_StringArray(char separator, string[] values, int startIndex, int count, string expected)
        {
            if (startIndex + count == values.Length && count != 0)
            {
                Assert.AreEqual(expected, string.Join(separator, values));

                List<string> iEnumerableStringOptimized = [.. values];
                Assert.AreEqual(expected, string.Join(separator, iEnumerableStringOptimized));
                Assert.AreEqual(expected, string.Join(separator, iEnumerableStringOptimized)); // Call the generic IEnumerable<T>-based overload

                Queue<string> iEnumerableStringNotOptimized = new(values);
                Assert.AreEqual(expected, string.Join(separator, iEnumerableStringNotOptimized));
                Assert.AreEqual(expected, string.Join(separator, iEnumerableStringNotOptimized));

                List<object> iEnumerableObject = [.. values];
                Assert.AreEqual(expected, string.Join(separator, iEnumerableObject));

                // Bug/Documented behavior: Join(string, object[]) returns "" when the first item in the array is null
                if (values.Length == 0 || values[0] != null)
                {
                    object[] arrayOfObjects = values;
                    Assert.AreEqual(expected, string.Join(separator, arrayOfObjects));
                }
            }
            Assert.AreEqual(expected, StringEx.Join(separator, values, startIndex, count));
        }

        [Test]
        public static void Join_String_NullValues_ThrowsArgumentNullException()
        {
            Assert.AreEqual("value", Assert.Throws<ArgumentNullException>(() => string.Join("$$", null)).ParamName);
            Assert.AreEqual("value", Assert.Throws<ArgumentNullException>(() => string.Join("$$", null, 0, 0)).ParamName);
            Assert.AreEqual("values", Assert.Throws<ArgumentNullException>(() => string.Join("|", (IEnumerable<string>)null)).ParamName);
            Assert.AreEqual("values", Assert.Throws<ArgumentNullException>(() => string.Join<string>("|", null)).ParamName); // Generic overload
        }

        [Test]
        public static void Join_String_NegativeCount_ThrowsArgumentOutOfRangeException()
        {
            Assert.AreEqual("count", Assert.Throws<ArgumentOutOfRangeException>(() => string.Join("$$", ["Foo"], 0, -1)).ParamName);
        }

        [TestCase(2, 1)]
        [TestCase(2, 0)]
        [TestCase(1, 2)]
        [TestCase(1, 1)]
        [TestCase(0, 2)]
        [TestCase(-1, 0)]
        public static void Join_String_InvalidStartIndexCount_ThrowsArgumentOutOfRangeException(int startIndex, int count)
        {
            Assert.AreEqual("startIndex", Assert.Throws<ArgumentOutOfRangeException>(() => string.Join("$$", ["Foo"], startIndex, count)).ParamName);
        }

        public static IEnumerable<object[]> Join_ObjectArray_TestData()
        {
            yield return new object[] { "$$", new object[] { }, "" };
            yield return new object[] { "$$", new object[] { new ObjectWithNullToString() }, "" };
            yield return new object[] { "$$", new object[] { "Foo" }, "Foo" };
            yield return new object[] { "$$", new object[] { "Foo", "Bar", "Baz" }, "Foo$$Bar$$Baz" };
            yield return new object[] { null, new object[] { "Foo", "Bar", "Baz" }, "FooBarBaz" };
            yield return new object[] { "$$", new object[] { "Foo", null, "Baz" }, "Foo$$$$Baz" };

            // Join should ignore objects that have a null ToString() value
            yield return new object[] { "|", new object[] { new ObjectWithNullToString(), "Foo", new ObjectWithNullToString(), "Bar", new ObjectWithNullToString() }, "|Foo||Bar|" };
        }

        [TestCaseSource(nameof(Join_ObjectArray_TestData))]
        public static void Join_ObjectArray(string separator, object[] values, string expected)
        {
            Assert.AreEqual(expected, string.Join(separator, values));
            Assert.AreEqual(expected, string.Join(separator, (IEnumerable<object>)values));
        }

        public static IEnumerable<object[]> Join_CharSeparator_ObjectArray_TestData()
        {
            yield return new object[] { '$', new object[] { }, "" };
            yield return new object[] { '$', new object[] { new ObjectWithNullToString() }, "" };
            yield return new object[] { '$', new object[] { "Foo" }, "Foo" };
            yield return new object[] { '$', new object[] { "Foo", "Bar", "Baz" }, "Foo$Bar$Baz" };
            yield return new object[] { '$', new object[] { "Foo", null, "Baz" }, "Foo$$Baz" };

            // Test join when first value is null
            yield return new object[] { '$', new object[] { null, "Bar", "Baz" }, "$Bar$Baz" };

            // Join should ignore objects that have a null ToString() value
            yield return new object[] { '|', new object[] { new ObjectWithNullToString(), "Foo", new ObjectWithNullToString(), "Bar", new ObjectWithNullToString() }, "|Foo||Bar|" };
        }

        [TestCaseSource(nameof(Join_CharSeparator_ObjectArray_TestData))]
        public static void Join_CharSeparator_ObjectArray(char separator, object[] values, string expected)
        {
            Assert.AreEqual(expected, string.Join(separator, values));
            Assert.AreEqual(expected, string.Join(separator, (IEnumerable<object>)values));
        }

        [Test]
        public static void Join_ObjectArray_Null_ThrowsArgumentNullException()
        {
            Assert.AreEqual("values", Assert.Throws<ArgumentNullException>(() => string.Join("$$", (object[])null)).ParamName);
            Assert.AreEqual("values", Assert.Throws<ArgumentNullException>(() => string.Join("--", (IEnumerable<object>)null)).ParamName);
        }

        private class ObjectWithNullToString
        {
            public override string ToString() => null;
        }
    }
}
