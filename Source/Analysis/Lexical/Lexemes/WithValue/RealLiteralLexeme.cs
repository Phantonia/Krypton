using Krypton.Analysis.Lexical.Lexemes;
using System.Diagnostics;
using System.Globalization;

namespace Krypton.Analysis.Lexical.Lexemes.WithValue
{
    public sealed class RealLiteralLexeme : Lexeme
    {
        public RealLiteralLexeme(string value, int lineNumber) : base(lineNumber)
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
            Debug.Assert(NumberLiteralParser.TryParseReal(value, out _));
            Value = NumberLiteralParser.ParseReal(value);
        }
    }
}
