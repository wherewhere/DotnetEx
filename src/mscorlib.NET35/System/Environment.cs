namespace System
{
    /// <summary>
    /// Provides information about, and means to manipulate, the current environment and platform. This class cannot be inherited.
    /// </summary>
    public static partial class EnvironmentEx
    {
        /// <summary>
        /// The extension for the <see cref="Environment"/> class.
        /// </summary>
        extension(Environment)
        {
#if NET40_OR_GREATER
            /// <summary>
            /// Gets whether the current machine has only a single processor.
            /// </summary>
            internal static bool IsSingleProcessor => Environment.ProcessorCount == 1;
#endif

            /// <summary>
            /// Gets a value that indicates whether the current process is a 64-bit process.
            /// </summary>
            /// <value><see langword="true"/> if the process is 64-bit; otherwise, <see langword="false"/>.</value>
            public static bool Is64BitProcess =>
#if NET40_OR_GREATER
                Environment.Is64BitProcess;
#else
                IntPtr.Size == 8;
#endif

            /// <summary>
            /// Gets a value that indicates whether the current operating system is a 64-bit operating system.
            /// </summary>
            /// <value><see langword="true"/> if the operating system is 64-bit; otherwise, <see langword="false"/>.</value>
            public static bool Is64BitOperatingSystem =>
#if NET40_OR_GREATER
                Environment.Is64BitOperatingSystem;
#else
                Environment.Is64BitProcess || Is64BitOperatingSystemWhen32BitProcess;
#endif
        }
    }
}
