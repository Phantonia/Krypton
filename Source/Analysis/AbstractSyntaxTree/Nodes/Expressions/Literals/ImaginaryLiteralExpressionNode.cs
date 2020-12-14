using System.Text;

namespace Krypton.Analysis.AbstractSyntaxTree.Nodes.Expressions.Literals
{
    public sealed class ImaginaryLiteralExpressionNode : LiteralExpressionNode
    {
        public ImaginaryLiteralExpressionNode(double value, int lineNumber) : base(lineNumber)
        {
            Value = value;
        }

        public double Value { get; }

        public override ImaginaryLiteralExpressionNode Clone()
        {
            return new(Value, LineNumber);
        }

        public override void GenerateCode(StringBuilder stringBuilder)
        {
            stringBuilder.Append(Value);
        }
    }
}
