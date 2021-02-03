using Krypton.Framework;

namespace Krypton.Analysis.Ast.Expressions.Literals
{
    public sealed class StringLiteralExpressionNode : LiteralExpressionNode
    {
        internal StringLiteralExpressionNode(string value, int lineNumber) : base(FrameworkType.String, lineNumber)
        {
            Value = value;
        }

        public override object ObjectValue => Value;

        public string Value { get; }
    }
}
