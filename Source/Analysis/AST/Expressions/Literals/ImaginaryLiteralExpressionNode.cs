using Krypton.Analysis.Lexical;
using Krypton.Framework;

namespace Krypton.Analysis.Ast.Expressions.Literals
{
    public sealed class ImaginaryLiteralExpressionNode : LiteralExpressionNode
    {
        public ImaginaryLiteralExpressionNode(RationalLiteralValue value, int lineNumber) : base(FrameworkType.Complex, lineNumber)
        {
            Value = value;
        }

        public RationalLiteralValue Value { get; }
    }
}
