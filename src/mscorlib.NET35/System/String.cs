// ==++==
// 
//   Copyright (c) Microsoft Corporation.  All rights reserved.
// 
// ==--==
/*============================================================
**
** Class:  String
**
**
** Purpose: Your favorite String class.  Native methods 
** are implemented in StringNative.cpp
**
**
===========================================================*/
using System.Collections.Generic;
using System.Text;

namespace System
{
    /// <summary>
    /// Represents text as a sequence of UTF-16 code units.
    /// </summary>
    public static class StringEx
    {
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
        public static string Join(string separator, params object[] values)
        {
            if (values == null)
            {
                throw new ArgumentNullException("values");
            }

            if (values.Length == 0 || values[0] == null)
            {
                return string.Empty;
            }

            if (separator == null)
            {
                separator = string.Empty;
            }

            StringBuilder result = StringBuilderCache.Acquire();

            string value = values[0].ToString();
            if (value != null)
            {
                result.Append(value);
            }

            for (int i = 1; i < values.Length; i++)
            {
                result.Append(separator);
                if (values[i] != null)
                {
                    // handle the case where their ToString() override is broken
                    value = values[i].ToString();
                    if (value != null)
                    {
                        result.Append(value);
                    }
                }
            }
            return StringBuilderCache.GetStringAndRelease(result);
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
        public static string Join<T>(string separator, IEnumerable<T> values)
        {
            if (values == null)
            {
                throw new ArgumentNullException("values");
            }

            if (separator == null)
            {
                separator = string.Empty;
            }

            using IEnumerator<T> en = values.GetEnumerator();
            if (!en.MoveNext())
            {
                return string.Empty;
            }

            StringBuilder result = StringBuilderCache.Acquire();
            if (en.Current != null)
            {
                // handle the case that the enumeration has null entries
                // and the case where their ToString() override is broken
                string value = en.Current.ToString();
                if (value != null)
                {
                    result.Append(value);
                }
            }

            while (en.MoveNext())
            {
                result.Append(separator);
                if (en.Current != null)
                {
                    // handle the case that the enumeration has null entries
                    // and the case where their ToString() override is broken
                    string value = en.Current.ToString();
                    if (value != null)
                    {
                        result.Append(value);
                    }
                }
            }
            return StringBuilderCache.GetStringAndRelease(result);
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
        public static string Join(string separator, IEnumerable<string> values)
        {
            if (values == null)
            {
                throw new ArgumentNullException("values");
            }

            if (separator == null)
            {
                separator = string.Empty;
            }

            using IEnumerator<string> en = values.GetEnumerator();
            if (!en.MoveNext())
            {
                return string.Empty;
            }

            StringBuilder result = StringBuilderCache.Acquire();
            if (en.Current != null)
            {
                result.Append(en.Current);
            }

            while (en.MoveNext())
            {
                result.Append(separator);
                if (en.Current != null)
                {
                    result.Append(en.Current);
                }
            }
            return StringBuilderCache.GetStringAndRelease(result);
        }

        /// <summary>
        /// Indicates whether a specified string is <see langword="null"/>, empty, or consists only of white-space characters.
        /// </summary>
        /// <param name="value">The string to test.</param>
        /// <returns><see langword="true"/> if the <paramref name="value"/> parameter is <see langword="null"/> or <see cref="string.Empty"/>,
        /// or if <paramref name="value"/> consists exclusively of white-space characters.</returns>
        public static bool IsNullOrWhiteSpace(string value)
        {
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
        }
    }
}
