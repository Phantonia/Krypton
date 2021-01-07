namespace Krypton.Analysis.AbstractSyntaxTree.Nodes.Expressions.UnaryOperations
{
    public sealed class BitwiseNotUnaryOperationExpressionNode : UnaryOperationExpressionNode
    {
        public BitwiseNotUnaryOperationExpressionNode(ExpressionNode operand, int lineNumber) : base(operand, lineNumber) { }

        public override BitwiseNotUnaryOperationExpressionNode Clone()
        {
            return new(Operand.Clone(), LineNumber);
        }
    }
}
