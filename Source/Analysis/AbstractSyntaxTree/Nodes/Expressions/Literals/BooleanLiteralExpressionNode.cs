namespace Krypton.Analysis.AbstractSyntaxTree.Nodes.Expressions.Literals
{
    public sealed class BooleanLiteralExpressionNode : LiteralExpressionNode
    {
        public BooleanLiteralExpressionNode(bool value, int lineNumber) : base(lineNumber)
        {
            Value = value;
        }

        public bool Value { get; }

        public override BooleanLiteralExpressionNode Clone()
        {
            return new(Value, LineNumber);
        }
    }
}
