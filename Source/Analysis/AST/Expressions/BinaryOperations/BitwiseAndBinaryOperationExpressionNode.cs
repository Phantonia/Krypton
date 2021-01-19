﻿namespace Krypton.Analysis.AST.Expressions.BinaryOperations
{
    public sealed class BitwiseAndBinaryOperationExpressionNode : BinaryOperationExpressionNode
    {
        public BitwiseAndBinaryOperationExpressionNode(ExpressionNode left, ExpressionNode right, int lineNumber) : base(left, right, lineNumber) { }
    }
}