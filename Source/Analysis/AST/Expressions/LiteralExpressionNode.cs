using Krypton.Framework;
using System.Collections.Generic;
using System.Diagnostics;

namespace Krypton.Analysis.Ast.Expressions
{
    [DebuggerDisplay("{GetType().Name}; Value = {ObjectValue}")]
    public abstract class LiteralExpressionNode : ExpressionNode
    {
        private protected LiteralExpressionNode(FrameworkType associatedType, int lineNumber, int index) : base(lineNumber, index)
        {
            AssociatedType = associatedType;
        }

        public FrameworkType AssociatedType { get; }

        public abstract object ObjectValue { get; }

        public override void PopulateBranches(List<Node> list)
        {
            list.Add(this);
        }
    }
}
