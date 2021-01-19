﻿namespace Krypton.Analysis.AST.Expressions.BinaryOperations
{
    public sealed class UnequalityBinaryOperationExpressionNode : BinaryOperationExpressionNode
    {
        public UnequalityBinaryOperationExpressionNode(ExpressionNode left, ExpressionNode right, int lineNumber) : base(left, right, lineNumber) { }
    }
}