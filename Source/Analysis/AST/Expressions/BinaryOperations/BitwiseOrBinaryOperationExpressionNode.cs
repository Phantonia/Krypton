namespace Krypton.Analysis.AST.Expressions.BinaryOperations
{
    public sealed class BitwiseOrBinaryOperationExpressionNode : BinaryOperationExpressionNode
    {
        public BitwiseOrBinaryOperationExpressionNode(ExpressionNode left, ExpressionNode right, int lineNumber) : base(left, right, lineNumber) { }
    }
}
