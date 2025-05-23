﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace System
{
    /// <summary>
    /// Represent a range has start and end indexes.
    /// </summary>
    /// <remarks>
    /// Range is used by the C# compiler to support the range syntax.
    /// <code>
    /// int[] someArray = new int[5] { 1, 2, 3, 4, 5 };
    /// int[] subArray1 = someArray[0..2]; // { 1, 2 }
    /// int[] subArray2 = someArray[1..^0]; // { 2, 3, 4, 5 }
    /// </code>
    /// </remarks>
    public readonly struct Range : IEquatable<Range>
    {
        /// <summary>
        /// Represent the inclusive start index of the Range.
        /// </summary>
        public Index Start { get; }

        /// <summary>
        /// Represent the exclusive end index of the Range.
        /// </summary>
        public Index End { get; }

        /// <summary>
        /// Construct a Range object using the start and end indexes.
        /// </summary>
        /// <param name="start">Represent the inclusive start index of the range.</param>
        /// <param name="end">Represent the exclusive end index of the range.</param>
        public Range(Index start, Index end)
        {
            Start = start;
            End = end;
        }

        /// <summary>
        /// Indicates whether the current Range object is equal to another object of the same type.
        /// </summary>
        /// <param name="value">An object to compare with this object</param>
        public override bool Equals(object value) =>
            value is Range r &&
            r.Start.Equals(Start) &&
            r.End.Equals(End);

        /// <summary>
        /// Indicates whether the current Range object is equal to another Range object.
        /// </summary>
        /// <param name="other">An object to compare with this object</param>
        public bool Equals(Range other) => other.Start.Equals(Start) && other.End.Equals(End);

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        public override int GetHashCode() => HashCode.Combine(Start.GetHashCode(), End.GetHashCode());

        /// <summary>
        /// Converts the value of the current Range object to its equivalent string representation.
        /// </summary>
        public override string ToString() => $"{Start}..{End}";

        /// <summary>
        /// Create a Range object starting from start index to the end of the collection.
        /// </summary>
        public static Range StartAt(Index start) => new(start, Index.End);

        /// <summary>
        /// Create a Range object starting from first element in the collection to the end Index.
        /// </summary>
        public static Range EndAt(Index end) => new(Index.Start, end);

        /// <summary>
        /// Create a Range object starting from first element to the end.
        /// </summary>
        public static Range All => new(Index.Start, Index.End);

#if NET40_OR_GREATER
        /// <summary>
        /// Calculate the start offset and length of range object using a collection length.
        /// </summary>
        /// <param name="length">The length of the collection that the range will be used with. length has to be a positive value.</param>
        /// <remarks>
        /// For performance reason, we don't validate the input length parameter against negative values.
        /// It is expected Range will be used with collections which always have non negative length/count.
        /// We validate the range is inside the length scope though.
        /// </remarks>
        [ComponentModel.EditorBrowsable(ComponentModel.EditorBrowsableState.Never)]
        [Runtime.CompilerServices.MethodImpl((Runtime.CompilerServices.MethodImplOptions)0x100)]
        public Tuple<int, int> GetOffsetAndLength(int length)
        {
            int start = Start.GetOffset(length);
            int end = End.GetOffset(length);

            return (uint)end > (uint)length || (uint)start > (uint)end
                ? throw new ArgumentOutOfRangeException(nameof(length))
                : new Tuple<int, int>(start, end - start);
        }
#endif
    }
}