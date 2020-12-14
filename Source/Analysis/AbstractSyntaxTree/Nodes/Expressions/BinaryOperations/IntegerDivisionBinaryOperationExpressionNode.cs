using System.Text;

namespace Krypton.Analysis.AbstractSyntaxTree.Nodes.Expressions.BinaryOperations
{
    public sealed class IntegerDivisionBinaryOperationExpressionNode : BinaryOperationExpressionNode
    {
        public IntegerDivisionBinaryOperationExpressionNode(ExpressionNode left, ExpressionNode right, int lineNumber) : base(left, right, lineNumber) { }

        public override IntegerDivisionBinaryOperationExpressionNode Clone()
        {
            return new(Left, Right, LineNumber);
        }

        public override void GenerateCode(StringBuilder stringBuilder)
        {
            stringBuilder.Append("Math.floor((");
            Left.GenerateCode(stringBuilder);
            stringBuilder.Append(") / (");
            Right.GenerateCode(stringBuilder);
            stringBuilder.Append("))");
        }
    }
}
