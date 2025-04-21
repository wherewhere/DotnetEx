using NUnit.Framework;
using System;

namespace DotnetEx.Test
{
    /// <summary>
    /// The tests for the <see cref="ArgumentNullExceptionEx"/> class.
    /// </summary>
    [TestFixture]
    public static class ArgumentNullExceptionTests
    {
        [Test]
        public static unsafe void ThrowIfNull_NonNull_DoesntThrow()
        {
            foreach (object o in new[] { new object(), "", "argument" })
            {
                ArgumentNullException.ThrowIfNull(o);
                ArgumentNullException.ThrowIfNull(o, "paramName");
            }

            int i = 0;
            ArgumentNullException.ThrowIfNull(&i);
            ArgumentNullException.ThrowIfNull(&i, "paramName");
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("name")]
        public static unsafe void ThrowIfNull_Null_ThrowsArgumentNullException(string paramName)
        {
            Assert.AreEqual(paramName, Assert.Throws<ArgumentNullException>(() => ArgumentNullException.ThrowIfNull((object)null, paramName)).ParamName);
            Assert.AreEqual(paramName, Assert.Throws<ArgumentNullException>(() => ArgumentNullException.ThrowIfNull((void*)null, paramName)).ParamName);
        }

        [Test]
        public static unsafe void ThrowIfNull_UsesArgumentExpression()
        {
            object someObject = null;
            Assert.AreEqual(nameof(someObject), Assert.Throws<ArgumentNullException>(() => ArgumentNullException.ThrowIfNull(someObject)).ParamName);

            byte* somePointer = null;
            Assert.AreEqual(nameof(somePointer), Assert.Throws<ArgumentNullException>(() => ArgumentNullException.ThrowIfNull(somePointer)).ParamName);
        }
    }
}
