namespace Krypton.Analysis.AbstractSyntaxTree
{
    public abstract class ExpressionNode : Node
    {
        protected ExpressionNode(int lineNumber) : base(lineNumber) { }
    }
}
