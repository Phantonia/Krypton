namespace Krypton.Analysis.Lexical.Lexemes.SyntaxCharacters
{
    public sealed class SemicolonLexeme : SyntaxCharacterLexeme
    {
        public SemicolonLexeme(int lineNumber) : base(lineNumber) { }

        public override string Content => ";";
    }
}
