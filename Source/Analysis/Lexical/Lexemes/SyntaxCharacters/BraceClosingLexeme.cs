namespace Krypton.Analysis.Lexical.Lexemes.SyntaxCharacters
{
    public sealed class BraceClosingLexeme : SyntaxCharacterLexeme
    {
        public BraceClosingLexeme(int lineNumber) : base(lineNumber) { }

        public override string Content => "}";
    }
}
