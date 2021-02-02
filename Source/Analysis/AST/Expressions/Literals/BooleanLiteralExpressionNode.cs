using Krypton.Framework;

namespace Krypton.Analysis.Ast.Expressions.Literals
{
    public sealed class BooleanLiteralExpressionNode : LiteralExpressionNode
    {
        public BooleanLiteralExpressionNode(bool value, int lineNumber) : base(FrameworkType.Bool, lineNumber)
        {
            Value = value;
        }

        public bool Value { get; }
    }
}
