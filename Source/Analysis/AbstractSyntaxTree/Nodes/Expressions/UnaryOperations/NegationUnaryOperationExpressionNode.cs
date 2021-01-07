﻿namespace Krypton.Analysis.AbstractSyntaxTree.Nodes.Expressions.UnaryOperations
{
    public sealed class NegationUnaryOperationExpressionNode : UnaryOperationExpressionNode
    {
        public NegationUnaryOperationExpressionNode(ExpressionNode operand, int lineNumber) : base(operand, lineNumber) { }

        public override NegationUnaryOperationExpressionNode Clone()
        {
            return new(Operand.Clone(), LineNumber);
        }
    }
}
