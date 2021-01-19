using Krypton.Analysis.Lexical;

namespace Krypton.Analysis.AbstractSyntaxTree.Nodes.Expressions.Literals
{
    public sealed class ImaginaryLiteralExpressionNode : LiteralExpressionNode
    {
        public ImaginaryLiteralExpressionNode(RationalLiteralValue value, int lineNumber) : base(lineNumber)
        {
            Value = value;
        }

        public RationalLiteralValue Value { get; }
    }
}
