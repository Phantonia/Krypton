using Krypton.Framework;
using System;

namespace Krypton.CompilationData.Syntax.Tokens
{
    public abstract class LiteralToken : Token
    {
        private protected LiteralToken(ReadOnlyMemory<char> text,
                                       FrameworkType associatedType,
                                       int lineNumber,
                                       Trivia leadingTrivia)
            : base(lineNumber, leadingTrivia)
        {
            Text = text;
            AssociatedType = associatedType;
        }

        public FrameworkType AssociatedType { get; }

        public sealed override ReadOnlyMemory<char> Text { get; }

        public abstract object ObjectValue { get; }
    }

    public sealed class LiteralToken<TLiteral> : LiteralToken
        where TLiteral : notnull
    {
        public LiteralToken(TLiteral value,
                            ReadOnlyMemory<char> text,
                            FrameworkType associatedType,
                            int lineNumber,
                            Trivia leadingTrivia)
            : base(text, associatedType, lineNumber, leadingTrivia)
        {
            LiteralHelper.AssertTypeIsLiteralType<TLiteral>();
            Value = value;
        }

        public override object ObjectValue => Value;

        public TLiteral Value { get; }

        protected override string GetDebuggerDisplay() => $"{base.GetDebuggerDisplay()}; Value = {Value}";
    }
}
