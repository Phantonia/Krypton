using Krypton.CompilationData;
using Krypton.CompilationData.Syntax;
using Krypton.CompilationData.Syntax.Tokens;
using Krypton.Utilities;
using System;
using Text = System.ReadOnlyMemory<char>;

namespace Krypton.Analysis.Lexical
{
    partial class Lexer
    {
        private Text GetLastCharacter()
            => code.AsMemory(index - 1, length: 1);

        private Text GetLastNCharacters(int n)
            => code.AsMemory(index - n, length: n);

        private Token LexAsteriskOrDoubleAsteriskOrAsteriskEqualsOrDoubleAsteriskEquals() // *
        {
            int triviaEndingIndex = index - 1;

            index++;

            if (code.TryGet(index) == '*') // **
            {
                index++;

                //if (code.TryGet(index) == '=') // **=
                //{
                //    index++;

                //    throw new NotImplementedException();
                //}
                //else // **
                {
                    Text text = GetLastNCharacters(2);
                    Trivia trivia = GetTrivia(triviaEndingIndex);
                    triviaStartingIndex = index;
                    return new OperatorToken(Operator.DoubleAsterisk, text, lineNumber, trivia);
                }
            }
            //else if (code.TryGet(index) == '=') // *=
            //{
            //    index++;

            //    throw new NotImplementedException();
            //}
            else // *
            {
                Text text = GetLastCharacter();
                Trivia trivia = GetTrivia(triviaEndingIndex);
                triviaStartingIndex = index;
                return new OperatorToken(Operator.Asterisk, text, lineNumber, trivia);
            }
        }

        private Token LexExlamationMark() // !
        {
            int triviaEndingIndex = index - 1;

            index++;

            if (code.TryGet(index) == '=') // !=
            {
                index++;

                Text text = GetLastNCharacters(2);
                Trivia trivia = GetTrivia(triviaEndingIndex);
                triviaStartingIndex = index;
                return new OperatorToken(Operator.ExclamationEquals, text, lineNumber, trivia);
            }
            else // !
            {
                Trivia trivia = GetTrivia(triviaEndingIndex);
                triviaStartingIndex = index;
                Text text = code.AsMemory(index - 1, 1); // just "!"
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
                        index++;

                        Text text = GetLastNCharacters(2);
                        Trivia trivia = GetTrivia(triviaEndingIndex);
                        triviaStartingIndex = index;
                        return new OperatorToken(Operator.SingleLeftArrow, text, lineNumber, trivia);
                    }
                case '=': // <=
                    {
                        index++;

                        Text text = GetLastNCharacters(2);
                        Trivia trivia = GetTrivia(triviaEndingIndex);
                        triviaStartingIndex = index;
                        return new OperatorToken(Operator.LessThanEquals, text, lineNumber, trivia);
                    }
                default: // <
                    {
                        Text text = GetLastCharacter();
                        Trivia trivia = GetTrivia(triviaEndingIndex);
                        triviaStartingIndex = index;
                        return new OperatorToken(Operator.LessThan, text, lineNumber, trivia);
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

                        Text text = GetLastNCharacters(2);
                        Trivia trivia = GetTrivia(triviaEndingIndex);
                        triviaStartingIndex = index;
                        return new OperatorToken(Operator.SingleRightArrow, text, lineNumber, trivia);
                    }
                //case '=': // -=
                //    index++;
                //    throw new NotImplementedException();
                default: // -
                    {
                        Text text = GetLastCharacter();
                        Trivia trivia = GetTrivia(triviaEndingIndex);
                        triviaStartingIndex = index;
                        return new OperatorToken(Operator.Minus, text, lineNumber, trivia);
                    }
            }
        }

        private Token LexSpecificToken(SyntaxCharacter syntaxCharacter)
        {
            Text text = code.AsMemory(index, length: 1);
            Trivia trivia = GetTrivia(index - 1);
            index++;
            triviaStartingIndex = index;
            return new SyntaxCharacterToken(syntaxCharacter, text, lineNumber, trivia);
        }

        private Token LexSpecificLexeme(Operator @operator)
        {
            Text text = code.AsMemory(index, length: 1);
            Trivia trivia = GetTrivia(index - 1);
            index++;
            triviaStartingIndex = index;
            return new OperatorToken(@operator, text, lineNumber, trivia);
        }

        private Token LexWithPossibleEquals(Operator @operator)
        {
            int triviaEndingIndex = index - 1;
            index++;

            //if (code.TryGet(index) == '=')
            //{
            //    Trivia trivia = GetTrivia(triviaEndingIndex);
            //    index++;
            //    triviaStartingIndex = index;
            //    throw new NotImplementedException();
            //}
            //else
            {
                Text text = GetLastCharacter();
                Trivia trivia = GetTrivia(triviaEndingIndex);
                triviaStartingIndex = index;
                return new OperatorToken(@operator, text, lineNumber, trivia);
            }
        }

        private Token LexWithPossibleEquals(Operator withoutEquals, Operator withEquals)
        {
            int triviaEndingIndex = index - 1;
            index++;

            if (code.TryGet(index) == '=')
            {
                Text text = GetLastNCharacters(2);
                Trivia trivia = GetTrivia(triviaEndingIndex);
                index++;
                triviaStartingIndex = index;
                return new OperatorToken(withEquals, text, lineNumber, trivia);
            }
            else
            {
                Text text = GetLastCharacter();
                Trivia trivia = GetTrivia(triviaEndingIndex);
                index++;
                triviaStartingIndex = index;
                return new OperatorToken(withoutEquals, text, lineNumber, trivia);
            }
        }

        private Token LexWithPossibleEquals(SyntaxCharacter withoutEquals, Operator withEquals)
        {
            int triviaEndingIndex = index - 1;
            index++;

            if (code.TryGet(index) == '=')
            {
                index++;

                Text text = GetLastNCharacters(2);
                Trivia trivia = GetTrivia(triviaEndingIndex);
                triviaStartingIndex = index;
                return new OperatorToken(withEquals, text, lineNumber, trivia);
            }
            else
            {
                Text text = GetLastCharacter();
                Trivia trivia = GetTrivia(triviaEndingIndex);
                triviaStartingIndex = index;
                return new SyntaxCharacterToken(withoutEquals, text, lineNumber, trivia);
            }
        }
    }
}
