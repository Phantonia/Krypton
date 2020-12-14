using System.Globalization;
using System.Text;

namespace Krypton.Analysis.AbstractSyntaxTree.Nodes.Expressions.Literals
{
    public sealed class RationalLiteralExpressionNode : LiteralExpressionNode
    {
        public RationalLiteralExpressionNode(double value, int lineNumber) : base(lineNumber)
        {
            Value = value;
        }

        public double Value { get; }

        public override RationalLiteralExpressionNode Clone()
        {
            return new(Value, LineNumber);
        }

        public override void GenerateCode(StringBuilder stringBuilder)
        {
            stringBuilder.Append(Value.ToString(CultureInfo.InvariantCulture));
        }
    }
}
