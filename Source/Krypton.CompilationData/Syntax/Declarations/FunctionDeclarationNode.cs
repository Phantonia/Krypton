using Krypton.CompilationData.Syntax.Clauses;
using Krypton.CompilationData.Syntax.Tokens;
using Krypton.Utilities;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Krypton.CompilationData.Syntax.Declarations
{
    public sealed class FunctionDeclarationNode : NamedDeclarationNode
    {
        public FunctionDeclarationNode(ReservedKeywordToken funcKeyword,
                                       IdentifierToken name,
                                       SyntaxCharacterToken openingParenthesis,
                                       IEnumerable<ParameterDeclarationNode>? parameters,
                                       IEnumerable<SyntaxCharacterToken>? commas,
                                       SyntaxCharacterToken closingParenthesis,
                                       AsClauseNode? returnTypeClause,
                                       BodyNode body,
                                       SyntaxNode? parent = null)
            : base(name, parent)
        {
            Debug.Assert(funcKeyword.Keyword == ReservedKeyword.Func);
            Debug.Assert(openingParenthesis.SyntaxCharacter == SyntaxCharacter.ParenthesisOpening);
            Debug.Assert(commas?.All(t => t.SyntaxCharacter == SyntaxCharacter.Comma) ?? true);
            Debug.Assert(closingParenthesis.SyntaxCharacter == SyntaxCharacter.ParenthesisClosing);

            FuncKeywordToken = funcKeyword;
            OpeningParenthesisToken = openingParenthesis;
            ParameterNodes = parameters?.Select(p => p.WithParent(this)).MakeReadOnly() ?? default;
            CommaTokens = commas.MakeReadOnly();
            ClosingParenthesisToken = closingParenthesis;
            ReturnTypeClauseNode = returnTypeClause?.WithParent(this);
            BodyNode = body;

            Debug.Assert(CommaTokens.Count == ParameterNodes.Count
                     || (CommaTokens.Count == 0 && ParameterNodes.Count == 0));
        }

        public AsClauseNode? ReturnTypeClauseNode { get; }

        public BodyNode BodyNode { get; }

        public SyntaxCharacterToken ClosingParenthesisToken { get; }

        public ReadOnlyList<SyntaxCharacterToken> CommaTokens { get; }

        public ReservedKeywordToken FuncKeywordToken { get; }

        public override bool IsLeaf => false;

        public SyntaxCharacterToken OpeningParenthesisToken { get; }

        public ReadOnlyList<ParameterDeclarationNode> ParameterNodes { get; }

        public FunctionDeclarationNode WithChildren(ReservedKeywordToken? funcKeyword = null,
                                                    IdentifierToken? name = null,
                                                    SyntaxCharacterToken? openingParenthesis = null,
                                                    IEnumerable<ParameterDeclarationNode>? parameters = null,
                                                    IndexWither<ParameterDeclarationNode>[]? parameterWithers = null,
                                                    IEnumerable<SyntaxCharacterToken>? commas = null,
                                                    IndexWither<SyntaxCharacterToken>[]? commaWithers = null,
                                                    SyntaxCharacterToken? closingParenthesis = null,
                                                    AsClauseNode? returnTypeClause = null,
                                                    bool overwriteReturnTypeClause = false,
                                                    BodyNode? body = null)
            => new(funcKeyword ?? FuncKeywordToken,
                   name ?? NameToken,
                   openingParenthesis ?? OpeningParenthesisToken,
                   parameters ?? ParameterNodes.With(parameterWithers),
                   commas ?? CommaTokens.With(commaWithers),
                   closingParenthesis ?? ClosingParenthesisToken,
                   returnTypeClause ?? (overwriteReturnTypeClause ? null : ReturnTypeClauseNode),
                   body ?? BodyNode);

        public override FunctionDeclarationNode WithParent(SyntaxNode newParent)
            => new(FuncKeywordToken, NameToken, OpeningParenthesisToken, ParameterNodes,
                   CommaTokens, ClosingParenthesisToken, ReturnTypeClauseNode, BodyNode, newParent);

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
    }
}
