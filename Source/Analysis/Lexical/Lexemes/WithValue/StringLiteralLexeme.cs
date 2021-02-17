using System;

namespace Krypton.Analysis.Lexical.Lexemes.WithValue
{
    internal sealed class StringLiteralLexeme : Lexeme
    {
        public StringLiteralLexeme(string value, int lineNumber, int index) : base(lineNumber, index)
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
