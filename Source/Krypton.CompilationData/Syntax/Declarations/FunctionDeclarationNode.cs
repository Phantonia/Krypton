using Krypton.CompilationData.Symbols;
using Krypton.CompilationData.Syntax.Clauses;
using Krypton.CompilationData.Syntax.Tokens;
using Krypton.CompilationData.Syntax.Types;
using Krypton.Utilities;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Krypton.CompilationData.Syntax.Declarations
{
    public sealed record FunctionDeclarationNode : NamedDeclarationNode, IExecutableNode
    {
        public FunctionDeclarationNode(ReservedKeywordToken funcKeyword,
                                       IdentifierToken name,
                                       SyntaxCharacterToken openingParenthesis,
                                       IEnumerable<ParameterDeclarationNode>? parameters,
                                       IEnumerable<SyntaxCharacterToken>? commas,
                                       SyntaxCharacterToken closingParenthesis,
                                       AsClauseNode? returnTypeClause,
                                       BodyNode body)
            : base(name)
        {
            Debug.Assert(funcKeyword.Keyword == ReservedKeyword.Func);
            Debug.Assert(openingParenthesis.SyntaxCharacter == SyntaxCharacter.ParenthesisOpening);
            Debug.Assert(commas?.All(t => t.SyntaxCharacter == SyntaxCharacter.Comma) ?? true);
            Debug.Assert(closingParenthesis.SyntaxCharacter == SyntaxCharacter.ParenthesisClosing);

            FuncKeywordToken = funcKeyword;
            OpeningParenthesisToken = openingParenthesis;
            ParameterNodes = parameters?.ToImmutableList() ?? ImmutableList<ParameterDeclarationNode>.Empty;
            CommaTokens = commas?.ToImmutableList() ?? ImmutableList<SyntaxCharacterToken>.Empty;
            ClosingParenthesisToken = closingParenthesis;
            ReturnTypeClauseNode = returnTypeClause;
            BodyNode = body;

            Debug.Assert(CommaTokens.Count == ParameterNodes.Count
                     || (CommaTokens.Count == 0 && ParameterNodes.Count == 0));
        }

        public AsClauseNode? ReturnTypeClauseNode { get; init; }

        public BodyNode BodyNode { get; init; }

        public SyntaxCharacterToken ClosingParenthesisToken { get; init; }

        public ImmutableList<SyntaxCharacterToken> CommaTokens { get; init; }

        public ReservedKeywordToken FuncKeywordToken { get; init; }

        public override bool IsLeaf => false;

        public SyntaxCharacterToken OpeningParenthesisToken { get; init; }

        public ImmutableList<ParameterDeclarationNode> ParameterNodes { get; init; }

        public override BoundDeclarationNode Bind(Symbol symbol)
            => new(this, symbol);

        public override void WriteCode(TextWriter output)
        {
            FuncKeywordToken.WriteCode(output);
            NameToken.WriteCode(output);
            OpeningParenthesisToken.WriteCode(output);

            foreach (var (parameter, comma) in ParameterNodes.Zip(CommaTokens))
            {
                parameter.WriteCode(output);
                comma.WriteCode(output);
            }

            ClosingParenthesisToken.WriteCode(output);
            ReturnTypeClauseNode?.WriteCode(output);
            BodyNode.WriteCode(output);
        }

        TypeNode? IExecutableNode.ReturnTypeNode => ReturnTypeClauseNode?.TypeNode;
    }
}
