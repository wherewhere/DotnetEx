using NUnit.Framework;
using System;
using System.Globalization;
using System.Text;

namespace DotnetEx.Test
{
    /// <summary>
    /// The tests for the <see cref="StringBuilderEx.AppendInterpolatedStringHandler"/> struct.
    /// </summary>
    [TestFixture]
    public static class StringBuilderInterpolationTests
    {
        [Test]
        public static void Append_Nop()
        {
            StringBuilder sb = new();
            StringBuilderEx.AppendInterpolatedStringHandler iab = new(1, 2, sb);

            Assert.AreSame(sb, sb.Append(ref iab));
            Assert.AreSame(sb, sb.Append(CultureInfo.InvariantCulture, ref iab));

            Assert.AreEqual(0, sb.Length);
        }

        [Test]
        public static void AppendLine_AppendsNewLine()
        {
            StringBuilder sb = new();
            StringBuilderEx.AppendInterpolatedStringHandler iab = new(1, 2, sb);

            Assert.AreSame(sb, sb.AppendLine(ref iab));
            Assert.AreSame(sb, sb.AppendLine(CultureInfo.InvariantCulture, ref iab));

            Assert.AreEqual(Environment.NewLine + Environment.NewLine, sb.ToString());
        }

        [TestCase(0, 0)]
        [TestCase(1, 1)]
        [TestCase(42, 84)]
        [TestCase(-1, 0)]
        [TestCase(-1, -1)]
        [TestCase(-16, 1)]
        public static void LengthAndHoleArguments_Valid(int baseLength, int holeCount)
        {
            StringBuilder sb = new();

            new StringBuilderEx.AppendInterpolatedStringHandler(baseLength, holeCount, sb);

            foreach (IFormatProvider provider in new IFormatProvider[] { null, new ConcatFormatter(), CultureInfo.InvariantCulture, CultureInfo.CurrentCulture, new CultureInfo("en-US"), new CultureInfo("fr-FR") })
            {
                new StringBuilderEx.AppendInterpolatedStringHandler(baseLength, holeCount, sb, provider);
            }
        }

        [Test]
        public static void AppendLiteral()
        {
            StringBuilder expected = new();
            StringBuilder actual = new();
            StringBuilderEx.AppendInterpolatedStringHandler iab = new(0, 0, actual);

            foreach (string s in new[] { "", "a", "bc", "def", "this is a long string", "!" })
            {
                expected.Append(s);
                iab.AppendLiteral(s);
            }

            actual.Append(ref iab);

            Assert.AreEqual(expected.ToString(), actual.ToString());
        }

        [Test]
        public static void AppendFormatted_String()
        {
            StringBuilder expected = new();
            StringBuilder actual = new();
            StringBuilderEx.AppendInterpolatedStringHandler iab = new(0, 0, actual);

            foreach (string s in new[] { null, "", "a", "bc", "def", "this is a longer string", "!" })
            {
                // string
                expected.AppendFormat("{0}", s);
                iab.AppendFormatted(s);

                // string, format
                expected.AppendFormat("{0:X2}", s);
                iab.AppendFormatted(s, "X2");

                foreach (int alignment in new[] { 0, 3, -3 })
                {
                    // string, alignment
                    expected.AppendFormat("{0," + alignment.ToString(CultureInfo.InvariantCulture) + "}", s);
                    iab.AppendFormatted(s, alignment);

                    // string, alignment, format
                    expected.AppendFormat("{0," + alignment.ToString(CultureInfo.InvariantCulture) + ":X2}", s);
                    iab.AppendFormatted(s, alignment, "X2");
                }
            }

            actual.Append(ref iab);

            Assert.AreEqual(expected.ToString(), actual.ToString());
        }

