namespace Krypton.Analysis.AbstractSyntaxTree.Nodes.Expressions.BinaryOperations
{
    public sealed class LogicalAndBinaryOperationExpressionNode : BinaryOperationExpressionNode
    {
        public LogicalAndBinaryOperationExpressionNode(ExpressionNode left, ExpressionNode right, int lineNumber) : base(left, right, lineNumber) { }

        public override LogicalAndBinaryOperationExpressionNode Clone()
        {
            return new(Left.Clone(), Right.Clone(), LineNumber);
        }
    }
}
