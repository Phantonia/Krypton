using Krypton.CompilationData;
using Krypton.CompilationData.Syntax.Tokens;

namespace Krypton.Analysis.Lexical
{
    partial class Lexer
    {
        private Token LexDotOrDoubleDotOrSingleLineComment() // .
        {
            code.Read();

            if (code.Peek() == '.')
            {
                code.Read();

                if (code.Peek() == '.')
                {
                    PutIntoTrivia("...");

                    while (code.Peek() is not '\n' and not -1)
                    {
                        PutIntoTrivia((char)code.Read());
                    }

                    if (code.Peek() == -1)
                    {
                        return new EndOfFileToken(lineNumber, GetTrivia());
                    }
                }
                else
                {
                    code.Read();
                    return new OperatorToken(Operator.DoubleDot, lineNumber, GetTrivia());
                }
            }

            return new SyntaxCharacterToken(SyntaxCharacter.Dot, lineNumber, GetTrivia());
        }

        private Token LexGreaterThanOrMultiLineComment() // >
        {
            code.Read();
            switch (code.Peek())
            {
                case '>': // >>
                    code.Read();
                    PutIntoTrivia(">>");

                    if (code.Peek() == '>') // >>>
                    {
                        PutIntoTrivia('>');
                        return LexStrongMultiLineComment();
                    }

                    return LexWeakMultiLineComment();
                case '=':
                    code.Read();
                    return new OperatorToken(Operator.GreaterThanEquals, lineNumber, GetTrivia());
                default:
                    return new OperatorToken(Operator.GreaterThan, lineNumber, GetTrivia());
            }
        }

        private Token LexStrongMultiLineComment()
        {
            int numberOfOpenedComments = 1;

            while (code.Peek() is not -1)
            {
                char nextChar = (char)code.Read();
                PutIntoTrivia(nextChar);

                switch (nextChar)
                {
                    case '\n':
                        lineNumber++;
                        break;
                    case '<': // <
                        if (code.Peek() == '<') // <<
                        {
                            PutIntoTrivia('<');
                            code.Read();

                            if (code.Peek() == '<') // <<<
                            {
                                PutIntoTrivia('<');
                                code.Read();

                                numberOfOpenedComments--;
                            }
                        }
                        break;
                    case '>':
                        if (code.Peek() == '>') // <<
                        {
                            PutIntoTrivia('>');
                            code.Read();

                            if (code.Peek() == '>') // <<<
                            {
                                PutIntoTrivia('>');
                                code.Read();

                                numberOfOpenedComments++;
                            }
                        }
                        break;
                }

                if (numberOfOpenedComments == 0)
                {
                    return NextToken();
                }
            }

            return new EndOfFileToken(lineNumber, GetTrivia());
        }

        private Token LexWeakMultiLineComment()
        {
            while (code.Peek() is not -1)
            {
                char nextChar = (char)code.Read();
                PutIntoTrivia(nextChar);

                switch (nextChar)
                {
                    case '\n':
                        lineNumber++;
                        break;
                    case '<':
                        if (code.Peek() == '<') // <<
                        {
                            PutIntoTrivia('<');
                            code.Read();
                            return NextToken();
                        }
                        break;
                }
            }

            return new EndOfFileToken(lineNumber, GetTrivia());
        }
    }
}
