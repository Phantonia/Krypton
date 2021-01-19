namespace Krypton.Analysis.AST.Expressions.UnaryOperations
{
    public sealed class BitwiseNotUnaryOperationExpressionNode : UnaryOperationExpressionNode
    {
        public BitwiseNotUnaryOperationExpressionNode(ExpressionNode operand, int lineNumber) : base(operand, lineNumber) { }
    }
}
