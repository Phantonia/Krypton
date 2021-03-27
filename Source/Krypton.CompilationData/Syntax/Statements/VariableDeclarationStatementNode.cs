using Krypton.CompilationData.Syntax.Expressions;
using Krypton.CompilationData.Syntax.Tokens;
using Krypton.CompilationData.Syntax.Types;
using System.Diagnostics;
using System.IO;

namespace Krypton.CompilationData.Syntax.Statements
{
    public sealed class VariableDeclarationStatementNode : SingleStatementNode
    {
        public VariableDeclarationStatementNode(ReservedKeywordToken declaratorKeyword,
                                                IdentifierToken identifier,
                                                ReservedKeywordToken? asKeyword,
                                                TypeNode? type,
                                                SyntaxCharacterToken? equals,
                                                ExpressionNode? initialValue,
                                                SyntaxCharacterToken semicolon,
                                                SyntaxNode? parent = null)
            : base(semicolon, parent)
        {
            DeclaratorKeywordToken = declaratorKeyword;
            IdentifierToken = identifier;
            AsKeywordToken = asKeyword;
            TypeNode = type?.WithParent(this);
            EqualsToken = equals;
            InitialValueNode = initialValue?.WithParent(this);

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

        public VariableDeclarationStatementNode WithChildren(ReservedKeywordToken? declaratorKeyword = null,
                                                             IdentifierToken? identifier = null,
                                                             ReservedKeywordToken? asKeyword = null,
                                                             bool overwriteAsKeyword = false,
                                                             TypeNode? type = null,
                                                             bool overwriteType = false,
                                                             SyntaxCharacterToken? equals = null,
                                                             bool overwriteEquals = false,
                                                             ExpressionNode? initialValue = null,
                                                             bool overwriteInitialValue = false,
                                                             SyntaxCharacterToken? semicolon = null)
            => new(declaratorKeyword ?? DeclaratorKeywordToken,
                   identifier ?? IdentifierToken,
                   asKeyword ?? (overwriteAsKeyword ? null : AsKeywordToken),
                   type ?? (overwriteType ? null : TypeNode),
                   equals ?? (overwriteEquals ? null : EqualsToken),
                   initialValue ?? (overwriteInitialValue ? null : InitialValueNode),
                   semicolon ?? SemicolonToken);

        public override VariableDeclarationStatementNode WithParent(SyntaxNode newParent)
            => new(DeclaratorKeywordToken,
                   IdentifierToken,
                   AsKeywordToken,
                   TypeNode,
                   EqualsToken,
                   InitialValueNode,
                   SemicolonToken,
                   newParent);

        public override void WriteCode(TextWriter output) => throw new System.NotImplementedException();
    }
}
