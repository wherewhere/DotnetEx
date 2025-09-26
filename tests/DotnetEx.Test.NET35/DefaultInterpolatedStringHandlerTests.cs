using NUnit.Framework;
using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;

namespace DotnetEx.Test
{
    /// <summary>
    /// The tests for the <see cref="DefaultInterpolatedStringHandler"/> struct.
    /// </summary>
    [TestFixture]
    public class DefaultInterpolatedStringHandlerTests
    {
        [TestCase(0, 0)]
        [TestCase(1, 1)]
        [TestCase(42, 84)]
        [TestCase(-1, 0)]
        [TestCase(-1, -1)]
        [TestCase(-16, 1)]
        public static void LengthAndHoleArguments_Valid(int literalLength, int formattedCount)
        {
            new DefaultInterpolatedStringHandler(literalLength, formattedCount).ToStringAndClear();

            foreach (IFormatProvider provider in new IFormatProvider[] { null, new ConcatFormatter(), CultureInfo.InvariantCulture, CultureInfo.CurrentCulture, new CultureInfo("en-US"), new CultureInfo("fr-FR") })
            {
                new DefaultInterpolatedStringHandler(literalLength, formattedCount, provider).ToStringAndClear();
            }
        }

        [Test]
        public static void ToString_DoesntClear()
        {
            DefaultInterpolatedStringHandler handler = new(0, 0);
            handler.AppendLiteral("hi");
            for (int i = 0; i < 3; i++)
            {
                Assert.AreEqual("hi", handler.ToString());
            }
            Assert.AreEqual("hi", handler.ToStringAndClear());
        }

        [Test]
        public static void ToStringAndClear_Clears()
        {
            DefaultInterpolatedStringHandler handler = new(0, 0);
            handler.AppendLiteral("hi");
            Assert.AreEqual("hi", handler.ToStringAndClear());
            Assert.AreEqual(string.Empty, handler.ToStringAndClear());
        }

        [Test]
        public static void Clear_Clears()
        {
            DefaultInterpolatedStringHandler handler = new(0, 0);
            handler.AppendLiteral("hi");
            Assert.AreEqual("hi", handler.ToString());
            handler.Clear();
            Assert.AreEqual(string.Empty, handler.ToString());
        }

        [Test]
        public static void AppendLiteral()
        {
            var expected = new StringBuilder();
            DefaultInterpolatedStringHandler actual = new(0, 0);

            foreach (string s in new[] { "", "a", "bc", "def", "this is a long string", "!" })
            {
                expected.Append(s);
                actual.AppendLiteral(s);
            }

            Assert.AreEqual(expected.ToString(), actual.ToString());
            Assert.AreEqual(expected.ToString(), actual.ToStringAndClear());
        }

        [Test]
        public static void AppendFormatted_String()
        {
            StringBuilder expected = new();
            DefaultInterpolatedStringHandler actual = new(0, 0);

            foreach (string s in new[] { null, "", "a", "bc", "def", "this is a longer string", "!" })
            {
                // string
                expected.AppendFormat("{0}", s);
                actual.AppendFormatted(s);

                // string, format
                expected.AppendFormat("{0:X2}", s);
                actual.AppendFormatted(s, "X2");

                foreach (int alignment in new[] { 0, 3, -3 })
                {
                    // string, alignment
                    expected.AppendFormat("{0," + alignment.ToString(CultureInfo.InvariantCulture) + "}", s);
                    actual.AppendFormatted(s, alignment);

                    // string, alignment, format
                    expected.AppendFormat("{0," + alignment.ToString(CultureInfo.InvariantCulture) + ":X2}", s);
                    actual.AppendFormatted(s, alignment, "X2");
                }
            }

            Assert.AreEqual(expected.ToString(), actual.ToString());
            Assert.AreEqual(expected.ToString(), actual.ToStringAndClear());
        }

        [Test]
        public static void AppendFormatted_String_ICustomFormatter()
        {
            ConcatFormatter provider = new();

            StringBuilder expected = new();
            DefaultInterpolatedStringHandler actual = new(0, 0, provider);

            foreach (string s in new[] { null, "", "a" })
            {
                // string
                expected.AppendFormat(provider, "{0}", s);
                actual.AppendFormatted(s);

                // string, format
                expected.AppendFormat(provider, "{0:X2}", s);
                actual.AppendFormatted(s, "X2");

                // string, alignment
                expected.AppendFormat(provider, "{0,3}", s);
                actual.AppendFormatted(s, 3);

                // string, alignment, format
                expected.AppendFormat(provider, "{0,-3:X2}", s);
                actual.AppendFormatted(s, -3, "X2");
            }

            Assert.AreEqual(expected.ToString(), actual.ToString());
            Assert.AreEqual(expected.ToString(), actual.ToStringAndClear());
        }

