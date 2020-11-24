using Krypton.Analysis.Lexical.Lexemes;
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

        public string Value { get; private set; }

        public string Parse()
        {
            throw new NotImplementedException();
        }

        protected override void Construct(string value)
        {
            Value = value;
        }
    }
}
