namespace System
{
    /// <summary>
    /// Represents information about an operating system, such as the version and platform identifier. This class cannot be inherited.
    /// </summary>
    public static class OperatingSystemEx
    {
        /// <summary>
        /// The extension for the <see cref="OperatingSystem"/> class.
        /// </summary>
        extension(OperatingSystem)
        {
            /// <summary>
            /// Get the name of operating system platform
            /// </summary>
            public static string OSPlatform
            {
                get
                {
                    PlatformID platform = Environment.OSVersion.Platform;

                    if (platform is PlatformID.Win32NT or PlatformID.Win32S or PlatformID.Xbox
                        or PlatformID.Win32Windows or PlatformID.WinCE)
                    {
                        return "WINDOWS";
                    }
                    else if (platform == PlatformID.MacOSX)
                    {
                        return "OSX";
                    }
                    else if (platform == PlatformID.Unix)
                    {
                        string unixName = Utilities.ReadProcessOutput("uname") ?? string.Empty;
                        return unixName.Contains("Darwin") ? "OSX" : "LINUX";
                    }
                    else
                    {
                        return "UNKNOWN";
                    }
                }
            }

            /// <summary>
            /// Indicates whether the current application is running on the specified platform.
            /// </summary>
            /// <param name="platform">Case-insensitive platform name. Examples: Browser, Linux, FreeBSD, Android, iOS, macOS, tvOS, watchOS, Windows.</param>
            public static bool IsOSPlatform(string platform)
            {
                return platform == null
                    ? throw new ArgumentNullException(nameof(platform))
                    : platform.Equals(OperatingSystem.OSPlatform, StringComparison.OrdinalIgnoreCase);
            }

            /// <summary>
            /// Check for the OS with a >= version comparison. Used to guard APIs that were added in the given OS release.
            /// </summary>
            /// <param name="platform">Case-insensitive platform name. Examples: Browser, Linux, FreeBSD, Android, iOS, macOS, tvOS, watchOS, Windows.</param>
            /// <param name="major">Major OS version number.</param>
            /// <param name="minor">Minor OS version number (optional).</param>
            /// <param name="build">Build OS version number (optional).</param>
            /// <param name="revision">Revision OS version number (optional).</param>
            public static bool IsOSPlatformVersionAtLeast(string platform, int major, int minor = 0, int build = 0, int revision = 0)
                => OperatingSystem.IsOSPlatform(platform) && OperatingSystem.IsOSVersionAtLeast(major, minor, build, revision);

            /// <summary>
            /// Indicates whether the current application is running as WASM in a Browser.
            /// </summary>
            public static bool IsBrowser() => OperatingSystem.IsOSPlatform("BROWSER");

            /// <summary>
            /// Indicates whether the current application is running on Linux.
            /// </summary>
            public static bool IsLinux() => OperatingSystem.IsOSPlatform("LINUX");

            /// <summary>
            /// Indicates whether the current application is running on FreeBSD.
            /// </summary>
            public static bool IsFreeBSD() => OperatingSystem.IsOSPlatform("FREEBSD");

            /// <summary>
            /// Check for the FreeBSD version (returned by 'uname') with a >= version comparison. Used to guard APIs that were added in the given FreeBSD release.
            /// </summary>
            public static bool IsFreeBSDVersionAtLeast(int major, int minor = 0, int build = 0, int revision = 0)
                => OperatingSystem.IsFreeBSD() && OperatingSystem.IsOSVersionAtLeast(major, minor, build, revision);

            /// <summary>
            /// Indicates whether the current application is running on Android.
            /// </summary>
            public static bool IsAndroid() => OperatingSystem.IsOSPlatform("ANDROID");

            /// <summary>
            /// Check for the Android API level (returned by 'ro.build.version.sdk') with a >= version comparison. Used to guard APIs that were added in the given Android release.
            /// </summary>
            public static bool IsAndroidVersionAtLeast(int major, int minor = 0, int build = 0, int revision = 0)
                => OperatingSystem.IsAndroid() && OperatingSystem.IsOSVersionAtLeast(major, minor, build, revision);

            /// <summary>
            /// Indicates whether the current application is running on iOS or MacCatalyst.
            /// </summary>
            public static bool IsIOS() => OperatingSystem.IsOSPlatform("IOS");

