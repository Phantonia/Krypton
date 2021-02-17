using Krypton.Analysis.Errors;
using Krypton.Analysis.Lexical.Lexemes;
using Krypton.Analysis.Lexical.Lexemes.WithValue;
using Krypton.Utilities;
using System;

namespace Krypton.Analysis.Lexical
{
    partial class Lexer
    {
        private Lexeme LexBinaryInteger()
        {
            int lexemeIndex = index;

            index++;

            int startIndex = index;

            for (; index < Code.Length; index++)
            {
                if (!Code[index].IsBinary() & Code[index] != '_')
                {
                    return new IntegerLiteralLexeme(Code[startIndex..index], IntegerStyle.Base2, lineNumber, lexemeIndex);
                }
            }

            return new IntegerLiteralLexeme(Code[startIndex..], IntegerStyle.Base2, lineNumber, lexemeIndex);
        }

        private Lexeme LexCharLiteralLexeme()
        {
            int lexemeIndex = index;

            index++;

            int startIndex = index;
            bool escaped = false;

            for (; index < Code.Length; index++)
            {
                if (Code[index] == '\\')
                {
                    escaped = !escaped;
                }
                else
                {
                    escaped = false;

                    if (Code[index] == '\'' & !escaped)
                    {
                        int endIndex = index;
                        index++;
                        return CharLiteralLexeme.Create(Code[startIndex..endIndex], lineNumber, lexemeIndex);
                    }
                    else if (Code[index] == '\n')
                    {
                        return new InvalidLexeme(Code[startIndex..index], ErrorCode.UnclosedCharLiteral, lineNumber, lexemeIndex);
                    }
                }
            }

            return new InvalidLexeme(Code[startIndex..], ErrorCode.UnclosedCharLiteral, lineNumber, lexemeIndex);
        }

        private Lexeme LexDecimalIntegerOrRational()
        {
            int startIndex = index;
            bool alreadyHadDecimalPoint = false;
            int lexemeIndex = index;

            index++;

            for (; index < Code.Length; index++)
            {
                if (Code[index] == '_')
                {
                    if (Code[index - 1] == '.' || Code.TryGet(index + 1) == '.')
                    {
                        return Finished();
                    }
                }
                else if (Code[index] == '.')
                {
                    if (char.IsNumber(Code.TryGet(index + 1) ?? '\0'))
                    {
                        if (!alreadyHadDecimalPoint)
                        {
                            alreadyHadDecimalPoint = true;
                        }
                        else
                        {
                            return new RationalLiteralLexeme(Code[startIndex..index], lineNumber, lexemeIndex);
                        }
                    }
                    else
                    {
                        if (!alreadyHadDecimalPoint)
                        {
                            return new IntegerLiteralLexeme(Code[startIndex..index], IntegerStyle.Base10, lineNumber, lexemeIndex);
                        }
                        else
                        {
                            return new RationalLiteralLexeme(Code[startIndex..index], lineNumber, lexemeIndex);
                        }
                    }
                }
                else if (!char.IsDigit(Code[index]))
                {
                    return Finished();
                }

                Lexeme Finished()
                {
                    if (Code[index] == 'i')
                    {
                        index++;

                        return new ImaginaryLiteralLexeme(Code[startIndex..index], lineNumber, lexemeIndex);
                    }

                    if (alreadyHadDecimalPoint)
                    {
                        return new RationalLiteralLexeme(Code[startIndex..index], lineNumber, lexemeIndex);
                    }
                    else
                    {
                        return new IntegerLiteralLexeme(Code[startIndex..index], IntegerStyle.Base10, lineNumber, lexemeIndex);
                    }
                }
            }

            if (alreadyHadDecimalPoint)
            {
                return new RationalLiteralLexeme(Code[startIndex..], lineNumber, lexemeIndex);
            }
            else
            {
                return new IntegerLiteralLexeme(Code[startIndex..], IntegerStyle.Base10, lineNumber, lexemeIndex);
            }
        }

        private Lexeme LexHexadecimalInteger()
        {
            int lexemeIndex = index;

            index++;

            int startIndex = index;

            bool? isUpper = null; // null = we don't know

            bool willBeInvalid = false;

            for (; index < Code.Length; index++)
            {
                bool isHex = Code[index].IsHex(out bool? isUpperTmp);

                isUpper ??= isUpperTmp;

                if (isUpperTmp.HasValue & isUpper != isUpperTmp)
                {
                    willBeInvalid = true;
                }

                if (!isHex & Code[index] != '_')
                {
                    if (willBeInvalid)
                    {
                        return new InvalidLexeme(Code[startIndex..index], ErrorCode.HexLiteralWithMixedCase, lineNumber, lexemeIndex);
                    }
                    else
                    {
                        return new IntegerLiteralLexeme(Code[startIndex..index], IntegerStyle.Base16, lineNumber, lexemeIndex);
                    }
                }
            }

            if (willBeInvalid)
            {
                return new InvalidLexeme(Code[startIndex..], ErrorCode.HexLiteralWithMixedCase, lineNumber, lexemeIndex);
            }
            else
            {
                return new IntegerLiteralLexeme(Code[startIndex..], IntegerStyle.Base16, lineNumber, lexemeIndex);
            }
        }

        private Lexeme LexIdentifier()
        {
            int startIndex = index;

            index++;

            string identifier;

            for (; index < Code.Length; index++)
            {
                bool temp = char.IsDigit(Code[index]);
                char tmp = Code[index];
                if (index == Code.Length
                    || (!Code[index].IsLetterOrUnderscore()
                        && !char.IsDigit(Code[index])))
                {
                    identifier = Code[startIndex..index];
                    goto LeftForLoop;
                }
            }

            identifier = Code[startIndex..];

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

            for (; index < Code.Length; index++)
            {
                if (Code[index] == '"' & !escaped)
                {
                    int endIndex = index;
                    index++;

                    string stringCode = Code[startIndex..endIndex];

                    if (StringLiteralParser.TryParse(stringCode, out string value))
                    {
                        return new StringLiteralLexeme(Code[startIndex..endIndex], lineNumber, lexemeIndex);
                    }
                    else
                    {
                        return new InvalidLexeme($"\"{stringCode}\"", ErrorCode.EscapeSequenceError, lineNumber, lexemeIndex);
                    }
                }
                else if (Code[index] == '\\')
                {
                    escaped = !escaped;
                }
                else if (Code[index] == '\n')
                {
                    return new InvalidLexeme(Code[startIndex..index], ErrorCode.UnclosedStringLiteral, lineNumber, lexemeIndex);
                }
                else
                {
                    escaped = false;
                }
            }

            return new InvalidLexeme(Code[startIndex..], ErrorCode.UnclosedStringLiteral, lineNumber, lexemeIndex);
        }
    }
}
