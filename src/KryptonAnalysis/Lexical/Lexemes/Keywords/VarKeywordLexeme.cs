namespace Krypton.Analysis.Lexical.Lexemes.Keywords
{
    public sealed class VarKeywordLexeme : KeywordLexeme
    {
        public VarKeywordLexeme(int lineNumber) : base(lineNumber) { }

        public override string Content => "Var";
    }

    public sealed class OutKeywordLexeme : KeywordLexeme
    {
        public OutKeywordLexeme(int lineNumber) : base(lineNumber) { }

        public override string Content => "Out";
    }
}