            /// <summary>
            /// Check for the iOS/MacCatalyst version (returned by 'libobjc.get_operatingSystemVersion') with a >= version comparison. Used to guard APIs that were added in the given iOS release.
            /// </summary>
            public static bool IsIOSVersionAtLeast(int major, int minor = 0, int build = 0)
                => OperatingSystem.IsIOS() && OperatingSystem.IsOSVersionAtLeast(major, minor, build, 0);

            /// <summary>
            /// Indicates whether the current application is running on macOS.
            /// </summary>
            public static bool IsMacOS() => OperatingSystem.IsOSPlatform("OSX");

            internal static bool IsOSXLike() => OperatingSystem.IsOSPlatform("OSX");

            /// <summary>
            /// Check for the macOS version (returned by 'libobjc.get_operatingSystemVersion') with a >= version comparison. Used to guard APIs that were added in the given macOS release.
            /// </summary>
            public static bool IsMacOSVersionAtLeast(int major, int minor = 0, int build = 0)
                => OperatingSystem.IsMacOS() && OperatingSystem.IsOSVersionAtLeast(major, minor, build, 0);

            /// <summary>
            /// Indicates whether the current application is running on Mac Catalyst.
            /// </summary>
            public static bool IsMacCatalyst() => OperatingSystem.IsOSPlatform("OSX");

            /// <summary>
            /// Check for the Mac Catalyst version (iOS version as presented in Apple documentation) with a >= version comparison. Used to guard APIs that were added in the given Mac Catalyst release.
            /// </summary>
            public static bool IsMacCatalystVersionAtLeast(int major, int minor = 0, int build = 0)
                => OperatingSystem.IsMacCatalyst() && OperatingSystem.IsOSVersionAtLeast(major, minor, build, 0);

            /// <summary>
            /// Indicates whether the current application is running on tvOS.
            /// </summary>
            public static bool IsTvOS() => OperatingSystem.IsOSPlatform("TVOS");

            /// <summary>
            /// Check for the tvOS version (returned by 'libobjc.get_operatingSystemVersion') with a >= version comparison. Used to guard APIs that were added in the given tvOS release.
            /// </summary>
            public static bool IsTvOSVersionAtLeast(int major, int minor = 0, int build = 0)
                => OperatingSystem.IsTvOS() && OperatingSystem.IsOSVersionAtLeast(major, minor, build, 0);

            /// <summary>
            /// Indicates whether the current application is running on watchOS.
            /// </summary>
            public static bool IsWatchOS() => OperatingSystem.IsOSPlatform("WATCHOS");

            /// <summary>
            /// Check for the watchOS version (returned by 'libobjc.get_operatingSystemVersion') with a >= version comparison. Used to guard APIs that were added in the given watchOS release.
            /// </summary>
            public static bool IsWatchOSVersionAtLeast(int major, int minor = 0, int build = 0)
                => OperatingSystem.IsWatchOS() && OperatingSystem.IsOSVersionAtLeast(major, minor, build, 0);

            /// <summary>
            /// Indicates whether the current application is running on Windows.
            /// </summary>
            public static bool IsWindows() => OperatingSystem.IsOSPlatform("WINDOWS");

            /// <summary>
            /// Check for the Windows version (returned by 'RtlGetVersion') with a >= version comparison. Used to guard APIs that were added in the given Windows release.
            /// </summary>
            public static bool IsWindowsVersionAtLeast(int major, int minor = 0, int build = 0, int revision = 0)
                => OperatingSystem.IsWindows() && OperatingSystem.IsOSVersionAtLeast(major, minor, build, revision);

            private static bool IsOSVersionAtLeast(int major, int minor, int build, int revision)
            {
                Version current = Environment.OSVersion.Version;

                if (current.Major != major)
                {
                    return current.Major > major;
                }
                if (current.Minor != minor)
                {
                    return current.Minor > minor;
                }
                if (current.Build != build)
                {
                    return current.Build > build;
                }

                return current.Revision >= revision
                    || (current.Revision == -1 && revision == 0); // it is unavailable on OSX and Environment.OSVersion.Version.Revision returns -1
            }
        }
    }
}