        [Test]
        public static void AppendFormatted_ReferenceTypes()
        {
            StringBuilder expected = new();
            DefaultInterpolatedStringHandler actual = new(0, 0);

            foreach (string rawInput in new[] { null, "", "a", "bc", "def", "this is a longer string", "!" })
            {
                foreach (object o in new object[]
                {
                    rawInput, // raw string directly; ToString will return itself
                    new StringWrapper(rawInput), // wrapper object that returns string from ToString
                    new FormattableStringWrapper(rawInput), // IFormattable wrapper around string
                })
                {
                    // object
                    expected.AppendFormat("{0}", o);
                    actual.AppendFormatted(o);
                    if (o is IHasToStringState tss1)
                    {
                        Assert.True(string.IsNullOrEmpty(tss1.ToStringState.LastFormat));
                        AssertModeMatchesType(tss1);
                    }

                    // object, format
                    expected.AppendFormat("{0:X2}", o);
                    actual.AppendFormatted(o, "X2");
                    if (o is IHasToStringState tss2)
                    {
                        Assert.AreEqual("X2", tss2.ToStringState.LastFormat);
                        AssertModeMatchesType(tss2);
                    }

                    foreach (int alignment in new[] { 0, 3, -3 })
                    {
                        // object, alignment
                        expected.AppendFormat("{0," + alignment.ToString(CultureInfo.InvariantCulture) + "}", o);
                        actual.AppendFormatted(o, alignment);
                        if (o is IHasToStringState tss3)
                        {
                            Assert.True(string.IsNullOrEmpty(tss3.ToStringState.LastFormat));
                            AssertModeMatchesType(tss3);
                        }

                        // object, alignment, format
                        expected.AppendFormat("{0," + alignment.ToString(CultureInfo.InvariantCulture) + ":X2}", o);
                        actual.AppendFormatted(o, alignment, "X2");
                        if (o is IHasToStringState tss4)
                        {
                            Assert.AreEqual("X2", tss4.ToStringState.LastFormat);
                            AssertModeMatchesType(tss4);
                        }
                    }
                }
            }

            Assert.AreEqual(expected.ToString(), actual.ToString());
            Assert.AreEqual(expected.ToString(), actual.ToStringAndClear());
        }

        [TestCase(false)]
        [TestCase(true)]
        public static void AppendFormatted_ReferenceTypes_CreateProviderFlowed(bool useScratch)
        {
            CultureInfo provider = new("en-US");
            DefaultInterpolatedStringHandler handler = useScratch ?
                new DefaultInterpolatedStringHandler(1, 2, provider, new StringBuilder(16)) :
                new DefaultInterpolatedStringHandler(1, 2, provider);

            foreach (IHasToStringState tss in new IHasToStringState[] { new FormattableStringWrapper("hello") })
            {
                handler.AppendFormatted(tss);
                Assert.AreSame(provider, tss.ToStringState.LastProvider);

                handler.AppendFormatted(tss, 1);
                Assert.AreSame(provider, tss.ToStringState.LastProvider);

                handler.AppendFormatted(tss, "X2");
                Assert.AreSame(provider, tss.ToStringState.LastProvider);

                handler.AppendFormatted(tss, 1, "X2");
                Assert.AreSame(provider, tss.ToStringState.LastProvider);
            }

            handler.ToStringAndClear();
        }

        [Test]
        public static void AppendFormatted_ReferenceTypes_ICustomFormatter()
        {
            ConcatFormatter provider = new();

            StringBuilder expected = new();
            DefaultInterpolatedStringHandler actual = new(0, 0, provider);

            foreach (string s in new[] { null, "", "a" })
            {
                foreach (IHasToStringState tss in new IHasToStringState[] { new FormattableStringWrapper(s) })
                {
                    void AssertTss(IHasToStringState tss, string format)
                    {
                        Assert.AreEqual(format, tss.ToStringState.LastFormat);
                        Assert.AreSame(provider, tss.ToStringState.LastProvider);
                        Assert.AreEqual(ToStringMode.ICustomFormatterFormat, tss.ToStringState.ToStringMode);
                    }

                    // object
                    expected.AppendFormat(provider, "{0}", tss);
                    actual.AppendFormatted(tss);
                    AssertTss(tss, null);

                    // object, format
                    expected.AppendFormat(provider, "{0:X2}", tss);
                    actual.AppendFormatted(tss, "X2");
                    AssertTss(tss, "X2");

                    // object, alignment
                    expected.AppendFormat(provider, "{0,3}", tss);
                    actual.AppendFormatted(tss, 3);
                    AssertTss(tss, null);

                    // object, alignment, format
                    expected.AppendFormat(provider, "{0,-3:X2}", tss);
                    actual.AppendFormatted(tss, -3, "X2");
                    AssertTss(tss, "X2");
                }
            }

            Assert.AreEqual(expected.ToString(), actual.ToString());
            Assert.AreEqual(expected.ToString(), actual.ToStringAndClear());
        }

