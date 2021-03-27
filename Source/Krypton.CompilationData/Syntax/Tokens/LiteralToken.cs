namespace Krypton.CompilationData.Syntax.Tokens
{
    public sealed class LiteralToken<TLiteral> : Token
    {
        public LiteralToken(TLiteral value, int lineNumber, Trivia leadingTrivia)
            : base(lineNumber, leadingTrivia)
        {
            LiteralHelper.AssertTypeIsLiteralType<TLiteral>();
            Value = value;
        }

        public override string Text => LiteralHelper.LiteralToText(Value);

        public TLiteral Value { get; }

        protected override string GetDebuggerDisplay() => $"{base.GetDebuggerDisplay()}; Value = {Value}";
    }
}
