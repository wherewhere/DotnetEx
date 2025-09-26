namespace System
{
    public static partial class StringEx
    {
        /// <summary>
        /// The extension for the <see cref="string"/> class.
        /// </summary>
        /// <param name="text">The <see cref="string"/> to extend.</param>
        extension(string text)
        {
            /// <summary>
            /// Returns a value indicating whether a specified string occurs within this string, using the specified comparison rules.
            /// </summary>
            /// <param name="value">The string to seek.</param>
            /// <param name="comparisonType">One of the enumeration values that specifies the rules to use in the comparison.</param>
            /// <returns><see langword="true"/> if the <paramref name="value"/> parameter occurs within this string, or if <paramref name="value"/> is the empty string (""); otherwise, <see langword="false"/>.</returns>
            public bool Contains(string value, StringComparison comparisonType) => text.IndexOf(value, comparisonType) >= 0;

            /// <summary>
            /// Returns a value indicating whether a specified character occurs within this string.
            /// </summary>
            /// <param name="value">The character to seek.</param>
            /// <returns><see langword="true"/> if the <paramref name="value"/> parameter occurs within this string; otherwise, <see langword="false"/>.</returns>
            public bool Contains(char value) => text.IndexOf(value) >= 0;

            /// <summary>
            /// Returns a value indicating whether a specified character occurs within this string, using the specified comparison rules.
            /// </summary>
            /// <param name="value">The character to seek.</param>
            /// <param name="comparisonType">One of the enumeration values that specifies the rules to use in the comparison.</param>
            /// <returns><see langword="true"/> if the <paramref name="value"/> parameter occurs within this string; otherwise, <see langword="false"/>.</returns>
            public bool Contains(char value, StringComparison comparisonType) => text.IndexOf(value, comparisonType) >= 0;

            /// <summary>
            /// Reports the zero-based index of the first occurrence of the specified Unicode character in this string. A parameter specifies the type of search to use for the specified character.
            /// </summary>
            /// <param name="value">The character to seek.</param>
            /// <param name="comparisonType">An enumeration value that specifies the rules for the search.</param>
            /// <returns>The zero-based index of <paramref name="value"/> if that character is found, or -1 if it is not.</returns>
            public int IndexOf(char value, StringComparison comparisonType) => comparisonType switch
            {
                StringComparison.Ordinal => text.IndexOf(value),
                _ => text.IndexOf(new string([value]), comparisonType)
            };
        }
    }
}
