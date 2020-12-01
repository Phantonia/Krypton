namespace Krypton.Analysis.Lexical.Lexemes.SyntaxCharacters
{
    public sealed class ColonLexeme : SyntaxCharacterLexeme
    {
        public ColonLexeme(int lineNumber) : base(lineNumber) { }

        public override string Content => ":";
    }
}
