namespace Krypton.Analysis.Lexical.Lexemes.SyntaxCharacters
{
    public sealed class DoubleEqualsLexeme : SyntaxCharacterLexeme
    {
        public DoubleEqualsLexeme(int lineNumber) : base(lineNumber) { }

        public override string Content => "==";
    }
}
