using Krypton.CompilationData.Symbols;
using Krypton.CompilationData.Syntax;
using Krypton.Utilities;
using System.Collections.Generic;

namespace Krypton.CompilationData
{
    public sealed class Compilation
    {
        public Compilation(ProgramNode programNode,
                           IEnumerable<Symbol> symbols,
                           IEnumerable<Diagnostic> diagnostics)
        {
            ProgramNode = programNode;
            Symbols = symbols.Finalize();
            Diagnostics = diagnostics.Finalize();
        }

        public FinalList<Diagnostic> Diagnostics { get; }

        public bool IsValid => Diagnostics.Count == 0;

        public ProgramNode ProgramNode { get; }

        public FinalList<Symbol> Symbols { get; }
    }
}
