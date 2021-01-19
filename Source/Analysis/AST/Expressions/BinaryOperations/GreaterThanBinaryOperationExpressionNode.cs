﻿namespace Krypton.Analysis.AST.Expressions.BinaryOperations
{
    public sealed class GreaterThanBinaryOperationExpressionNode : BinaryOperationExpressionNode
    {
        public GreaterThanBinaryOperationExpressionNode(ExpressionNode left, ExpressionNode right, int lineNumber) : base(left, right, lineNumber) { }
    }
}