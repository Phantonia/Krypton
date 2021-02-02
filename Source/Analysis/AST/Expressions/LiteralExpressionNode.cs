using Krypton.Framework;
using System.Collections.Generic;

namespace Krypton.Analysis.Ast.Expressions
{
    public abstract class LiteralExpressionNode : ExpressionNode
    {
        private protected LiteralExpressionNode(FrameworkType associatedType, int lineNumber) : base(lineNumber)
        {
            AssociatedType = associatedType;
        }

        public FrameworkType AssociatedType { get; }

        public override void PopulateBranches(List<Node> list)
        {
            list.Add(this);
        }
    }
}
