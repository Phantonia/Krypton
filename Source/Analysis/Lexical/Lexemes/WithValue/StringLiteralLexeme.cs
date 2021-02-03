using System;

namespace Krypton.Analysis.Lexical.Lexemes.WithValue
{
    public sealed class StringLiteralLexeme : Lexeme
    {
        public StringLiteralLexeme(string value, int lineNumber) : base(lineNumber)
        {
            Value = value;
        }

        public override string Content => Value;

        public string Value { get; }

        public string Parse()
        {
            throw new NotImplementedException("Notim: Can't parse string literals yet");
        }
    }
}
