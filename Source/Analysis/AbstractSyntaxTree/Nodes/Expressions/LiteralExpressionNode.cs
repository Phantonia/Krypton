﻿namespace Krypton.Analysis.AbstractSyntaxTree.Nodes.Expressions
{
    public abstract class LiteralExpressionNode : ExpressionNode
    {
        protected LiteralExpressionNode(int lineNumber) : base(lineNumber) { }

        public abstract override LiteralExpressionNode Clone();
    }
}
