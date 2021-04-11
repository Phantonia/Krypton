namespace Krypton.CompilationData.Syntax.Tokens
{
    public sealed class LiteralToken<TLiteral> : Token
    {
        public LiteralToken(TLiteral value,
                            string text,
                            int lineNumber,
                            Trivia leadingTrivia)
            : base(lineNumber, leadingTrivia)
        {
            LiteralHelper.AssertTypeIsLiteralType<TLiteral>();
            Text = text;
            Value = value;
        }

        public override string Text { get; }

        public TLiteral Value { get; }

        protected override string GetDebuggerDisplay() => $"{base.GetDebuggerDisplay()}; Value = {Value}";
    }
}
