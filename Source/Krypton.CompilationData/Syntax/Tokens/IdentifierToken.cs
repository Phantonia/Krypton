using System;

namespace Krypton.CompilationData.Syntax.Tokens
{
    public sealed class IdentifierToken : Token
    {
        public IdentifierToken(ReadOnlyMemory<char> identifier, int lineNumber, Trivia leadingTrivia)
            : base(lineNumber, leadingTrivia)
        {
            Text = identifier;
        }

        public override ReadOnlyMemory<char> Text { get; }
    }
}
