using Krypton.CompilationData.Syntax.Tokens;
using System.Diagnostics;

namespace Krypton.CompilationData.Syntax.Statements
{
    public abstract record SingleStatementNode : StatementNode
    {
        private protected SingleStatementNode(SyntaxCharacterToken semicolon)
        {
            SemicolonToken = semicolon;

            Debug.Assert(SemicolonToken.SyntaxCharacter == SyntaxCharacter.Semicolon);
        }

        public SyntaxCharacterToken SemicolonToken { get; init; }
    }
}
