// Licensed to the .NET Foundation under one or more agreements.
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
        }
    }
}
