using Krypton.CompilationData.Syntax.Clauses;
using Krypton.CompilationData.Syntax.Expressions;
using Krypton.CompilationData.Syntax.Tokens;
using System.Diagnostics;
using System.IO;

namespace Krypton.CompilationData.Syntax.Statements
{
    public sealed record VariableDeclarationStatementNode : SingleStatementNode
    {
        public VariableDeclarationStatementNode(ReservedKeywordToken declaratorKeyword,
                                                IdentifierToken identifier,
                                                AsClauseNode? asClause,
                                                SyntaxCharacterToken? equals,
                                                ExpressionNode? initialValue,
                                                SyntaxCharacterToken semicolon)
            : base(semicolon)
        {
            DeclaratorKeywordToken = declaratorKeyword;
            IdentifierToken = identifier;
            AsClauseNode = asClause;
            EqualsToken = equals;
            InitialValueNode = initialValue;

            Debug.Assert(DeclaratorKeywordToken.Keyword is ReservedKeyword.Var or ReservedKeyword.Let);
            Debug.Assert(InitialValueNode != null ? EqualsToken != null : EqualsToken == null);
            Debug.Assert((EqualsToken?.SyntaxCharacter ?? SyntaxCharacter.Semicolon) == SyntaxCharacter.Semicolon);
        }

        public AsClauseNode? AsClauseNode { get; }

        public ReservedKeywordToken DeclaratorKeywordToken { get; } // Let or Var

        public SyntaxCharacterToken? EqualsToken { get; }

        public IdentifierToken IdentifierToken { get; }

        public ExpressionNode? InitialValueNode { get; }

        public override bool IsLeaf => false;

        public override Token LexicallyFirstToken => DeclaratorKeywordToken;

        public override void WriteCode(TextWriter output)
        {
            DeclaratorKeywordToken.WriteCode(output);
            IdentifierToken.WriteCode(output);
            AsClauseNode?.WriteCode(output);
            EqualsToken?.WriteCode(output);
            InitialValueNode?.WriteCode(output);
        }
    }
}
