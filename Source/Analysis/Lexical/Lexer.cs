using Krypton.CompilationData;
using Krypton.CompilationData.Syntax;
using Krypton.CompilationData.Syntax.Tokens;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;

namespace Krypton.Analysis.Lexical
{
    internal sealed partial class Lexer
    {
        private const int EndOfCode = -1;

        public Lexer(TextReader code, Analyser analyser)
        {
            this.code = code;
            this.analyser = analyser;
        }

        private readonly Analyser analyser;
        private readonly TextReader code;
        private int lineNumber = 1;
        private List<char> triviaCharacters = new();

        public Collection<Token>? LexAll()
        {
            Collection<Token> collection = new();

            Token nextToken = NextToken();

            while (true)
            {
                if (nextToken is InvalidToken invalidToken)
                {
                    analyser.ReportDiagnostic(new Diagnostic(invalidToken.DiagnosticsCode, IsError: true, invalidToken));
                    return null;
                }

                collection.Add(nextToken);

                if (nextToken is EndOfFileToken)
                {
                    break;
                }

                nextToken = NextToken();
            }

            return collection;
        }

        public Token NextToken()
        {
            return code.Peek() switch
            {
                EndOfCode => new EndOfFileToken(lineNumber, GetTrivia()),

                ';' => LexSpecificToken(SyntaxCharacter.Semicolon),
                ',' => LexSpecificToken(SyntaxCharacter.Comma),
                ':' => LexSpecificToken(SyntaxCharacter.Colon),
                '.' => LexDotOrDoubleDotOrSingleLineComment(),
                ')' => LexSpecificToken(SyntaxCharacter.ParenthesisClosing),
                '[' => LexSpecificToken(SyntaxCharacter.SquareBracketOpening),
                ']' => LexSpecificToken(SyntaxCharacter.SquareBracketClosing),
                '{' => LexSpecificToken(SyntaxCharacter.BraceOpening),
                '}' => LexSpecificToken(SyntaxCharacter.BraceClosing),
                '<' => LexLessThanOrLeftShift(),
                '>' => LexGreaterThanOrMultiLineComment(),
                '=' => LexWithPossibleEquals(() => new SyntaxCharacterToken(SyntaxCharacter.Equals, lineNumber, GetTrivia()),
                                             () => new OperatorToken(Operator.DoubleEquals, lineNumber, GetTrivia())),
                
                '+' => LexWithPossibleEquals(Operator.Plus),
                '-' => LexMinusOrRightShift(),
                '*' => LexAsteriskOrDoubleAsteriskOperatorOrCompoundAssignment(),
                '/' => LexWithPossibleEquals(Operator.ForeSlash),
                '&' => LexWithPossibleEquals(Operator.Ampersand),
                '|' => LexWithPossibleEquals(Operator.Pipe),
                '^' => LexWithPossibleEquals(Operator.Caret),
                '~' => LexSpecificToken(Operator.Tilde),
                '!' => LexExclamationMark(),

                '"' => LexStringLiteralToken(),
                '\'' => LexCharLiteralToken(),

                _ => LexOther(),
            };
        }

        private Trivia GetTrivia()
        {
            char[] characters = triviaCharacters.ToArray();
            ReadOnlyMemory<char> memory = new(characters);
            triviaCharacters.Clear();
            return new Trivia(memory);
        }

        private Token LexOther()
        {
            char currentChar = (char)code.Read();

            if (char.IsWhiteSpace(currentChar))
            {
                PutIntoTrivia(currentChar);
                
                while (code.Peek() != EndOfCode && char.IsWhiteSpace((char)code.Peek()))
                {
                    PutIntoTrivia((char)code.Read());
                }

                return NextToken();
            }

            if (char.IsNumber(currentChar))
            {

            }
        }

        private void PutIntoTrivia(char c)
        {
            triviaCharacters.Add(c);
        }

        private void PutIntoTrivia(string str)
        {
            foreach (char c in str)
            {
                PutIntoTrivia(c);
            }
        }
    }
}
