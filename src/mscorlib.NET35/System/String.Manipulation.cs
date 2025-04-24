using System.Collections.Generic;
using System.Text;

namespace System
{
    public static partial class StringEx
    {
        /// <summary>
        /// The extension for the <see cref="string"/> class.
        /// </summary>
        extension(string text)
        {
            /// <summary>
            /// Concatenates the members of a constructed <see cref="IEnumerable{String}"/> collection of type <see cref="string"/>.
            /// </summary>
            /// <param name="values">A collection object that implements <see cref="IEnumerable{String}"/> and whose generic type argument is <see cref="string"/>.</param>
            /// <returns>The concatenated strings in <paramref name="values"/>, or <see cref="string.Empty"/> if <paramref name="values"/> is an empty <see cref="IEnumerable{String}"/>.</returns>
            public static string Concat(IEnumerable<string?> values)
            {
#if NET40_OR_GREATER
                return string.Concat(values);
#else
                ArgumentNullException.ThrowIfNull(values);

                using IEnumerator<string?> en = values.GetEnumerator();
                if (!en.MoveNext())
                    return string.Empty;

                string? firstValue = en.Current;

                if (!en.MoveNext())
                {
                    return firstValue ?? string.Empty;
                }

                StringBuilder result = StringBuilderCache.Acquire();

                result.Append(firstValue);

                do
                {
                    result.Append(en.Current);
                }
                while (en.MoveNext());

                return StringBuilderCache.GetStringAndRelease(result);
#endif
            }

            /// <summary>
            /// Concatenates the members of an <see cref="IEnumerable{T}"/> implementation.
            /// </summary>
            /// <typeparam name="T">The type of the members of <paramref name="values"/>.</typeparam>
            /// <param name="values">A collection object that implements the <see cref="IEnumerable{T}"/> interface.</param>
            /// <returns>The concatenated members in <paramref name="values"/>.</returns>
            public static string Concat<T>(IEnumerable<T> values)
            {
#if NET40_OR_GREATER
                return string.Concat(values);
#else
                ArgumentNullException.ThrowIfNull(values);

                if (typeof(T) == typeof(string))
                {
                    if (values is string?[] valuesArray)
                    {
                        return string.Concat(valuesArray);
                    }
                }

                using IEnumerator<T> e = values.GetEnumerator();
                if (!e.MoveNext())
                {
                    // If the enumerator is empty, just return an empty string.
                    return string.Empty;
                }

                if (e is IEnumerator<char> en)
                {
                    // Special-case T==char, as we can handle that case much more efficiently,
                    // and string.Concat(IEnumerable<char>) can be used as an efficient
                    // enumerable-based equivalent of new string(char[]).

                    char c = en.Current; // save the first value
                    if (!en.MoveNext())
                    {
                        // There was only one char.  Return a string from it directly.
                        return new string([c]);
                    }

                    // Create the builder, add the char we already enumerated,
                    // add the rest, and then get the resulting string.
                    StringBuilder result = StringBuilderCache.Acquire();
                    result.Append(c); // first value
                    do
                    {
                        c = en.Current;
                        result.Append(c);
                    }
                    while (en.MoveNext());
                    return StringBuilderCache.GetStringAndRelease(result);
                }
                else
                {
                    // For all other Ts, fall back to calling ToString on each and appending the resulting
                    // string to a builder.

                    string? firstString = e.Current?.ToString();  // save the first value
                    if (!e.MoveNext())
                    {
                        return firstString ?? string.Empty;
                    }

                    StringBuilder result = StringBuilderCache.Acquire();

                    result.Append(firstString);
                    do
                    {
                        result.Append(e.Current?.ToString());
                    }
                    while (e.MoveNext());

                    return StringBuilderCache.GetStringAndRelease(result);
                }
#endif
            }

