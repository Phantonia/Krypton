using Krypton.Framework;

namespace Krypton.Analysis.Ast.Expressions.Literals
{
    public sealed class StringLiteralExpressionNode : LiteralExpressionNode
    {
        internal StringLiteralExpressionNode(string value, int lineNumber, int index) : base(FrameworkType.String, lineNumber, index)
        {
            Value = value;
        }

        public override object ObjectValue => Value;

        public string Value { get; }
    }
}
