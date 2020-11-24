namespace Krypton.Analysis.Lexical.Lexemes.SyntaxCharacters
{
    public sealed class DotLexeme : SyntaxCharacterLexeme
    {
        public DotLexeme(int lineNumber) : base(lineNumber) { }
        public override string Content => ".";
    }
}
