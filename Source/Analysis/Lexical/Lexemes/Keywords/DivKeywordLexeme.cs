namespace Krypton.Analysis.Lexical.Lexemes.Keywords
{
    public sealed class DivKeywordLexeme : KeywordLexeme
    {
        public DivKeywordLexeme(int lineNumber) : base(lineNumber) { }

        public override string Content => "Div";
    }
}
