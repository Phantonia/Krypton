using Krypton.Analysis.AbstractSyntaxTree.Nodes.Expressions;
using Krypton.Analysis.Errors;
using Krypton.Analysis.Lexical;
using Krypton.Analysis.Lexical.Lexemes.SyntaxCharacters;
using Krypton.Analysis.Lexical.Lexemes.WithValue;

namespace Krypton.Analysis.Grammatical.Expressions
{
    public sealed class ExpressionParser
    {
        public ExpressionParser(LexemeCollection lexemes)
        {
            Lexemes = lexemes;
        }

        public LexemeCollection Lexemes { get; }

        public ExpressionNode? ParseNextExpression(ref int index)
        {
            ExpressionNode? root = null;

            // Look at first lexeme
            switch (Lexemes[index])
            {
                case BooleanLiteralLexeme bll:
                    root = new BooleanLiteralExpressionNode(bll.Value, bll.LineNumber);
                    break;
                case IntegerLiteralLexeme ill:
                    root = new IntegerLiteralExpressionNode(ill.Value, ill.LineNumber);
                    break;
                case StringLiteralLexeme sll:
                    root = new StringLiteralExpressionNode(sll.Value, sll.LineNumber);
                    break;
                case CharLiteralLexeme cll:
                    root = new CharLiteralExpressionNode(cll.Value, cll.LineNumber);
                    break;
                case ImaginaryLiteralLexeme imll:
                    root = new ImaginaryLiteralExpressionNode(imll.Value, imll.LineNumber);
                    break;
                case RealLiteralLexeme rll:
                    root = new RealLiteralExpressionNode(rll.Value, rll.LineNumber);
                    break;
                case ParenthesisOpeningLexeme pol:
                    {
                        index++;
                        root = ParseNextExpression(ref index);

                        index++;

                        if (Lexemes[index] is not ParenthesisClosingLexeme)
                        {
                            ErrorProvider.ReportMissingClosingParenthesis(Lexemes[index].Content, Lexemes[index].LineNumber);
                            return null;
                        }
                    }
                    break;
            }

            return root;
        }
    }
}
