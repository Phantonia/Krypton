namespace Krypton.Analysis.AbstractSyntaxTree.Nodes.Expressions
{
    public abstract class ExpressionNode : Node
    {
        protected ExpressionNode(int lineNumber) : base(lineNumber) { }

        public abstract override ExpressionNode Clone();
    }
}
