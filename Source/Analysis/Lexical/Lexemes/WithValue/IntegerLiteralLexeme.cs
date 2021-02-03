using System.Diagnostics;

namespace Krypton.Analysis.Lexical.Lexemes.WithValue
{
    public sealed class IntegerLiteralLexeme : Lexeme
    {
        public IntegerLiteralLexeme(string value, IntegerStyle style, int lineNumber, int index) : base(lineNumber, index)
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

        public IntegerLiteralLexeme(int value, int lineNumber, int index) : base(lineNumber, index)
        {
            Value = value;
        }

        public override string Content => Value.ToString();

        public long Value { get; }
    }
}
