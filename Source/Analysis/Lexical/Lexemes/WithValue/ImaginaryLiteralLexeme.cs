using System.Diagnostics;

namespace Krypton.Analysis.Lexical.Lexemes.WithValue
{
    public sealed class ImaginaryLiteralLexeme : Lexeme
    {
        public ImaginaryLiteralLexeme(string value, int lineNumber) : base(lineNumber)
        {
            Debug.Assert(value[^1] == 'i');
            value = value[..^1];

            if (NumberLiteralParser.TryParseRational(value, out RationalLiteralValue rational))
            {
                Value = rational;
                return;
            }

            long integer = NumberLiteralParser.ParseDecimal(value);
            Value = new RationalLiteralValue(integer, 0);
        }

        public override string Content => $"{Value}i";

        public RationalLiteralValue Value { get; }
    }
}
