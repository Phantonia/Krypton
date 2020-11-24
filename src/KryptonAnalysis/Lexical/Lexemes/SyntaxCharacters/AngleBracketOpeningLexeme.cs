namespace Krypton.Analysis.Lexical.Lexemes.SyntaxCharacters
{
    public sealed class AngleBracketOpeningLexeme : SyntaxCharacterLexeme
    {
        public AngleBracketOpeningLexeme(int lineNumber) : base(lineNumber) { }

        public override string Content => "<";
    }
}
