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
            index++;

            if (Code.TryGet(index) == '*') // **
            {
                index++;

                if (Code.TryGet(index) == '=') // **=
                {
                    index++;

                    return new CompoundAssignmentEqualsLexeme(Operator.DoubleAsterisk, lineNumber);
                }
                else // **
                {
                    return new CharacterOperatorLexeme(Operator.DoubleAsterisk, lineNumber);
                }
            }
            else if (Code.TryGet(index) == '=') // *=
            {
                index++;

                return new CompoundAssignmentEqualsLexeme(Operator.Asterisk, lineNumber);
            }
            else // *
            {
                return new CharacterOperatorLexeme(Operator.Asterisk, lineNumber);
            }
        }

        private Lexeme LexExlamationMark() // !
        {
            index++;

            if (Code.TryGet(index) == '=') // !=
            {
                index++;
                return new CharacterOperatorLexeme(Operator.ExclamationEquals, lineNumber);
            }
            else // !
            {
                return new InvalidLexeme("!", ErrorCode.UnknownLexeme, lineNumber);
            }
        }

        private Lexeme LexLessThanOrLeftShift() // <
        {
            index++;

            char? nextChar = Code.TryGet(index);

            switch (nextChar)
            {
                case '-': // <-
                    index++;
                    return new CharacterOperatorLexeme(Operator.SingleLeftArrow, lineNumber);
                case '=': // <=
                    index++;
                    return new CharacterOperatorLexeme(Operator.LessThanEquals, lineNumber);
                default: // <
                    return new CharacterOperatorLexeme(Operator.LessThan, lineNumber);
            }
        }

        private Lexeme LexMinusSignOrRightShift() // -
        {
            index++;

            char? nextChar = Code.TryGet(index);

            switch (nextChar)
            {
                case '>': // ->
                    index++;
                    return new CharacterOperatorLexeme(Operator.SingleRightArrow, lineNumber);
                case '=': // -=
                    index++;
                    return new CompoundAssignmentEqualsLexeme(Operator.Minus, lineNumber);
                default: // -
                    return new CharacterOperatorLexeme(Operator.Minus, lineNumber);
            }
        }

        private Lexeme LexSpecificLexeme(SyntaxCharacter syntaxCharacter)
        {
            index++;
            return new SyntaxCharacterLexeme(syntaxCharacter, lineNumber);
        }

        private Lexeme LexWithPossibleEquals(Operator @operator)
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

        private Lexeme LexWithPossibleEquals(Operator withoutEquals, Operator withEquals)
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

        private Lexeme LexWithPossibleEquals(SyntaxCharacter withoutEquals, Operator withEquals)
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
