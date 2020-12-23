namespace Krypton.Analysis.AbstractSyntaxTree.Nodes.Expressions.BinaryOperations
{
    public sealed class GreaterThanBinaryOperationExpressionNode : BinaryOperationExpressionNode
    {
        public GreaterThanBinaryOperationExpressionNode(ExpressionNode left, ExpressionNode right, int lineNumber) : base(left, right, lineNumber) { }

        public override GreaterThanBinaryOperationExpressionNode Clone()
        {
            return new(Left.Clone(), Right.Clone(), LineNumber);
        }
    }
}
