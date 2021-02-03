using Krypton.Framework;
using Krypton.Framework.Literals;

namespace Krypton.Analysis.Ast.Expressions.Literals
{
    public sealed class ImaginaryLiteralExpressionNode : LiteralExpressionNode
    {
        internal ImaginaryLiteralExpressionNode(Rational value, int lineNumber) : base(FrameworkType.Complex, lineNumber)
        {
            Value = value;
        }

        public override object ObjectValue => Value;

        public Rational Value { get; }
    }
}
