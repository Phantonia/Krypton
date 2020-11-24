using System.Text;

namespace Krypton.Analysis.AbstractSyntaxTree.Nodes.Expressions
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
            return new StringLiteralExpressionNode(Content, LineNumber);
        }

        public override void GenerateCode(StringBuilder stringBuilder)
        {
            stringBuilder.Append('"')
                         .Append(Content)
                         .Append('"');
        }
    }
}
