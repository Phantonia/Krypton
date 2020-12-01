namespace Krypton.Analysis.Lexical.Lexemes.Keywords
{
    public sealed class NotKeywordLexeme : KeywordLexeme
    {
        public NotKeywordLexeme(int lineNumber) : base(lineNumber) { }

        public override string Content => "Not";
    }
}
