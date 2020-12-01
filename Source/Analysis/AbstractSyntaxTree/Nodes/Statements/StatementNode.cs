namespace Krypton.Analysis.AbstractSyntaxTree.Nodes.Statements
{
    public abstract class StatementNode : Node
    {
        protected StatementNode(int lineNumber) : base(lineNumber) { }

        public abstract override StatementNode Clone();
    }
}
