namespace Krypton.Analysis.Lexical.Lexemes.SyntaxCharacters
{
    public sealed class EqualsLexeme : SyntaxCharacterLexeme
    {
        public EqualsLexeme(int lineNumber) : base(lineNumber) { }
        public override string Content => "=";
    }
}
