using System;

namespace Krypton.CompilationData.Syntax.Tokens
{
    public sealed class LiteralToken<TLiteral> : Token
    {
        public LiteralToken(TLiteral value, ReadOnlyMemory<char> text, int lineNumber, Trivia leadingTrivia)
            : base(lineNumber, leadingTrivia)
        {
            LiteralHelper.AssertTypeIsLiteralType<TLiteral>();
            Value = value;
            Text = text;
        }

        public override ReadOnlyMemory<char> Text { get; }

        public TLiteral Value { get; }

        protected override string GetDebuggerDisplay() => $"{base.GetDebuggerDisplay()}; Value = {Value}";
    }
}
