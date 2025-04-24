using NUnit.Framework;
using System;

namespace DotnetEx.Test
{
    /// <summary>
    /// The tests for the <see cref="ArgumentNullExceptionEx"/> class.
    /// </summary>
    [TestFixture]
    public static class ArgumentOutOfRangeExceptionTests
    {
        private const string HelpersParamName = "value";
        private static Action ZeroHelper<T>(T value) where T : struct, IComparable<T> => () => ArgumentOutOfRangeException.ThrowIfZero(value);
        private static Action NegativeOrZeroHelper<T>(T value) where T : struct, IComparable<T> => () => ArgumentOutOfRangeException.ThrowIfNegativeOrZero(value);
        private static Action GreaterThanHelper<T>(T value, T other) where T : IComparable<T> => () => ArgumentOutOfRangeException.ThrowIfGreaterThan(value, other);
        private static Action GreaterThanOrEqualHelper<T>(T value, T other) where T : IComparable<T> => () => ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(value, other);
        private static Action LessThanHelper<T>(T value, T other) where T : IComparable<T> => () => ArgumentOutOfRangeException.ThrowIfLessThan(value, other);
        private static Action LessThanOrEqualHelper<T>(T value, T other) where T : IComparable<T> => () => ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(value, other);
        private static Action EqualHelper<T>(T value, T other) where T : IEquatable<T> => () => ArgumentOutOfRangeException.ThrowIfEqual(value, other);
        private static Action NotEqualHelper<T>(T value, T other) where T : IEquatable<T> => () => ArgumentOutOfRangeException.ThrowIfNotEqual(value, other);

        [Test]
        public static void GenericHelpers_ThrowIfZero_Throws()
        {
            Assert.AreEqual(0, Throws<ArgumentOutOfRangeException>(HelpersParamName, ZeroHelper(0)).ActualValue);
            Assert.AreEqual(0u, Throws<ArgumentOutOfRangeException>(HelpersParamName, ZeroHelper(0u)).ActualValue);

            Assert.AreEqual(0.0f, Throws<ArgumentOutOfRangeException>(HelpersParamName, ZeroHelper(0.0f)).ActualValue);
            Assert.AreEqual(-0.0f, Throws<ArgumentOutOfRangeException>(HelpersParamName, ZeroHelper(-0.0f)).ActualValue);
            Assert.AreEqual(0d, Throws<ArgumentOutOfRangeException>(HelpersParamName, ZeroHelper(0d)).ActualValue);
            Assert.AreEqual(+0.0, Throws<ArgumentOutOfRangeException>(HelpersParamName, ZeroHelper(+0.0)).ActualValue);
            Assert.AreEqual(-0.0, Throws<ArgumentOutOfRangeException>(HelpersParamName, ZeroHelper(-0.0)).ActualValue);

            ZeroHelper(1)();
        }

        [Test]
        public static void GenericHelpers_ThrowIfNegativeZero_Throws()
        {
            Assert.AreEqual(-1, Throws<ArgumentOutOfRangeException>(HelpersParamName, NegativeOrZeroHelper(-1)).ActualValue);
            Assert.AreEqual(-0.0f, Throws<ArgumentOutOfRangeException>(HelpersParamName, NegativeOrZeroHelper(-0.0f)).ActualValue);
            Assert.AreEqual(-0.0, Throws<ArgumentOutOfRangeException>(HelpersParamName, NegativeOrZeroHelper(-0.0)).ActualValue);

            NegativeOrZeroHelper(1)();
        }

        [Test]
        public static void GenericHelpers_ThrowIfGreaterThan_Throws()
        {
            Assert.AreEqual(1, Throws<ArgumentOutOfRangeException>(HelpersParamName, GreaterThanHelper(1, 0)).ActualValue);
            Assert.AreEqual(1u, Throws<ArgumentOutOfRangeException>(HelpersParamName, GreaterThanHelper(1u, 0u)).ActualValue);
            Assert.AreEqual(1.000000001, Throws<ArgumentOutOfRangeException>(HelpersParamName, GreaterThanHelper(1.000000001, 1)).ActualValue);
            Assert.AreEqual(1.00001f, Throws<ArgumentOutOfRangeException>(HelpersParamName, GreaterThanHelper(1.00001f, 1)).ActualValue);

            GreaterThanHelper(1, 2)();
        }

        [Test]
        public static void GenericHelpers_ThrowIfGreaterThanOrEqual_Throws()
        {
            Assert.AreEqual(1, Throws<ArgumentOutOfRangeException>(HelpersParamName, GreaterThanOrEqualHelper(1, 1)).ActualValue);
            Assert.AreEqual(1u, Throws<ArgumentOutOfRangeException>(HelpersParamName, GreaterThanOrEqualHelper(1u, 1u)).ActualValue);
            Assert.AreEqual((double)1, Throws<ArgumentOutOfRangeException>(HelpersParamName, GreaterThanOrEqualHelper(1d, 1)).ActualValue);
            Assert.AreEqual(1f, Throws<ArgumentOutOfRangeException>(HelpersParamName, GreaterThanOrEqualHelper(1f, 1)).ActualValue);

            Assert.AreEqual(3, Throws<ArgumentOutOfRangeException>(HelpersParamName, GreaterThanOrEqualHelper(3, 1)).ActualValue);
            Assert.AreEqual(4u, Throws<ArgumentOutOfRangeException>(HelpersParamName, GreaterThanOrEqualHelper(4u, 1u)).ActualValue);
            Assert.AreEqual((double)1.1, Throws<ArgumentOutOfRangeException>(HelpersParamName, GreaterThanOrEqualHelper(1.1, 1)).ActualValue);
            Assert.AreEqual(2.1f, Throws<ArgumentOutOfRangeException>(HelpersParamName, GreaterThanOrEqualHelper(2.1f, 1)).ActualValue);

            GreaterThanOrEqualHelper(1, 2)();
        }

