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

        // Convenience method for sb.Length=0;
        /// <summary>
        /// Removes all characters from the current <see cref="StringBuilder"/> instance.
        /// </summary>
        /// <param name="builder">The <see cref="StringBuilder"/> to removes all characters.</param>
        /// <returns>An object whose <see cref="StringBuilder.Length"/> is 0 (zero).</returns>
        public static StringBuilder Clear(this StringBuilder builder)
        {
            builder.Length = 0;
            return builder;
        }
    }
}
