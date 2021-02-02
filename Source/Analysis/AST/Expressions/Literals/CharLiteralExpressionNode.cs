using Krypton.Framework;

namespace Krypton.Analysis.Ast.Expressions.Literals
{
    public sealed class CharLiteralExpressionNode : LiteralExpressionNode
    {
        public CharLiteralExpressionNode(char value, int lineNumber) : base(FrameworkType.Char, lineNumber)
        {
            Value = value;
        }

        public char Value { get; }
    }
}
