using Krypton.Analysis.Errors;
using Krypton.CompilationData;
using Krypton.CompilationData.Syntax;
using Krypton.CompilationData.Syntax.Tokens;
using Krypton.Framework;
using Krypton.Utilities;
using System;
using System.Collections.Generic;

namespace Krypton.Analysis.Lexical
{
    internal sealed partial class Lexer
    {
        public Lexer(string code, Analyser analyser)
        {
            this.analyser = analyser;
            this.code = code;
        }

        private readonly Analyser analyser;
        private readonly string code;
        private int index = 0;
        private int lineNumber = 1;
        private int triviaStartingIndex;

        public ReadOnlyList<Token>? LexAll()
        {
            List<Token> tokens = new();
            LexemeCollection collection = new();

            Token? nextToken = NextToken();

            while (nextToken != null)
            {
                if (nextToken is InvalidToken invalidToken)
                {
                    analyser.ReportDiagnostic(new Diagnostic(invalidToken.DiagnosticsCode, IsError: true, invalidToken));
                    return null;
                }

                tokens.Add(nextToken);

                nextToken = NextToken();
            }

            collection.Add(new EndOfFileLexeme(lineNumber, code.Length));

            return tokens.MakeReadOnly();
        }

        public Token? NextToken()
        {
            return code.TryGet(index) switch
            {
                null => null,

                ';' => LexSpecificToken(SyntaxCharacter.Semicolon),
                ',' => LexSpecificToken(SyntaxCharacter.Comma),
                ':' => LexSpecificToken(SyntaxCharacter.Colon),
                '.' => LexDotOrSingleLineComment(),
                '(' => LexSpecificToken(SyntaxCharacter.ParenthesisOpening),
                ')' => LexSpecificToken(SyntaxCharacter.ParenthesisClosing),
                '[' => LexSpecificToken(SyntaxCharacter.SquareBracketOpening),
                ']' => LexSpecificToken(SyntaxCharacter.SquareBracketClosing),
                '{' => LexSpecificToken(SyntaxCharacter.BraceOpening),
                '}' => LexSpecificToken(SyntaxCharacter.BraceClosing),
                '<' => LexLessThanOrLeftShift(),
                '>' => LexGreaterThanOrMultilineComment(),
                '=' => LexWithPossibleEquals(SyntaxCharacter.Equals, Operator.DoubleEquals),

                '+' => LexWithPossibleEquals(Operator.Plus),
                '-' => LexMinusSignOrRightShift(),
                '*' => LexAsteriskOrDoubleAsteriskOrAsteriskEqualsOrDoubleAsteriskEquals(),
                '/' => LexWithPossibleEquals(Operator.ForeSlash),
                '&' => LexWithPossibleEquals(Operator.Ampersand),
                '|' => LexWithPossibleEquals(Operator.Pipe),
                '^' => LexWithPossibleEquals(Operator.Caret),
                '!' => LexExlamationMark(),
                '~' => LexSpecificLexeme(Operator.Tilde),

                '"' => LexStringLiteralLexeme(),
                '\'' => LexCharLiteralLexeme(),

                _ => LexOther()
            };
        }

        private Lexeme? LexOther()
        {
            int lexemeIndex = index;
            char currentChar = code[index];

            if (char.IsWhiteSpace(currentChar))
            {
                index++;

                for (; index < code.Length; index++)
                {
                    if (!char.IsWhiteSpace(code[index]))
                    {
                        return NextToken();
                    }
                    else if (code[index] == '\n')
                    {
                        lineNumber++;
                    }
                }

                return null;
            }
            else if (char.IsNumber(currentChar))
            {
                if (code.TryGet(index) == '0')
                {
                    switch (code.TryGet(index + 1))
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

                return new InvalidLexeme(currentChar.ToString(), ErrorCode.UnknownLexeme, lineNumber, lexemeIndex);
            }
        }

        private Trivia GetTrivia(int triviaEndingIndex)
        {
            ReadOnlyMemory<char> text = code.AsMemory(triviaStartingIndex,
                                                      triviaEndingIndex - triviaStartingIndex);
            return new Trivia(text);
        }
    }
}
