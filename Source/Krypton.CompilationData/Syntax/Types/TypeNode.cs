using Krypton.CompilationData.Symbols;

namespace Krypton.CompilationData.Syntax.Types
{
    public abstract class TypeNode : SyntaxNode
    {
        private protected TypeNode(SyntaxNode? parent) : base(parent) { }

        public abstract BoundTypeNode Bind(TypeSymbol typeSymbol);

        public abstract override TypeNode WithParent(SyntaxNode newParent);
    }
}
