using Krypton.Framework;

namespace Krypton.Analysis.Ast.Expressions.Literals
{
    public sealed class BooleanLiteralExpressionNode : LiteralExpressionNode
    {
        internal BooleanLiteralExpressionNode(bool value, int lineNumber, int index) : base(FrameworkType.Bool, lineNumber, index)
        {
            Value = value;
        }

        public override object ObjectValue => Value;

        public bool Value { get; }
    }
}
