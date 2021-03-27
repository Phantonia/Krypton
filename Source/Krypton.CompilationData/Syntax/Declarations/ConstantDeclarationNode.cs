using Krypton.CompilationData.Symbols;
using Krypton.CompilationData.Syntax.Clauses;
using Krypton.CompilationData.Syntax.Expressions;
using Krypton.CompilationData.Syntax.Tokens;
using System.Diagnostics;
using System.IO;

namespace Krypton.CompilationData.Syntax.Declarations
{
    public sealed class ConstantDeclarationNode : NamedDeclarationNode
    {
        public ConstantDeclarationNode(ReservedKeywordToken constKeyword,
                                       IdentifierToken name,
                                       AsClauseNode? asClause,
                                       SyntaxCharacterToken equals,
                                       ExpressionNode value,
                                       SyntaxCharacterToken semicolon,
                                       SyntaxNode? parent = null)
            : base(name, parent)
        {
            Debug.Assert(constKeyword.Keyword == ReservedKeyword.Const);
            Debug.Assert(equals.SyntaxCharacter == SyntaxCharacter.Equals);
            Debug.Assert(semicolon.SyntaxCharacter == SyntaxCharacter.Semicolon);

            ConstKeywordToken = constKeyword;
            AsClauseNode = asClause?.WithParent(this);
            EqualsToken = equals;
            ValueNode = value.WithParent(this);
            SemicolonToken = semicolon;
        }

        public AsClauseNode? AsClauseNode { get; }

        public ReservedKeywordToken ConstKeywordToken { get; }

        public SyntaxCharacterToken EqualsToken { get; }

        public override bool IsLeaf => false;

        public SyntaxCharacterToken SemicolonToken { get; }

        public ExpressionNode ValueNode { get; }

        public override BoundDeclarationNode<ConstantDeclarationNode> Bind(Symbol symbol)
            => new(this, symbol);

        public ConstantDeclarationNode WithChildren(ReservedKeywordToken? constKeyword = null,
                                                    IdentifierToken? name = null,
                                                    AsClauseNode? asClause = null,
                                                    bool overwriteAsClause = false,
                                                    SyntaxCharacterToken? equals = null,
                                                    ExpressionNode? value = null,
                                                    SyntaxCharacterToken? semicolon = null)
            => new(constKeyword ?? ConstKeywordToken,
                   name ?? NameToken,
                   asClause ?? (overwriteAsClause ? null : AsClauseNode),
                   equals ?? EqualsToken,
                   value ?? ValueNode,
                   semicolon ?? SemicolonToken);

        public override ConstantDeclarationNode WithParent(SyntaxNode newParent)
            => new(ConstKeywordToken, NameToken, AsClauseNode, EqualsToken, ValueNode, SemicolonToken, newParent);

        public override void WriteCode(TextWriter output)
        {
            ConstKeywordToken.WriteCode(output);
            NameToken.WriteCode(output);
            EqualsToken.WriteCode(output);
            ValueNode.WriteCode(output);
            SemicolonToken.WriteCode(output);
        }
    }
}
