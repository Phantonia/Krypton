using Krypton.Analysis.AbstractSyntaxTree.Nodes.Symbols;

namespace Krypton.Analysis.AbstractSyntaxTree
{
    public interface IDeclaration
    {
        SymbolNode CreateSymbol();
    }
}
