using Krypton.Framework;
using Krypton.Framework.Literals;

namespace Krypton.Analysis.Ast.Expressions.Literals
{
    public sealed class ImaginaryLiteralExpressionNode : LiteralExpressionNode
    {
        internal ImaginaryLiteralExpressionNode(Rational value, int lineNumber, int index) : base(FrameworkType.Complex, lineNumber, index)
        {
            Value = value;
        }

        public override object ObjectValue => Value;

        public Rational Value { get; }
    }
}
