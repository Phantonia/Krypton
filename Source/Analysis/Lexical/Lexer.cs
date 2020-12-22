using Krypton.Analysis.Errors;
using Krypton.Analysis.Lexical.Lexemes;
using Krypton.Analysis.Lexical.Lexemes.WithValue;
using Krypton.Analysis.Utilities;
using System;

namespace Krypton.Analysis.Lexical
{
    public sealed class Lexer
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
                '<' => LexWithPossibleEquals(CharacterOperator.LessThan, CharacterOperator.LessThanEquals),
                '>' => LexGreaterThanOrMultilineCommentEx(),
                '=' => LexWithPossibleEquals(SyntaxCharacter.Equals, CharacterOperator.DoubleEquals),

                '+' => LexWithPossibleEquals(CharacterOperator.Plus),
                '-' => LexWithPossibleEquals(CharacterOperator.Minus),
                '*' => LexAsteriskOrDoubleAsteriskOrAsteriskEqualsOrDoubleAsteriskEquals(),
                '/' => LexWithPossibleEquals(CharacterOperator.ForeSlash),
                '&' => LexWithPossibleEquals(CharacterOperator.Ampersand),
                '|' => LexWithPossibleEquals(CharacterOperator.Pipe),
                '^' => LexWithPossibleEquals(CharacterOperator.Caret),

                '"' => LexStringLiteralLexeme(),
                '\'' => LexCharLiteralLexeme(),

                _ => LexOther()
            };
        }

        private Lexeme LexAsteriskOrDoubleAsteriskOrAsteriskEqualsOrDoubleAsteriskEquals()
        {
            index++;

            if (Code.TryGet(index) == '*')
            {
                index++;

                if (Code.TryGet(index) == '=')
                {
                    index++;

                    return new CompoundAssignmentEqualsLexeme(CharacterOperator.DoubleAsterisk, lineNumber);
                }
                else
                {
                    return new CharacterOperatorLexeme(CharacterOperator.DoubleAsterisk, lineNumber);
                }
            }
            else if (Code.TryGet(index) == '=')
            {
                index++;

                return new CompoundAssignmentEqualsLexeme(CharacterOperator.Asterisk, lineNumber);
            }
            else
            {
                return new CharacterOperatorLexeme(CharacterOperator.Asterisk, lineNumber);
            }
        }

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

            for(; index < Code.Length; index++)
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

        private Lexeme? LexDotOrSingleLineComment()
        {
            index++;
            if (Code.TryGet(index) == '.' && Code.TryGet(index + 1) == '.')
            {
                index += 2;

                for (; index < Code.Length; index++)
                {
                    if (Code[index] == '\n')
                    {
                        lineNumber++;
                        index++;
                        return NextLexeme();
                    }
                }
                return null;
            }
            else
            {
                return new SyntaxCharacterLexeme(SyntaxCharacter.Dot, lineNumber);
            }
        }

        private Lexeme? LexGreaterThanOrMultilineCommentEx()
        {
            index++;

            if (Code.TryGet(index) == '>')
            {
                index++;

                if (Code.TryGet(index) == '>')
                {
                    int openedComments = 0;

                    for (; index < Code.Length; index++)
                    {
                        if (Code[index - 2] == '<'
                            && Code[index - 1] == '<'
                            && Code[index] == '<')
                        {
                            openedComments--;

                            if (openedComments == 0)
                            {
                                index++;
                                return NextLexeme();
                            }
                        }
                        else if (Code[index - 2] == '>'
                            && Code[index - 1] == '>'
                            && Code[index] == '>')
                        {
                            openedComments++;
                        }
                        else if (Code[index] == '\n')
                        {
                            lineNumber++;
                        }
                    }

                    return null;
                }
                else
                {
                    for (; index < Code.Length; index++)
                    {
                        if (Code[index - 2] != '<'
                            && Code[index - 1] == '<'
                            && Code[index] == '<'
                            && Code.TryGet(index + 1) != '<')
                        {
                            index++;
                            return NextLexeme();
                        }
                        else if (Code[index] == '\n')
                        {
                            lineNumber++;
                        }
                    }

                    return null;
                }
            }
            else
            {
                return new CharacterOperatorLexeme(CharacterOperator.GreaterThan, lineNumber);
            }
        }

        private Lexeme? LexGreaterThanOrMultilineComment()
        {
            index++;

            if (Code.TryGet(index) == '>')
            {
                index++;
                if (Code.TryGet(index) == '>') // >>>
                {
                    for (; index < Code.Length; index++)
                    {
                        if (Code[index - 2] == '<'
                          & Code[index - 1] == '<'
                          & Code[index] == '<')
                        {
                            index++;
                            return NextLexeme();
                        }
                        else if (Code[index] == '\n')
                        {
                            lineNumber++;
                        }
                    }
                    return null;
                }
                else
                {
                    int openedComments = 1;

                    for (; index < Code.Length; index++)
                    {
                        if (Code[index - 1] == '<'
                          & Code[index] == '<')
                        {
                            openedComments--;
                        }
                        else if (Code[index - 1] == '>'
                               & Code[index] == '>')
                        {
                            openedComments++;
                        }
                        else if (Code[index] == '\n')
                        {
                            lineNumber++;
                        }

                        if (openedComments == 0)
                        {
                            index++;
                            return NextLexeme();
                        }
                    }

                    return null;
                }
            }
            else
            {
                return new CharacterOperatorLexeme(CharacterOperator.GreaterThan, lineNumber);
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

        private Lexeme LexSpecificLexeme(SyntaxCharacter syntaxCharacter)
        {
            index++;
            return new SyntaxCharacterLexeme(syntaxCharacter, lineNumber);
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

        private Lexeme LexWithPossibleEquals(CharacterOperator @operator)
        {
            index++;

            if (Code.TryGet(index) == '=')
            {
                index++;
                return new CompoundAssignmentEqualsLexeme(@operator, lineNumber);
            }
            else
            {
                return new CharacterOperatorLexeme(@operator, lineNumber);
            }
        }

        private Lexeme LexWithPossibleEquals(CharacterOperator withoutEquals, CharacterOperator withEquals)
        {
            index++;

            if (Code.TryGet(index) == '=')
            {
                index++;
                return new CharacterOperatorLexeme(withEquals, lineNumber);
            }
            else
            {
                return new CharacterOperatorLexeme(withoutEquals, lineNumber);
            }
        }

        private Lexeme LexWithPossibleEquals(SyntaxCharacter withoutEquals, CharacterOperator withEquals)
        {
            index++;

            if (Code.TryGet(index) == '=')
            {
                index++;
                return new CharacterOperatorLexeme(withEquals, lineNumber);
            }
            else
            {
                return new SyntaxCharacterLexeme(withoutEquals, lineNumber);
            }
        }
    }
}
