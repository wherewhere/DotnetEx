using NUnit.Framework;
using System;

namespace DotnetEx.Test
{
    /// <summary>
    /// The tests for the <see cref="ObjectDisposedExceptionEx"/> class.
    /// </summary>
    [TestFixture]
    public static class ObjectDisposedExceptionTests
    {
        [Test]
        public static void Throw_Object()
        {
            object obj = new();
            ObjectDisposedException ex = Assert.Throws<ObjectDisposedException>(() => ObjectDisposedException.ThrowIf(true, obj));

            Assert.AreEqual("System.Object", ex.ObjectName);
        }

        [Test]
        public static void Throw_Type()
        {
            Type type = new object().GetType();
            ObjectDisposedException ex = Assert.Throws<ObjectDisposedException>(() => ObjectDisposedException.ThrowIf(true, type));

            Assert.AreEqual("System.Object", ex.ObjectName);
        }
    }
}
