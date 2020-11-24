namespace Krypton.Analysis.Lexical.Lexemes.SyntaxCharacters
{
    public sealed class ParenthesisOpeningLexeme : SyntaxCharacterLexeme
    {
        public ParenthesisOpeningLexeme(int lineNumber) : base(lineNumber) { }

        public override string Content => "(";
    }
}
