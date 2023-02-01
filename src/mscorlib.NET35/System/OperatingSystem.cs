using System.Diagnostics;
using System.Resources;
using System.Runtime.Serialization;

namespace System
{
    public sealed class OperatingSystemEx : ISerializable, ICloneable
    {
        private readonly string _servicePack;
        private string _versionString;

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

        public OperatingSystemEx(PlatformID platform, Version version) : this(platform, version, null)
        {
        }

        internal OperatingSystemEx(PlatformID platform, Version version, string servicePack)
        {
            if (platform is < PlatformID.Win32S or > PlatformID.MacOSX)
            {
                throw new ArgumentOutOfRangeException(nameof(platform), platform, string.Format(Strings.Arg_EnumIllegalVal, platform));
            }

            Platform = platform;
            Version = version ?? throw new ArgumentNullException(nameof(version));
            _servicePack = servicePack;
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            throw new PlatformNotSupportedException();
        }

        public PlatformID Platform { get; }

        public string ServicePack => _servicePack ?? string.Empty;

        public Version Version { get; }

        public object Clone() => new OperatingSystemEx(Platform, Version, _servicePack);

        public override string ToString() => VersionString;

        public string VersionString
        {
            get
            {
                if (_versionString == null)
                {
                    string os;
                    switch (Platform)
                    {
                        case PlatformID.Win32S: os = "Microsoft Win32S "; break;
                        case PlatformID.Win32Windows: os = (Version.Major > 4 || (Version.Major == 4 && Version.Minor > 0)) ? "Microsoft Windows 98 " : "Microsoft Windows 95 "; break;
                        case PlatformID.Win32NT: os = "Microsoft Windows NT "; break;
                        case PlatformID.WinCE: os = "Microsoft Windows CE "; break;
                        case PlatformID.Unix: os = "Unix "; break;
                        case PlatformID.Xbox: os = "Xbox "; break;
                        case PlatformID.MacOSX: os = "Mac OS X "; break;
                        default:
                            Debug.Fail($"Unknown platform {Platform}");
                            os = "<unknown> "; break;
                    }

                    _versionString = string.IsNullOrEmpty(_servicePack) ?
                        $"{os}{Version}" :
                        $"{os}{Version.ToString(3)} {_servicePack}";
                }

                return _versionString;
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
                : platform.Equals(OSPlatform, StringComparison.OrdinalIgnoreCase);
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
            => IsOSPlatform(platform) && IsOSVersionAtLeast(major, minor, build, revision);

        /// <summary>
        /// Indicates whether the current application is running as WASM in a Browser.
        /// </summary>
        public static bool IsBrowser() => IsOSPlatform("BROWSER");

        /// <summary>
        /// Indicates whether the current application is running on Linux.
        /// </summary>
        public static bool IsLinux() => IsOSPlatform("LINUX");

        /// <summary>
        /// Indicates whether the current application is running on FreeBSD.
        /// </summary>
        public static bool IsFreeBSD() => IsOSPlatform("FREEBSD");

        /// <summary>
        /// Check for the FreeBSD version (returned by 'uname') with a >= version comparison. Used to guard APIs that were added in the given FreeBSD release.
        /// </summary>
        public static bool IsFreeBSDVersionAtLeast(int major, int minor = 0, int build = 0, int revision = 0)
            => IsFreeBSD() && IsOSVersionAtLeast(major, minor, build, revision);

        /// <summary>
        /// Indicates whether the current application is running on Android.
        /// </summary>
        public static bool IsAndroid() => IsOSPlatform("ANDROID");

        /// <summary>
        /// Check for the Android API level (returned by 'ro.build.version.sdk') with a >= version comparison. Used to guard APIs that were added in the given Android release.
        /// </summary>
        public static bool IsAndroidVersionAtLeast(int major, int minor = 0, int build = 0, int revision = 0)
            => IsAndroid() && IsOSVersionAtLeast(major, minor, build, revision);

        /// <summary>
        /// Indicates whether the current application is running on iOS or MacCatalyst.
        /// </summary>
        public static bool IsIOS() => IsOSPlatform("IOS");

        /// <summary>
        /// Check for the iOS/MacCatalyst version (returned by 'libobjc.get_operatingSystemVersion') with a >= version comparison. Used to guard APIs that were added in the given iOS release.
        /// </summary>
        public static bool IsIOSVersionAtLeast(int major, int minor = 0, int build = 0)
            => IsIOS() && IsOSVersionAtLeast(major, minor, build, 0);

        /// <summary>
        /// Indicates whether the current application is running on macOS.
        /// </summary>
        public static bool IsMacOS() => IsOSPlatform("OSX");

        internal static bool IsOSXLike() => IsOSPlatform("OSX");

        /// <summary>
        /// Check for the macOS version (returned by 'libobjc.get_operatingSystemVersion') with a >= version comparison. Used to guard APIs that were added in the given macOS release.
        /// </summary>
        public static bool IsMacOSVersionAtLeast(int major, int minor = 0, int build = 0)
            => IsMacOS() && IsOSVersionAtLeast(major, minor, build, 0);

        /// <summary>
        /// Indicates whether the current application is running on Mac Catalyst.
        /// </summary>
        public static bool IsMacCatalyst() => IsOSPlatform("OSX");

        /// <summary>
        /// Check for the Mac Catalyst version (iOS version as presented in Apple documentation) with a >= version comparison. Used to guard APIs that were added in the given Mac Catalyst release.
        /// </summary>
        public static bool IsMacCatalystVersionAtLeast(int major, int minor = 0, int build = 0)
            => IsMacCatalyst() && IsOSVersionAtLeast(major, minor, build, 0);

        /// <summary>
        /// Indicates whether the current application is running on tvOS.
        /// </summary>
        public static bool IsTvOS() => IsOSPlatform("TVOS");

        /// <summary>
        /// Check for the tvOS version (returned by 'libobjc.get_operatingSystemVersion') with a >= version comparison. Used to guard APIs that were added in the given tvOS release.
        /// </summary>
        public static bool IsTvOSVersionAtLeast(int major, int minor = 0, int build = 0)
            => IsTvOS() && IsOSVersionAtLeast(major, minor, build, 0);

        /// <summary>
        /// Indicates whether the current application is running on watchOS.
        /// </summary>
        public static bool IsWatchOS() => IsOSPlatform("WATCHOS");

        /// <summary>
        /// Check for the watchOS version (returned by 'libobjc.get_operatingSystemVersion') with a >= version comparison. Used to guard APIs that were added in the given watchOS release.
        /// </summary>
        public static bool IsWatchOSVersionAtLeast(int major, int minor = 0, int build = 0)
            => IsWatchOS() && IsOSVersionAtLeast(major, minor, build, 0);

        /// <summary>
        /// Indicates whether the current application is running on Windows.
        /// </summary>
        public static bool IsWindows() => IsOSPlatform("WINDOWS");

        /// <summary>
        /// Check for the Windows version (returned by 'RtlGetVersion') with a >= version comparison. Used to guard APIs that were added in the given Windows release.
        /// </summary>
        public static bool IsWindowsVersionAtLeast(int major, int minor = 0, int build = 0, int revision = 0)
            => IsWindows() && IsOSVersionAtLeast(major, minor, build, revision);

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
