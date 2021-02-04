﻿using Krypton.Framework;
using System.Collections.Generic;
using System.Diagnostics;

namespace Krypton.Analysis.Ast.Expressions
{
    [DebuggerDisplay("{GetType().Name}; Operator = {Operator}")]
    public sealed class BinaryOperationExpressionNode : ExpressionNode
    {
        internal BinaryOperationExpressionNode(ExpressionNode leftOperand, ExpressionNode rightOperand, Operator @operator, int lineNumber, int index) : base(lineNumber, index)
        {
            LeftOperandNode = leftOperand;
            LeftOperandNode.ParentNode = this;
            RightOperandNode = rightOperand;
            Operator = @operator;
            RightOperandNode.ParentNode = this;
        }

        public ExpressionNode LeftOperandNode { get; }

        public Operator Operator { get; }

        public ExpressionNode RightOperandNode { get; }

        public override void PopulateBranches(List<Node> list)
        {
            list.Add(this);
            LeftOperandNode.PopulateBranches(list);
            RightOperandNode.PopulateBranches(list);
        }
    }
}
