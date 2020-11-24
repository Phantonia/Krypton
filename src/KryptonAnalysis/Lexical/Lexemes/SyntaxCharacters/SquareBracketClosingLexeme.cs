namespace Krypton.Analysis.Lexical.Lexemes.SyntaxCharacters
{
    public sealed class SquareBracketClosingLexeme : SyntaxCharacterLexeme
    {
        public SquareBracketClosingLexeme(int lineNumber) : base(lineNumber) { }

        public override string Content => "]";
    }
}
