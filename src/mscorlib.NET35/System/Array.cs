namespace System
{
    /// <summary>
    /// Provides methods for creating, manipulating, searching, and sorting arrays, thereby serving as the base class for all arrays in the common language runtime.
    /// </summary>
    public static class ArrayEx
    {
        /// <summary>
        /// The extension for the <see cref="Array"/> class.
        /// </summary>
        extension(Array)
        {
            /// <summary>
            /// Returns an empty array.
            /// </summary>
            /// <typeparam name="T">The type of the elements of the array.</typeparam>
            /// <returns>An empty array.</returns>
            public static T[] Empty<T>() =>
#if NET46_OR_GREATER
                [];
#else
                EmptyArray<T>.Value;
#endif
        }

#if !NET46_OR_GREATER
        private static class EmptyArray<T>
        {
            internal static readonly T[] Value = [];
        }
#endif
    }
}
