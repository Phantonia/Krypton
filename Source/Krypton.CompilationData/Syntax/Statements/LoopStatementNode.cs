namespace Krypton.CompilationData.Syntax.Statements
{
    public abstract class LoopStatementNode : BodiedStatementNode
    {
        private protected LoopStatementNode(BodyNode body, SyntaxNode? parent)
            : base(body, parent) { }

        public abstract override LoopStatementNode WithParent(SyntaxNode newParent);
    }
}