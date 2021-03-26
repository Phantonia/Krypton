namespace Krypton.CompilationData.Syntax.Tokens
{
    public sealed class IdentifierToken : Token
    {
        public IdentifierToken(string identifier, int lineNumber, Trivia leadingTrivia)
            : base(lineNumber, leadingTrivia)
        {
            Text = identifier;
        }

        public override string Text { get; }
    }
}
