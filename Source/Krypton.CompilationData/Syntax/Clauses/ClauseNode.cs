namespace Krypton.CompilationData.Syntax.Clauses
{
    public abstract class ClauseNode : SyntaxNode
    {
        private protected ClauseNode(SyntaxNode? parent) : base(parent) { }

        public abstract override ClauseNode WithParent(SyntaxNode newParent);
    }
}
