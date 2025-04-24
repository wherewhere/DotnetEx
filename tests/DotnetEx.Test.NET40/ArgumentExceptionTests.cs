using NUnit.Framework;
using System;

namespace DotnetEx.Test
{
    /// <summary>
    /// The tests for the <see cref="ArgumentExceptionEx"/> class.
    /// </summary>
    [TestFixture]
    public static class ArgumentExceptionTests
    {
        [Test]
        public static void ThrowIfNullOrEmpty_ThrowsForInvalidInput()
        {
            Assert.AreEqual(null, Assert.Throws<ArgumentNullException>(() => ArgumentException.ThrowIfNullOrEmpty(null, null)).ParamName);
            Assert.AreEqual("something", Assert.Throws<ArgumentNullException>(() => ArgumentException.ThrowIfNullOrEmpty(null, "something")).ParamName);

            Assert.AreEqual(null, Assert.Throws<ArgumentException>(() => ArgumentException.ThrowIfNullOrEmpty("", null)).ParamName);
            Assert.AreEqual("something", Assert.Throws<ArgumentException>(() => ArgumentException.ThrowIfNullOrEmpty("", "something")).ParamName);

            ArgumentException.ThrowIfNullOrEmpty(" ");
            ArgumentException.ThrowIfNullOrEmpty(" ", "something");
            ArgumentException.ThrowIfNullOrEmpty("abc", "something");
        }

        [Test]
        public static void ThrowIfNullOrEmpty_UsesArgumentExpression_ParameterNameMatches()
        {
            string someString = null;
            Assert.AreEqual(nameof(someString), Assert.Throws<ArgumentNullException>(() => ArgumentException.ThrowIfNullOrEmpty(someString)).ParamName);

            someString = "";
            Assert.AreEqual(nameof(someString), Assert.Throws<ArgumentException>(() => ArgumentException.ThrowIfNullOrEmpty(someString)).ParamName);

            someString = "abc";
            ArgumentException.ThrowIfNullOrEmpty(someString);
        }

        [Test]
        public static void ThrowIfNullOrWhiteSpace_ThrowsForInvalidInput()
        {
            Assert.AreEqual(null, Assert.Throws<ArgumentNullException>(() => ArgumentException.ThrowIfNullOrWhiteSpace(null, null)).ParamName);
            Assert.AreEqual("something", Assert.Throws<ArgumentNullException>(() => ArgumentException.ThrowIfNullOrWhiteSpace(null, "something")).ParamName);

            Assert.AreEqual(null, Assert.Throws<ArgumentException>(() => ArgumentException.ThrowIfNullOrWhiteSpace("", null)).ParamName);
            Assert.AreEqual("something", Assert.Throws<ArgumentException>(() => ArgumentException.ThrowIfNullOrWhiteSpace("", "something")).ParamName);

            string allWhitespace = "\u0009\u000A\u000B\u000C\u000D\u0020\u0085\u00A0\u1680\u2000\u2001\u2002\u2003\u2004\u2005\u2006\u2007\u2008\u2009\u200A\u2028\u2029\u202F\u205F\u3000";
            Assert.AreEqual("something", Assert.Throws<ArgumentException>(() => ArgumentException.ThrowIfNullOrWhiteSpace(" ", "something")).ParamName);
            Assert.AreEqual("something", Assert.Throws<ArgumentException>(() => ArgumentException.ThrowIfNullOrWhiteSpace(allWhitespace, "something")).ParamName);
            ArgumentException.ThrowIfNullOrWhiteSpace("a" + allWhitespace, "something");
            ArgumentException.ThrowIfNullOrWhiteSpace(allWhitespace + "a", "something");
            ArgumentException.ThrowIfNullOrWhiteSpace(allWhitespace[..5] + "a" + allWhitespace[5..], "something");
        }

        [Test]
        public static void ThrowIfNullOrWhiteSpace_UsesArgumentExpression_ParameterNameMatches()
        {
            string someString = null;
            Assert.AreEqual(nameof(someString), Assert.Throws<ArgumentNullException>(() => ArgumentException.ThrowIfNullOrWhiteSpace(someString)).ParamName);

            someString = "";
            Assert.AreEqual(nameof(someString), Assert.Throws<ArgumentException>(() => ArgumentException.ThrowIfNullOrWhiteSpace(someString)).ParamName);

            someString = "    ";
            Assert.AreEqual(nameof(someString), Assert.Throws<ArgumentException>(() => ArgumentException.ThrowIfNullOrWhiteSpace(someString)).ParamName);
        }
    }
}
