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
            /// <summary>
            /// Gets whether the current machine has only a single processor.
            /// </summary>
            internal static bool IsSingleProcessor => Environment.ProcessorCount == 1;

            /// <summary>
            /// Gets a value that indicates whether the current process is a 64-bit process.
            /// </summary>
            /// <value><see langword="true"/> if the process is 64-bit; otherwise, <see langword="false"/>.</value>
            public static bool Is64BitProcess => IntPtr.Size == 8;

            /// <summary>
            /// Gets a value that indicates whether the current operating system is a 64-bit operating system.
            /// </summary>
            /// <value><see langword="true"/> if the operating system is 64-bit; otherwise, <see langword="false"/>.</value>
            public static bool Is64BitOperatingSystem => Environment.Is64BitProcess || Is64BitOperatingSystemWhen32BitProcess;
        }
    }
}
