using System.Diagnostics;

namespace Krypton.Analysis.Lexical.Lexemes.WithValue
{
    public sealed class BooleanLiteralLexeme : Lexeme
    {
        public BooleanLiteralLexeme(bool value, int lineNumber) : base(lineNumber)
        {
            Value = value;
        }

        public override string Content => Value ? "True" : "False";

        public bool Value { get; private set; }

        protected override void Construct(string value)
        {
            Debug.Assert(value == "True" | value == "False");
            Value = value == "True";
        }
    }
}