        [Test]
        public static void AppendFormatted_String_ICustomFormatter()
        {
            ConcatFormatter provider = new();

            StringBuilder expected = new();
            StringBuilder actual = new();
            StringBuilderEx.AppendInterpolatedStringHandler iab = new(0, 0, actual, provider);

            foreach (string s in new[] { null, "", "a" })
            {
                // string
                expected.AppendFormat(provider, "{0}", s);
                iab.AppendFormatted(s);

                // string, format
                expected.AppendFormat(provider, "{0:X2}", s);
                iab.AppendFormatted(s, "X2");

                // string, alignment
                expected.AppendFormat(provider, "{0,3}", s);
                iab.AppendFormatted(s, 3);

                // string, alignment, format
                expected.AppendFormat(provider, "{0,-3:X2}", s);
                iab.AppendFormatted(s, -3, "X2");
            }

            actual.Append(provider, ref iab);

            Assert.AreEqual(expected.ToString(), actual.ToString());
        }

        [Test]
        public static void AppendFormatted_ReferenceTypes()
        {
            StringBuilder expected = new();
            StringBuilder actual = new();
            StringBuilderEx.AppendInterpolatedStringHandler iab = new(0, 0, actual);

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
                    iab.AppendFormatted(o);
                    if (o is IHasToStringState tss1)
                    {
                        Assert.True(string.IsNullOrEmpty(tss1.ToStringState.LastFormat));
                        AssertModeMatchesType(tss1);
                    }

                    // object, format
                    expected.AppendFormat("{0:X2}", o);
                    iab.AppendFormatted(o, "X2");
                    if (o is IHasToStringState tss2)
                    {
                        Assert.AreEqual("X2", tss2.ToStringState.LastFormat);
                        AssertModeMatchesType(tss2);
                    }

                    foreach (int alignment in new[] { 0, 3, -3 })
                    {
                        // object, alignment
                        expected.AppendFormat("{0," + alignment.ToString(CultureInfo.InvariantCulture) + "}", o);
                        iab.AppendFormatted(o, alignment);
                        if (o is IHasToStringState tss3)
                        {
                            Assert.True(string.IsNullOrEmpty(tss3.ToStringState.LastFormat));
                            AssertModeMatchesType(tss3);
                        }

                        // object, alignment, format
                        expected.AppendFormat("{0," + alignment.ToString(CultureInfo.InvariantCulture) + ":X2}", o);
                        iab.AppendFormatted(o, alignment, "X2");
                        if (o is IHasToStringState tss4)
                        {
                            Assert.AreEqual("X2", tss4.ToStringState.LastFormat);
                            AssertModeMatchesType(tss4);
                        }
                    }
                }
            }

            actual.Append(ref iab);