            /// <summary>
            /// Concatenates an array of strings, using the specified separator between each member.
            /// </summary>
            /// <param name="separator">The character to use as a separator. <paramref name="separator"/> is included in the returned string only if <paramref name="value"/> has more than one element.</param>
            /// <param name="value">An array of strings to concatenate.</param>
            /// <returns>A string that consists of the elements of <paramref name="value"/> delimited by the <paramref name="separator"/> character.
            /// <para>-or-</para> <see cref="string.Empty"/> if <paramref name="value"/> has zero elements.</returns>
            public static string Join(char separator, params string?[] value)
            {
                ArgumentNullException.ThrowIfNull(value);

                if (value.Length <= 1)
                {
                    return value.Length <= 0 ?
                        string.Empty :
                        value[0] ?? string.Empty;
                }

                return string.Join(new string([separator]), value);
            }

            /// <summary>
            /// Concatenates an array of strings, using the specified separator between each member, starting with the element in <paramref name="value"/> located at the <paramref name="startIndex"/> position, and concatenating up to <paramref name="count"/> elements.
            /// </summary>
            /// <param name="separator">Concatenates an array of strings, using the specified separator between each member, starting with the element located at the specified index and including a specified number of elements.</param>
            /// <param name="value">An array of strings to concatenate.</param>
            /// <param name="startIndex">The first item in <paramref name="value"/> to concatenate.</param>
            /// <param name="count">The number of elements from <paramref name="value"/> to concatenate, starting with the element in the <paramref name="startIndex"/> position.</param>
            /// <returns>A string that consists of <paramref name="count"/> elements of <paramref name="value"/> starting at <paramref name="startIndex"/> delimited by the separator character.
            /// <para>-or-</para> <see cref="string.Empty"/> if <paramref name="count"/> is zero.</returns>
            public static string Join(char separator, string?[] value, int startIndex, int count)
            {
                ArgumentNullException.ThrowIfNull(value);

                if (value.Length <= 1)
                {
                    return value.Length <= 0 ?
                        string.Empty :
                        value[0] ?? string.Empty;
                }

                return string.Join(new string([separator]), value, startIndex, count);
            }

            /// <summary>
            /// Concatenates the members of a constructed <see cref="IEnumerable{T}"/> collection of type <see cref="string"/>,
            /// using the specified separator between each member.
            /// </summary>
            /// <param name="separator">The string to use as a separator. <paramref name="separator"/> is included in
            /// the returned string only if values has more than one element.</param>
            /// <param name="values">A collection that contains the strings to concatenate.</param>
            /// <returns>A string that consists of the elements of <paramref name="values"/> delimited by the <paramref name="separator"/> string.
            /// <para>-or-</para><see cref="string.Empty"/> if <paramref name="values"/> has zero elements.</returns>
            /// <exception cref="ArgumentNullException"><paramref name="values"/> is <see langword="null"/>.</exception>
            public static string Join(string? separator, IEnumerable<string?> values)
            {
#if NET40_OR_GREATER
                return string.Join(separator, values);
#else
                ArgumentNullException.ThrowIfNull(values);

                using IEnumerator<string?> en = values.GetEnumerator();
                if (!en.MoveNext())
                {
                    return string.Empty;
                }

                string? firstValue = en.Current;

                if (!en.MoveNext())
                {
                    // Only one value available
                    return firstValue ?? string.Empty;
                }

                // Null separator and values are handled by the StringBuilder
                StringBuilder result = StringBuilderCache.Acquire();

                result.Append(firstValue);

                do
                {
                    result.Append(separator);
                    result.Append(en.Current);
                }
                while (en.MoveNext());

                return StringBuilderCache.GetStringAndRelease(result);
#endif
            }

