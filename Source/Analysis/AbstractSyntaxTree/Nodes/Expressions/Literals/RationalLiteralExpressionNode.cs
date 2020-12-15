using Krypton.Analysis.Lexical;
using System.Globalization;
using System.Text;

namespace Krypton.Analysis.AbstractSyntaxTree.Nodes.Expressions.Literals
{
    public sealed class RationalLiteralExpressionNode : LiteralExpressionNode
    {
        public RationalLiteralExpressionNode(RationalLiteralValue value, int lineNumber) : base(lineNumber)
        {
            Value = value;
        }

        public RationalLiteralValue Value { get; }

        public override RationalLiteralExpressionNode Clone()
        {
            return new(Value, LineNumber);
        }

        public override void GenerateCode(StringBuilder stringBuilder)
        {
            stringBuilder.Append(Value.ToString());
        }
    }
}
