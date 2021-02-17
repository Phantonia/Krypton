using Krypton.Framework.Literals;
using System.Diagnostics;

namespace Krypton.Analysis.Lexical.Lexemes.WithValue
{
    internal sealed class RationalLiteralLexeme : Lexeme
    {
        public RationalLiteralLexeme(string value, int lineNumber, int index) : base(lineNumber, index)
        {
            Debug.Assert(NumberLiteralParser.TryParseRational(value, out _));
            Value = NumberLiteralParser.ParseRational(value);
        }

        public override string Content => Value.ToString();

        public Rational Value { get; }
    }
}
