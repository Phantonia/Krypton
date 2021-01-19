namespace Krypton.Analysis.AST.Expressions.BinaryOperations
{
    public sealed class LogicalAndBinaryOperationExpressionNode : BinaryOperationExpressionNode
    {
        public LogicalAndBinaryOperationExpressionNode(ExpressionNode left, ExpressionNode right, int lineNumber) : base(left, right, lineNumber) { }
    }
}
