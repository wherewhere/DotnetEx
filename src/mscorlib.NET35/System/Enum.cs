using System.Diagnostics.CodeAnalysis;

namespace System
{
    /// <summary>
    /// Provides the base class for enumerations.
    /// </summary>
    public static class EnumEx
    {
        /// <summary>
        /// The extension for the <see cref="Enum"/> class.
        /// </summary>
        extension(Enum)
        {
            /// <summary>
            /// Converts the string representation of the name or numeric value of one or more enumerated constants specified by <typeparamref name="TEnum"/> to an equivalent enumerated object.
            /// </summary>
            /// <typeparam name="TEnum">An enumeration type.</typeparam>
            /// <param name="value">A string containing the name or value to convert.</param>
            /// <returns><typeparamref name="TEnum"/> An object of type <typeparamref name="TEnum"/> whose value is represented by <paramref name="value"/>.</returns>
            /// <exception cref="ArgumentNullException"><paramref name="value"/> is <see langword="null"/>.</exception>
            /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an <see cref="Enum"/> type</exception>
            /// <exception cref="ArgumentException"><paramref name="value"/> does not contain enumeration information</exception>
            public static TEnum Parse<TEnum>(string value) where TEnum : struct =>
                Parse<TEnum>(value, false);

            /// <summary>
            /// Converts the string representation of the name or numeric value of one or more enumerated constants specified by <typeparamref name="TEnum"/> to an equivalent enumerated object.
            /// A parameter specifies whether the operation is case-insensitive.
            /// </summary>
            /// <typeparam name="TEnum">An enumeration type.</typeparam>
            /// <param name="value">A string containing the name or value to convert.</param>
            /// <param name="ignoreCase"><see langword="true"/> to ignore case; <see langword="false"/> to regard case.</param>
            /// <returns><typeparamref name="TEnum"/> An object of type <typeparamref name="TEnum"/> whose value is represented by <paramref name="value"/>.</returns>
            /// <exception cref="ArgumentNullException"><paramref name="value"/> is <see langword="null"/>.</exception>
            /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an <see cref="Enum"/> type</exception>
            /// <exception cref="ArgumentException"><paramref name="value"/> does not contain enumeration information</exception>
            public static TEnum Parse<TEnum>(string value, bool ignoreCase) where TEnum : struct =>
                (TEnum)Enum.Parse(typeof(TEnum), value, ignoreCase);

            /// <summary>
            /// Converts the string representation of the name or numeric value of one or more enumerated constants to an equivalent enumerated object.
            /// </summary>
            /// <param name="enumType">The enum type to use for parsing.</param>
            /// <param name="value">The string representation of the name or numeric value of one or more enumerated constants.</param>
            /// <param name="result">When this method returns <see langword="true"/>, an object containing an enumeration constant representing the parsed value.</param>
            /// <returns><see langword="true"/> if the conversion succeeded; <see langword="false"/> otherwise.</returns>
            public static bool TryParse(Type enumType, string? value, [NotNullWhen(true)] out object? result) =>
                TryParse(enumType, value, false, out result);

            /// <summary>
            /// Converts the string representation of the name or numeric value of one or more enumerated constants to an equivalent enumerated object.
            /// A parameter specifies whether the operation is case-insensitive.
            /// </summary>
            /// <param name="enumType">The enum type to use for parsing.</param>
            /// <param name="value">The string representation of the name or numeric value of one or more enumerated constants.</param>
            /// <param name="ignoreCase"><see langword="true"/> to read <paramref name="enumType"/> in case insensitive mode; <see langword="false"/> to read <paramref name="enumType"/> in case sensitive mode.</param>
            /// <param name="result">When this method returns <see langword="true"/>, an object containing an enumeration constant representing the parsed value.</param>
            /// <returns><see langword="true"/> if the conversion succeeded; <see langword="false"/> otherwise.</returns>
            public static bool TryParse(Type enumType, string? value, bool ignoreCase, [NotNullWhen(true)] out object? result)
            {
                try
                {
                    result = Enum.Parse(enumType, value, ignoreCase);
                    return true;
                }
                catch (Exception ex) when (enumType.IsEnum || ex is not ArgumentException)
                {
                }
                result = null;
                return false;
            }

            /// <summary>
            /// Converts the string representation of the name or numeric value of one or more enumerated constants
            /// to an equivalent enumerated object. The return value indicates whether the conversion succeeded.
            /// </summary>
            /// <typeparam name="TEnum">The enumeration type to which to convert <paramref name="value"/>.</typeparam>
            /// <param name="value">The case-sensitive string representation of the enumeration name or underlying value to convert.</param>
            /// <param name="result">When this method returns, contains an object of type <typeparamref name="TEnum"/> whose
            /// value is represented by <paramref name="value"/> if the parse operation succeeds. If the parse operation fails, contains the
            /// default value of the underlying type of <typeparamref name="TEnum"/>. This parameter is passed uninitialized.</param>
            /// <returns><see langword="true"/> if the <paramref name="value"/> parameter was converted successfully; otherwise, <see langword="false"/>.</returns>
            /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enumeration type</exception>
            public static bool TryParse<TEnum>(string value, out TEnum result) where TEnum : struct =>
#if NET40_OR_GREATER
                Enum.TryParse(value, out result);
#else
                TryParse(value, false, out result);
#endif

            /// <summary>
            /// Converts the string representation of the name or numeric value of one or more enumerated constants
            /// to an equivalent enumerated object. A parameter specifies whether the operation is case-sensitive.
            /// The return value indicates whether the conversion succeeded.
            /// </summary>
            /// <typeparam name="TEnum">The enumeration type to which to convert <paramref name="value"/>.</typeparam>
            /// <param name="value">The string representation of the enumeration name or underlying value to convert.</param>
            /// <param name="ignoreCase"><see langword="true"/> to ignore case; <see langword="false"/> to consider case.</param>
            /// <param name="result">When this method returns, contains an object of type <typeparamref name="TEnum"/> whose
            /// value is represented by <paramref name="value"/> if the parse operation succeeds. If the parse operation fails, contains the
            /// default value of the underlying type of <typeparamref name="TEnum"/>. This parameter is passed uninitialized.</param>
            /// <returns><see langword="true"/> if the <paramref name="value"/> parameter was converted successfully; otherwise, <see langword="false"/>.</returns>
            /// <exception cref="ArgumentException"><typeparamref name="TEnum"/> is not an enumeration type</exception>
            public static bool TryParse<TEnum>(string value, bool ignoreCase, out TEnum result) where TEnum : struct
            {
#if NET40_OR_GREATER
                return Enum.TryParse(value, ignoreCase, out result);
#else
                try
                {
                    result = (TEnum)Enum.Parse(typeof(TEnum), value, ignoreCase);
                    return true;
                }
                catch (Exception ex) when (typeof(TEnum).IsEnum || ex is not ArgumentException)
                {
                }
                result = default;
                return false;
#endif
            }
        }
    }
}
