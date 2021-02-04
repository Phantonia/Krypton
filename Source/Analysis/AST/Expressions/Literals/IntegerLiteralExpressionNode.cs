using Krypton.Framework;

namespace Krypton.Analysis.Ast.Expressions.Literals
{
    public sealed class IntegerLiteralExpressionNode : LiteralExpressionNode
    {
        internal IntegerLiteralExpressionNode(long value, int lineNumber, int index) : base(FrameworkType.Int, lineNumber, index)
        {
            Value = value;
        }

        public override object ObjectValue => Value;

        public long Value { get; }
    }
}
