using Krypton.CompilationData.Syntax.Tokens;
using Krypton.Utilities;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Krypton.CompilationData.Syntax.Expressions
{
    public sealed class InvocationExpressionNode : ExpressionNode
    {
        public InvocationExpressionNode(ExpressionNode invokeeNode,
                                        SyntaxCharacterToken openingParenthesisToken,
                                        IEnumerable<ExpressionNode> argumentNodes,
                                        IEnumerable<SyntaxCharacterToken> commaTokens,
                                        SyntaxCharacterToken closingParenthesisToken)
            : this(invokeeNode,
                   openingParenthesisToken,
                   argumentNodes,
                   commaTokens,
                   closingParenthesisToken,
                   parent: null)
        { }

        public InvocationExpressionNode(ExpressionNode invokeeNode,
                                        SyntaxCharacterToken openingParenthesisToken,
                                        IEnumerable<ExpressionNode> argumentNodes,
                                        IEnumerable<SyntaxCharacterToken> commaTokens,
                                        SyntaxCharacterToken closingParenthesisToken,
                                        SyntaxNode? parent)
            : base(parent)
        {
            InvokeeNode = invokeeNode;
            OpeningParenthesisToken = openingParenthesisToken;
            ArgumentNodes = argumentNodes.Select(n => n.WithParent(this)).MakeReadOnly();
            CommaTokens = commaTokens.MakeReadOnly();
            ClosingParenthesisToken = closingParenthesisToken;

            Debug.Assert(CommaTokens.Count == ArgumentNodes.Count - 1);
        }

        public ReadOnlyList<ExpressionNode> ArgumentNodes { get; }

        public SyntaxCharacterToken ClosingParenthesisToken { get; }

        public ReadOnlyList<SyntaxCharacterToken> CommaTokens { get; }

        public ExpressionNode InvokeeNode { get; }

        public override bool IsLeaf => false;

        public SyntaxCharacterToken OpeningParenthesisToken { get; }

        public InvocationExpressionNode WithChildren(ExpressionNode? invokeeNode = null,
                                                     SyntaxCharacterToken? openingParenthesisToken = null,
                                                     IEnumerable<ExpressionNode>? argumentNodes = null,
                                                     IndexWither<ExpressionNode>[]? argumentNodeWithers = null,
                                                     IEnumerable<SyntaxCharacterToken>? commaTokens = null,
                                                     IndexWither<SyntaxCharacterToken>[]? commaTokenWithers = null,
                                                     SyntaxCharacterToken? closingParenthesisToken = null)
        {
            Debug.Assert((argumentNodes == null) ^ (argumentNodeWithers == null));
            Debug.Assert((commaTokens == null) ^ (commaTokenWithers == null));

            return new InvocationExpressionNode(invokeeNode ?? InvokeeNode,
                                                openingParenthesisToken ?? OpeningParenthesisToken,
                                                argumentNodes ?? ArgumentNodes.With(argumentNodeWithers),
                                                commaTokens ?? CommaTokens.With(commaTokenWithers),
                                                closingParenthesisToken ?? ClosingParenthesisToken);

        }

        public override InvocationExpressionNode WithParent(SyntaxNode newParent)
            => new(InvokeeNode,
                   OpeningParenthesisToken,
                   ArgumentNodes,
                   CommaTokens,
                   ClosingParenthesisToken,
                   newParent);

        public override void WriteCode(TextWriter output)
        {
            InvokeeNode.WriteCode(output);
            OpeningParenthesisToken.WriteCode(output);

            foreach (var (argument, comma) in ArgumentNodes.Zip(CommaTokens))
            {
                argument.WriteCode(output);
                comma.WriteCode(output);
            }

            ArgumentNodes[^1].WriteCode(output);
            ClosingParenthesisToken.WriteCode(output);
        }
    }
}
