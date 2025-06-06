using System;
using System.Diagnostics;
using System.Resources;

internal static class Utilities
{
    internal static string ReadProcessOutput(string fileName, string args = "")
    {
        if (string.IsNullOrWhiteSpace(fileName))
        {
            throw new ArgumentException(Strings.Argument_EmptyValue, nameof(fileName));
        }

        ProcessStartInfo processInfo = new()
        {
            FileName = fileName,
            Arguments = args ?? string.Empty,
            UseShellExecute = false,
            RedirectStandardOutput = true
        };

        using Process process = Process.Start(processInfo);

        string output = process.StandardOutput.ReadToEnd();
        process.WaitForExit();

        return output;
    }
}