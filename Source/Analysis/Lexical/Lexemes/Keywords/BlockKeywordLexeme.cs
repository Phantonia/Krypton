namespace Krypton.Analysis.Lexical.Lexemes.Keywords
{
    public sealed class BlockKeywordLexeme : KeywordLexeme
    {
        public BlockKeywordLexeme(int lineNumber) : base(lineNumber) { }

        public override string Content => "Block";
    }
}
