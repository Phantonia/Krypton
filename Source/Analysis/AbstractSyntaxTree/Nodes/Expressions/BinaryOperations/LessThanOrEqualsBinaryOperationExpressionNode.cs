namespace Krypton.Analysis.AbstractSyntaxTree.Nodes.Expressions.BinaryOperations
{
    public sealed class LessThanOrEqualsBinaryOperationExpressionNode : BinaryOperationExpressionNode
    {
        public LessThanOrEqualsBinaryOperationExpressionNode(ExpressionNode left, ExpressionNode right, int lineNumber) : base(left, right, lineNumber) { }

        public override LessThanOrEqualsBinaryOperationExpressionNode Clone()
        {
            return new(Left.Clone(), Right.Clone(), LineNumber);
        }
    }
}
