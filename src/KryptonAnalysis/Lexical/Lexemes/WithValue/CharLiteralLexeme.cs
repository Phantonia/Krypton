using Krypton.Analysis.Errors;
using System.Diagnostics;

namespace Krypton.Analysis.Lexical.Lexemes.WithValue
{
    public sealed class CharLiteralLexeme : LexemeWithValue
    {
        private CharLiteralLexeme(char value, int lineNumber) : base(lineNumber)
        {
            Content = value.ToString();
            Value = value;
        }

        public override string Content { get; }

        public char Value { get; private set; }

        protected override void Construct(string value)
        {
            // Not supported
            Debug.Assert(false);
        }

        public static Lexeme Create(string value, int lineNumber)
        {
            if (EscapeSequences.TryParse(value, out char result))
            {
                return new CharLiteralLexeme(result, lineNumber);
            }
            else
            {
                return new InvalidLexeme(value, ErrorCode.UnknownEscapeSequence, lineNumber);
            }
        }

        
    }
}
