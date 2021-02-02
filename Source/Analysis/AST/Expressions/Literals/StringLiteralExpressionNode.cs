using Krypton.Framework;

namespace Krypton.Analysis.Ast.Expressions.Literals
{
    public sealed class StringLiteralExpressionNode : LiteralExpressionNode
    {
        public StringLiteralExpressionNode(string content, int lineNumber) : base(FrameworkType.String, lineNumber)
        {
            Content = content;
        }

        public string Content { get; }
    }
}
