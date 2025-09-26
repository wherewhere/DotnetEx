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
            /// Determines whether the end of this string instance matches the specified character.
            /// </summary>
            /// <param name="value">The character to compare to the character at the end of this instance.</param>
            /// <returns><see langword="true"/> if <paramref name="value"/> matches the end of this instance; otherwise, <see langword="false"/>.</returns>
            public bool EndsWith(char value) => text.EndsWith(new string([value]));

            /// <summary>
            /// Determines whether this string instance starts with the specified character.
            /// </summary>
            /// <param name="value">The character to compare.</param>
            /// <returns><see langword="true"/> if <paramref name="value"/> matches the beginning of this instance; otherwise, <see langword="false"/>.</returns>
            public bool StartsWith(char value) => text.StartsWith(new string([value]));
        }
    }
}
