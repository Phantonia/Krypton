using System;
using System.Diagnostics;
using System.IO;

namespace Krypton.CompilationData.Syntax.Tokens
{
    [DebuggerDisplay("{" + nameof(GetDebuggerDisplay) + "(),nq}")]
    public abstract class Token : IToken, IWritable
    {
        // New members have to be added to the interface as well!

        private protected Token(ReadOnlyMemory<char> text, int lineNumber, Trivia leadingTrivia)
        {
            Text = text;
            LineNumber = lineNumber;
            LeadingTrivia = leadingTrivia;
        }

        public Trivia LeadingTrivia { get; }

        public int LineNumber { get; }

        public ReadOnlyMemory<char> Text { get; }

        protected virtual string GetDebuggerDisplay() => $"{GetType().Name}: \"{Text}\"";

        public string TextToString()
            => new(Text.Span);

        public sealed override string ToString()
            => TextToString();

        public void WriteCode(TextWriter textWriter)
        {
            textWriter.Write(Text);
        }
    }
}
