using System.Diagnostics;

namespace Krypton.Analysis.Lexical.Lexemes.WithValue
{
    public sealed class RationalLiteralLexeme : Lexeme
    {
        public RationalLiteralLexeme(string value, int lineNumber) : base(lineNumber)
        {
            Init(value);
        }

        public override string Content => Value.ToString();

        public double Value { get; private set; }

        protected override void Construct(string value)
        {
            Init(value);
        }

        private void Init(string value)
        {
            Debug.Assert(NumberLiteralParser.TryParseRational(value, out _));
            Value = NumberLiteralParser.ParseRational(value);
        }
    }
}
