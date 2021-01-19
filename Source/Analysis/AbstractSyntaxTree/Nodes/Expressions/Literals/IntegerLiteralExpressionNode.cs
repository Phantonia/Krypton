namespace Krypton.Analysis.AbstractSyntaxTree.Nodes.Expressions.Literals
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
