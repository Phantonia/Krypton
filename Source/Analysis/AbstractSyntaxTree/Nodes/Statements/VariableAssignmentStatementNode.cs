using Krypton.Analysis.AbstractSyntaxTree.Nodes.Expressions;
using System.Collections.Generic;

namespace Krypton.Analysis.AbstractSyntaxTree.Nodes.Statements
{
    public sealed class VariableAssignmentStatementNode : StatementNode
    {
        public VariableAssignmentStatementNode(IdentifierNode identifier, ExpressionNode assignedValue, int lineNumber) : base(lineNumber)
        {
            IdentifierNode = identifier;
            IdentifierNode.Parent = this;
            AssignedValue = assignedValue;
            AssignedValue.Parent = this;
        }

        public ExpressionNode AssignedValue { get; }

        public string Identifier => IdentifierNode.Identifier;

        public IdentifierNode IdentifierNode { get; }

        public override VariableAssignmentStatementNode Clone()
        {
            return new(IdentifierNode.Clone(), AssignedValue.Clone(), LineNumber);
        }

        public override void PopulateBranches(List<Node> list)
        {
            list.Add(this);
            IdentifierNode.PopulateBranches(list);
            AssignedValue.PopulateBranches(list);
        }
    }
}