        [Test]
        public static void AppendFormatted_ValueTypes()
        {
            static void Test<T>(T t)
            {
                StringBuilder expected = new();
                DefaultInterpolatedStringHandler actual = new(0, 0);

                // struct
                expected.AppendFormat("{0}", t);
                actual.AppendFormatted(t);
                Assert.True(string.IsNullOrEmpty(((IHasToStringState)t).ToStringState.LastFormat));
                AssertModeMatchesType(((IHasToStringState)t));

                // struct, format
                expected.AppendFormat("{0:X2}", t);
                actual.AppendFormatted(t, "X2");
                Assert.AreEqual("X2", ((IHasToStringState)t).ToStringState.LastFormat);
                AssertModeMatchesType(((IHasToStringState)t));

                foreach (int alignment in new[] { 0, 3, -3 })
                {
                    // struct, alignment
                    expected.AppendFormat("{0," + alignment.ToString(CultureInfo.InvariantCulture) + "}", t);
                    actual.AppendFormatted(t, alignment);
                    Assert.True(string.IsNullOrEmpty(((IHasToStringState)t).ToStringState.LastFormat));
                    AssertModeMatchesType(((IHasToStringState)t));

                    // struct, alignment, format
                    expected.AppendFormat("{0," + alignment.ToString(CultureInfo.InvariantCulture) + ":X2}", t);
                    actual.AppendFormatted(t, alignment, "X2");
                    Assert.AreEqual("X2", ((IHasToStringState)t).ToStringState.LastFormat);
                    AssertModeMatchesType(((IHasToStringState)t));
                }

                Assert.AreEqual(expected.ToString(), actual.ToString());
                Assert.AreEqual(expected.ToString(), actual.ToStringAndClear());
            }

            Test(new FormattableInt32Wrapper(42));
            Test((FormattableInt32Wrapper?)new FormattableInt32Wrapper(42));
        }

        [TestCase(false)]
        [TestCase(true)]
        public static void AppendFormatted_ValueTypes_CreateProviderFlowed(bool useScratch)
        {
            void Test<T>(T t)
            {
                CultureInfo provider = new("en-US");
                DefaultInterpolatedStringHandler handler = useScratch ?
                    new DefaultInterpolatedStringHandler(1, 2, provider, new StringBuilder(16)) :
                    new DefaultInterpolatedStringHandler(1, 2, provider);

                handler.AppendFormatted(t);
                Assert.AreSame(provider, ((IHasToStringState)t).ToStringState.LastProvider);

                handler.AppendFormatted(t, 1);
                Assert.AreSame(provider, ((IHasToStringState)t).ToStringState.LastProvider);

                handler.AppendFormatted(t, "X2");
                Assert.AreSame(provider, ((IHasToStringState)t).ToStringState.LastProvider);

                handler.AppendFormatted(t, 1, "X2");
                Assert.AreSame(provider, ((IHasToStringState)t).ToStringState.LastProvider);

                handler.ToStringAndClear();
            }

            Test(new FormattableInt32Wrapper(42));
            Test((FormattableInt32Wrapper?)new FormattableInt32Wrapper(42));
        }

