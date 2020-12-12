using System.Text;

namespace Krypton.Analysis.AbstractSyntaxTree.Nodes.Expressions.BinaryOperations
{
    public sealed class GreaterThanOrEqualsBinaryOperationExpressionNode : BinaryOperationExpressionNode
    {
        public GreaterThanOrEqualsBinaryOperationExpressionNode(ExpressionNode left, ExpressionNode right, int lineNumber) : base(left, right, lineNumber) { }

        public override GreaterThanOrEqualsBinaryOperationExpressionNode Clone()
        {
            return new(Left, Right, LineNumber);
        }

        public override void GenerateCode(StringBuilder stringBuilder)
        {
            stringBuilder.Append('(');
            Left.GenerateCode(stringBuilder);
            stringBuilder.Append(") >= (");
            Right.GenerateCode(stringBuilder);
            stringBuilder.Append(')');
        }
    }
}
