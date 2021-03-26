using System.Diagnostics;
using System.IO;

namespace Krypton.CompilationData.Syntax.Tokens
{
    [DebuggerDisplay("{" + nameof(GetDebuggerDisplay) + "(),nq}")]
    public abstract class Token : IToken
    {
        private protected Token(int lineNumber, Trivia leadingTrivia)
        {
            LineNumber = lineNumber;
            LeadingTrivia = leadingTrivia;
        }

        public Trivia LeadingTrivia { get; }

        public int LineNumber { get; }

        public abstract string Text { get; }

        public void WriteCode(TextWriter textWriter)
        {
            textWriter.Write(Text);
        }

        private string GetDebuggerDisplay() => $"{GetType().Name}: \"{Text}\"";
    }
}
