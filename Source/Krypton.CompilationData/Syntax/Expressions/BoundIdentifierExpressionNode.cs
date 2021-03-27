using Krypton.CompilationData.Symbols;
using System.IO;

namespace Krypton.CompilationData.Syntax.Expressions
{
    public sealed class BoundIdentifierExpressionNode : ExpressionNode
    {
        public BoundIdentifierExpressionNode(IdentifierExpressionNode identifierExpression,
                                             Symbol symbol,
                                             SyntaxNode? parent = null) : base(parent)
        {
            Symbol = symbol;
            IdentifierExpressionNode = identifierExpression.WithParent(this);
        }

        public IdentifierExpressionNode IdentifierExpressionNode { get; }

        public override bool IsLeaf => false;

        public Symbol Symbol { get; }

        public override TypedExpressionNode<BoundIdentifierExpressionNode> Type(TypeSymbol type)
            => new(this, type);

        public override BoundIdentifierExpressionNode WithParent(SyntaxNode newParent)
            => new(IdentifierExpressionNode, Symbol, newParent);

        public override void WriteCode(TextWriter output) => IdentifierExpressionNode.WriteCode(output);
    }
}
