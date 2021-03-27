using Krypton.CompilationData.Symbols;
using Krypton.CompilationData.Syntax.Tokens;
using System.Diagnostics;
using System.IO;

namespace Krypton.CompilationData.Syntax.Expressions
{
    public sealed class BracketedExpressionNode : ExpressionNode
    {
        public BracketedExpressionNode(SyntaxCharacterToken openingParenthesis,
                                       ExpressionNode expression,
                                       SyntaxCharacterToken closingParenthesis,
                                       SyntaxNode? parent = null)
            : base(parent)
        {
            Debug.Assert(openingParenthesis.SyntaxCharacter == SyntaxCharacter.ParenthesisOpening);
            Debug.Assert(closingParenthesis.SyntaxCharacter == SyntaxCharacter.ParenthesisClosing);

            OpeningParenthesisToken = openingParenthesis;
            ExpressionNode = expression.WithParent(this);
            ClosingParenthesisToken = closingParenthesis;
        }

        public SyntaxCharacterToken ClosingParenthesisToken { get; }

        public ExpressionNode ExpressionNode { get; }

        public override bool IsLeaf => false;

        public SyntaxCharacterToken OpeningParenthesisToken { get; }

        public override TypedExpressionNode<BracketedExpressionNode> Bind(TypeSymbol type)
            => new(this, type);

        public override BracketedExpressionNode WithParent(SyntaxNode newParent)
            => new(OpeningParenthesisToken, ExpressionNode, ClosingParenthesisToken, newParent);

        public override void WriteCode(TextWriter output)
        {
            ClosingParenthesisToken.WriteCode(output);
            ExpressionNode.WriteCode(output);
            OpeningParenthesisToken.WriteCode(output);
        }
    }
}
