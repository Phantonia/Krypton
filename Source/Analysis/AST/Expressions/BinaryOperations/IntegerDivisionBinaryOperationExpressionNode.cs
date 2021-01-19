﻿namespace Krypton.Analysis.AST.Expressions.BinaryOperations
{
    public sealed class IntegerDivisionBinaryOperationExpressionNode : BinaryOperationExpressionNode
    {
        public IntegerDivisionBinaryOperationExpressionNode(ExpressionNode left, ExpressionNode right, int lineNumber) : base(left, right, lineNumber) { }
    }
}