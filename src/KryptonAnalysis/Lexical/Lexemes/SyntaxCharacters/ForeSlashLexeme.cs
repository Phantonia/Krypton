namespace Krypton.Analysis.Lexical.Lexemes.SyntaxCharacters
{
    public sealed class ForeSlashLexeme : SyntaxCharacterLexeme
    {
        public ForeSlashLexeme(int lineNumber) : base(lineNumber) { }
        public override string Content => "/";
    }
}
