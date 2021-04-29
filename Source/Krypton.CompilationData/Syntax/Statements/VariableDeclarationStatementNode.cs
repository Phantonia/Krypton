using Krypton.CompilationData.Syntax.Expressions;
using Krypton.CompilationData.Syntax.Tokens;
using Krypton.CompilationData.Syntax.Types;
using System.Diagnostics;
using System.IO;

namespace Krypton.CompilationData.Syntax.Statements
{
    public sealed record VariableDeclarationStatementNode : SingleStatementNode
    {
        public VariableDeclarationStatementNode(ReservedKeywordToken declaratorKeyword,
                                                IdentifierToken identifier,
                                                ReservedKeywordToken? asKeyword,
                                                TypeNode? type,
                                                SyntaxCharacterToken? equals,
                                                ExpressionNode? initialValue,
                                                SyntaxCharacterToken semicolon)
            : base(semicolon)
        {
            DeclaratorKeywordToken = declaratorKeyword;
            IdentifierToken = identifier;
            AsKeywordToken = asKeyword;
            TypeNode = type;
            EqualsToken = equals;
            InitialValueNode = initialValue;

            Debug.Assert(DeclaratorKeywordToken.Keyword is ReservedKeyword.Var or ReservedKeyword.Let);
            Debug.Assert(TypeNode != null ? AsKeywordToken != null : AsKeywordToken == null);
            Debug.Assert(InitialValueNode != null ? EqualsToken != null : EqualsToken == null);
            Debug.Assert(TypeNode != null || InitialValueNode != null);
            Debug.Assert((AsKeywordToken?.Keyword ?? ReservedKeyword.As) == ReservedKeyword.As);
            Debug.Assert((EqualsToken?.SyntaxCharacter ?? SyntaxCharacter.Semicolon) == SyntaxCharacter.Semicolon);
        }

        public ReservedKeywordToken? AsKeywordToken { get; }

        public ReservedKeywordToken DeclaratorKeywordToken { get; } // Let or Var

        public SyntaxCharacterToken? EqualsToken { get; }

        public IdentifierToken IdentifierToken { get; }

        public ExpressionNode? InitialValueNode { get; }

        public override bool IsLeaf => false;

        public TypeNode? TypeNode { get; }

        public override void WriteCode(TextWriter output)
        {
            DeclaratorKeywordToken.WriteCode(output);
            IdentifierToken.WriteCode(output);
            AsKeywordToken?.WriteCode(output);
            TypeNode?.WriteCode(output);
            EqualsToken?.WriteCode(output);
            InitialValueNode?.WriteCode(output);
        }
    }
}
