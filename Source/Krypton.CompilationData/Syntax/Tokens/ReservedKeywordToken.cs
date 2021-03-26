namespace Krypton.CompilationData.Syntax.Tokens
{
    public sealed class ReservedKeywordToken : Token
    {
        public ReservedKeywordToken(ReservedKeyword keyword, int lineNumber, Trivia leadingTrivia)
            : base(lineNumber, leadingTrivia)
        {
            Keyword = keyword;
        }

        public ReservedKeyword Keyword { get; }

        public override string Text => Keyword.ToString();
    }
}
