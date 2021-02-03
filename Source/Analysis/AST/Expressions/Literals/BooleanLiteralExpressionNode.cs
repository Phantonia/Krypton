using Krypton.Framework;

namespace Krypton.Analysis.Ast.Expressions.Literals
{
    public sealed class BooleanLiteralExpressionNode : LiteralExpressionNode
    {
        internal BooleanLiteralExpressionNode(bool value, int lineNumber) : base(FrameworkType.Bool, lineNumber)
        {
            Value = value;
        }

        public override object ObjectValue => Value;

        public bool Value { get; }
    }
}
