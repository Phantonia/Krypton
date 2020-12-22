using Krypton.Analysis.Errors;

namespace Krypton.Analysis.Lexical.Lexemes.WithValue
{
    public sealed class CharLiteralLexeme : Lexeme
    {
        private CharLiteralLexeme(char value, int lineNumber) : base(lineNumber)
        {
            Content = value.ToString();
            Value = value;
        }

        public override string Content { get; }

        public char Value { get; private set; }

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
