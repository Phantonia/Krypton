namespace Krypton.CompilationData.Syntax.Statements
{
    public abstract class StatementNode : SyntaxNode
    {
        private protected StatementNode(SyntaxNode? parent) : base(parent) { }

        public abstract override StatementNode WithParent(SyntaxNode newParent);
    }
}
