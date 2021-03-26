using Krypton.CompilationData.Syntax.Tokens;
using Krypton.CompilationData.Syntax.TypeSpecs;
using System.IO;

namespace Krypton.CompilationData.Syntax.Expressions
{
    public sealed class ConversionExpressionNode : ExpressionNode
    {
        public ConversionExpressionNode(ExpressionNode operandNode,
                                        ReservedKeywordToken operatorKeywordToken,
                                        TypeSpecNode typeNode)
            : this(operandNode, operatorKeywordToken, typeNode, parent: null) { }

        public ConversionExpressionNode(ExpressionNode operandNode,
                                        ReservedKeywordToken operatorKeywordToken,
                                        TypeSpecNode typeNode,
                                        SyntaxNode? parent)
            : base(parent)
        {
            OperandNode = operandNode.WithParent(this);
            OperatorKeywordToken = operatorKeywordToken;
            TypeNode = typeNode.WithParent(this);
        }

        public override bool IsLeaf => false;

        public ExpressionNode OperandNode { get; }

        public ReservedKeywordToken OperatorKeywordToken { get; }

        public TypeSpecNode TypeNode { get; }

        public ConversionExpressionNode WithChildren(ExpressionNode? operandNode = null,
                                                     ReservedKeywordToken? operatorKeywordToken = null,
                                                     TypeSpecNode? typeNode = null)
            => new(operandNode ?? OperandNode,
                   operatorKeywordToken ?? OperatorKeywordToken,
                   typeNode ?? TypeNode);

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
