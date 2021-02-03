namespace Krypton.Analysis.Lexical.Lexemes
{
    public sealed class EndOfFileLexeme : Lexeme
    {
        public EndOfFileLexeme(int lineNumber, int index) : base(lineNumber, index) { }

        public override string Content => string.Empty;
    }
}
