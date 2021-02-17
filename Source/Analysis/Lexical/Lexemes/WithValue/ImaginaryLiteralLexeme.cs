using Krypton.Framework.Literals;
using System.Diagnostics;

namespace Krypton.Analysis.Lexical.Lexemes.WithValue
{
    internal sealed class ImaginaryLiteralLexeme : Lexeme
    {
        public ImaginaryLiteralLexeme(string value, int lineNumber, int index) : base(lineNumber, index)
        {
            Debug.Assert(value[^1] == 'i');
            value = value[..^1];

            if (NumberLiteralParser.TryParseRational(value, out Rational rational))
            {
                Value = rational;
                return;
            }

            Debug.Assert(NumberLiteralParser.TryParseDecimal(value, out _));
            long integer = NumberLiteralParser.ParseDecimal(value);
            Value = new Rational(integer, 1);
        }

        public override string Content => $"{Value}i";

        public Rational Value { get; }
    }
}
