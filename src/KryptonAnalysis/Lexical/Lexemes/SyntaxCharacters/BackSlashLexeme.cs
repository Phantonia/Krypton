namespace Krypton.Analysis.Lexical.Lexemes.SyntaxCharacters
{
    public sealed class BackSlashLexeme : SyntaxCharacterLexeme
    {
        public BackSlashLexeme(int lineNumber) : base(lineNumber) { }

        public override string Content => "\\";
    }
}