        [Test]
        public static void GenericHelpers_ThrowIfLessThan_Throws()
        {
            Assert.AreEqual(0, Throws<ArgumentOutOfRangeException>(HelpersParamName, LessThanHelper(0, 1)).ActualValue);
            Assert.AreEqual(0u, Throws<ArgumentOutOfRangeException>(HelpersParamName, LessThanHelper(0u, 1u)).ActualValue);
            Assert.AreEqual((double)1, Throws<ArgumentOutOfRangeException>(HelpersParamName, LessThanHelper(1, 1.000000001)).ActualValue);
            Assert.AreEqual(1f, Throws<ArgumentOutOfRangeException>(HelpersParamName, LessThanHelper(1, 1.00001f)).ActualValue);

            LessThanHelper(2, 1)();
        }

        [Test]
        public static void GenericHelpers_ThrowIfLessThanOrEqual_Throws()
        {
            Assert.AreEqual(-1, Throws<ArgumentOutOfRangeException>(HelpersParamName, LessThanOrEqualHelper(-1, 1)).ActualValue);
            Assert.AreEqual(0u, Throws<ArgumentOutOfRangeException>(HelpersParamName, LessThanOrEqualHelper(0u, 1u)).ActualValue);
            Assert.AreEqual((double)0.9, Throws<ArgumentOutOfRangeException>(HelpersParamName, LessThanOrEqualHelper(0.9, 1)).ActualValue);
            Assert.AreEqual(-0.1f, Throws<ArgumentOutOfRangeException>(HelpersParamName, LessThanOrEqualHelper(-0.1f, 1)).ActualValue);

            Assert.AreEqual(1, Throws<ArgumentOutOfRangeException>(HelpersParamName, LessThanOrEqualHelper(1, 1)).ActualValue);
            Assert.AreEqual(1u, Throws<ArgumentOutOfRangeException>(HelpersParamName, LessThanOrEqualHelper(1u, 1u)).ActualValue);
            Assert.AreEqual(1d, Throws<ArgumentOutOfRangeException>(HelpersParamName, LessThanOrEqualHelper(1d, 1)).ActualValue);
            Assert.AreEqual(1f, Throws<ArgumentOutOfRangeException>(HelpersParamName, LessThanOrEqualHelper(1f, 1)).ActualValue);

            LessThanHelper(2, 1)();
        }

        [Test]
        public static void GenericHelpers_ThrowIfEqual_Throws()
        {
            Assert.AreEqual(1, Throws<ArgumentOutOfRangeException>(HelpersParamName, EqualHelper(1, 1)).ActualValue);
            Assert.AreEqual(1u, Throws<ArgumentOutOfRangeException>(HelpersParamName, EqualHelper(1u, 1u)).ActualValue);
            Assert.AreEqual(1d, Throws<ArgumentOutOfRangeException>(HelpersParamName, EqualHelper(1d, 1)).ActualValue);
            Assert.AreEqual(1f, Throws<ArgumentOutOfRangeException>(HelpersParamName, EqualHelper(1f, 1)).ActualValue);
            Assert.Null(Throws<ArgumentOutOfRangeException>(HelpersParamName, EqualHelper<string>(null, null)).ActualValue);

            EqualHelper(1, 2)();
            EqualHelper("test1", "test2")();
            EqualHelper("test1", null)();
            EqualHelper(null, "test2")();
        }

        [Test]
        public static void GenericHelpers_ThrowIfNotEqual_Throws()
        {
            Assert.AreEqual(-1, Throws<ArgumentOutOfRangeException>(HelpersParamName, NotEqualHelper(-1, 1)).ActualValue);
            Assert.AreEqual(2u, Throws<ArgumentOutOfRangeException>(HelpersParamName, NotEqualHelper(2u, 1u)).ActualValue);
            Assert.AreEqual((double)2, Throws<ArgumentOutOfRangeException>(HelpersParamName, NotEqualHelper(2d, 1)).ActualValue);
            Assert.AreEqual(1f, Throws<ArgumentOutOfRangeException>(HelpersParamName, NotEqualHelper(1f, 2)).ActualValue);
            Assert.AreEqual("test", Throws<ArgumentOutOfRangeException>(HelpersParamName, NotEqualHelper("test", null)).ActualValue);
            Assert.Null(Throws<ArgumentOutOfRangeException>(HelpersParamName, NotEqualHelper(null, "test")).ActualValue);

            NotEqualHelper(2, 2)();
            NotEqualHelper("test", "test")();
        }

        private static T Throws<T>(string expectedParamName, Action action) where T : ArgumentException
        {
            T exception = Assert.Throws<T>(action.Invoke);
            Assert.AreEqual(expectedParamName, exception.ParamName);
            return exception;
        }
    }
}
