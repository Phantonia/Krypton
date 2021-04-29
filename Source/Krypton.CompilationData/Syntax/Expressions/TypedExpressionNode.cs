using Krypton.CompilationData.Symbols;
using System.IO;

namespace Krypton.CompilationData.Syntax.Expressions
{
    public sealed record TypedExpressionNode : ExpressionNode
    {
        internal TypedExpressionNode(ExpressionNode expression, TypeSymbol typeSymbol)
        {
            ExpressionNode = expression;
            TypeSymbol = typeSymbol;
        }

        public ExpressionNode ExpressionNode { get; init; }

        public override bool IsLeaf => false;

        public TypeSymbol TypeSymbol { get; init; }

        public override TypedExpressionNode Type(TypeSymbol type)
            => type == TypeSymbol ? this : new TypedExpressionNode(ExpressionNode, type);

        public override void WriteCode(TextWriter output)
            => ExpressionNode.WriteCode(output);
    }
}
