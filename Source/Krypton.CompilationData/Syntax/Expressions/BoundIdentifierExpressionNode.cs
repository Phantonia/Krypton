using Krypton.CompilationData.Symbols;
using System.IO;

namespace Krypton.CompilationData.Syntax.Expressions
{
    public sealed record BoundIdentifierExpressionNode : ExpressionNode
    {
        public BoundIdentifierExpressionNode(IdentifierExpressionNode identifierExpression,
                                             Symbol symbol)
        {
            Symbol = symbol;
            IdentifierExpressionNode = identifierExpression;
        }

        public IdentifierExpressionNode IdentifierExpressionNode { get; init; }

        public override bool IsLeaf => false;

        public Symbol Symbol { get; init; }

        public override TypedExpressionNode Type(TypeSymbol type)
            => new(this, type);

        public override void WriteCode(TextWriter output) => IdentifierExpressionNode.WriteCode(output);
    }
}
