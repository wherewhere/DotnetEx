namespace System
{
    public static partial class EnvironmentEx
    {
        /// <summary>
        /// Gets whether the current machine has only a single processor.
        /// </summary>
        internal static bool IsSingleProcessor => Environment.ProcessorCount == 1;

        private static string[] s_commandLineArgs;

        internal static void SetCommandLineArgs(string[] cmdLineArgs) // invoked from VM
        {
            s_commandLineArgs = cmdLineArgs;
        }

        public static bool Is64BitProcess = IntPtr.Size == 8;

        public static bool Is64BitOperatingSystem = Is64BitProcess || Is64BitOperatingSystemWhen32BitProcess;
    }
}
