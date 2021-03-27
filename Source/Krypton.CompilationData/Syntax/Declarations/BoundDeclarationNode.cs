using Krypton.CompilationData.Symbols;
using System.IO;

namespace Krypton.CompilationData.Syntax.Declarations
{
    public abstract class BoundDeclarationNode : DeclarationNode
    {
        private protected BoundDeclarationNode(Symbol symbol,
                                               SyntaxNode? parent)
            : base(parent)
        {
            Symbol = symbol;
        }

        public abstract DeclarationNode DeclarationNode { get; }

        public Symbol Symbol { get; }

        public abstract override BoundDeclarationNode WithParent(SyntaxNode newParent);
    }

    public sealed class BoundDeclarationNode<TDeclaration> : BoundDeclarationNode
        where TDeclaration : DeclarationNode
    {
        public BoundDeclarationNode(TDeclaration declaration,
                                    Symbol symbol,
                                    SyntaxNode? parent = null)
            : base(symbol, parent)
        {
            DeclarationNode = (TDeclaration)declaration.WithParent(this);
        }

        public override TDeclaration DeclarationNode { get; }

        public override bool IsLeaf => false;

        public override BoundDeclarationNode<TDeclaration> Bind(Symbol symbol)
            => symbol == Symbol ? this : new(DeclarationNode, symbol);

        public override BoundDeclarationNode<TDeclaration> WithParent(SyntaxNode newParent)
            => new(DeclarationNode, Symbol, newParent);

        public override void WriteCode(TextWriter output) => DeclarationNode.WriteCode(output);
    }
}
