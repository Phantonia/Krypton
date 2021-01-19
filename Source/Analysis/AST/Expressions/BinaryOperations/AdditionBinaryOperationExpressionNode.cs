﻿namespace Krypton.Analysis.AST.Expressions.BinaryOperations
{
    public sealed class AdditionBinaryOperationExpressionNode : BinaryOperationExpressionNode
    {
        public AdditionBinaryOperationExpressionNode(ExpressionNode left, ExpressionNode right, int lineNumber) : base(left, right, lineNumber) { }
    }
}