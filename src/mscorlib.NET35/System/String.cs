﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace System
{
    /// <summary>
    /// Represents text as a sequence of UTF-16 code units.
    /// </summary>
    public static partial class StringEx
    {
        /// <summary>
        /// The extension for the <see cref="string"/> class.
        /// </summary>
        extension(string)
        {
            /// <summary>
            /// Creates a new string by using the specified provider to control the formatting of the specified interpolated string.
            /// </summary>
            /// <param name="provider">An object that supplies culture-specific formatting information.</param>
            /// <param name="handler">The interpolated string.</param>
            /// <returns>The string that results for formatting the interpolated string using the specified format provider.</returns>
            public static string Create(IFormatProvider? provider, [InterpolatedStringHandlerArgument(nameof(provider))] ref DefaultInterpolatedStringHandler handler) =>
                handler.ToStringAndClear();

            /// <summary>
            /// Indicates whether a specified string is <see langword="null"/>, empty, or consists only of white-space characters.
            /// </summary>
            /// <param name="value">The string to test.</param>
            /// <returns><see langword="true"/> if the <paramref name="value"/> parameter is <see langword="null"/> or <see cref="string.Empty"/>,
            /// or if <paramref name="value"/> consists exclusively of white-space characters.</returns>
            public static bool IsNullOrWhiteSpace([NotNullWhen(false)] string? value)
            {
#if NET40_OR_GREATER
                return string.IsNullOrWhiteSpace(value);
#else
                if (value == null)
                {
                    return true;
                }

                for (int i = 0; i < value.Length; i++)
                {
                    if (!char.IsWhiteSpace(value[i]))
                    {
                        return false;
                    }
                }

                return true;
#endif
            }
        }
    }
}
