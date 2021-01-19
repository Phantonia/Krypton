namespace Krypton.Analysis.Ast.Expressions.Literals
{
    public sealed class StringLiteralExpressionNode : LiteralExpressionNode
    {
        public StringLiteralExpressionNode(string content, int lineNumber) : base(lineNumber)
        {
            Content = content;
        }

        public string Content { get; }
    }
}
