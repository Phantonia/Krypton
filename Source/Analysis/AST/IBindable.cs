using Krypton.Analysis.Ast.Identifiers;
using Krypton.Analysis.Ast.Symbols;

namespace Krypton.Analysis.Ast
{
    public interface IBindable : INode
    {
        IdentifierNode IdentifierNode { get; }

        void Bind(SymbolNode symbol);
    }
}