        [Test]
        public static void AppendFormatted_ValueTypes_ICustomFormatter()
        {
            ConcatFormatter provider = new();

            void Test<T>(T t)
            {
                void AssertTss(T tss, string format)
                {
                    Assert.AreEqual(format, ((IHasToStringState)tss).ToStringState.LastFormat);
                    Assert.AreSame(provider, ((IHasToStringState)tss).ToStringState.LastProvider);
                    Assert.AreEqual(ToStringMode.ICustomFormatterFormat, ((IHasToStringState)tss).ToStringState.ToStringMode);
                }

                StringBuilder expected = new();
                DefaultInterpolatedStringHandler actual = new(0, 0, provider);

                // struct
                expected.AppendFormat(provider, "{0}", t);
                actual.AppendFormatted(t);
                AssertTss(t, null);

                // struct, format
                expected.AppendFormat(provider, "{0:X2}", t);
                actual.AppendFormatted(t, "X2");
                AssertTss(t, "X2");

                // struct, alignment
                expected.AppendFormat(provider, "{0,3}", t);
                actual.AppendFormatted(t, 3);
                AssertTss(t, null);

                // struct, alignment, format
                expected.AppendFormat(provider, "{0,-3:X2}", t);
                actual.AppendFormatted(t, -3, "X2");
                AssertTss(t, "X2");

                Assert.AreEqual(expected.ToString(), actual.ToString());
                Assert.AreEqual(expected.ToString(), actual.ToStringAndClear());
            }

            Test(new FormattableInt32Wrapper(42));
            Test((FormattableInt32Wrapper?)new FormattableInt32Wrapper(42));
        }

        [TestCase(false)]
        [TestCase(true)]
        public static void Grow_Large(bool useScratch)
        {
            StringBuilder expected = new();
            DefaultInterpolatedStringHandler handler = useScratch ?
                new DefaultInterpolatedStringHandler(3, 1000, null, new StringBuilder(16)) :
                new DefaultInterpolatedStringHandler(3, 1000);

            for (int i = 0; i < 1000; i++)
            {
                handler.AppendFormatted(i);
                expected.Append(i);

                handler.AppendFormatted(i, 3);
                expected.AppendFormat("{0,3}", i);
            }

            Assert.AreEqual(expected.ToString(), handler.ToString());
            Assert.AreEqual(expected.ToString(), handler.ToStringAndClear());
        }

        private static void AssertModeMatchesType<T>(T tss) where T : IHasToStringState
        {
            ToStringMode expected =
                tss is IFormattable ? ToStringMode.IFormattableToString :
                ToStringMode.ObjectToString;
            Assert.AreEqual(expected, tss.ToStringState.ToStringMode);
        }

        private sealed class FormattableStringWrapper(string s) : IFormattable, IHasToStringState
        {
            public ToStringState ToStringState { get; } = new ToStringState();

            public string ToString(string format, IFormatProvider formatProvider)
            {
                ToStringState.LastFormat = format;
                ToStringState.LastProvider = formatProvider;
                ToStringState.ToStringMode = ToStringMode.IFormattableToString;
                return s;
            }

            public override string ToString()
            {
                ToStringState.LastFormat = null;
                ToStringState.LastProvider = null;
                ToStringState.ToStringMode = ToStringMode.ObjectToString;
                return s;
            }
        }

        private readonly struct FormattableInt32Wrapper(int i) : IFormattable, IHasToStringState
        {
            public ToStringState ToStringState { get; } = new ToStringState();

            public string ToString(string format, IFormatProvider formatProvider)
            {
                ToStringState.LastFormat = format;
                ToStringState.LastProvider = formatProvider;
                ToStringState.ToStringMode = ToStringMode.IFormattableToString;
                return i.ToString(format, formatProvider);
            }

            public override string ToString()
            {
                ToStringState.LastFormat = null;
                ToStringState.LastProvider = null;
                ToStringState.ToStringMode = ToStringMode.ObjectToString;
                return i.ToString();
            }
        }

        private sealed class ToStringState
        {
            public string LastFormat { get; set; }
            public IFormatProvider LastProvider { get; set; }
            public ToStringMode ToStringMode { get; set; }
        }

        private interface IHasToStringState
        {
            ToStringState ToStringState { get; }
        }

        private enum ToStringMode
        {
            ObjectToString,
            IFormattableToString,
            ISpanFormattableTryFormat,
            ICustomFormatterFormat,
        }

        private sealed class StringWrapper(string s)
        {
            public override string ToString() => s;
        }

        private sealed class ConcatFormatter : IFormatProvider, ICustomFormatter
        {
            public object GetFormat(Type formatType) => formatType == typeof(ICustomFormatter) ? this : null;

            public string Format(string format, object arg, IFormatProvider formatProvider)
            {
                string s = format + " " + arg + formatProvider;

                if (arg is IHasToStringState tss)
                {
                    // Set after using arg.ToString() in concat above
                    tss.ToStringState.LastFormat = format;
                    tss.ToStringState.LastProvider = formatProvider;
                    tss.ToStringState.ToStringMode = ToStringMode.ICustomFormatterFormat;
                }

                return s;
            }
        }
    }
}
