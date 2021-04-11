using Krypton.CompilationData;
using Krypton.CompilationData.Syntax.Tokens;
using System;

namespace Krypton.Analysis.Lexical
{
    partial class Lexer
    {
        private Token LexAsteriskOrDoubleAsteriskOperatorOrCompoundAssignment() // *
        {
            code.Read();

            switch (code.Peek())
            {
                case '=': // *
                    code.Read();
                    return new CompoundAssignmentToken(Operator.Asterisk, lineNumber, GetTrivia());
                case '*': // **
                    code.Read();

                    if (code.Peek() == '=') // **=
                    {
                        code.Read();
                        return new CompoundAssignmentToken(Operator.DoubleAsterisk, lineNumber, GetTrivia());
                    }

                    return new OperatorToken(Operator.DoubleAsterisk, lineNumber, GetTrivia());
                default: // *
                    return new OperatorToken(Operator.Asterisk, lineNumber, GetTrivia());
            }
        }

        private Token LexExclamationMark() // !
        {
            code.Read();

            if (code.Peek() == '=')
            {
                code.Read();
                return new OperatorToken(Operator.ExclamationEquals, lineNumber, GetTrivia());
            }

            return new InvalidToken("!", DiagnosticsCode.UnknownToken, lineNumber, GetTrivia());
        }

        private Token LexLessThanOrLeftShift() // <
        {
            code.Read();

            switch (code.Peek())
            {
                case '-': // <-
                    code.Read();
                    return new OperatorToken(Operator.SingleLeftArrow, lineNumber, GetTrivia());
                case '=': // <=
                    code.Read();
                    return new OperatorToken(Operator.LessThanEquals, lineNumber, GetTrivia());
                default:
                    return new OperatorToken(Operator.LessThan, lineNumber, GetTrivia());
            }
        }

        private Token LexMinusOrRightShift() // -
        {
            code.Read();

            switch (code.Peek())
            {
                case '>': // ->
                    code.Read();
                    return new OperatorToken(Operator.SingleRightArrow, lineNumber, GetTrivia());
                case '=': // -=
                    code.Read();
                    return new CompoundAssignmentToken(Operator.Minus, lineNumber, GetTrivia());
                default:
                    return new OperatorToken(Operator.Minus, lineNumber, GetTrivia());
            }
        }

        private Token LexSpecificToken(SyntaxCharacter syntaxCharacter)
        {
            code.Read();
            return new SyntaxCharacterToken(syntaxCharacter, lineNumber, GetTrivia());
        }

        private Token LexSpecificToken(Operator @operator)
        {
            code.Read();
            return new OperatorToken(@operator, lineNumber, GetTrivia());
        }

        private Token LexWithPossibleEquals(Func<Token> withoutEquals, Func<Token> withEquals)
        {
            code.Read();

            if (code.Peek() == '=')
            {
                code.Read();

                return withoutEquals();
            }

            return withEquals();
        }

        private Token LexWithPossibleEquals(Operator @operator)
            => LexWithPossibleEquals(() => new OperatorToken(@operator, lineNumber, GetTrivia()),
                                     () => new CompoundAssignmentToken(@operator, lineNumber, GetTrivia()));
    }
}
