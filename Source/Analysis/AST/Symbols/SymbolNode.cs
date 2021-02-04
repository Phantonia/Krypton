using System.Diagnostics;

namespace Krypton.Analysis.Ast.Symbols
{
    [DebuggerDisplay("{GetType().Name}; Identifier = {Identifier}")]
    public abstract class SymbolNode : Node
    {
        private protected SymbolNode(string identifier, int lineNumber, int index) : base(lineNumber, index)
        {
            Identifier = identifier;
        }

        public string Identifier { get; }
    }
}
