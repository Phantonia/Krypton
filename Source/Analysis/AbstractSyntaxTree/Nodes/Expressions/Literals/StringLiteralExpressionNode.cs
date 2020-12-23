namespace Krypton.Analysis.AbstractSyntaxTree.Nodes.Expressions.Literals
{
    public sealed class StringLiteralExpressionNode : LiteralExpressionNode
    {
        public StringLiteralExpressionNode(string content, int lineNumber) : base(lineNumber)
        {
            Content = content;
        }

        public string Content { get; }

        public override StringLiteralExpressionNode Clone()
        {
            return new(Content, LineNumber);
        }
    }
}
