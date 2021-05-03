namespace Krypton.CompilationData.Syntax.Statements
{
    public abstract record BodiedStatementNode : StatementNode
    {
        private protected BodiedStatementNode(BodyNode body)
        {
            BodyNode = body;
        }

        public BodyNode BodyNode { get; init; }

        protected override string GetDebuggerDisplay() => $"{base.GetDebuggerDisplay()}; Number of statements: {BodyNode.StatementNodes.Count}";
    }
}
