using Krypton.Analysis.Ast.Identifiers;
using System.Collections.Generic;

namespace Krypton.Analysis.Ast.Expressions
{
    public class PropertyGetExpressionNode : ExpressionNode
    {
        internal PropertyGetExpressionNode(ExpressionNode expression, IdentifierNode propertyIdentifierNode, int lineNumber, int index) : base(lineNumber, index)
        {
            ExpressionNode = expression;
            PropertyIdentifierNode = propertyIdentifierNode;
        }

        public ExpressionNode ExpressionNode { get; }

        public string PropertyIdentifier => PropertyIdentifierNode.Identifier;

        public IdentifierNode PropertyIdentifierNode { get; private set; }

        public override void PopulateBranches(List<Node> list)
        {
            list.Add(this);
            ExpressionNode.PopulateBranches(list);
            PropertyIdentifierNode.PopulateBranches(list);
        }
    }
}
