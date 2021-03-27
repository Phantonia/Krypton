using Krypton.CompilationData.Symbols;
using System.IO;

namespace Krypton.CompilationData.Syntax.Expressions
{
    public abstract class TypedExpressionNode : ExpressionNode
    {
        private protected TypedExpressionNode(TypeSymbol typeSymbol,
                                              SyntaxNode? parent)
            : base(parent)
        {
            TypeSymbol = typeSymbol;
        }

        public abstract ExpressionNode ExpressionNode { get; }

        public TypeSymbol TypeSymbol { get; }

        public abstract override TypedExpressionNode WithParent(SyntaxNode newParent);
    }

    public sealed class TypedExpressionNode<TExpression> : TypedExpressionNode
        where TExpression : ExpressionNode
    {
        public TypedExpressionNode(TExpression expression,
                                   TypeSymbol typeSymbol,
                                   SyntaxNode? parent = null)
            : base(typeSymbol, parent)
        {
            ExpressionNode = (TExpression)expression.WithParent(this);
        }

        public override TExpression ExpressionNode { get; }

        public override bool IsLeaf => false;

        public override TypedExpressionNode<TExpression> Type(TypeSymbol type)
            => type == TypeSymbol ? this : new(ExpressionNode, type);

        public TypedExpressionNode<TExpression> WithChildren(TExpression? expression = null,
                                                             TypeSymbol? typeSymbol = null)
            => new(expression ?? ExpressionNode,
                   typeSymbol ?? TypeSymbol);

        public override TypedExpressionNode<TExpression> WithParent(SyntaxNode newParent)
            => new(ExpressionNode, TypeSymbol, newParent);

        public override void WriteCode(TextWriter output) => ExpressionNode.WriteCode(output);
    }
}
