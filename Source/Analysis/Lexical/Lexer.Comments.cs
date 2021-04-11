using Krypton.CompilationData.Syntax;
using Krypton.CompilationData.Syntax.Tokens;
using Krypton.Utilities;

namespace Krypton.Analysis.Lexical
{
    partial class Lexer
    {
        private Token? LexDotOrSingleLineComment()
        {
            int triviaEndingIndex = index - 1;
            index++;

            if (code.TryGet(index) == '.' && code.TryGet(index + 1) == '.')
            {
                index += 2;

                for (; index < code.Length; index++)
                {
                    if (code[index] == '\n')
                    {
                        lineNumber++;
                        index++;
                        return NextToken();
                    }
                }

                return null;
            }
            else
            {
                Trivia trivia = GetTrivia(triviaEndingIndex);
                triviaStartingIndex = index;
                return new SyntaxCharacterToken(SyntaxCharacter.Dot, lineNumber, trivia);
            }
        }

        private Token? LexGreaterThanOrMultilineComment()
        {
            int triviaEndingIndex = index - 1;
            int lexemeIndex = index;
            index++;

            if (code.TryGet(index) == '>') // >>
            {
                index++;

                if (code.TryGet(index) == '>') // >>>
                {
                    int openedComments = 0;

                    for (; index < code.Length; index++)
                    {
                        if (code[index - 2] == '<'
                            && code[index - 1] == '<'
                            && code[index] == '<') // <<<
                        {
                            openedComments--;

                            if (openedComments == 0)
                            {
                                index++;
                                return NextToken();
                            }
                        }
                        else if (code[index - 2] == '>'
                            && code[index - 1] == '>'
                            && code[index] == '>') // >>>
                        {
                            openedComments++;
                        }
                        else if (code[index] == '\n')
                        {
                            lineNumber++;
                        }
                    }

                    return null;
                }
                else
                {
                    for (; index < code.Length; index++)
                    {
                        if (code[index - 2] != '<'
                            && code[index - 1] == '<'
                            && code[index] == '<'
                            && code.TryGet(index + 1) != '<') // <<
                        {
                            index++;
                            return NextToken();
                        }
                        else if (code[index] == '\n')
                        {
                            lineNumber++;
                        }
                    }

                    return null;
                }
            }
            else
            {
                Trivia trivia = GetTrivia(triviaEndingIndex);
                triviaStartingIndex = index;
                return new OperatorToken(CompilationData.Operator.GreaterThan, lineNumber, trivia);
            }
        }
    }
}
