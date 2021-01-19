using Krypton.Analysis.Lexical;

namespace Krypton.Analysis.Ast.Expressions.Literals
{
    public sealed class RationalLiteralExpressionNode : LiteralExpressionNode
    {
        public RationalLiteralExpressionNode(RationalLiteralValue value, int lineNumber) : base(lineNumber)
        {
            Value = value;
        }

        public RationalLiteralValue Value { get; }
    }
}
