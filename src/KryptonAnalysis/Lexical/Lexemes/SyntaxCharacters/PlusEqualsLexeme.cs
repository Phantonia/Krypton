namespace Krypton.Analysis.Lexical.Lexemes.SyntaxCharacters
{

    public sealed class PlusEqualsLexeme : SyntaxCharacterLexeme
    {
        public PlusEqualsLexeme(int lineNumber) : base(lineNumber) { }

        public override string Content => "+=";
    }
}
