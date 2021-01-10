using Krypton.Analysis.AbstractSyntaxTree.Nodes.Symbols;

namespace Krypton.Analysis.Semantical.Binding
{
    public interface IIdentifierMap
    {
        SymbolNode? this[string identifier] { get; }

        bool AddSymbol(string identifier, SymbolNode symbol);
    }
}