using Krypton.CompilationData;
using Krypton.CompilationData.Syntax;
using Krypton.CompilationData.Syntax.Tokens;
using Krypton.Utilities;
using System;

namespace Krypton.Analysis.Lexical
{
    partial class Lexer
    {
        private Token LexAsteriskOrDoubleAsteriskOrAsteriskEqualsOrDoubleAsteriskEquals() // *
        {
            int triviaEndingIndex = index - 1;

            index++;

            if (code.TryGet(index) == '*') // **
            {
                index++;

                if (code.TryGet(index) == '=') // **=
                {
                    index++;

                    throw new NotImplementedException();
                }
                else // **
                {
                    Trivia trivia = GetTrivia(triviaEndingIndex);
                    triviaStartingIndex = index;
                    return new OperatorToken(Operator.DoubleAsterisk, lineNumber, trivia);
                }
            }
            else if (code.TryGet(index) == '=') // *=
            {
                index++;

                throw new NotImplementedException();
            }
            else // *
            {
                Trivia trivia = GetTrivia(triviaEndingIndex);
                triviaStartingIndex = index;
                return new OperatorToken(Operator.Asterisk, lineNumber, trivia);
            }
        }

        private Token LexExlamationMark() // !
        {
            int triviaEndingIndex = index - 1;

            index++;

            if (code.TryGet(index) == '=') // !=
            {
                Trivia trivia = GetTrivia(triviaEndingIndex);
                index++;
                triviaStartingIndex = index;
                return new OperatorToken(Operator.ExclamationEquals, lineNumber, trivia);
            }
            else // !
            {
                Trivia trivia = GetTrivia(triviaEndingIndex);
                triviaStartingIndex = index;
                ReadOnlyMemory<char> text = code.AsMemory(index - 1, 1); // just "!"
                return new InvalidToken(text, DiagnosticsCode.UnknownLexeme, lineNumber, trivia);
            }
        }

        private Token LexLessThanOrLeftShift() // <
        {
            int triviaEndingIndex = index - 1;

            index++;

            switch (code.TryGet(index))
            {
                case '-': // <-
                    {
                        Trivia trivia = GetTrivia(triviaEndingIndex);
                        index++;
                        triviaStartingIndex = index;
                        return new OperatorToken(Operator.SingleLeftArrow, lineNumber, trivia);
                    }
                case '=': // <=
                    {
                        Trivia trivia = GetTrivia(triviaEndingIndex);
                        index++;
                        triviaStartingIndex = index;
                        return new OperatorToken(Operator.LessThanEquals, lineNumber, trivia);
                    }
                default: // <
                    {
                        Trivia trivia = GetTrivia(triviaEndingIndex);
                        triviaStartingIndex = index;
                        return new OperatorToken(Operator.LessThan, lineNumber, trivia);
                    }
            }
        }

        private Token LexMinusSignOrRightShift() // -
        {
            int triviaEndingIndex = index - 1;

            index++;

            switch (code.TryGet(index))
            {
                case '>': // ->
                    {
                        index++;
                        Trivia trivia = GetTrivia(triviaEndingIndex);
                        triviaStartingIndex = index;
                        return new OperatorToken(Operator.SingleRightArrow, lineNumber, trivia);
                    }
                case '=': // -=
                    index++;
                    throw new NotImplementedException();
                default: // -
                    {
                        Trivia trivia = GetTrivia(triviaEndingIndex);
                        triviaStartingIndex = index;
                        return new OperatorToken(Operator.Minus, lineNumber, trivia);
                    }
            }
        }

        private Token LexSpecificToken(SyntaxCharacter syntaxCharacter)
        {
            Trivia trivia = GetTrivia(index - 1);
            index++;
            triviaStartingIndex = index;
            return new SyntaxCharacterToken(syntaxCharacter, lineNumber, trivia);
        }

        private Token LexSpecificLexeme(Operator @operator)
        {
            Trivia trivia = GetTrivia(index - 1);
            index++;
            triviaStartingIndex = index;
            return new OperatorToken(@operator, lineNumber, trivia);
        }

        private Token LexWithPossibleEquals(Operator @operator)
        {
            int triviaEndingIndex = index - 1;
            index++;

            if (code.TryGet(index) == '=')
            {
                Trivia trivia = GetTrivia(triviaEndingIndex);
                index++;
                triviaStartingIndex = index;
                throw new NotImplementedException();
            }
            else
            {
                Trivia trivia = GetTrivia(triviaEndingIndex);
                triviaStartingIndex = index;
                return new OperatorToken(@operator, lineNumber, trivia);
            }
        }

        private Token LexWithPossibleEquals(Operator withoutEquals, Operator withEquals)
        {
            int triviaEndingIndex = index - 1;
            index++;

            if (code.TryGet(index) == '=')
            {
                Trivia trivia = GetTrivia(triviaEndingIndex);
                index++;
                triviaStartingIndex = index;
                return new OperatorToken(withEquals, lineNumber, trivia);
            }
            else
            {
                Trivia trivia = GetTrivia(triviaEndingIndex);
                index++;
                triviaStartingIndex = index;
                return new OperatorToken(withoutEquals, lineNumber, trivia);
            }
        }

        private Token LexWithPossibleEquals(SyntaxCharacter withoutEquals, Operator withEquals)
        {
            int triviaEndingIndex = index - 1;
            index++;

            if (code.TryGet(index) == '=')
            {
                Trivia trivia = GetTrivia(triviaEndingIndex);
                index++;
                triviaStartingIndex = index;
                return new OperatorToken(withEquals, lineNumber, trivia);
            }
            else
            {
                Trivia trivia = GetTrivia(triviaEndingIndex);
                triviaStartingIndex = index;
                return new SyntaxCharacterToken(withoutEquals, lineNumber, trivia);
            }
        }
    }
}
