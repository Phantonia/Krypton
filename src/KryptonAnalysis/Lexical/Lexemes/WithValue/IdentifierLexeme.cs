namespace Krypton.Analysis.Lexical.Lexemes.WithValue
{
    public sealed class IdentifierLexeme : Lexeme
    {
        public IdentifierLexeme(string identifier, int lineNumber) : base(lineNumber)
        {
            content = identifier;
        }

        private string content;

        public override string Content => content;

        protected override void Construct(string identifier)
        {
            content = identifier;
        }
    }
}
