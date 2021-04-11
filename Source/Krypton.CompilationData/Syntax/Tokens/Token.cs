using System;
using System.Diagnostics;
using System.IO;

namespace Krypton.CompilationData.Syntax.Tokens
{
    [DebuggerDisplay("{" + nameof(GetDebuggerDisplay) + "(),nq}")]
    public abstract class Token : IToken, IWritable
    {
        // New members have to be added to the interface as well!

        private protected Token(int lineNumber, Trivia leadingTrivia)
        {
            LineNumber = lineNumber;
            LeadingTrivia = leadingTrivia;
        }

        public Trivia LeadingTrivia { get; }

        public int LineNumber { get; }

        public abstract ReadOnlyMemory<char> Text { get; }

        protected virtual string GetDebuggerDisplay() => $"{GetType().Name}: \"{Text}\"";

        public sealed override string ToString()
            => new(Text.Span);

        public void WriteCode(TextWriter textWriter)
        {
            textWriter.Write(Text);
        }
    }
}
