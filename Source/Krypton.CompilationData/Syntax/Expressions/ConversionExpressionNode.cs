﻿using Krypton.CompilationData.Symbols;
using Krypton.CompilationData.Syntax.Tokens;
using Krypton.CompilationData.Syntax.Types;
using System.Diagnostics;
using System.IO;

namespace Krypton.CompilationData.Syntax.Expressions
{
    public sealed record ConversionExpressionNode : ExpressionNode
    {
        public ConversionExpressionNode(ExpressionNode operand,
                                        ReservedKeywordToken operatorKeyword,
                                        TypeNode type)
        {
            Debug.Assert(operatorKeyword.Keyword is ReservedKeyword.As or ReservedKeyword.To);

            OperandNode = operand;
            OperatorKeywordToken = operatorKeyword;
            TypeNode = type.WithParent(this);
        }

        public override bool IsLeaf => false;

        public ExpressionNode OperandNode { get; init; }

        public ReservedKeywordToken OperatorKeywordToken { get; init; }

        public TypeNode TypeNode { get; init; }

        public override TypedExpressionNode Type(TypeSymbol type)
            => new(this, type);

        public override void WriteCode(TextWriter output)
        {
            OperandNode.WriteCode(output);
            OperatorKeywordToken.WriteCode(output);
            TypeNode.WriteCode(output);
        }
    }
}
