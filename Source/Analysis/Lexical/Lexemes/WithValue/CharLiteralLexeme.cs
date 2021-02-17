using Krypton.Analysis.Errors;

namespace Krypton.Analysis.Lexical.Lexemes.WithValue
{
    internal sealed class CharLiteralLexeme : Lexeme
    {
        private CharLiteralLexeme(char value, int lineNumber, int index) : base(lineNumber, index)
        {
            Content = value.ToString();
            Value = value;
        }

        public override string Content { get; }

        public char Value { get; private set; }

        public static Lexeme Create(string value, int lineNumber, int index)
        {
            if (EscapeSequences.TryParse(value, out char result))
            {
                return new CharLiteralLexeme(result, lineNumber, index);
            }
            else
            {
                return new InvalidLexeme(value, ErrorCode.EscapeSequenceError, lineNumber, index);
            }
        }
    }
}
