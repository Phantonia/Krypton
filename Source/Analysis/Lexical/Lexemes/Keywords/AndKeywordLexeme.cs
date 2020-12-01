namespace Krypton.Analysis.Lexical.Lexemes.Keywords
{
    public sealed class AndKeywordLexeme : KeywordLexeme
    {
        public AndKeywordLexeme(int lineNumber) : base(lineNumber) { }

        public override string Content => "And";
    }
}