            /// <summary>
            /// Concatenates the string representations of an array of objects, using the specified separator between each member.
            /// </summary>
            /// <param name="separator">The character to use as a separator. <paramref name="separator"/> is included in the returned string only if <paramref name="values"/> has more than one element.</param>
            /// <param name="values">An array of objects whose string representations will be concatenated.</param>
            /// <returns>A string that consists of the elements of <paramref name="values"/> delimited by the <paramref name="separator"/> character.
            /// <para>-or-</para> <see cref="string.Empty"/> if <paramref name="values"/> has zero elements.</returns>
            public static string Join(char separator, params object?[] values)
            {
                ArgumentNullException.ThrowIfNull(values);

                if (values.Length <= 0)
                {
                    return string.Empty;
                }

                string? firstString = values[0]?.ToString();

                if (values.Length == 1)
                {
                    return firstString ?? string.Empty;
                }

                StringBuilder result = StringBuilderCache.Acquire();

                result.Append(firstString);

                for (int i = 1; i < values.Length; i++)
                {
                    result.Append(separator);
                    object? value = values[i];
                    if (value != null)
                    {
                        result.Append(value.ToString());
                    }
                }

                return StringBuilderCache.GetStringAndRelease(result);
            }

            /// <summary>
            /// Concatenates the elements of an object array, using the specified separator between each element.
            /// </summary>
            /// <param name="separator">The string to use as a separator. <paramref name="separator"/> is included in
            /// the returned string only if <paramref name="values"/> has more than one element.</param>
            /// <param name="values">An array that contains the elements to concatenate.</param>
            /// <returns>A string that consists of the elements of <paramref name="values"/> delimited by the <paramref name="separator"/> string.
            /// <para>-or-</para><see cref="string.Empty"/> if <paramref name="values"/> has zero elements.<para>-or-</para>
            /// .NET Framework only: <see cref="string.Empty"/> if the first element of <paramref name="values"/> is <see langword="null"/>.</returns>
            /// <exception cref="ArgumentNullException"><paramref name="values"/> is <see langword="null"/>.</exception>
            public static string Join(string? separator, params object?[] values)
            {
#if NET40_OR_GREATER
                return string.Join(separator, values);
#else
                ArgumentNullException.ThrowIfNull(values);

                if (values.Length <= 0)
                {
                    return string.Empty;
                }

                string? firstString = values[0]?.ToString();

                if (values.Length == 1)
                {
                    return firstString ?? string.Empty;
                }

                StringBuilder result = StringBuilderCache.Acquire();

                result.Append(firstString);

                for (int i = 1; i < values.Length; i++)
                {
                    result.Append(separator);
                    object? value = values[i];
                    if (value != null)
                    {
                        result.Append(value.ToString());
                    }
                }

                return StringBuilderCache.GetStringAndRelease(result);
#endif
            }

            /// <summary>
            /// Concatenates the members of a collection, using the specified separator between each member.
            /// </summary>
            /// <typeparam name="T">The type of the members of <paramref name="values"/>.</typeparam>
            /// <param name="separator">The character to use as a separator. <paramref name="separator"/> is included in the returned string only if <paramref name="values"/> has more than one element.</param>
            /// <param name="values">A collection that contains the objects to concatenate.</param>
            /// <returns>A string that consists of the members of <paramref name="values"/> delimited by the <paramref name="separator"/> character.
            /// <para>-or-</para> <see cref="string.Empty"/> if <paramref name="values"/> has no elements.</returns>
            public static string Join<T>(char separator, IEnumerable<T> values)
            {
                ArgumentNullException.ThrowIfNull(values);

                if (typeof(T) == typeof(string))
                {
                    if (values is string?[] valuesArray)
                    {
                        return string.Join(separator, valuesArray);
                    }
                }

                using IEnumerator<T> e = values.GetEnumerator();
                if (!e.MoveNext())
                {
                    // If the enumerator is empty, just return an empty string.
                    return string.Empty;
                }

                if (e is IEnumerator<char> en)
                {
                    // Special-case T==char, as we can handle that case much more efficiently,
                    // and string.Concat(IEnumerable<char>) can be used as an efficient
                    // enumerable-based equivalent of new string(char[]).

                    char c = en.Current; // save the first value
                    if (!en.MoveNext())
                    {
                        // There was only one char.  Return a string from it directly.
                        return new string([c]);
                    }

                    // Create the builder, add the char we already enumerated,
                    // add the rest, and then get the resulting string.
                    StringBuilder result = StringBuilderCache.Acquire();
                    result.Append(c); // first value
                    do
                    {
                        result.Append(separator);
                        c = en.Current;
                        result.Append(c);
                    }
                    while (en.MoveNext());
                    return StringBuilderCache.GetStringAndRelease(result);
                }
                else
                {
                    // For all other Ts, fall back to calling ToString on each and appending the resulting
                    // string to a builder.

                    string? firstString = e.Current?.ToString();  // save the first value
                    if (!e.MoveNext())
                    {
                        return firstString ?? string.Empty;
                    }

                    StringBuilder result = StringBuilderCache.Acquire();

                    result.Append(firstString);
                    do
                    {
                        result.Append(separator);
                        result.Append(e.Current?.ToString());
                    }
                    while (e.MoveNext());

                    return StringBuilderCache.GetStringAndRelease(result);
                }
            }

