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
            index++;

            int startIndex = index;

            for (; index < Code.Length; index++)
            {
                if (!Code[index].IsBinary() & Code[index] != '_')
                {
                    return new IntegerLiteralLexeme(Code[startIndex..index], IntegerStyle.Base2, lineNumber);
                }
            }

            return new IntegerLiteralLexeme(Code[startIndex..], IntegerStyle.Base2, lineNumber);
        }

        private Lexeme LexCharLiteralLexeme()
        {
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
                        return CharLiteralLexeme.Create(Code[startIndex..endIndex], lineNumber);
                    }
                    else if (Code[index] == '\n')
                    {
                        return new InvalidLexeme(Code[startIndex..index], ErrorCode.UnclosedCharLiteral, lineNumber);
                    }
                }
            }

            return new InvalidLexeme(Code[startIndex..], ErrorCode.UnclosedCharLiteral, lineNumber);
        }

        private Lexeme LexDecimalIntegerOrRational()
        {
            int startIndex = index;
            bool alreadyHadDecimalPoint = false;

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
                            return new RationalLiteralLexeme(Code[startIndex..index], lineNumber);
                        }
                    }
                    else
                    {
                        if (!alreadyHadDecimalPoint)
                        {
                            return new IntegerLiteralLexeme(Code[startIndex..index], IntegerStyle.Base10, lineNumber);
                        }
                        else
                        {
                            return new RationalLiteralLexeme(Code[startIndex..index], lineNumber);
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

                        return new ImaginaryLiteralLexeme(Code[startIndex..index], lineNumber);
                    }

                    if (alreadyHadDecimalPoint)
                    {
                        return new RationalLiteralLexeme(Code[startIndex..index], lineNumber);
                    }
                    else
                    {
                        return new IntegerLiteralLexeme(Code[startIndex..index], IntegerStyle.Base10, lineNumber);
                    }
                }
            }

            if (alreadyHadDecimalPoint)
            {
                return new RationalLiteralLexeme(Code[startIndex..], lineNumber);
            }
            else
            {
                return new IntegerLiteralLexeme(Code[startIndex..], IntegerStyle.Base10, lineNumber);
            }
        }

        private Lexeme LexHexadecimalInteger()
        {
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
                        return new InvalidLexeme(Code[startIndex..index], ErrorCode.HexLiteralWithMixedCase, lineNumber);
                    }
                    else
                    {
                        return new IntegerLiteralLexeme(Code[startIndex..index], IntegerStyle.Base16, lineNumber);
                    }
                }
            }

            if (willBeInvalid)
            {
                return new InvalidLexeme(Code[startIndex..], ErrorCode.HexLiteralWithMixedCase, lineNumber);
            }
            else
            {
                return new IntegerLiteralLexeme(Code[startIndex..], IntegerStyle.Base16, lineNumber);
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
                return new SyntaxCharacterLexeme(SyntaxCharacter.Underscore, lineNumber);
            }

            if (Enum.TryParse(identifier, out ReservedKeyword keyword))
            {
                return KeywordLexeme.Create(keyword, lineNumber);
            }
            else
            {
                bool isTrue = identifier == "True";
                if (isTrue || identifier == "False")
                {
                    return new BooleanLiteralLexeme(isTrue, lineNumber);
                }
            }

            return new IdentifierLexeme(identifier, lineNumber);
        }

        private Lexeme LexStringLiteralLexeme()
        {
            index++;

            int startIndex = index;
            bool escaped = false;

            for (; index < Code.Length; index++)
            {
                if (Code[index] == '"' & !escaped)
                {
                    int endIndex = index;
                    index++;
                    return new StringLiteralLexeme(Code[startIndex..endIndex], lineNumber);
                }
                else if (Code[index] == '\\')
                {
                    escaped = !escaped;
                }
                else if (Code[index] == '\n')
                {
                    return new InvalidLexeme(Code[startIndex..index], ErrorCode.UnclosedStringLiteral, lineNumber);
                }
            }

            return new InvalidLexeme(Code[startIndex..], ErrorCode.UnclosedStringLiteral, lineNumber);
        }
    }
}
