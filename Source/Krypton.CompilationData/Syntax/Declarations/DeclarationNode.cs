using Krypton.CompilationData.Symbols;
using System.Diagnostics.CodeAnalysis;

namespace Krypton.CompilationData.Syntax.Declarations
{
    public abstract class DeclarationNode : SyntaxNode
    {
        private protected DeclarationNode(SyntaxNode? parent) : base(parent) { }

        public bool IsBound => this is BoundDeclarationNode;

        public abstract BoundDeclarationNode Bind(Symbol symbol);

        public bool TryGetSymbol([NotNullWhen(true)] out Symbol? symbol)
        {
            if (this is BoundDeclarationNode boundDeclaration)
            {
                symbol = boundDeclaration.Symbol;
                return true;
            }

            symbol = null;
            return false;
        }

        public abstract override DeclarationNode WithParent(SyntaxNode newParent);
    }
}
