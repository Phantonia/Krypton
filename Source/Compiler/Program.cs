using System;
using System.IO;

namespace Krypton.Compiler
{
    internal static class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length >= 1 && File.Exists(args[0]))
            {
                Console.WriteLine($"Compiling file {args[0]}");
            }
            else
            {
                Console.WriteLine("Please paste the link to the file to compile.");
            }
        }
    }
}
