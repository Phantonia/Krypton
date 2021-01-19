using Krypton.Analysis.AST.Identifiers;
using Krypton.Analysis.AST.Symbols;

namespace Krypton.Analysis.AST
{
    public interface IBindable : INode
    {
        IdentifierNode IdentifierNode { get; }

        void Bind(SymbolNode symbol);
    }
}
