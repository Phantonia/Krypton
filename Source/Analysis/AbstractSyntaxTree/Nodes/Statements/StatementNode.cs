namespace Krypton.Analysis.AbstractSyntaxTree.Nodes.Statements
{
    public abstract class StatementNode : Node
    {
        protected StatementNode(int lineNumber) : base(lineNumber) { }

        public StatementNode? Next { get; internal set; }

        public StatementNode? Previous { get; internal set; }

        public abstract override StatementNode Clone();
    }
}
