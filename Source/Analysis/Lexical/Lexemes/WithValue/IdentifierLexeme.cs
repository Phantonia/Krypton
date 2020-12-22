namespace Krypton.Analysis.Lexical.Lexemes.WithValue
{
    public sealed class IdentifierLexeme : Lexeme
    {
        public IdentifierLexeme(string identifier, int lineNumber) : base(lineNumber)
        {
            Content = identifier;
        }

        public override string Content { get; }
    }
}
