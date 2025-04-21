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

        [Test]
        public static void Join_String_NullValues_ThrowsArgumentNullException()
        {
            Assert.AreEqual("value", Assert.Throws<ArgumentNullException>(() => string.Join("$$", null)).ParamName);
            Assert.AreEqual("value", Assert.Throws<ArgumentNullException>(() => string.Join("$$", null, 0, 0)).ParamName);
            Assert.AreEqual("values", Assert.Throws<ArgumentNullException>(() => string.Join("|", (IEnumerable<string>)null)).ParamName);
            Assert.AreEqual("values", Assert.Throws<ArgumentNullException>(() => string.Join<string>("|", null)).ParamName); // Generic overload
        }
    }
}
