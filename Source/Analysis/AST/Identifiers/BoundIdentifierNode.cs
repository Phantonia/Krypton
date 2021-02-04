using Krypton.Analysis.Ast.Symbols;
using System.Collections.Generic;

namespace Krypton.Analysis.Ast.Identifiers
{
    public sealed class BoundIdentifierNode : IdentifierNode
    {
        internal BoundIdentifierNode(string identifier, SymbolNode symbol, int lineNumber, int index) : base(identifier, lineNumber, index)
        {
            Symbol = symbol;
        }

        public SymbolNode Symbol { get; }

        public override void PopulateBranches(List<Node> list)
        {
            list.Add(this);
            // It's important that we don't populate with Symbol.
            // In the best case we repeat stuff in the enumeration;
            // in the worst case (recursive definitions) we StackOverflow.
        }
    }
}
