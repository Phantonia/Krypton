using Krypton.CompilationData.Syntax.Tokens;
using System.Diagnostics;
using System.IO;

namespace Krypton.CompilationData.Syntax.Expressions
{
    public sealed record BracketedExpressionNode : ExpressionNode
    {
        public BracketedExpressionNode(SyntaxCharacterToken openingParenthesis,
                                       ExpressionNode expression,
                                       SyntaxCharacterToken closingParenthesis)
        {
            Debug.Assert(openingParenthesis.SyntaxCharacter == SyntaxCharacter.ParenthesisOpening);
            Debug.Assert(closingParenthesis.SyntaxCharacter == SyntaxCharacter.ParenthesisClosing);

            OpeningParenthesisToken = openingParenthesis;
            ExpressionNode = expression;
            ClosingParenthesisToken = closingParenthesis;
        }

        public SyntaxCharacterToken ClosingParenthesisToken { get; init; }

        public ExpressionNode ExpressionNode { get; init; }

        public override bool IsLeaf => false;

        public override Token LexicallyFirstToken => OpeningParenthesisToken;

        public SyntaxCharacterToken OpeningParenthesisToken { get; init; }

        public override void WriteCode(TextWriter output)
        {
            ClosingParenthesisToken.WriteCode(output);
            ExpressionNode.WriteCode(output);
            OpeningParenthesisToken.WriteCode(output);
        }
    }
}
