using Krypton.CompilationData;
using Krypton.CompilationData.Syntax;
using Krypton.CompilationData.Syntax.Tokens;
using Krypton.Utilities;
using System;

namespace Krypton.Analysis.Lexical
{
    partial class Lexer
    {
        private LiteralToken<long> LexBinaryInteger()
        {
            int triviaEndingIndex = index - 1;

            index++;

            int startIndex = index;

            for (; index < code.Length; index++)
            {
                if (!code[index].IsBinary() & code[index] != '_')
                {
                    return MakeToken(code.AsMemory()[startIndex..index]);
                }
            }

            return MakeToken(code.AsMemory()[startIndex..]);

            LiteralToken<long> MakeToken(ReadOnlyMemory<char> text)
            {
                Trivia trivia = GetTrivia(triviaEndingIndex);
                index++;

                long value = NumberLiteralParser.ParseBinary(text.Span);

                return new LiteralToken<long>(value, text, lineNumber, trivia);
            }
        }

        private Token LexCharLiteralLexeme()
        {
            int triviaEndingIndex = index - 1;

            index++;

            int startIndex = index;
            bool escaped = false;

            for (; index < code.Length; index++)
            {
                if (code[index] == '\\')
                {
                    escaped = !escaped;
                }
                else
                {
                    escaped = false;

                    if (code[index] == '\'' & !escaped)
                    {
                        return MakeValidToken(code.AsMemory()[startIndex..index]);
                    }
                    else if (code[index] == '\n')
                    {
                        return MakeInvalidToken(code.AsMemory()[startIndex..index]);
                    }
                }
            }

            return MakeInvalidToken(code.AsMemory()[startIndex..]);

            InvalidToken MakeInvalidToken(ReadOnlyMemory<char> text)
            {
                Trivia trivia = GetTrivia(triviaEndingIndex);
                index++;

                return new InvalidToken(text, DiagnosticsCode.UnclosedCharLiteral, lineNumber, trivia);
            }

            LiteralToken<char> MakeValidToken(ReadOnlyMemory<char> text)
            {
                Trivia trivia = GetTrivia(triviaEndingIndex);
                index++;

                char value = EscapeSequences.Parse(text.Span);

                return new LiteralToken<char>(value, text, lineNumber, trivia);
            }
        }

        // Used by LexDeciamlIntegerOrRational
        private delegate T LiteralParser<T>(ReadOnlySpan<char> input);

        private Token LexDecimalIntegerOrRational()
        {
            int triviaEndingIndex = index - 1;
            int startIndex = index;
            bool alreadyHadDecimalPoint = false;
            int lexemeIndex = index;

            index++;

            for (; index < code.Length; index++)
            {
                if (code[index] == '_')
                {
                    if (code[index - 1] == '.' || code.TryGet(index + 1) == '.')
                    {
                        return Finished();
                    }
                }
                else if (code[index] == '.')
                {
                    if (char.IsNumber(code.TryGet(index + 1) ?? '\0'))
                    {
                        if (!alreadyHadDecimalPoint)
                        {
                            alreadyHadDecimalPoint = true;
                        }
                        else
                        {
                            return MakeToken(code.AsMemory()[startIndex..index], NumberLiteralParser.ParseRational);
                        }
                    }
                    else
                    {
                        if (!alreadyHadDecimalPoint)
                        {
                            return MakeToken(code.AsMemory()[startIndex..index], NumberLiteralParser.ParseDecimal);
                        }
                        else
                        {
                            return MakeToken(code.AsMemory()[startIndex..index], NumberLiteralParser.ParseRational);
                        }
                    }
                }
                else if (!char.IsDigit(code[index]))
                {
                    return Finished();
                }

                Token Finished()
                {
                    if (code[index] == 'i')
                    {
                        index++;

                        return MakeToken(code.AsMemory()[startIndex..index],
                                         s => new RationalComplex(0, NumberLiteralParser.ParseRational(s)));
                    }

                    if (alreadyHadDecimalPoint)
                    {
                        return MakeToken(code.AsMemory()[startIndex..index], NumberLiteralParser.ParseRational);
                    }
                    else
                    {
                        return MakeToken(code.AsMemory()[startIndex..index], NumberLiteralParser.ParseDecimal);
                    }
                }
            }

            if (alreadyHadDecimalPoint)
            {
                return MakeToken(code.AsMemory()[startIndex..], NumberLiteralParser.ParseRational);
            }
            else
            {
                return MakeToken(code.AsMemory()[startIndex..], NumberLiteralParser.ParseDecimal);
            }

            LiteralToken<T> MakeToken<T>(ReadOnlyMemory<char> text, LiteralParser<T> literalParser)
            {
                Trivia trivia = GetTrivia(triviaEndingIndex);
                index++;

                T value = literalParser(text.Span);

                return new LiteralToken<T>(value, text, lineNumber, trivia);
            }
        }

