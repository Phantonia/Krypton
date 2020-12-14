using System.Text;

namespace Krypton.Analysis.AbstractSyntaxTree.Nodes.Expressions
{
    public sealed class BooleanLiteralExpressionNode : LiteralExpressionNode
    {
        public BooleanLiteralExpressionNode(bool value, int lineNumber) : base(lineNumber)
        {
            Value = value;
        }

        public bool Value { get; }

        public override BooleanLiteralExpressionNode Clone()
        {
            return new(Value, LineNumber);
        }

        public override void GenerateCode(StringBuilder stringBuilder)
        {
            // Using only Value would append "True" or "False", which are not valid Javascript
            stringBuilder.Append(Value == true ? "true" : "false");
        }
    }
}
