using System.Text;

namespace Krypton.Analysis.AbstractSyntaxTree.Nodes.Expressions
{
    public sealed class RealLiteralExpressionNode : LiteralExpressionNode
    {
        public RealLiteralExpressionNode(double value, int lineNumber) : base(lineNumber)
        {
            Value = value;
        }

        public double Value { get; }

        public override RealLiteralExpressionNode Clone()
        {
            return new(Value, LineNumber);
        }

        public override void GenerateCode(StringBuilder stringBuilder)
        {
            stringBuilder.Append(Value);
        }
    }
}
