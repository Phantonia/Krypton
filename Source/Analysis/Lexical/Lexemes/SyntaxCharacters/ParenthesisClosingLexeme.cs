namespace Krypton.Analysis.Lexical.Lexemes.SyntaxCharacters
{
    public sealed class ParenthesisClosingLexeme : SyntaxCharacterLexeme
    {
        public ParenthesisClosingLexeme(int lineNumber) : base(lineNumber) { }

        public override string Content => ")";
    }
}
