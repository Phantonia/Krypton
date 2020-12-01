using System.Diagnostics;

namespace Krypton.Analysis.Lexical.Lexemes.WithValue
{
    public sealed class ImaginaryLiteralLexeme : Lexeme
    {
        public ImaginaryLiteralLexeme(string value, int lineNumber) : base(lineNumber)
        {
            Init(value);
        }

        public override string Content => $"{Value}i";

        public double Value { get; private set; }

        protected override void Construct(string value)
        {
            Init(value);
        }

        private void Init(string value)
        {
            Debug.Assert(value[^1] == 'i');
            Value = NumberLiteralParser.ParseReal(value[..^1]);
        }
    }
}
