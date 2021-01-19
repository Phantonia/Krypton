namespace Krypton.Analysis.AbstractSyntaxTree.Nodes.Expressions.BinaryOperations
{
    public sealed class GreaterThanOrEqualsBinaryOperationExpressionNode : BinaryOperationExpressionNode
    {
        public GreaterThanOrEqualsBinaryOperationExpressionNode(ExpressionNode left, ExpressionNode right, int lineNumber) : base(left, right, lineNumber) { }
    }
}
