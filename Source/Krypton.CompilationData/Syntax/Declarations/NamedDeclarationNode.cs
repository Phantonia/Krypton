using Krypton.CompilationData.Syntax.Tokens;

namespace Krypton.CompilationData.Syntax.Declarations
{
    public abstract class NamedDeclarationNode : DeclarationNode
    {
        private protected NamedDeclarationNode(IdentifierToken name,
                                               SyntaxNode? parent)
            : base(parent)
        {
            NameToken = name;
        }

        public string Name => NameToken.Text;

        public IdentifierToken NameToken { get; }

        protected override string GetDebuggerDisplay() => $"{base.GetDebuggerDisplay()}; Name = {Name}";

        public abstract override NamedDeclarationNode WithParent(SyntaxNode newParent);
    }
}