            Assert.AreEqual(expected.ToString(), actual.ToString());
        }

        [Test]
        public static void AppendFormatted_ReferenceTypes_CreateProviderFlowed()
        {
            CultureInfo provider = new("en-US");
            StringBuilder sb = new();
            StringBuilderEx.AppendInterpolatedStringHandler iab = new(1, 2, sb, provider);

            foreach (IHasToStringState tss in new IHasToStringState[] { new FormattableStringWrapper("hello") })
            {
                iab.AppendFormatted(tss);
                Assert.AreSame(provider, tss.ToStringState.LastProvider);

                iab.AppendFormatted(tss, 1);
                Assert.AreSame(provider, tss.ToStringState.LastProvider);

                iab.AppendFormatted(tss, "X2");
                Assert.AreSame(provider, tss.ToStringState.LastProvider);

                iab.AppendFormatted(tss, 1, "X2");
                Assert.AreSame(provider, tss.ToStringState.LastProvider);
            }

            sb.Append(ref iab);
        }

        [Test]
        public static void AppendFormatted_ReferenceTypes_ICustomFormatter()
        {
            ConcatFormatter provider = new();

            StringBuilder expected = new();
            StringBuilder actual = new();
            StringBuilderEx.AppendInterpolatedStringHandler iab = new(0, 0, actual, provider);

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
                    iab.AppendFormatted(tss);
                    AssertTss(tss, null);

                    // object, format
                    expected.AppendFormat(provider, "{0:X2}", tss);
                    iab.AppendFormatted(tss, "X2");
                    AssertTss(tss, "X2");

                    // object, alignment
                    expected.AppendFormat(provider, "{0,3}", tss);
                    iab.AppendFormatted(tss, 3);
                    AssertTss(tss, null);

                    // object, alignment, format
                    expected.AppendFormat(provider, "{0,-3:X2}", tss);
                    iab.AppendFormatted(tss, -3, "X2");
                    AssertTss(tss, "X2");
                }
            }

            actual.Append(provider, ref iab);

            Assert.AreEqual(expected.ToString(), actual.ToString());
        }

        [Test]
        public static void AppendFormatted_ValueTypes()
        {
            static void Test<T>(T t)
            {
                StringBuilder expected = new();
                StringBuilder actual = new();
                StringBuilderEx.AppendInterpolatedStringHandler iab = new(0, 0, actual);

                // struct
                expected.AppendFormat("{0}", t);
                iab.AppendFormatted(t);
                Assert.True(string.IsNullOrEmpty(((IHasToStringState)t).ToStringState.LastFormat));
                AssertModeMatchesType(((IHasToStringState)t));

                // struct, format
                expected.AppendFormat("{0:X2}", t);
                iab.AppendFormatted(t, "X2");
                Assert.AreEqual("X2", ((IHasToStringState)t).ToStringState.LastFormat);
                AssertModeMatchesType(((IHasToStringState)t));

                foreach (int alignment in new[] { 0, 3, -3 })
                {
                    // struct, alignment
                    expected.AppendFormat("{0," + alignment.ToString(CultureInfo.InvariantCulture) + "}", t);
                    iab.AppendFormatted(t, alignment);
                    Assert.True(string.IsNullOrEmpty(((IHasToStringState)t).ToStringState.LastFormat));
                    AssertModeMatchesType(((IHasToStringState)t));

                    // struct, alignment, format
                    expected.AppendFormat("{0," + alignment.ToString(CultureInfo.InvariantCulture) + ":X2}", t);
                    iab.AppendFormatted(t, alignment, "X2");
                    Assert.AreEqual("X2", ((IHasToStringState)t).ToStringState.LastFormat);
                    AssertModeMatchesType(((IHasToStringState)t));
                }

                actual.Append(ref iab);

                Assert.AreEqual(expected.ToString(), actual.ToString());
            }

            Test(new FormattableInt32Wrapper(42));
            Test((FormattableInt32Wrapper?)new FormattableInt32Wrapper(42));
        }

        [Test]
        public static void AppendFormatted_ValueTypes_CreateProviderFlowed()
        {
            static void Test<T>(T t)
            {
                CultureInfo provider = new("en-US");
                StringBuilder sb = new();
                StringBuilderEx.AppendInterpolatedStringHandler iab = new(1, 2, sb, provider);

                iab.AppendFormatted(t);
                Assert.AreSame(provider, ((IHasToStringState)t).ToStringState.LastProvider);

                iab.AppendFormatted(t, 1);
                Assert.AreSame(provider, ((IHasToStringState)t).ToStringState.LastProvider);

                iab.AppendFormatted(t, "X2");
                Assert.AreSame(provider, ((IHasToStringState)t).ToStringState.LastProvider);

                iab.AppendFormatted(t, 1, "X2");
                Assert.AreSame(provider, ((IHasToStringState)t).ToStringState.LastProvider);

                sb.Append(ref iab);
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
                StringBuilder actual = new();
                StringBuilderEx.AppendInterpolatedStringHandler iab = new(0, 0, actual, provider);

                // struct
                expected.AppendFormat(provider, "{0}", t);
                iab.AppendFormatted(t);
                AssertTss(t, null);

                // struct, format
                expected.AppendFormat(provider, "{0:X2}", t);
                iab.AppendFormatted(t, "X2");
                AssertTss(t, "X2");

                // struct, alignment
                expected.AppendFormat(provider, "{0,3}", t);
                iab.AppendFormatted(t, 3);
                AssertTss(t, null);

                // struct, alignment, format
                expected.AppendFormat(provider, "{0,-3:X2}", t);
                iab.AppendFormatted(t, -3, "X2");
                AssertTss(t, "X2");

                Assert.AreEqual(expected.ToString(), actual.ToString());

                actual.Append(ref iab);
            }

            Test(new FormattableInt32Wrapper(42));
            Test((FormattableInt32Wrapper?)new FormattableInt32Wrapper(42));
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
