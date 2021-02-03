using Krypton.Framework;

namespace Krypton.Analysis.Ast.Expressions.Literals
{
    public sealed class IntegerLiteralExpressionNode : LiteralExpressionNode
    {
        internal IntegerLiteralExpressionNode(long value, int lineNumber) : base(FrameworkType.Int, lineNumber)
        {
            Value = value;
        }

        public override object ObjectValue => Value;

        public long Value { get; }
    }
}
