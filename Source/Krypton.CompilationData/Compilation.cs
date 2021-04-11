using Krypton.CompilationData.Symbols;
using Krypton.CompilationData.Syntax;
using Krypton.Utilities;

namespace Krypton.CompilationData
{
    public sealed class Compilation
    {
        public Compilation(ProgramNode program, ReadOnlyList<Symbol> symbols, ReadOnlyList<Diagnostic> diagnostics)
        {
            Program = program;
            Symbols = symbols;
            Diagnostics = diagnostics;
        }

        public ReadOnlyList<Diagnostic> Diagnostics { get; }

        public ProgramNode Program { get; }

        public ReadOnlyList<Symbol> Symbols { get; }
    }
}
