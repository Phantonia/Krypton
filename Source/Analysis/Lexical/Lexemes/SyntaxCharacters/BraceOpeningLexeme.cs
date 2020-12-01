namespace Krypton.Analysis.Lexical.Lexemes.SyntaxCharacters
{
    public sealed class BraceOpeningLexeme : SyntaxCharacterLexeme
    {
        public BraceOpeningLexeme(int lineNumber) : base(lineNumber) { }

        public override string Content => "{";
    }
}
