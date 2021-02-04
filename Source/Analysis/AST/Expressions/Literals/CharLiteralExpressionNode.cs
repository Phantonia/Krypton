using Krypton.Framework;

namespace Krypton.Analysis.Ast.Expressions.Literals
{
    public sealed class CharLiteralExpressionNode : LiteralExpressionNode
    {
        internal CharLiteralExpressionNode(char value, int lineNumber, int index) : base(FrameworkType.Char, lineNumber, index)
        {
            Value = value;
        }

        public override object ObjectValue => Value;

        public char Value { get; }
    }
}
