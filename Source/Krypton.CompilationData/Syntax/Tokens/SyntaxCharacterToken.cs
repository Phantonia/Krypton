using System;

namespace Krypton.CompilationData.Syntax.Tokens
{
    public sealed class SyntaxCharacterToken : Token
    {
        public SyntaxCharacterToken(SyntaxCharacter syntaxCharacter,
                                    ReadOnlyMemory<char> text,
                                    int lineNumber,
                                    Trivia leadingTrivia)
            : base(text, lineNumber, leadingTrivia)
        {
            SyntaxCharacter = syntaxCharacter;
        }

        public SyntaxCharacter SyntaxCharacter { get; }
    }
}
