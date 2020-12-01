namespace Krypton.Analysis.Lexical.Lexemes.Keywords
{
    public sealed class AsKeywordLexeme : KeywordLexeme
    {
        public AsKeywordLexeme(int lineNumber) : base(lineNumber) { }

        public override string Content => "As";
    }
}
