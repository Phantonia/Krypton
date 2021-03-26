namespace Krypton.CompilationData.Syntax.Tokens
{
    public sealed class EndOfFileToken : Token
    {
        public EndOfFileToken(int lineNumber, Trivia leadingTrivia) : base(lineNumber, leadingTrivia) { }

        public override string Text => string.Empty;
    }
}