            /// <summary>
            /// Concatenates the members of a collection, using the specified separator between each member.
            /// </summary>
            /// <typeparam name="T">The type of the members of <paramref name="values"/>.</typeparam>
            /// <param name="separator">The string to use as a separator. <paramref name="separator"/> is included in
            /// the returned string only if <paramref name="values"/> has more than one element.</param>
            /// <param name="values">A collection that contains the objects to concatenate.</param>
            /// <returns>A string that consists of the elements of <paramref name="values"/> delimited by the <paramref name="separator"/> string.
            /// <para>-or-</para><see cref="string.Empty"/> if <paramref name="values"/> has zero elements.</returns>
            /// <exception cref="ArgumentNullException"><paramref name="values"/> is <see langword="null"/>.</exception>
            public static string Join<T>(string? separator, IEnumerable<T> values)
            {
#if NET40_OR_GREATER
                return string.Join(separator, values);
#else

                ArgumentNullException.ThrowIfNull(values);

                if (typeof(T) == typeof(string))
                {
                    if (values is string?[] valuesArray)
                    {
                        return string.Join(separator, valuesArray);
                    }
                }

                using IEnumerator<T> e = values.GetEnumerator();
                if (!e.MoveNext())
                {
                    // If the enumerator is empty, just return an empty string.
                    return string.Empty;
                }

                if (e is IEnumerator<char> en)
                {
                    // Special-case T==char, as we can handle that case much more efficiently,
                    // and string.Concat(IEnumerable<char>) can be used as an efficient
                    // enumerable-based equivalent of new string(char[]).

                    char c = en.Current; // save the first value
                    if (!en.MoveNext())
                    {
                        // There was only one char.  Return a string from it directly.
                        return new string([c]);
                    }

                    // Create the builder, add the char we already enumerated,
                    // add the rest, and then get the resulting string.
                    StringBuilder result = StringBuilderCache.Acquire();
                    result.Append(c); // first value
                    do
                    {
                        result.Append(separator);
                        c = en.Current;
                        result.Append(c);
                    }
                    while (en.MoveNext());
                    return StringBuilderCache.GetStringAndRelease(result);
                }
                else
                {
                    // For all other Ts, fall back to calling ToString on each and appending the resulting
                    // string to a builder.

                    string? firstString = e.Current?.ToString();  // save the first value
                    if (!e.MoveNext())
                    {
                        return firstString ?? string.Empty;
                    }

                    StringBuilder result = StringBuilderCache.Acquire();

                    result.Append(firstString);
                    do
                    {
                        result.Append(separator);
                        result.Append(e.Current?.ToString());
                    }
                    while (e.MoveNext());

                    return StringBuilderCache.GetStringAndRelease(result);
                }
#endif
            }

