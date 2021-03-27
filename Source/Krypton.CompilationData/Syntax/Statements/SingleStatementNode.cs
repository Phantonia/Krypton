using Krypton.CompilationData.Syntax.Tokens;
using System.Diagnostics;

namespace Krypton.CompilationData.Syntax.Statements
{
    public abstract class SingleStatementNode : StatementNode
    {
        private protected SingleStatementNode(SyntaxCharacterToken semicolon,
                                              SyntaxNode? parent)
            : base(parent)
        {
            SemicolonToken = semicolon;

            Debug.Assert(SemicolonToken.SyntaxCharacter == SyntaxCharacter.Semicolon);
        }

        public SyntaxCharacterToken SemicolonToken { get; }

        public abstract override SingleStatementNode WithParent(SyntaxNode newParent);
    }
}
