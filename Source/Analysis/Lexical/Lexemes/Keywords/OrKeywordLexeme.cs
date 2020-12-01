namespace Krypton.Analysis.Lexical.Lexemes.Keywords
{
    public sealed class OrKeywordLexeme : KeywordLexeme
    {
        public OrKeywordLexeme(int lineNumber) : base(lineNumber) { }

        public override string Content => "Or";
    }
}
