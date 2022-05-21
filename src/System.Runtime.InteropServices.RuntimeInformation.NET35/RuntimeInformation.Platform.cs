namespace System.Runtime.InteropServices
{
    public static partial class RuntimeInformation
    {
        private static string? s_osDescription;
        private static readonly int s_osArchPlusOne;

        public static string OSDescription
        {
            get
            {
                string? osDescription = s_osDescription;
                if (osDescription is null)
                {
                    switch (OperatingSystemEx.OSPlatform)
                    {
                        case "WINDOWS":
                            OperatingSystem os = Environment.OSVersion;
                            Version v = os.Version;

                            const string Version = "Microsoft Windows";
                            s_osDescription = osDescription = string.IsNullOrEmpty(os.ServicePack) ?
                                $"{Version} {(uint)v.Major}.{(uint)v.Minor}.{(uint)v.Build}" :
                                $"{Version} {(uint)v.Major}.{(uint)v.Minor}.{(uint)v.Build} {os.ServicePack}";
                            break;
                    }
                }

                return osDescription;
            }
        }

        /// <summary>
        /// Operating system architecture.
        /// </summary>
        public static Architecture OSArchitecture => EnvironmentEx.Is64BitOperatingSystem ? Architecture.X64 : Architecture.X86;
    }
}
