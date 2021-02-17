using System.Diagnostics;

namespace Krypton.Analysis.Lexical.Lexemes
{
    [DebuggerDisplay("{" + nameof(GetDebuggerDisplay) + "(),nq}")]
    internal abstract class Lexeme : ILexeme
    {
        protected Lexeme(int lineNumber, int index)
        {
            LineNumber = lineNumber;
            Index = index;
        }

        public abstract string Content { get; }

        public int Index { get; }

        public int LineNumber { get; }

        private string GetDebuggerDisplay()
        {
            return $"{GetType().Name}: {Content}";
        }
    }
}
