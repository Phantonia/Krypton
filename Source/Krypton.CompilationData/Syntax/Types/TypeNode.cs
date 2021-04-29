using Krypton.CompilationData.Symbols;

namespace Krypton.CompilationData.Syntax.Types
{
    public abstract record TypeNode : SyntaxNode
    {
        private protected TypeNode() { }

        public abstract BoundTypeNode Bind(TypeSymbol typeSymbol);
    }
}
