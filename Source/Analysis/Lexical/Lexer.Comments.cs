using Krypton.Analysis.Lexical.Lexemes;
using Krypton.Framework;
using Krypton.Utilities;

namespace Krypton.Analysis.Lexical
{
    partial class Lexer
    {
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

        private Lexeme? LexGreaterThanOrMultilineComment()
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
                return new CharacterOperatorLexeme(Operator.GreaterThan, lineNumber);
            }
        }
    }
}
