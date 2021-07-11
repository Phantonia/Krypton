using Krypton.CompilationData.Symbols;
using Krypton.CompilationData.Syntax.Clauses;
using Krypton.CompilationData.Syntax.Expressions;
using Krypton.CompilationData.Syntax.Tokens;
using Krypton.CompilationData.Syntax.Types;
using System.Diagnostics;
using System.IO;

namespace Krypton.CompilationData.Syntax.Declarations
{
    public sealed record ConstantDeclarationNode : NamedDeclarationNode
    {
        public ConstantDeclarationNode(ReservedKeywordToken constKeyword,
                                       IdentifierToken name,
                                       AsClauseNode? asClause,
                                       SyntaxCharacterToken equals,
                                       ExpressionNode value,
                                       SyntaxCharacterToken semicolon)
            : base(name)
        {
            Debug.Assert(constKeyword.Keyword == ReservedKeyword.Const);
            Debug.Assert(equals.SyntaxCharacter == SyntaxCharacter.Equals);
            Debug.Assert(semicolon.SyntaxCharacter == SyntaxCharacter.Semicolon);

            ConstKeywordToken = constKeyword;
            AsClauseNode = asClause;
            EqualsToken = equals;
            ValueNode = value;
            SemicolonToken = semicolon;
        }

        public AsClauseNode? AsClauseNode { get; init; }

        public ReservedKeywordToken ConstKeywordToken { get; init; }

        public SyntaxCharacterToken EqualsToken { get; init; }

        public override bool IsLeaf => false;

        public override Token LexicallyFirstToken => ConstKeywordToken;

        public SyntaxCharacterToken SemicolonToken { get; init; }

        public TypeNode? TypeNode => AsClauseNode?.TypeNode;

        public ExpressionNode ValueNode { get; init; }

        public override BoundDeclarationNode Bind(Symbol symbol)
            => new(this, symbol);

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
