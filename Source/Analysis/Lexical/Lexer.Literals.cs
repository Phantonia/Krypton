using Krypton.Analysis.Errors;
using Krypton.CompilationData.Syntax.Tokens;
using Krypton.Utilities;
using System;

namespace Krypton.Analysis.Lexical
{
    partial class Lexer
    {
        private Token LexBinaryInteger()
        {
            int lexemeIndex = index;

            index++;

            int startIndex = index;

            for (; index < code.Length; index++)
            {
                if (!code[index].IsBinary() & code[index] != '_')
                {
                    return new IntegerLiteralLexeme(code[startIndex..index], IntegerStyle.Base2, lineNumber, lexemeIndex);
                }
            }

            return new IntegerLiteralLexeme(code[startIndex..], IntegerStyle.Base2, lineNumber, lexemeIndex);
        }

        private Lexeme LexCharLiteralLexeme()
        {
            int lexemeIndex = index;

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
                        int endIndex = index;
                        index++;
                        return CharLiteralLexeme.Create(code[startIndex..endIndex], lineNumber, lexemeIndex);
                    }
                    else if (code[index] == '\n')
                    {
                        return new InvalidLexeme(code[startIndex..index], ErrorCode.UnclosedCharLiteral, lineNumber, lexemeIndex);
                    }
                }
            }

            return new InvalidLexeme(code[startIndex..], ErrorCode.UnclosedCharLiteral, lineNumber, lexemeIndex);
        }

        private Lexeme LexDecimalIntegerOrRational()
        {
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
                            return new RationalLiteralLexeme(code[startIndex..index], lineNumber, lexemeIndex);
                        }
                    }
                    else
                    {
                        if (!alreadyHadDecimalPoint)
                        {
                            return new IntegerLiteralLexeme(code[startIndex..index], IntegerStyle.Base10, lineNumber, lexemeIndex);
                        }
                        else
                        {
                            return new RationalLiteralLexeme(code[startIndex..index], lineNumber, lexemeIndex);
                        }
                    }
                }
                else if (!char.IsDigit(code[index]))
                {
                    return Finished();
                }

                Lexeme Finished()
                {
                    if (code[index] == 'i')
                    {
                        index++;

                        return new ImaginaryLiteralLexeme(code[startIndex..index], lineNumber, lexemeIndex);
                    }

                    if (alreadyHadDecimalPoint)
                    {
                        return new RationalLiteralLexeme(code[startIndex..index], lineNumber, lexemeIndex);
                    }
                    else
                    {
                        return new IntegerLiteralLexeme(code[startIndex..index], IntegerStyle.Base10, lineNumber, lexemeIndex);
                    }
                }
            }

            if (alreadyHadDecimalPoint)
            {
                return new RationalLiteralLexeme(code[startIndex..], lineNumber, lexemeIndex);
            }
            else
            {
                return new IntegerLiteralLexeme(code[startIndex..], IntegerStyle.Base10, lineNumber, lexemeIndex);
            }
        }

        private Lexeme LexHexadecimalInteger()
        {
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
                        return new InvalidLexeme(code[startIndex..index], ErrorCode.HexLiteralWithMixedCase, lineNumber, lexemeIndex);
                    }
                    else
                    {
                        return new IntegerLiteralLexeme(code[startIndex..index], IntegerStyle.Base16, lineNumber, lexemeIndex);
                    }
                }
            }

            if (willBeInvalid)
            {
                return new InvalidLexeme(code[startIndex..], ErrorCode.HexLiteralWithMixedCase, lineNumber, lexemeIndex);
            }
            else
            {
                return new IntegerLiteralLexeme(code[startIndex..], IntegerStyle.Base16, lineNumber, lexemeIndex);
            }
        }

        private Lexeme LexIdentifier()
        {
            int startIndex = index;

            index++;

            string identifier;

            for (; index < code.Length; index++)
            {
                bool temp = char.IsDigit(code[index]);
                char tmp = code[index];
                if (index == code.Length
                    || (!code[index].IsLetterOrUnderscore()
                        && !char.IsDigit(code[index])))
                {
                    identifier = code[startIndex..index];
                    goto LeftForLoop;
                }
            }

            identifier = code[startIndex..];

        LeftForLoop:
            if (identifier == "_")
            {
                return new SyntaxCharacterLexeme(SyntaxCharacter.Underscore, lineNumber, startIndex);
            }

            if (Enum.TryParse(identifier, out ReservedKeyword keyword))
            {
                return KeywordLexeme.Create(keyword, lineNumber, startIndex);
            }
            else
            {
                bool isTrue = identifier == "True";
                if (isTrue || identifier == "False")
                {
                    return new BooleanLiteralLexeme(isTrue, lineNumber, startIndex);
                }
            }

            return new IdentifierLexeme(identifier, lineNumber, startIndex);
        }

        private Lexeme LexStringLiteralLexeme()
        {
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

                    string stringCode = code[startIndex..endIndex];

                    if (StringLiteralParser.TryParse(stringCode, out string value))
                    {
                        return new StringLiteralLexeme(value, lineNumber, lexemeIndex);
                    }
                    else
                    {
                        return new InvalidLexeme($"\"{stringCode}\"", ErrorCode.EscapeSequenceError, lineNumber, lexemeIndex);
                    }
                }
                else if (code[index] == '\\')
                {
                    escaped = !escaped;
                }
                else if (code[index] == '\n')
                {
                    return new InvalidLexeme(code[startIndex..index], ErrorCode.UnclosedStringLiteral, lineNumber, lexemeIndex);
                }
                else
                {
                    escaped = false;
                }
            }

            return new InvalidLexeme(code[startIndex..], ErrorCode.UnclosedStringLiteral, lineNumber, lexemeIndex);
        }
    }
}
