namespace Krypton.Analysis.Ast.Expressions.BinaryOperations
{
    public sealed class EqualityBinaryOperationExpressionNode : BinaryOperationExpressionNode
    {
        public EqualityBinaryOperationExpressionNode(ExpressionNode left, ExpressionNode right, int lineNumber) : base(left, right, lineNumber) { }
    }
}
