namespace Krypton.Analysis.Lexical.Lexemes.Keywords
{
    public sealed class WhileKeywordLexeme : KeywordLexeme
    {
        public WhileKeywordLexeme(int lineNumber) : base(lineNumber) { }

        public override string Content => "While";
    }
}
