namespace Krypton.Analysis.Lexical.Lexemes.WithValue
{
    internal sealed class IdentifierLexeme : Lexeme
    {
        public IdentifierLexeme(string identifier, int lineNumber, int index) : base(lineNumber, index)
        {
            Content = identifier;
        }

        public override string Content { get; }
    }
}
