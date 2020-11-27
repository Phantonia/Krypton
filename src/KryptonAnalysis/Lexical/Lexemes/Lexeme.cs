using System.Diagnostics;
using System.Runtime.Serialization;

namespace Krypton.Analysis.Lexical.Lexemes
{
    [DebuggerDisplay("{DebuggerDisplay()}")]
    public abstract class Lexeme
    {
        protected Lexeme(int lineNumber)
        {
            LineNumber = lineNumber;
        }

        public abstract string Content { get; }

        public int LineNumber { get; private set; }

        protected virtual void Construct() { }

        protected virtual void Construct(string value) { }

        private string DebuggerDisplay() => $"{GetType().Name}: {Content}";

        public static TLexeme New<TLexeme>(int lineNumber)
            where TLexeme : LexemeWithoutValue
        {
            TLexeme lexeme = (TLexeme)FormatterServices.GetUninitializedObject(typeof(TLexeme));
            lexeme.LineNumber = lineNumber;
            lexeme.Construct();
            return lexeme;
        }

        public static TLexeme New<TLexeme>(string value, int lineNumber)
            where TLexeme : LexemeWithValue
        {
            TLexeme lexeme = (TLexeme)FormatterServices.GetUninitializedObject(typeof(TLexeme));
            lexeme.LineNumber = lineNumber;
            lexeme.Construct(value);
            return lexeme;
        }
    }
}
