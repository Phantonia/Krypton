﻿using System.Collections.Generic;

namespace Krypton.Analysis.Ast.Expressions
{
    public abstract class LiteralExpressionNode : ExpressionNode
    {
        protected private LiteralExpressionNode(int lineNumber) : base(lineNumber) { }

        public override void PopulateBranches(List<Node> list)
        {
            list.Add(this);
        }
    }
}
