namespace Krypton.Analysis.Lexical.Lexemes.SyntaxCharacters
{
    public sealed class UnderscoreLexeme : SyntaxCharacterLexeme
    {
        public UnderscoreLexeme(int lineNumber) : base(lineNumber) { }

        public override string Content => "_";
    }
}
