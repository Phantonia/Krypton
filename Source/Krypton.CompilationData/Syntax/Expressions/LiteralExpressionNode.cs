using Krypton.CompilationData.Symbols;
using Krypton.CompilationData.Syntax.Tokens;
using System.IO;

namespace Krypton.CompilationData.Syntax.Expressions
{
    public sealed record LiteralExpressionNode<TLiteral> : ExpressionNode
    {
        public LiteralExpressionNode(LiteralToken<TLiteral> literalToken)
        {
            LiteralHelper.AssertTypeIsLiteralType<TLiteral>();
            LiteralToken = literalToken;
        }

        public override bool IsLeaf => true;

        public LiteralToken<TLiteral> LiteralToken { get; init; }

        protected override string GetDebuggerDisplay() => $"{base.GetDebuggerDisplay()}; Value = {LiteralToken.Value}";

        public override TypedExpressionNode Type(TypeSymbol type)
            => new(this, type);

        public override void WriteCode(TextWriter output)
        {
            LiteralToken.WriteCode(output);
        }
    }
}
