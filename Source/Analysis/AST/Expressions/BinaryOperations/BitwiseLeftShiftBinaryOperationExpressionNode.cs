﻿namespace Krypton.Analysis.AST.Expressions.BinaryOperations
{
    public sealed class BitwiseLeftShiftBinaryOperationExpressionNode : BinaryOperationExpressionNode
    {
        public BitwiseLeftShiftBinaryOperationExpressionNode(ExpressionNode left, ExpressionNode right, int lineNumber) : base(left, right, lineNumber) { }
    }
}