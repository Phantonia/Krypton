using Krypton.CompilationData.Symbols;
using System.IO;

namespace Krypton.CompilationData.Syntax.Types
{
    public abstract class BoundTypeNode : TypeNode
    {
        private protected BoundTypeNode(TypeSymbol type, SyntaxNode? parent) : base(parent)
        {
            TypeSymbol = type;
        }

        public abstract TypeNode TypeNode { get; }

        public TypeSymbol TypeSymbol { get; }

        public abstract override BoundTypeNode WithParent(SyntaxNode newParent);
    }

    public sealed class BoundTypeNode<TType> : BoundTypeNode
        where TType : TypeNode
    {
        public BoundTypeNode(TType typeNode,
                             TypeSymbol typeSymbol,
                             SyntaxNode? parent = null)
            : base(typeSymbol, parent)
        {
            TypeNode = typeNode;
        }

        public override bool IsLeaf => false;

        public override TType TypeNode { get; }

        public override BoundTypeNode<TType> Bind(TypeSymbol typeSymbol)
            => typeSymbol == TypeSymbol ? this : new(TypeNode, typeSymbol);

        public override BoundTypeNode<TType> WithParent(SyntaxNode newParent)
            => new(TypeNode, TypeSymbol, newParent);

        public override void WriteCode(TextWriter output) => TypeNode.WriteCode(output);
    }
}
