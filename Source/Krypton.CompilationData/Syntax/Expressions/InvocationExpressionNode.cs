using Krypton.CompilationData.Symbols;
using Krypton.CompilationData.Syntax.Tokens;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Krypton.CompilationData.Syntax.Expressions
{
    public sealed record InvocationExpressionNode : ExpressionNode
    {
        public InvocationExpressionNode(ExpressionNode invokee,
                                        SyntaxCharacterToken openingParenthesis,
                                        IEnumerable<ExpressionNode>? arguments,
                                        IEnumerable<SyntaxCharacterToken>? commas,
                                        SyntaxCharacterToken closingParenthesis)
        {
            InvokeeNode = invokee;
            OpeningParenthesisToken = openingParenthesis;
            ArgumentNodes = arguments?.ToImmutableList() ?? ImmutableList<ExpressionNode>.Empty;
            CommaTokens = commas?.ToImmutableList() ?? ImmutableList<SyntaxCharacterToken>.Empty;
            ClosingParenthesisToken = closingParenthesis;

            Debug.Assert(CommaTokens.Count == ArgumentNodes.Count - 1);
        }

        public ImmutableList<ExpressionNode> ArgumentNodes { get; init; }

        public SyntaxCharacterToken ClosingParenthesisToken { get; init; }

        public ImmutableList<SyntaxCharacterToken> CommaTokens { get; init; }

        public ExpressionNode InvokeeNode { get; init; }

        public override bool IsLeaf => false;

        public SyntaxCharacterToken OpeningParenthesisToken { get; init; }

        protected override string GetDebuggerDisplay() => $"{base.GetDebuggerDisplay()}; Number of arguments: {ArgumentNodes.Count}";

        public override TypedExpressionNode Type(TypeSymbol type)
            => new(this, type);

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
