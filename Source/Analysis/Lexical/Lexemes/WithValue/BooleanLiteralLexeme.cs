namespace Krypton.Analysis.Lexical.Lexemes.WithValue
{
    internal sealed class BooleanLiteralLexeme : Lexeme
    {
        public BooleanLiteralLexeme(bool value, int lineNumber, int index) : base(lineNumber, index)
        {
            Value = value;
        }

        public override string Content => Value ? "True" : "False";

        public bool Value { get; }
    }
}
