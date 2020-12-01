namespace Krypton.Analysis.Lexical.Lexemes.SyntaxCharacters
{
    public sealed class DoubleAsteriskEqualsLexeme : SyntaxCharacterLexeme
    {
        public DoubleAsteriskEqualsLexeme(int lineNumber) : base(lineNumber) { }

        public override string Content => "**=";
    }
}
