using System;
using System.Text;

namespace Krypton.Analysis.AbstractSyntaxTree.Nodes.Expressions.BinaryOperations
{
    public sealed class RationalDivisionBinaryOperationExpressionNode : BinaryOperationExpressionNode
    {
        public RationalDivisionBinaryOperationExpressionNode(ExpressionNode left, ExpressionNode right, int lineNumber) : base(left, right, lineNumber) { }

        public override RationalDivisionBinaryOperationExpressionNode Clone()
        {
            return new(Left, Right, LineNumber);
        }

        public override void GenerateCode(StringBuilder stringBuilder)
        {
            throw new NotImplementedException();
        }
    }
}
