using System;

namespace Krypton.CompilationData.Syntax.Tokens
{
    public sealed class EndOfFileToken : Token
    {
        public EndOfFileToken(int lineNumber, Trivia leadingTrivia) : base(lineNumber, leadingTrivia) { }

        public override ReadOnlyMemory<char> Text => string.Empty;

        protected override string GetDebuggerDisplay() => nameof(EndOfFileToken);
    }
}
