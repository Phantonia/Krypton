namespace Krypton.Analysis.AST.Expressions.UnaryOperations
{
    public sealed class LogicalNotUnaryOperationExpressionNode : UnaryOperationExpressionNode
    {
        public LogicalNotUnaryOperationExpressionNode(ExpressionNode operand, int lineNumber) : base(operand, lineNumber) { }
    }
}
