using System;

namespace Krypton.CompilationData.Syntax.Tokens
{
    public abstract class LiteralToken : Token
    {
        private protected LiteralToken(ReadOnlyMemory<char> text,
                                       Type associatedType,
                                       int lineNumber,
                                       Trivia leadingTrivia)
            : base(text, lineNumber, leadingTrivia)
        {
            AssociatedType = associatedType;
        }

        public Type AssociatedType { get; }

        public abstract object ObjectValue { get; }
    }

    public sealed class LiteralToken<TLiteral> : LiteralToken
        where TLiteral : notnull
    {
        public LiteralToken(TLiteral value,
                            ReadOnlyMemory<char> text,
                            Type associatedType,
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