        private Token LexHexadecimalInteger()
        {
            int triviaEndingIndex = index - 1;
            int lexemeIndex = index;

            index++;

            int startIndex = index;

            bool? isUpper = null; // null = we don't know

            bool willBeInvalid = false;

            for (; index < code.Length; index++)
            {
                bool isHex = code[index].IsHex(out bool? isUpperTmp);

                isUpper ??= isUpperTmp;

                if (isUpperTmp.HasValue & isUpper != isUpperTmp)
                {
                    willBeInvalid = true;
                }

                if (!isHex & code[index] != '_')
                {
                    if (willBeInvalid)
                    {
                        return MakeInvalidToken(code.AsMemory()[startIndex..index]);
                    }
                    else
                    {
                        return MakeValidToken(code.AsMemory()[startIndex..index]);
                    }
                }
            }

            if (willBeInvalid)
            {
                return MakeInvalidToken(code.AsMemory()[startIndex..]);
            }
            else
            {
                return MakeValidToken(code.AsMemory()[startIndex..]);
            }

            InvalidToken MakeInvalidToken(ReadOnlyMemory<char> text)
            {
                Trivia trivia = GetTrivia(triviaEndingIndex);
                index++;

                return new InvalidToken(text,
                                        DiagnosticsCode.HexLiteralWithMixedCase,
                                        lineNumber,
                                        trivia);
            }

            LiteralToken<long> MakeValidToken(ReadOnlyMemory<char> text)
            {
                Trivia trivia = GetTrivia(triviaEndingIndex);
                index++;

                long value = NumberLiteralParser.ParseHexadecimal(text.Span);

                return new LiteralToken<long>(value, text, lineNumber, trivia);
            }
        }

        private Token LexIdentifier()
        {
            int triviaEndingIndex = index - 1;
            int startIndex = index;

            index++;

            ReadOnlyMemory<char> identifierOrKeyword;

            for (; index < code.Length; index++)
            {
                bool temp = char.IsDigit(code[index]);
                char tmp = code[index];
                if (index == code.Length
                    || (!code[index].IsLetterOrUnderscore()
                        && !char.IsDigit(code[index])))
                {
                    identifierOrKeyword = code.AsMemory()[startIndex..index];
                    goto LeftForLoop;
                }
            }

            identifierOrKeyword = code.AsMemory()[startIndex..];

        LeftForLoop:
            if (identifierOrKeyword.Span == "_")
            {
                Trivia trivia = GetTrivia(triviaEndingIndex);
                index++;

                return new SyntaxCharacterToken(SyntaxCharacter.Underscore, lineNumber, trivia);
            }

            if (ReservedKeywords.IsKeyword(identifierOrKeyword.Span, out ReservedKeyword keyword))
            {
                Trivia trivia = GetTrivia(triviaEndingIndex);

                if (ReservedKeywords.IsOperatorKeyword(keyword, out Operator @operator))
                {
                    return new OperatorToken(@operator, lineNumber, trivia);
                }
                else if (ReservedKeywords.IsBooleanLiteralKeyword(keyword, out bool value))
                {
                    return new LiteralToken<bool>(value, identifierOrKeyword, lineNumber, trivia);
                }

                return new ReservedKeywordToken(keyword, lineNumber, trivia);
            }
            else
            {
                bool isTrue = identifierOrKeyword.Span == "True";
                if (isTrue || identifierOrKeyword.Span == "False")
                {
                    Trivia trivia = GetTrivia(triviaEndingIndex);
                    return new LiteralToken<bool>(isTrue, identifierOrKeyword, lineNumber, trivia);
                }
            }

            {
                Trivia trivia = GetTrivia(triviaEndingIndex);
                return new IdentifierToken(identifierOrKeyword, lineNumber, trivia);
            }
        }

        private Token LexStringLiteralLexeme()
        {
            int triviaEndingIndex = index - 1;
            int lexemeIndex = index;

            index++;

            int startIndex = index;
            bool escaped = false;

            for (; index < code.Length; index++)
            {
                if (code[index] == '"' & !escaped)
                {
                    int endIndex = index;
                    index++;

                    Trivia trivia = GetTrivia(triviaEndingIndex);

                    ReadOnlyMemory<char> stringCode = code.AsMemory()[startIndex..endIndex];

                    if (StringLiteralParser.TryParse(stringCode.Span, out string value))
                    {
                        return new LiteralToken<string>(value, stringCode, lineNumber, trivia);
                    }
                    else
                    {
                        return new InvalidToken(stringCode, DiagnosticsCode.EscapeSequenceError, lineNumber, trivia);
                    }
                }
                else if (code[index] == '\\')
                {
                    escaped = !escaped;
                }
                else if (code[index] == '\n')
                {
                    Trivia trivia = GetTrivia(triviaEndingIndex);
                    return new InvalidToken(code.AsMemory()[startIndex..index],
                                            DiagnosticsCode.UnclosedStringLiteral,
                                            lineNumber,
                                            trivia);
                }
                else
                {
                    escaped = false;
                }
            }

            {
                Trivia trivia = GetTrivia(triviaEndingIndex);
                return new InvalidToken(code.AsMemory()[startIndex..],
                                        DiagnosticsCode.UnclosedStringLiteral,
                                        lineNumber,
                                        trivia);
            }
        }
    }
}
