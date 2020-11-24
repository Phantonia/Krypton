namespace Krypton.Analysis.Lexical.Lexemes.SyntaxCharacters
{
    public sealed class SquareBracketOpeningLexeme : SyntaxCharacterLexeme
    {
        public SquareBracketOpeningLexeme(int lineNumber) : base(lineNumber) { }

        public override string Content => "[";
    }
}
