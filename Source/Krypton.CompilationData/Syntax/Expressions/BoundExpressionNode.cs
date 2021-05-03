using Krypton.CompilationData.Symbols;
using System.IO;

namespace Krypton.CompilationData.Syntax.Expressions
{
    public sealed record BoundExpressionNode<TExpressionNode, TSymbol> : ExpressionNode
        where TExpressionNode : ExpressionNode
        where TSymbol : Symbol
    {
        public BoundExpressionNode(TExpressionNode expression, TSymbol symbol)
        {
            ExpressionNode = expression;
            Symbol = symbol;
        }

        public TExpressionNode ExpressionNode { get; init; }

        public override bool IsLeaf => false;

        public TSymbol Symbol { get; init; }

        public override void WriteCode(TextWriter output)
            => ExpressionNode.WriteCode(output);
    }
}
