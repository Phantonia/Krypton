using Krypton.CompilationData.Symbols;
using System.IO;

namespace Krypton.CompilationData.Syntax.Expressions
{
    public record TypedExpressionNode : ExpressionNode
    {
        internal TypedExpressionNode(ExpressionNode expression, TypeSymbol type)
        {
            ExpressionNode = expression;
            typeSymbol = type;
        }

        internal TypedExpressionNode(ExpressionNode expression, ImplicitConversionSymbol conversion)
        {
            ExpressionNode = expression;
            ImplicitConversionSymbol = conversion;
            typeSymbol = conversion.TargetTypeSymbol;
        }

        private readonly TypeSymbol typeSymbol;

        public ExpressionNode ExpressionNode { get; init; }

        public ImplicitConversionSymbol? ImplicitConversionSymbol { get; init; }

        public override bool IsLeaf => false;

        public virtual TypeSymbol TypeSymbol
        {
            get => ImplicitConversionSymbol?.TargetTypeSymbol ?? typeSymbol;
            init
            {
                ImplicitConversionSymbol = null;
                typeSymbol = value;
            }
        }

        public override void WriteCode(TextWriter output)
            => ExpressionNode.WriteCode(output);
    }
}