            /// <summary>
            /// Splits a string into substrings based on a specified delimiting character and, optionally, options.
            /// </summary>
            /// <param name="separator">A character that delimits the substrings in this string.</param>
            /// <param name="options">A bitwise combination of the enumeration values that specifies whether to trim substrings and include empty substrings.</param>
            /// <returns>An array whose elements contain the substrings from this instance that are delimited by <paramref name="separator"/>.</returns>
            public string[] Split(char separator, StringSplitOptions options = StringSplitOptions.None) =>
                text.Split([separator], options);

            /// <summary>
            /// Splits a string into a maximum number of substrings based on a specified delimiting character and, optionally, options. Splits a string into a maximum number of substrings based on the provided character separator, optionally omitting empty substrings from the result.
            /// </summary>
            /// <param name="separator">A character that delimits the substrings in this instance.</param>
            /// <param name="count">The maximum number of elements expected in the array.</param>
            /// <param name="options">A bitwise combination of the enumeration values that specifies whether to trim substrings and include empty substrings.</param>
            /// <returns>An array that contains at most <paramref name="count"/> substrings from this instance that are delimited by <paramref name="separator"/>.</returns>
            public string[] Split(char separator, int count, StringSplitOptions options = StringSplitOptions.None) =>
                text.Split([separator], count, options);

            /// <summary>
            /// Splits a string into substrings that are based on the provided string separator.
            /// </summary>
            /// <param name="separator">A string that delimits the substrings in this string.</param>
            /// <param name="options">A bitwise combination of the enumeration values that specifies whether to trim substrings and include empty substrings.</param>
            /// <returns>An array whose elements contain the substrings from this instance that are delimited by <paramref name="separator"/>.</returns>
            public string[] Split(string? separator, StringSplitOptions options = StringSplitOptions.None) =>
                text.Split([separator], options);

            /// <summary>
            /// Splits a string into a maximum number of substrings based on a specified delimiting string and, optionally, options.
            /// </summary>
            /// <param name="separator">A string that delimits the substrings in this instance.</param>
            /// <param name="count">The maximum number of elements expected in the array.</param>
            /// <param name="options">A bitwise combination of the enumeration values that specifies whether to trim substrings and include empty substrings.</param>
            /// <returns>An array that contains at most <paramref name="count"/> substrings from this instance that are delimited by <paramref name="separator"/>.</returns>
            public string[] Split(string? separator, int count, StringSplitOptions options = StringSplitOptions.None) =>
                text.Split([separator], count, options);

            /// <summary>
            /// Removes all leading and trailing instances of a character from the current string.
            /// </summary>
            /// <param name="trimChar">A Unicode character to remove.</param>
            /// <returns>The string that remains after all instances of the <paramref name="trimChar"/> character are removed from the start and end of the current string. If no characters can be trimmed from the current instance, the method returns the current instance unchanged.</returns>
            public string Trim(char trimChar) => text.Trim([trimChar]);

            /// <summary>
            /// Removes all the leading occurrences of a specified character from the current string.
            /// </summary>
            /// <param name="trimChar">The Unicode character to remove.</param>
            /// <returns>The string that remains after all occurrences of the <paramref name="trimChar"/> character are removed from the start of the current string. If no characters can be trimmed from the current instance, the method returns the current instance unchanged.</returns>
            public string TrimStart(char trimChar) => text.TrimStart([trimChar]);

            /// <summary>
            /// Removes all the trailing occurrences of a character from the current string.
            /// </summary>
            /// <param name="trimChar">A Unicode character to remove.</param>
            /// <returns>The string that remains after all occurrences of the <paramref name="trimChar"/> character are removed from the end of the current string. If no characters can be trimmed from the current instance, the method returns the current instance unchanged.</returns>
            public string TrimEnd(char trimChar) => text.TrimEnd([trimChar]);
        }
    }
}
