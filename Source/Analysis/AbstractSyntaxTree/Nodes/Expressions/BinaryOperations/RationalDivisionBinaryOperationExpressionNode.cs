﻿namespace Krypton.Analysis.AbstractSyntaxTree.Nodes.Expressions.BinaryOperations
{
    public sealed class RationalDivisionBinaryOperationExpressionNode : BinaryOperationExpressionNode
    {
        public RationalDivisionBinaryOperationExpressionNode(ExpressionNode left, ExpressionNode right, int lineNumber) : base(left, right, lineNumber) { }

        public override RationalDivisionBinaryOperationExpressionNode Clone()
        {
            return new(Left.Clone(), Right.Clone(), LineNumber);
        }
    }
}
