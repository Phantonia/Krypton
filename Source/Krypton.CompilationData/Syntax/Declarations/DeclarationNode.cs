namespace Krypton.CompilationData.Syntax.Declarations
{
    public abstract class DeclarationNode : SyntaxNode
    {
        private protected DeclarationNode(SyntaxNode? parent) : base(parent) { }

        public abstract override DeclarationNode WithParent(SyntaxNode newParent);
    }
}
