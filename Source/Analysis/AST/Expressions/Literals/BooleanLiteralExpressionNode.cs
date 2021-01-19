namespace Krypton.Analysis.AST.Expressions.Literals
{
    public sealed class BooleanLiteralExpressionNode : LiteralExpressionNode
    {
        public BooleanLiteralExpressionNode(bool value, int lineNumber) : base(lineNumber)
        {
            Value = value;
        }

        public bool Value { get; }
    }
}
