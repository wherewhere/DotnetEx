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
    public static class StringEx
    {
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
