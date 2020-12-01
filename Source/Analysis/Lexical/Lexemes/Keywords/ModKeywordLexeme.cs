namespace Krypton.Analysis.Lexical.Lexemes.Keywords
{
    public sealed class ModKeywordLexeme : KeywordLexeme
    {
        public ModKeywordLexeme(int lineNumber) : base(lineNumber) { }

        public override string Content => "Mod";
    }
}
