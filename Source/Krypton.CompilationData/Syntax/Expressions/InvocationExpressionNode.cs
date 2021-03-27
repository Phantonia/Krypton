using Krypton.CompilationData.Symbols;
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
        public InvocationExpressionNode(ExpressionNode invokee,
                                        SyntaxCharacterToken openingParenthesis,
                                        IEnumerable<ExpressionNode>? arguments,
                                        IEnumerable<SyntaxCharacterToken>? commas,
                                        SyntaxCharacterToken closingParenthesis,
                                        SyntaxNode? parent = null)
            : base(parent)
        {
            InvokeeNode = invokee;
            OpeningParenthesisToken = openingParenthesis;
            ArgumentNodes = arguments?.Select(n => n.WithParent(this)).MakeReadOnly() ?? default;
            CommaTokens = commas.MakeReadOnly();
            ClosingParenthesisToken = closingParenthesis;

            Debug.Assert(CommaTokens.Count == ArgumentNodes.Count - 1);
        }

        public ReadOnlyList<ExpressionNode> ArgumentNodes { get; }

        public SyntaxCharacterToken ClosingParenthesisToken { get; }

        public ReadOnlyList<SyntaxCharacterToken> CommaTokens { get; }

        public ExpressionNode InvokeeNode { get; }

        public override bool IsLeaf => false;

        public SyntaxCharacterToken OpeningParenthesisToken { get; }

        protected override string GetDebuggerDisplay() => $"{base.GetDebuggerDisplay()}; Number of arguments: {ArgumentNodes.Count}";

        public override TypedExpressionNode<InvocationExpressionNode> Type(TypeSymbol type)
            => new(this, type);

        public InvocationExpressionNode WithChildren(ExpressionNode? invokee = null,
                                                     SyntaxCharacterToken? openingParenthesis = null,
                                                     IEnumerable<ExpressionNode>? arguments = null,
                                                     IndexWither<ExpressionNode>[]? argumentWithers = null,
                                                     IEnumerable<SyntaxCharacterToken>? commas = null,
                                                     IndexWither<SyntaxCharacterToken>[]? commaWithers = null,
                                                     SyntaxCharacterToken? closingParenthesis = null)
            => new(invokee ?? InvokeeNode,
                   openingParenthesis ?? OpeningParenthesisToken,
                   arguments ?? ArgumentNodes.With(argumentWithers),
                   commas ?? CommaTokens.With(commaWithers),
                   closingParenthesis ?? ClosingParenthesisToken);

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
