namespace Krypton.Analysis.Ast.Statements
{
    public abstract class StatementNode : Node
    {
        protected private StatementNode(int lineNumber) : base(lineNumber) { }

        public StatementNode? Next { get; internal set; }

        public StatementNode? Previous { get; internal set; }
    }
}
