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

        public abstract override BodiedStatementNode WithParent(SyntaxNode newParent);
    }
}
