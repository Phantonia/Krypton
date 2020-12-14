namespace Krypton.Analysis.AbstractSyntaxTree.Nodes.Expressions.BinaryOperations
{
    public abstract class BinaryOperationExpressionNode : ExpressionNode
    {
        protected BinaryOperationExpressionNode(ExpressionNode left, ExpressionNode right, int lineNumber) : base(lineNumber)
        {
            Left = left;
            Right = right;
        }

        public ExpressionNode Left { get; }

        public ExpressionNode Right { get; }

        public abstract override BinaryOperationExpressionNode Clone();
    }
}
