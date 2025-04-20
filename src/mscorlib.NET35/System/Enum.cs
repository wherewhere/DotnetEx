// ==++==
//
//   Copyright (c) Microsoft Corporation.  All rights reserved.
//
// ==--==
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
            /// Converts the string representation of the name or numeric value of one or more enumerated constants
            /// to an equivalent enumerated object. The return value indicates whether the conversion succeeded.
            /// </summary>
            /// <typeparam name="TEnum">The enumeration type to which to convert <paramref name="value"/>.</typeparam>
            /// <param name="value">The case-sensitive string representation of the enumeration name or underlying value to convert.</param>
            /// <param name="result">When this method returns, contains an object of type <typeparamref name="TEnum"/> whose
            /// value is represented by <paramref name="value"/> if the parse operation succeeds. If the parse operation fails, contains the
            /// default value of the underlying type of <typeparamref name="TEnum"/>. This parameter is passed uninitialized.</param>
            /// <returns><see langword="true"/> if the <paramref name="value"/> parameter was converted successfully; otherwise, <see langword="false"/>.</returns>
            public static bool TryParse<TEnum>(string value, out TEnum result) where TEnum : struct
            {
                return TryParse(value, false, out result);
            }

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
            public static bool TryParse<TEnum>(string value, bool ignoreCase, out TEnum result) where TEnum : struct
            {
                string strTypeFixed = value.Replace(' ', '_');
                if (Enum.IsDefined(typeof(TEnum), strTypeFixed))
                {
                    result = (TEnum)Enum.Parse(typeof(TEnum), strTypeFixed, ignoreCase);
                    return true;
                }
                else
                {
                    foreach (string str in Enum.GetNames(typeof(TEnum)))
                    {
                        if (str.Equals(strTypeFixed, StringComparison.OrdinalIgnoreCase))
                        {
                            result = (TEnum)Enum.Parse(typeof(TEnum), str, ignoreCase);
                            return true;
                        }
                    }
                    result = default;
                    return false;
                }
            }
        }
    }
}
