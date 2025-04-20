﻿using System;
using System.Runtime.InteropServices;

namespace DotnetEx.Test.NET35
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine(Environment.Is64BitProcess);
            Console.WriteLine(RuntimeInformation.OSDescription);
            Console.WriteLine(RuntimeInformation.RuntimeIdentifier);
            Console.WriteLine(RuntimeInformation.FrameworkDescription);

            Console.Write("Press any key to exit...");
            Console.ReadKey(true);
        }
    }
}
