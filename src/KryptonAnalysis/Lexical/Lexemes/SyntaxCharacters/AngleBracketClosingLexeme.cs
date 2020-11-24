namespace Krypton.Analysis.Lexical.Lexemes.SyntaxCharacters
{
    public sealed class AngleBracketClosingLexeme : SyntaxCharacterLexeme
    {
        public AngleBracketClosingLexeme(int lineNumber) : base(lineNumber) { }

        public override string Content => ">";
    }
}
