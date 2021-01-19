namespace Krypton.Analysis.Ast.Expressions.UnaryOperations
{
    public sealed class NegationUnaryOperationExpressionNode : UnaryOperationExpressionNode
    {
        public NegationUnaryOperationExpressionNode(ExpressionNode operand, int lineNumber) : base(operand, lineNumber) { }
    }
}
