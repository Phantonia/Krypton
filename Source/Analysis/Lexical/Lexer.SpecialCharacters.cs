﻿using Krypton.Analysis.Errors;
using Krypton.Analysis.Lexical.Lexemes;
using Krypton.Analysis.Utilities;

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

                    return new CompoundAssignmentEqualsLexeme(CharacterOperator.DoubleAsterisk, lineNumber);
                }
                else // **
                {
                    return new CharacterOperatorLexeme(CharacterOperator.DoubleAsterisk, lineNumber);
                }
            }
            else if (Code.TryGet(index) == '=') // *=
            {
                index++;

                return new CompoundAssignmentEqualsLexeme(CharacterOperator.Asterisk, lineNumber);
            }
            else // *
            {
                return new CharacterOperatorLexeme(CharacterOperator.Asterisk, lineNumber);
            }
        }

        private Lexeme LexExlamationMark() // !
        {
            index++;

            if (Code.TryGet(index) == '=') // !=
            {
                index++;
                return new CharacterOperatorLexeme(CharacterOperator.ExclamationEquals, lineNumber);
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
                    return new CharacterOperatorLexeme(CharacterOperator.LeftShift, lineNumber);
                case '=': // <=
                    index++;
                    return new CharacterOperatorLexeme(CharacterOperator.LessThanEquals, lineNumber);
                default: // <
                    return new CharacterOperatorLexeme(CharacterOperator.LessThan, lineNumber);
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
                    return new CharacterOperatorLexeme(CharacterOperator.RightShift, lineNumber);
                case '=': // -=
                    index++;
                    return new CompoundAssignmentEqualsLexeme(CharacterOperator.Minus, lineNumber);
                default: // -
                    return new CharacterOperatorLexeme(CharacterOperator.Minus, lineNumber);
            }
        }

        private Lexeme LexSpecificLexeme(SyntaxCharacter syntaxCharacter)
        {
            index++;
            return new SyntaxCharacterLexeme(syntaxCharacter, lineNumber);
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
