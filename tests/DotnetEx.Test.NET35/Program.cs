using System;

namespace DotnetEx.Test.NET35
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine(EnvironmentEx.Is64BitProcess);
            Console.WriteLine("End");
            while (true) ;
        }
    }
}
