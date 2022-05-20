using System;

namespace System.Runtime.InteropServices
{
    public static partial class RuntimeInformation
	{
		/// <summary>
		/// Operating system platform.
		/// </summary>
		private static OSPlatform _osPlatform;

		/// <summary>
		/// Static constructor.
		/// </summary>
		static RuntimeInformation()
		{
			PlatformID platform = Environment.OSVersion.Platform;

			if (platform == PlatformID.Win32NT || platform == PlatformID.Win32S || platform == PlatformID.Xbox
				|| platform == PlatformID.Win32Windows || platform == PlatformID.WinCE)
			{
				_osPlatform = OSPlatform.Windows;
			}
			else if (platform == PlatformID.MacOSX)
			{
				_osPlatform = OSPlatform.OSX;
			}
			else if (platform == PlatformID.Unix)
			{
				string unixName = Utilities.ReadProcessOutput("uname") ?? string.Empty;
				if (unixName.Contains("Darwin"))
				{
					_osPlatform = OSPlatform.OSX;
				}
				else
				{
					_osPlatform = OSPlatform.Linux;
				}
			}
			else
			{
				_osPlatform = OSPlatform.Create("UNKNOWN");
			}
		}


		public static bool IsOSPlatform(OSPlatform osPlatform)
		{
			return osPlatform == _osPlatform;
		}
	}
}