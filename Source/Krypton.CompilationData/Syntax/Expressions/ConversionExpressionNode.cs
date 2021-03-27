using Krypton.CompilationData.Symbols;
using Krypton.CompilationData.Syntax.Tokens;
using Krypton.CompilationData.Syntax.Types;
using System.Diagnostics;
using System.IO;

namespace Krypton.CompilationData.Syntax.Expressions
{
    public sealed class ConversionExpressionNode : ExpressionNode
    {
        public ConversionExpressionNode(ExpressionNode operand,
                                        ReservedKeywordToken operatorKeyword,
                                        TypeNode type,
                                        SyntaxNode? parent = null)
            : base(parent)
        {
            Debug.Assert(operatorKeyword.Keyword is ReservedKeyword.As or ReservedKeyword.To);

            OperandNode = operand.WithParent(this);
            OperatorKeywordToken = operatorKeyword;
            TypeNode = type.WithParent(this);
        }

        public override bool IsLeaf => false;

        public ExpressionNode OperandNode { get; }

        public ReservedKeywordToken OperatorKeywordToken { get; }

        public TypeNode TypeNode { get; }

        public override TypedExpressionNode<ConversionExpressionNode> Type(TypeSymbol type)
            => new(this, type);

        public ConversionExpressionNode WithChildren(ExpressionNode? operand = null,
                                                     ReservedKeywordToken? operatorKeyword = null,
                                                     TypeNode? type = null)
            => new(operand ?? OperandNode,
                   operatorKeyword ?? OperatorKeywordToken,
                   type ?? TypeNode);

        public override ConversionExpressionNode WithParent(SyntaxNode newParent)
            => new(OperandNode, OperatorKeywordToken, TypeNode, newParent);

        public override void WriteCode(TextWriter output)
        {
            OperandNode.WriteCode(output);
            OperatorKeywordToken.WriteCode(output);
            TypeNode.WriteCode(output);
        }
    }
}
