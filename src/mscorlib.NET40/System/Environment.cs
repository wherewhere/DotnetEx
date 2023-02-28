namespace System
{
    /// <summary>
    /// Provides information about, and means to manipulate, the current environment and platform. This class cannot be inherited.
    /// </summary>
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
    }
}
