using Krypton.CompilationData.Syntax.Tokens;
using System;
using System.IO;

namespace Krypton.CompilationData.Syntax.Expressions
{
    public abstract record LiteralExpressionNode : ExpressionNode
    {
        private protected LiteralExpressionNode() { }

        public Type AssociatedType => NonGenericLiteralToken.AssociatedType;

        public abstract LiteralToken NonGenericLiteralToken { get; }

        public object ObjectValue => NonGenericLiteralToken.ObjectValue;
    }

    public sealed record LiteralExpressionNode<TLiteral> : LiteralExpressionNode
        where TLiteral : notnull
    {
        public LiteralExpressionNode(LiteralToken<TLiteral> literalToken)
        {
            LiteralHelper.AssertTypeIsLiteralType<TLiteral>();
            LiteralToken = literalToken;
        }

        public override bool IsLeaf => true;

        public LiteralToken<TLiteral> LiteralToken { get; init; }

        public override LiteralToken NonGenericLiteralToken => LiteralToken;

        public TLiteral Value => LiteralToken.Value;

        protected override string GetDebuggerDisplay() => $"{base.GetDebuggerDisplay()}; Value = {LiteralToken.Value}";

        public override void WriteCode(TextWriter output)
        {
            LiteralToken.WriteCode(output);
        }
    }
}
