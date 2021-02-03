using Krypton.Framework;
using Krypton.Framework.Literals;

namespace Krypton.Analysis.Ast.Expressions.Literals
{
    public sealed class RationalLiteralExpressionNode : LiteralExpressionNode
    {
        internal RationalLiteralExpressionNode(Rational value, int lineNumber) : base(FrameworkType.Rational, lineNumber)
        {
            Value = value;
        }

        public override object ObjectValue => Value;

        public Rational Value { get; }
    }
}
