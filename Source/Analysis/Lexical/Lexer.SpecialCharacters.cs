using Krypton.Analysis.Errors;
using Krypton.Analysis.Lexical.Lexemes;
using Krypton.Framework;
using Krypton.Utilities;

namespace Krypton.Analysis.Lexical
{
    partial class Lexer
    {

        private Lexeme LexAsteriskOrDoubleAsteriskOrAsteriskEqualsOrDoubleAsteriskEquals() // *
        {
            int lexemeIndex = index;

            index++;

            if (Code.TryGet(index) == '*') // **
            {
                index++;

                if (Code.TryGet(index) == '=') // **=
                {
                    index++;

                    return new CompoundAssignmentEqualsLexeme(Operator.DoubleAsterisk, lineNumber, lexemeIndex);
                }
                else // **
                {
                    return new CharacterOperatorLexeme(Operator.DoubleAsterisk, lineNumber, lexemeIndex);
                }
            }
            else if (Code.TryGet(index) == '=') // *=
            {
                index++;

                return new CompoundAssignmentEqualsLexeme(Operator.Asterisk, lineNumber, lexemeIndex);
            }
            else // *
            {
                return new CharacterOperatorLexeme(Operator.Asterisk, lineNumber, lexemeIndex);
            }
        }

        private Lexeme LexExlamationMark() // !
        {
            int lexemeIndex = index;

            index++;

            if (Code.TryGet(index) == '=') // !=
            {
                index++;
                return new CharacterOperatorLexeme(Operator.ExclamationEquals, lineNumber, lexemeIndex);
            }
            else // !
            {
                return new InvalidLexeme("!", ErrorCode.UnknownLexeme, lineNumber, lexemeIndex);
            }
        }

        private Lexeme LexLessThanOrLeftShift() // <
        {
            int lexemeIndex = index;

            index++;

            char? nextChar = Code.TryGet(index);

            switch (nextChar)
            {
                case '-': // <-
                    index++;
                    return new CharacterOperatorLexeme(Operator.SingleLeftArrow, lineNumber, lexemeIndex);
                case '=': // <=
                    index++;
                    return new CharacterOperatorLexeme(Operator.LessThanEquals, lineNumber, lexemeIndex);
                default: // <
                    return new CharacterOperatorLexeme(Operator.LessThan, lineNumber, lexemeIndex);
            }
        }

        private Lexeme LexMinusSignOrRightShift() // -
        {
            int lexemeIndex = index;

            index++;

            char? nextChar = Code.TryGet(index);

            switch (nextChar)
            {
                case '>': // ->
                    index++;
                    return new CharacterOperatorLexeme(Operator.SingleRightArrow, lineNumber, lexemeIndex);
                case '=': // -=
                    index++;
                    return new CompoundAssignmentEqualsLexeme(Operator.Minus, lineNumber, lexemeIndex);
                default: // -
                    return new CharacterOperatorLexeme(Operator.Minus, lineNumber, lexemeIndex);
            }
        }

        private Lexeme LexSpecificLexeme(SyntaxCharacter syntaxCharacter)
        {
            index++;
            return new SyntaxCharacterLexeme(syntaxCharacter, lineNumber, index - 1);
        }

        private Lexeme LexWithPossibleEquals(Operator @operator)
        {
            int lexemeIndex = index;
            index++;

            if (Code.TryGet(index) == '=')
            {
                index++;
                return new CompoundAssignmentEqualsLexeme(@operator, lineNumber, lexemeIndex);
            }
            else
            {
                return new CharacterOperatorLexeme(@operator, lineNumber, lexemeIndex);
            }
        }

        private Lexeme LexWithPossibleEquals(Operator withoutEquals, Operator withEquals)
        {
            int lexemeIndex = index;
            index++;

            if (Code.TryGet(index) == '=')
            {
                index++;
                return new CharacterOperatorLexeme(withEquals, lineNumber, lexemeIndex);
            }
            else
            {
                return new CharacterOperatorLexeme(withoutEquals, lineNumber, lexemeIndex);
            }
        }

        private Lexeme LexWithPossibleEquals(SyntaxCharacter withoutEquals, Operator withEquals)
        {
            int lexemeIndex = index;
            index++;

            if (Code.TryGet(index) == '=')
            {
                index++;
                return new CharacterOperatorLexeme(withEquals, lineNumber, lexemeIndex);
            }
            else
            {
                return new SyntaxCharacterLexeme(withoutEquals, lineNumber, lexemeIndex);
            }
        }
    }
}
