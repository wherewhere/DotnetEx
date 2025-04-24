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
        private static readonly char[] s_whiteSpaceCharacters = ['\u0009', '\u000a', '\u000b', '\u000c', '\u000d', '\u0020', '\u0085', '\u00a0', '\u1680'];

        public static IEnumerable<object[]> Concat_Strings_LessThan2_GreaterThan4_TestData()
        {
            // 0
            yield return new object[] { new string[0], "" };

            // 1
            yield return new object[] { new string[] { "1" }, "1" };
            yield return new object[] { new string[] { null }, "" };
            yield return new object[] { new string[] { "" }, "" };

            // 5
            yield return new object[] { new string[] { "1", "2", "3", "4", "5" }, "12345" };
            yield return new object[] { new string[] { null, "1", "2", "3", "4" }, "1234" };
            yield return new object[] { new string[] { "", "1", "2", "3", "4" }, "1234" };
            yield return new object[] { new string[] { "1", null, "2", "3", "4" }, "1234" };
            yield return new object[] { new string[] { "1", "", "2", "3", "4" }, "1234" };
            yield return new object[] { new string[] { "1", "2", null, "3", "4" }, "1234" };
            yield return new object[] { new string[] { "1", "2", "", "3", "4" }, "1234" };
            yield return new object[] { new string[] { "1", "2", "3", null, "4" }, "1234" };
            yield return new object[] { new string[] { "1", "2", "3", "", "4" }, "1234" };
            yield return new object[] { new string[] { "1", "2", "3", "4", null }, "1234" };
            yield return new object[] { new string[] { "1", "2", "3", "4", "" }, "1234" };
            yield return new object[] { new string[] { "1", null, "3", null, "5" }, "135" };
            yield return new object[] { new string[] { "1", "", "3", "", "5" }, "135" };
            yield return new object[] { new string[] { null, null, null, null, null }, "" };
            yield return new object[] { new string[] { "", "", "", "", "" }, "" };

            // 7
            yield return new object[] { new string[] { "abcd", "efgh", "ijkl", "mnop", "qrst", "uvwx", "yz" }, "abcdefghijklmnopqrstuvwxyz" };
        }

        public static IEnumerable<object[]> Concat_Strings_2_3_4_TestData()
        {
            // 2
            yield return new object[] { new string[] { "1", "2" }, "12" };
            yield return new object[] { new string[] { null, "1" }, "1" };
            yield return new object[] { new string[] { "", "1" }, "1" };
            yield return new object[] { new string[] { "1", null }, "1" };
            yield return new object[] { new string[] { "1", "" }, "1" };
            yield return new object[] { new string[] { null, null }, "" };
            yield return new object[] { new string[] { "", "" }, "" };

            // 3
            yield return new object[] { new string[] { "1", "2", "3" }, "123" };
            yield return new object[] { new string[] { null, "1", "2" }, "12" };
            yield return new object[] { new string[] { "", "1", "2" }, "12" };
            yield return new object[] { new string[] { "1", null, "2" }, "12" };
            yield return new object[] { new string[] { "1", "", "2" }, "12" };
            yield return new object[] { new string[] { "1", "2", null }, "12" };
            yield return new object[] { new string[] { "1", "2", "" }, "12" };
            yield return new object[] { new string[] { null, "2", null }, "2" };
            yield return new object[] { new string[] { "", "2", "" }, "2" };
            yield return new object[] { new string[] { null, null, null }, "" };
            yield return new object[] { new string[] { "", "", "" }, "" };

            // 4
            yield return new object[] { new string[] { "1", "2", "3", "4" }, "1234" };
            yield return new object[] { new string[] { null, "1", "2", "3" }, "123" };
            yield return new object[] { new string[] { "", "1", "2", "3" }, "123" };
            yield return new object[] { new string[] { "1", null, "2", "3" }, "123" };
            yield return new object[] { new string[] { "1", "", "2", "3" }, "123" };
            yield return new object[] { new string[] { "1", "2", null, "3" }, "123" };
            yield return new object[] { new string[] { "1", "2", "", "3" }, "123" };
            yield return new object[] { new string[] { "1", "2", "3", null }, "123" };
            yield return new object[] { new string[] { "1", "2", "3", "" }, "123" };
            yield return new object[] { new string[] { "1", null, null, null }, "1" };
            yield return new object[] { new string[] { "1", "", "", "" }, "1" };
            yield return new object[] { new string[] { null, "1", null, "2" }, "12" };
            yield return new object[] { new string[] { "", "1", "", "2" }, "12" };
            yield return new object[] { new string[] { null, null, null, null }, "" };
            yield return new object[] { new string[] { "", "", "", "" }, "" };
        }

        [TestCaseSource(nameof(Concat_Strings_2_3_4_TestData))]
        [TestCaseSource(nameof(Concat_Strings_LessThan2_GreaterThan4_TestData))]
        public static void Concat_String(string[] values, string expected)
        {
            void Validate(string result)
            {
                Assert.AreEqual(expected, result);
            }

            if (values.Length == 2)
            {
                Validate(string.Concat(values[0], values[1]));
            }
            else if (values.Length == 3)
            {
                Validate(string.Concat(values[0], values[1], values[2]));
            }
            else if (values.Length == 4)
            {
                Validate(string.Concat(values[0], values[1], values[2], values[3]));
            }

            Validate(string.Concat(values));
            Validate(StringEx.Concat((IEnumerable<string>)values));
            Validate(string.Concat<string>(values)); // Call the generic IEnumerable<T>-based overload
        }

        public static IEnumerable<TestCaseData> Concat_Objects_TestData()
        {
            yield return new TestCaseData(new object[] { 1 }, "1");

            yield return new TestCaseData(new object[] { 1, 2 }, "12");
            yield return new TestCaseData(new object[] { null, 1 }, "1");
            yield return new TestCaseData(new object[] { 1, null }, "1");

            yield return new TestCaseData(new object[] { 1, 2, 3 }, "123");
            yield return new TestCaseData(new object[] { null, 1, 2 }, "12");
            yield return new TestCaseData(new object[] { 1, null, 2 }, "12");
            yield return new TestCaseData(new object[] { 1, 2, null }, "12");

            yield return new TestCaseData(new object[] { 1, 2, 3, 4 }, "1234");
            yield return new TestCaseData(new object[] { null, 1, 2, 3 }, "123");
            yield return new TestCaseData(new object[] { 1, null, 2, 3 }, "123");
            yield return new TestCaseData(new object[] { 1, 2, 3, null }, "123");

            yield return new TestCaseData(new object[] { 1, 2, 3, 4, 5 }, "12345");
            yield return new TestCaseData(new object[] { null, 1, 2, 3, 4 }, "1234");
            yield return new TestCaseData(new object[] { 1, null, 2, 3, 4 }, "1234");
            yield return new TestCaseData(new object[] { 1, 2, 3, 4, null }, "1234");

            // Concat should ignore objects that have a null ToString() value
            yield return new TestCaseData(new object[] { new ObjectWithNullToString(), "Foo", new ObjectWithNullToString(), "Bar", new ObjectWithNullToString() }, "FooBar");
        }

        [TestCaseSource(nameof(Concat_Objects_TestData))]
        public static void Concat_Objects(object[] values, string expected)
        {
            if (values.Length == 1)
            {
                Assert.AreEqual(expected, string.Concat(values[0]));
            }
            else if (values.Length == 2)
            {
                Assert.AreEqual(expected, string.Concat(values[0], values[1]));
            }
            else if (values.Length == 3)
            {
                Assert.AreEqual(expected, string.Concat(values[0], values[1], values[2]));
            }
            else if (values.Length == 4)
            {
                Assert.AreEqual(expected, string.Concat(values[0], values[1], values[2], values[3]));
            }
            Assert.AreEqual(expected, string.Concat(values));
            Assert.AreEqual(expected, string.Concat<object>(values));
        }

        [TestCase(new char[0], "")]
        [TestCase(new char[] { 'a' }, "a")]
        [TestCase(new char[] { 'a', 'b' }, "ab")]
        [TestCase(new char[] { 'a', '\0', 'b' }, "a\0b")]
        [TestCase(new char[] { 'a', 'b', 'c', 'd', 'e', 'f', 'g' }, "abcdefg")]
        public static void Concat_CharEnumerable(char[] values, string expected)
        {
            Assert.AreEqual(expected, string.Concat<char>(values.Select(c => c)));
        }

        [Test]
        public static void Concat_Invalid()
        {
            Assert.AreEqual("values", Assert.Throws<ArgumentNullException>(() => StringEx.Concat((IEnumerable<string>)null)).ParamName); // Values is null
            Assert.AreEqual("values", Assert.Throws<ArgumentNullException>(() => string.Concat<string>(null)).ParamName); // Generic overload
            Assert.AreEqual("values", Assert.Throws<ArgumentNullException>(() => string.Concat(null)).ParamName); // Values is null

            Assert.AreEqual("args", Assert.Throws<ArgumentNullException>(() => string.Concat((object[])null)).ParamName); // Values is null
            Assert.AreEqual("values", Assert.Throws<ArgumentNullException>(() => string.Concat<string>(null)).ParamName); // Values is null
            Assert.AreEqual("values", Assert.Throws<ArgumentNullException>(() => string.Concat<object>(null)).ParamName); // Values is null
        }

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

        [TestCaseSource(nameof(IsNullOrWhitespace_TestData))]
        public static void IsNullOrWhitespace(string value, bool expected)
        {
            Assert.AreEqual(expected, string.IsNullOrWhiteSpace(value));
        }

        [Test]
        public static void ZeroLengthIsWhiteSpace()
        {
            string s1 = string.Empty;
            bool result = string.IsNullOrWhiteSpace(s1);
            Assert.True(result);
        }

        [Test]
        public static void IsWhiteSpaceTrueLatin1()
        {
            Random rand = new(42);
            for (int length = 0; length < 32; length++)
            {
                char[] a = rand.GetItems(s_whiteSpaceCharacters, length);
                string s1 = new(a);
                bool result = string.IsNullOrWhiteSpace(s1);
                Assert.True(result);

                for (int i = 0; i < s_whiteSpaceCharacters.Length - 1; i++)
                {
                    s1 = new string([.. Enumerable.Repeat(s_whiteSpaceCharacters[i], a.Length)]);
                    Assert.True(string.IsNullOrWhiteSpace(s1));
                }
            }
        }

        [Test]
        public static void IsWhiteSpaceTrue()
        {
            Random rand = new(42);
            for (int length = 0; length < 32; length++)
            {
                char[] a = rand.GetItems(s_whiteSpaceCharacters, length);

                string s1 = new(a);
                bool result = string.IsNullOrWhiteSpace(s1);
                Assert.True(result);
            }
        }

        [Test]
        public static void IsWhiteSpaceFalse()
        {
            Random rand = new(42);
            for (int length = 1; length < 32; length++)
            {
                char[] a = rand.GetItems(s_whiteSpaceCharacters, length);

                // first character is not a white-space character
                a[0] = 'a';
                string s1 = new(a);
                bool result = string.IsNullOrWhiteSpace(s1);
                Assert.False(result);
                a[0] = ' ';

                // last character is not a white-space character
                a[length - 1] = 'a';
                s1 = new string(a);
                result = string.IsNullOrWhiteSpace(s1);
                Assert.False(result);
                a[length - 1] = ' ';

                // character in the middle is not a white-space character
                a[length / 2] = 'a';
                s1 = new string(a);
                result = string.IsNullOrWhiteSpace(s1);
                Assert.False(result);
                a[length / 2] = ' ';
            }
        }

        [Test]
        public static void MakeSureNoIsWhiteSpaceChecksGoOutOfRange()
        {
            for (int length = 3; length < 64; length++)
            {
                char[] first = new char[length];
                first[0] = ' ';
                first[length - 1] = ' ';

                string s1 = new(first, 1, length - 2);
                bool result = string.IsNullOrWhiteSpace(s1);
                Assert.False(result);
            }
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

#if !NET40_OR_GREATER
            // Test join when first value is null
            yield return new object[] { "$$", new object[] { null, "Bar", "Baz" }, "$$Bar$$Baz" };
#endif

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
