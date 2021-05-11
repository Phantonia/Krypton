using Krypton.Core.CompilerServices;
using System;

namespace Krypton.Core
{
    [KryptonCompilerBan]
    [KryptonNamespaceSymbolsClass]
    public static class TopLevelSymbols
    {
        private const long PiNumerator = 104348;
        private const long PiDenominator = 33215;

        [KryptonRationalConstant(PiNumerator, PiDenominator)]
        public static readonly Rational Pi = new(PiNumerator, PiDenominator);

        public static void Output(string str)
            => Console.WriteLine(str);
    }
}
