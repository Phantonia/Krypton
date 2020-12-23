namespace Krypton.Analysis.AbstractSyntaxTree.Nodes.Expressions.BinaryOperations
{
    public sealed class ModuloBinaryOperationExpressionNode : BinaryOperationExpressionNode
    {
        public ModuloBinaryOperationExpressionNode(ExpressionNode left, ExpressionNode right, int lineNumber) : base(left, right, lineNumber) { }

        public override ModuloBinaryOperationExpressionNode Clone()
        {
            return new(Left.Clone(), Right.Clone(), LineNumber);
        }
    }
}
