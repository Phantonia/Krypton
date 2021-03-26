namespace Krypton.CompilationData.Syntax.Tokens
{
    public sealed class InvalidToken : Token
    {
        public InvalidToken(string text, int lineNumber, Trivia leadingTrivia)
            : base(lineNumber, leadingTrivia)
        {
            Text = text;
        }

        public override string Text { get; }
    }
}
