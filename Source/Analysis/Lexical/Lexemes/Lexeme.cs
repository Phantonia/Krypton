using System.Diagnostics;

namespace Krypton.Analysis.Lexical.Lexemes
{
    [DebuggerDisplay("{DebuggerDisplay()}")]
    public abstract class Lexeme : ILexeme
    {
        protected Lexeme(int lineNumber)
        {
            LineNumber = lineNumber;
        }

        public abstract string Content { get; }

        public int LineNumber { get; private set; }

        private string DebuggerDisplay() => $"{GetType().Name}: {Content}";
    }
}
