namespace Krypton.Analysis.Lexical.Lexemes.SyntaxCharacters
{
    public sealed class DoubleAsteriskLexeme : SyntaxCharacterLexeme
    {
        public DoubleAsteriskLexeme(int lineNumber) : base(lineNumber) { }
        public override string Content => "**";
    }
}
