using Krypton.Analysis.Errors;
using Krypton.Analysis.Lexical.Lexemes;
using Krypton.Utilities;

namespace Krypton.Analysis.Lexical
{
    public sealed partial class Lexer
    {
        public Lexer(string code)
        {
            Code = code;
        }

        private int index = 0;
        private int lineNumber = 1;

        public string Code { get; }

        public LexemeCollection LexAll()
        {
            LexemeCollection collection = new();

            Lexeme? nextLexeme = NextLexeme();

            while (nextLexeme != null)
            {
                collection.Add(nextLexeme);

                nextLexeme = NextLexeme();
            }

            collection.Seal(lineNumber);

            return collection;
        }

        public Lexeme? NextLexeme()
        {
            return Code.TryGet(index) switch
            {
                null => null,

                ';' => LexSpecificLexeme(SyntaxCharacter.Semicolon),
                ',' => LexSpecificLexeme(SyntaxCharacter.Comma),
                ':' => LexSpecificLexeme(SyntaxCharacter.Colon),
                '.' => LexDotOrSingleLineComment(),
                '(' => LexSpecificLexeme(SyntaxCharacter.ParenthesisOpening),
                ')' => LexSpecificLexeme(SyntaxCharacter.ParenthesisClosing),
                '[' => LexSpecificLexeme(SyntaxCharacter.SquareBracketOpening),
                ']' => LexSpecificLexeme(SyntaxCharacter.SquareBracketClosing),
                '{' => LexSpecificLexeme(SyntaxCharacter.BraceOpening),
                '}' => LexSpecificLexeme(SyntaxCharacter.BraceClosing),
                '<' => LexLessThanOrLeftShift(),
                '>' => LexGreaterThanOrMultilineComment(),
                '=' => LexWithPossibleEquals(SyntaxCharacter.Equals, CharacterOperator.DoubleEquals),

                '+' => LexWithPossibleEquals(CharacterOperator.Plus),
                '-' => LexMinusSignOrRightShift(),
                '*' => LexAsteriskOrDoubleAsteriskOrAsteriskEqualsOrDoubleAsteriskEquals(),
                '/' => LexWithPossibleEquals(CharacterOperator.ForeSlash),
                '&' => LexWithPossibleEquals(CharacterOperator.Ampersand),
                '|' => LexWithPossibleEquals(CharacterOperator.Pipe),
                '^' => LexWithPossibleEquals(CharacterOperator.Caret),
                '!' => LexExlamationMark(),

                '"' => LexStringLiteralLexeme(),
                '\'' => LexCharLiteralLexeme(),

                _ => LexOther()
            };
        }

        private Lexeme? LexOther()
        {
            char currentChar = Code[index];

            if (char.IsWhiteSpace(currentChar))
            {
                index++;

                for (; index < Code.Length; index++)
                {
                    if (!char.IsWhiteSpace(Code[index]))
                    {
                        return NextLexeme();
                    }
                    else if (Code[index] == '\n')
                    {
                        lineNumber++;
                    }
                }

                return null;
            }
            else if (char.IsNumber(currentChar))
            {
                if (Code.TryGet(index) == '0')
                {
                    switch (Code.TryGet(index + 1))
                    {
                        case 'x':
                            index++;
                            return LexHexadecimalInteger();
                        case 'b':
                            index++;
                            return LexBinaryInteger();
                        default:
                            return LexDecimalIntegerOrRational();
                    }
                }
                else
                {
                    return LexDecimalIntegerOrRational();
                }
            }
            else if (currentChar.IsLetterOrUnderscore())
            {
                return LexIdentifier();
            }
            else
            {
                index++;

                return new InvalidLexeme(currentChar.ToString(), ErrorCode.UnknownLexeme, lineNumber);
            }
        }
    }
}
