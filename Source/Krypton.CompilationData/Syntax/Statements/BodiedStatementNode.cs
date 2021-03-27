namespace Krypton.CompilationData.Syntax.Statements
{
    public abstract class BodiedStatementNode : StatementNode
    {
        private protected BodiedStatementNode(BodyNode body, SyntaxNode? parent)
            : base(parent)
        {
            BodyNode = body.WithParent(this);
        }

        public BodyNode BodyNode { get; }

        protected override string GetDebuggerDisplay() => $"{base.GetDebuggerDisplay()}; Number of statements: {BodyNode.StatementNodes.Count}";

        public abstract override BodiedStatementNode WithParent(SyntaxNode newParent);
    }
}
