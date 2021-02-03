namespace Krypton.Analysis.Ast.Statements
{
    public abstract class StatementNode : Node
    {
        private protected StatementNode(int lineNumber) : base(lineNumber) { }

        public StatementNode? NextStatementNode { get; internal set; }

        public StatementNode? PreviousStatementNode { get; internal set; }
    }
}
