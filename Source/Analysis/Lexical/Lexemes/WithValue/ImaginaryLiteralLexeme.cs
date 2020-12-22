using System.Diagnostics;

namespace Krypton.Analysis.Lexical.Lexemes.WithValue
{
    public sealed class ImaginaryLiteralLexeme : Lexeme
    {
        public ImaginaryLiteralLexeme(string value, int lineNumber) : base(lineNumber)
        {
            Debug.Assert(value[^1] == 'i');
            Value = NumberLiteralParser.ParseRational(value[..^1]);
        }

        public override string Content => $"{Value}i";

        public RationalLiteralValue Value { get; }
    }
}
