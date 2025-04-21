#if COMP_NETSTANDARD1_1
[assembly: System.Runtime.CompilerServices.TypeForwardedTo(typeof(System.Runtime.InteropServices.RuntimeInformation))]
#else
using System.Reflection;

namespace System.Runtime.InteropServices
{
    /// <summary>
    /// Provides information about the .NET runtime installation.
    /// </summary>
    public static partial class RuntimeInformation
    {
        private const string FrameworkName = ".NET Framework";

        /// <summary>
        /// Gets the name of the .NET installation on which an app is running.
        /// </summary>
        /// <value>The name of the .NET installation on which the app is running.</value>
        public static string FrameworkDescription
        {
            get
            {
                if (field == null)
                {
                    if (typeof(object).Assembly.GetCustomAttributes(typeof(AssemblyInformationalVersionAttribute), true) is [AssemblyInformationalVersionAttribute version, ..])
                    {
                        string versionString = version.InformationalVersion;

                        // Strip the git hash if there is one
                        int plusIndex = versionString.IndexOf('+');
                        if (plusIndex != -1)
                        {
                            versionString = versionString[..plusIndex];
                        }

                        field = !string.IsNullOrEmpty(versionString.Trim()) ? $"{FrameworkName} {versionString}" : FrameworkName;
                    }
                    else
                    {
                        field = FrameworkName;
                    }
                }

                return field;
            }

            private set;
        }

        /// <summary>
        /// Returns an opaque string that identifies the platform on which an app is running.
        /// </summary>
        /// <remarks>
        /// The property returns a string that identifies the operating system, typically including version,
        /// and processor architecture of the currently executing process.
        /// Since this string is opaque, it is not recommended to parse the string into its constituent parts.
        ///
        /// For more information, see https://docs.microsoft.com/dotnet/core/rid-catalog.
        /// </remarks>
        public static string RuntimeIdentifier
        {
            get
            {
                return OperatingSystem.OSPlatform switch
                {
                    "WINDOWS" => $"win-{OSArchitecture.ToString().ToLower()}",
                    "LINUX" => $"linux-{OSArchitecture.ToString().ToLower()}",
                    "OSX" => $"osx-{OSArchitecture.ToString().ToLower()}",
                    _ => $"unknown-{OSArchitecture.ToString().ToLower()}",
                };
            }
        }

        /// <summary>
        /// Indicates whether the current application is running on the specified platform.
        /// </summary>
        public static bool IsOSPlatform(OSPlatform osPlatform) => OperatingSystemEx.IsOSPlatform(osPlatform.Name);
    }
}
#endif