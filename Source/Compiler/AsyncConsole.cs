using System;
using System.Threading.Tasks;

namespace Krypton.Compiler
{
    internal static class AsyncConsole
    {
        public static Task<string?> ReadLineAsync()
        {
            return Task.Run(() => Console.ReadLine());
        }

        public static Task<ConsoleKeyInfo> ReadKeyAsync()
        {
            return Task.Run(() => Console.ReadKey());
        }

        public static async Task<string?> ReadTrimmedLineAsync()
        {
            return (await Task.Run(() => Console.ReadLine()))?.Trim();
        }
    }
}
