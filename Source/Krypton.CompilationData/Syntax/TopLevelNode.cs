namespace Krypton.CompilationData.Syntax
{
    public abstract class TopLevelNode : SyntaxNode
    {
        private protected TopLevelNode(SyntaxNode? parent)
            : base(parent) { }

        public abstract override TopLevelNode WithParent(SyntaxNode newParent);
    }
}
