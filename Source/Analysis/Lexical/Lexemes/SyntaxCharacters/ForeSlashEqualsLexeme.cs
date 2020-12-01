namespace Krypton.Analysis.Lexical.Lexemes.SyntaxCharacters
{
    public sealed class ForeSlashEqualsLexeme : SyntaxCharacterLexeme
    {
        public ForeSlashEqualsLexeme(int lineNumber) : base(lineNumber) { }

        public override string Content => "/=";
    }
}
