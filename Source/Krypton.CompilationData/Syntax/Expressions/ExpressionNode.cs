namespace Krypton.CompilationData.Syntax.Expressions
{
    public abstract class ExpressionNode : SyntaxNode
    {
        private protected ExpressionNode(SyntaxNode? parent) : base(parent) { }

        public abstract override ExpressionNode WithParent(SyntaxNode newParent);
    }
}
