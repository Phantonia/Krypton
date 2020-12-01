using System.Diagnostics;

namespace Krypton.Analysis.Lexical.Lexemes.WithValue
{
    public sealed class IntegerLiteralLexeme : Lexeme
    {
        public IntegerLiteralLexeme(string value, IntegerStyle style, int lineNumber) : base(lineNumber)
        {
            Init(value, style);
        }

        public IntegerLiteralLexeme(int value, int lineNumber) : base(lineNumber)
        {
            Value = value;
        }

        public override string Content => Value.ToString();

        public long Value { get; private set; }

        protected override void Construct(string value)
        {
            Init(value, IntegerStyle.None);
        }

        private void Init(string value, IntegerStyle style)
        {
            switch (style)
            {
                case IntegerStyle.Base10:
                    Debug.Assert(NumberLiteralParser.TryParseDecimal(value, out _));
                    Value = NumberLiteralParser.ParseDecimal(value);
                    break;
                case IntegerStyle.Base16:
                    Debug.Assert(NumberLiteralParser.TryParseHexadecimal(value, out _));
                    Value = NumberLiteralParser.ParseHexadecimal(value);
                    break;
                case IntegerStyle.Base2:
                    Debug.Assert(NumberLiteralParser.TryParseBinary(value, out _));
                    Value = NumberLiteralParser.ParseBinary(value);
                    break;
            }
        }
    }
}
