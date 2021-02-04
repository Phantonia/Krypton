using Krypton.Framework;
using Krypton.Framework.Literals;

namespace Krypton.Analysis.Ast.Expressions.Literals
{
    public sealed class RationalLiteralExpressionNode : LiteralExpressionNode
    {
        internal RationalLiteralExpressionNode(Rational value, int lineNumber, int index) : base(FrameworkType.Rational, lineNumber, index)
        {
            Value = value;
        }

        public override object ObjectValue => Value;

        public Rational Value { get; }
    }
}
