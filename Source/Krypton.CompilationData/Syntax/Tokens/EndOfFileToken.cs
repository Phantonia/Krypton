using System;

namespace Krypton.CompilationData.Syntax.Tokens
{
    public sealed class EndOfFileToken : Token
    {
        public EndOfFileToken(int lineNumber,
                              Trivia leadingTrivia) : base(text: ReadOnlyMemory<char>.Empty,
                                                           lineNumber,
                                                           leadingTrivia) { }

        protected override string GetDebuggerDisplay() => nameof(EndOfFileToken);
    }
}
