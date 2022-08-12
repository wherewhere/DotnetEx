using System.Diagnostics;
using System.Resources;

namespace System
{
    internal static class Utilities
    {
        internal static string ReadProcessOutput(string fileName)
        {
            return ReadProcessOutput(fileName, string.Empty);
        }

        internal static string ReadProcessOutput(string fileName, string args)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentException(Strings.Argument_EmptyValue, nameof(fileName));
            }

            string output;
            ProcessStartInfo processInfo = new()
            {
                FileName = fileName,
                Arguments = args ?? string.Empty,
                UseShellExecute = false,
                RedirectStandardOutput = true
            };

            using (Process process = Process.Start(processInfo))
            {
                output = process.StandardOutput.ReadToEnd();
                process.WaitForExit();
            }

            return output;
        }
    }
}