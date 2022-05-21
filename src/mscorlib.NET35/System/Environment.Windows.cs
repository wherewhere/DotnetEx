using System.Diagnostics;
using System.Runtime.InteropServices;

namespace System
{
    public static partial class EnvironmentEx
    {
        [DllImport("kernel32.dll", SetLastError = true, CallingConvention = CallingConvention.Winapi)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool IsWow64Process(
            [In] IntPtr hProcess,
            [Out] out bool wow64Process
        );

        private static bool Is64BitOperatingSystemWhen32BitProcess
        {
            get
            {
                if ((Environment.OSVersion.Version.Major == 5 && Environment.OSVersion.Version.Minor >= 1) ||
                    Environment.OSVersion.Version.Major >= 6)
                {
                    using Process p = Process.GetCurrentProcess();
                    return IsWow64Process(p.Handle, out bool retVal) && retVal;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
