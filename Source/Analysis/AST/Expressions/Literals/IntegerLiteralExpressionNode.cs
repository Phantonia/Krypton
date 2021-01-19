namespace Krypton.Analysis.AST.Expressions.Literals
{
    public sealed class IntegerLiteralExpressionNode : LiteralExpressionNode
    {
        public IntegerLiteralExpressionNode(long value, int lineNumber) : base(lineNumber)
        {
            Value = value;
        }

        public long Value { get; }
    }
}
