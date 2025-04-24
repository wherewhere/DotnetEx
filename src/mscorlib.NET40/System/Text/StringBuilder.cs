// ==++==
// 
//   Copyright (c) Microsoft Corporation.  All rights reserved.
// 
// ==--==
/*============================================================
**
** Class:  StringBuilder
**
**
** Purpose: implementation of the StringBuilder
** class.
**
===========================================================*/
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace System.Text
{
    /// <summary>
    /// Represents a mutable string of characters. This class cannot be inherited.
    /// </summary>
    public static class StringBuilderEx
    {
        //
        //
        // STATIC CONSTANTS
        //
        //
        internal const int DefaultCapacity = 16;
        // We want to keep chunk arrays out of large object heap (< 85K bytes ~ 40K chars) to be sure.
        // Making the maximum chunk size big means less allocation code called, but also more waste
        // in unused characters and slower inserts / replaces (since you do need to slide characters over
        // within a buffer).  
        internal const int MaxChunkSize = 8000;

        /// <summary>
        /// Appends the specified interpolated string to this instance.
        /// </summary>
        /// <param name="builder">The <see cref="StringBuilder"/> to which to append.</param>
        /// <param name="handler">The interpolated string to append.</param>
        /// <returns>A reference to this instance after the append operation has completed.</returns>
        public static StringBuilder Append(this StringBuilder builder, [InterpolatedStringHandlerArgument(nameof(builder))] ref AppendInterpolatedStringHandler handler) => builder;

        /// <summary>
        /// Appends the specified interpolated string to this instance.
        /// </summary>
        /// <param name="builder">The <see cref="StringBuilder"/> to which to append.</param>
        /// <param name="provider">An object that supplies culture-specific formatting information.</param>
        /// <param name="handler">The interpolated string to append.</param>
        /// <returns>A reference to this instance after the append operation has completed.</returns>
        public static StringBuilder Append(this StringBuilder builder, IFormatProvider? provider, [InterpolatedStringHandlerArgument(nameof(builder), nameof(provider))] ref AppendInterpolatedStringHandler handler) => builder;

        /// <summary>
        /// Appends the specified interpolated string followed by the default line terminator to the end of the current StringBuilder object.
        /// </summary>
        /// <param name="builder">The <see cref="StringBuilder"/> to which to append line.</param>
        /// <param name="handler">The interpolated string to append.</param>
        /// <returns>A reference to this instance after the append operation has completed.</returns>
        public static StringBuilder AppendLine(this StringBuilder builder, [InterpolatedStringHandlerArgument(nameof(builder))] ref AppendInterpolatedStringHandler handler) => builder.AppendLine();

        /// <summary>
        /// Appends the specified interpolated string followed by the default line terminator to the end of the current StringBuilder object.
        /// </summary>
        /// <param name="builder">The <see cref="StringBuilder"/> to which to append line.</param>
        /// <param name="provider">An object that supplies culture-specific formatting information.</param>
        /// <param name="handler">The interpolated string to append.</param>
        /// <returns>A reference to this instance after the append operation has completed.</returns>
        public static StringBuilder AppendLine(this StringBuilder builder, IFormatProvider? provider, [InterpolatedStringHandlerArgument(nameof(builder), nameof(provider))] ref AppendInterpolatedStringHandler handler) => builder.AppendLine();
        /// <summary>
        /// Provides a handler used by the language compiler to append interpolated strings into <see cref="StringBuilder"/> instances.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [InterpolatedStringHandler]
        public readonly struct AppendInterpolatedStringHandler
        {
            // Implementation note:
            // As this type is only intended to be targeted by the compiler, public APIs eschew argument validation logic
            // in a variety of places, e.g. allowing a null input when one isn't expected to produce a NullReferenceException rather
            // than an ArgumentNullException.

            /// <summary>
            /// The associated StringBuilder to which to append.
            /// </summary>
            internal readonly StringBuilder _stringBuilder;
            /// <summary>
            /// Optional provider to pass to IFormattable.ToString or ISpanFormattable.TryFormat calls.
            /// </summary>
            private readonly IFormatProvider? _provider;
            /// <summary>
            /// Whether <see cref="_provider"/> provides an ICustomFormatter.
            /// </summary>
            /// <remarks>
            /// Custom formatters are very rare.  We want to support them, but it's ok if we make them more expensive
            /// in order to make them as pay-for-play as possible.  So, we avoid adding another reference type field
            /// to reduce the size of the handler and to reduce required zero'ing, by only storing whether the provider
            /// provides a formatter, rather than actually storing the formatter.  This in turn means, if there is a
            /// formatter, we pay for the extra interface call on each AppendFormatted that needs it.
            /// </remarks>
            private readonly bool _hasCustomFormatter;

            /// <summary>
            /// Creates a handler used to append an interpolated string into a <see cref="StringBuilder"/>.
            /// </summary>
            /// <param name="literalLength">The number of constant characters outside of interpolation expressions in the interpolated string.</param>
            /// <param name="formattedCount">The number of interpolation expressions in the interpolated string.</param>
            /// <param name="stringBuilder">The associated StringBuilder to which to append.</param>
            /// <remarks>This is intended to be called only by compiler-generated code. Arguments are not validated as they'd otherwise be for members intended to be used directly.</remarks>
            public AppendInterpolatedStringHandler(int literalLength, int formattedCount, StringBuilder stringBuilder)
            {
                _stringBuilder = stringBuilder;
                _provider = null;
                _hasCustomFormatter = false;
            }

            /// <summary>
            /// Creates a handler used to translate an interpolated string into a <see cref="string"/>.
            /// </summary>
            /// <param name="literalLength">The number of constant characters outside of interpolation expressions in the interpolated string.</param>
            /// <param name="formattedCount">The number of interpolation expressions in the interpolated string.</param>
            /// <param name="stringBuilder">The associated StringBuilder to which to append.</param>
            /// <param name="provider">An object that supplies culture-specific formatting information.</param>
            /// <remarks>This is intended to be called only by compiler-generated code. Arguments are not validated as they'd otherwise be for members intended to be used directly.</remarks>
            public AppendInterpolatedStringHandler(int literalLength, int formattedCount, StringBuilder stringBuilder, IFormatProvider? provider)
            {
                _stringBuilder = stringBuilder;
                _provider = provider;
                _hasCustomFormatter = provider is not null && DefaultInterpolatedStringHandler.HasCustomFormatter(provider);
            }

            /// <summary>
            /// Writes the specified string to the handler.
            /// </summary>
            /// <param name="value">The string to write.</param>
            public void AppendLiteral(string value) => _stringBuilder.Append(value);

            #region AppendFormatted
            // Design note:
            // This provides the same set of overloads and semantics as DefaultInterpolatedStringHandler.

            #region AppendFormatted T
            /// <summary>
            /// Writes the specified value to the handler.
            /// </summary>
            /// <param name="value">The value to write.</param>
            /// <typeparam name="T">The type of the value to write.</typeparam>
            public void AppendFormatted<T>(T value)
            {
                // This method could delegate to AppendFormatted with a null format, but explicitly passing
                // default as the format to TryFormat helps to improve code quality in some cases when TryFormat is inlined,
                // e.g. for Int32 it enables the JIT to eliminate code in the inlined method based on a length check on the format.

                if (_hasCustomFormatter)
                {
                    // If there's a custom formatter, always use it.
                    AppendCustomFormatter(value, format: null);
                }
                else if (value is IFormattable formattable)
                {
                    _stringBuilder.Append(formattable.ToString(format: null, _provider)); // constrained call avoiding boxing for value types
                }
                else if (value is not null)
                {
                    _stringBuilder.Append(value.ToString());
                }
            }

            /// <summary>
            /// Writes the specified value to the handler.
            /// </summary>
            /// <param name="value">The value to write.</param>
            /// <param name="format">The format string.</param>
            /// <typeparam name="T">The type of the value to write.</typeparam>
            public void AppendFormatted<T>(T value, string? format)
            {
                if (_hasCustomFormatter)
                {
                    // If there's a custom formatter, always use it.
                    AppendCustomFormatter(value, format);
                }
                else if (value is IFormattable formattable)
                {
                    _stringBuilder.Append(formattable.ToString(format, _provider)); // constrained call avoiding boxing for value types
                }
                else if (value is not null)
                {
                    _stringBuilder.Append(value.ToString());
                }
            }

            /// <summary>
            /// Writes the specified value to the handler.
            /// </summary>
            /// <param name="value">The value to write.</param>
            /// <param name="alignment">Minimum number of characters that should be written for this value.  If the value is negative, it indicates left-aligned and the required minimum is the absolute value.</param>
            /// <typeparam name="T">The type of the value to write.</typeparam>
            public void AppendFormatted<T>(T value, int alignment) =>
                AppendFormatted(value, alignment, format: null);

            /// <summary>
            /// Writes the specified value to the handler.
            /// </summary>
            /// <param name="value">The value to write.</param>
            /// <param name="format">The format string.</param>
            /// <param name="alignment">Minimum number of characters that should be written for this value.  If the value is negative, it indicates left-aligned and the required minimum is the absolute value.</param>
            /// <typeparam name="T">The type of the value to write.</typeparam>
            public void AppendFormatted<T>(T value, int alignment, string? format)
            {
                int startingPos = _stringBuilder.Length;
                AppendFormatted(value, format);
                if (alignment != 0)
                {
                    AppendOrInsertAlignmentIfNeeded(startingPos, alignment);
                }
            }

            #endregion

            #region AppendFormatted string
            /// <summary>
            /// Writes the specified value to the handler.
            /// </summary>
            /// <param name="value">The value to write.</param>
            public void AppendFormatted(string? value)
            {
                if (!_hasCustomFormatter)
                {
                    _stringBuilder.Append(value);
                }
                else
                {
                    AppendFormatted<string?>(value);
                }
            }

            /// <summary>
            /// Writes the specified value to the handler.
            /// </summary>
            /// <param name="value">The value to write.</param>
            /// <param name="alignment">Minimum number of characters that should be written for this value.  If the value is negative, it indicates left-aligned and the required minimum is the absolute value.</param>
            /// <param name="format">The format string.</param>
            public void AppendFormatted(string? value, int alignment = 0, string? format = null) =>
                // Format is meaningless for strings and doesn't make sense for someone to specify.  We have the overload
                // simply to disambiguate between ROS<char> and object, just in case someone does specify a format, as
                // string is implicitly convertible to both. Just delegate to the T-based implementation.
                AppendFormatted<string?>(value, alignment, format);
            #endregion

            #region AppendFormatted object
            /// <summary>
            /// Writes the specified value to the handler.
            /// </summary>
            /// <param name="value">The value to write.</param>
            /// <param name="alignment">Minimum number of characters that should be written for this value.  If the value is negative, it indicates left-aligned and the required minimum is the absolute value.</param>
            /// <param name="format">The format string.</param>
            public void AppendFormatted(object? value, int alignment = 0, string? format = null) =>
                // This overload is expected to be used rarely, only if either a) something strongly typed as object is
                // formatted with both an alignment and a format, or b) the compiler is unable to target type to T. It
                // exists purely to help make cases from (b) compile. Just delegate to the T-based implementation.
                AppendFormatted<object?>(value, alignment, format);
            #endregion
            #endregion

            /// <summary>
            /// Formats the value using the custom formatter from the provider.
            /// </summary>
            /// <param name="value">The value to write.</param>
            /// <param name="format">The format string.</param>
            /// <typeparam name="T">The type of the value to write.</typeparam>
            [MethodImpl(MethodImplOptions.NoInlining)]
            private void AppendCustomFormatter<T>(T value, string? format)
            {
                ICustomFormatter? formatter = (ICustomFormatter?)_provider?.GetFormat(typeof(ICustomFormatter));
                if (formatter is not null)
                {
                    _stringBuilder.Append(formatter.Format(format, value, _provider));
                }
            }

            /// <summary>
            /// Handles adding any padding required for aligning a formatted value in an interpolation expression.
            /// </summary>
            /// <param name="startingPos">The position at which the written value started.</param>
            /// <param name="alignment">Non-zero minimum number of characters that should be written for this value.  If the value is negative, it indicates left-aligned and the required minimum is the absolute value.</param>
            private void AppendOrInsertAlignmentIfNeeded(int startingPos, int alignment)
            {
                int _pos = _stringBuilder.Length;
                int charsWritten = _pos - startingPos;

                bool leftAlign = false;
                if (alignment < 0)
                {
                    leftAlign = true;
                    alignment = -alignment;
                }

                int paddingNeeded = alignment - charsWritten;
                if (paddingNeeded > 0)
                {
                    _ = leftAlign
                        ? _stringBuilder.Insert(_pos, " ", paddingNeeded)
                        : _stringBuilder.Insert(startingPos, " ", paddingNeeded);
                }
            }
        }
    }
}
