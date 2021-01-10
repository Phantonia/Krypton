using Krypton.Analysis.AbstractSyntaxTree.Nodes.Identifiers;
using Krypton.Analysis.AbstractSyntaxTree.Nodes.Symbols;

namespace Krypton.Analysis.AbstractSyntaxTree
{
    public interface IBindable
    {
        IdentifierNode IdentifierNode { get; }

        void Bind(SymbolNode symbol);
    }
}
