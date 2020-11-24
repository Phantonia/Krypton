namespace Krypton.Analysis.Lexical.Lexemes
{
    public sealed class EndOfFileLexeme : Lexeme
    {
        public EndOfFileLexeme(int lineNumber) : base(lineNumber) { }

        public override string Content => string.Empty;
    }
}
