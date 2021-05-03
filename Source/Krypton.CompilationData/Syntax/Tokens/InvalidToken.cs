using System;

namespace Krypton.CompilationData.Syntax.Tokens
{
    public sealed class InvalidToken : Token
    {
        public InvalidToken(ReadOnlyMemory<char> text,
                            DiagnosticsCode diagnosticsCode,
                            int lineNumber,
                            Trivia leadingTrivia)
            : base(lineNumber, leadingTrivia)
        {
            Text = text;
            DiagnosticsCode = diagnosticsCode;
        }

        public DiagnosticsCode DiagnosticsCode { get; }

        public override ReadOnlyMemory<char> Text { get; }
    }
}
