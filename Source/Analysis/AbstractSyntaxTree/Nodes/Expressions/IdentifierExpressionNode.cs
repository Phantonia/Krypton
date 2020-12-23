using System.Collections.Generic;

namespace Krypton.Analysis.AbstractSyntaxTree.Nodes.Expressions
{
    public sealed class IdentifierExpressionNode : ExpressionNode
    {
        public IdentifierExpressionNode(string identifier, int lineNumber) : base(lineNumber)
        {
            IdentifierNode = new IdentifierNode(identifier, lineNumber)
            {
                Parent = this
            };
        }

        private IdentifierExpressionNode(IdentifierNode identifier, int lineNumber) : base(lineNumber)
        {
            IdentifierNode = identifier;
        }

        public string Identifier => IdentifierNode.Identifier;

        public IdentifierNode IdentifierNode { get; }

        public override IdentifierExpressionNode Clone()
        {
            return new(IdentifierNode.Clone(), LineNumber);
        }

        public override void PopulateBranches(List<Node> list)
        {
            list.Add(this);
            IdentifierNode.PopulateBranches(list);
        }
    }
}
