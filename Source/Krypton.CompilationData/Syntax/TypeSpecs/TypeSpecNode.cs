namespace Krypton.CompilationData.Syntax.TypeSpecs
{
    public abstract class TypeSpecNode : SyntaxNode
    {
        private protected TypeSpecNode(SyntaxNode? parent) : base(parent) { }

        public abstract override TypeSpecNode WithParent(SyntaxNode newParent);
    }
}
