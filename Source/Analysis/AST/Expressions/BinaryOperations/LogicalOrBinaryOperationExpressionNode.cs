﻿namespace Krypton.Analysis.AST.Expressions.BinaryOperations
{
    public sealed class LogicalOrBinaryOperationExpressionNode : BinaryOperationExpressionNode
    {
        public LogicalOrBinaryOperationExpressionNode(ExpressionNode left, ExpressionNode right, int lineNumber) : base(left, right, lineNumber) { }
    }
}