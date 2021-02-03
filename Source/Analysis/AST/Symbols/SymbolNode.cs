using System.Diagnostics;

namespace Krypton.Analysis.Ast.Symbols
{
    [DebuggerDisplay("{GetType().Name}; Identifier = {Identifier}")]
    public abstract class SymbolNode : Node
    {
        private protected SymbolNode(string identifier, int lineNumber) : base(lineNumber)
        {
            Identifier = identifier;
        }

        public string Identifier { get; }
    }
}
