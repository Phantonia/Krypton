using Krypton.Analysis.Lexical;
using Krypton.Framework;

namespace Krypton.Analysis.Ast.Expressions.Literals
{
    public sealed class RationalLiteralExpressionNode : LiteralExpressionNode
    {
        public RationalLiteralExpressionNode(RationalLiteralValue value, int lineNumber) : base(FrameworkType.Rational, lineNumber)
        {
            Value = value;
        }

        public RationalLiteralValue Value { get; }
    }
}
