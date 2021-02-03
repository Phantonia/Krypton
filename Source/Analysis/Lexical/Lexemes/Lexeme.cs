using System.Diagnostics;

namespace Krypton.Analysis.Lexical.Lexemes
{
    [DebuggerDisplay("{DebuggerDisplay()}")]
    public abstract class Lexeme : ILexeme
    {
        protected Lexeme(int lineNumber, int index)
        {
            LineNumber = lineNumber;
            Index = index;
        }

        public abstract string Content { get; }

        public int Index { get; }

        public int LineNumber { get; }

        private string DebuggerDisplay() => $"{GetType().Name}: {Content}";
    }
}
