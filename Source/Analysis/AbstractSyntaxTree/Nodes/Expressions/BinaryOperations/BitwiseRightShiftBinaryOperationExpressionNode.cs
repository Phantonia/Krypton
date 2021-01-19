namespace Krypton.Analysis.AbstractSyntaxTree.Nodes.Expressions.BinaryOperations
{
    public sealed class BitwiseRightShiftBinaryOperationExpressionNode : BinaryOperationExpressionNode
    {
        public BitwiseRightShiftBinaryOperationExpressionNode(ExpressionNode left, ExpressionNode right, int lineNumber) : base(left, right, lineNumber) { }
    }
}
