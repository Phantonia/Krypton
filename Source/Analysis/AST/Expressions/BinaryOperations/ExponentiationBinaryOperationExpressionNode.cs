﻿namespace Krypton.Analysis.AST.Expressions.BinaryOperations
{
    public sealed class ExponentiationBinaryOperationExpressionNode : BinaryOperationExpressionNode
    {
        public ExponentiationBinaryOperationExpressionNode(ExpressionNode left, ExpressionNode right, int lineNumber) : base(left, right, lineNumber) { }
    }
}