using Krypton.Analysis.AbstractSyntaxTree.Nodes.Expressions;
using System;
using System.Text;

namespace Krypton.Analysis.AbstractSyntaxTree.Nodes.Statements
{
    public sealed class VariableAssignmentStatementNode : StatementNode
    {
        public VariableAssignmentStatementNode(IdentifierNode identifier, ExpressionNode assignedValue, int lineNumber) : base(lineNumber)
        {
            IdentifierNode = identifier;
            AssignedValue = assignedValue;
        }

        public ExpressionNode AssignedValue { get; }

        public string Identifier => IdentifierNode.Identifier;

        public IdentifierNode IdentifierNode { get; }

        public override VariableAssignmentStatementNode Clone()
        {
            return new(IdentifierNode.Clone(), AssignedValue.Clone(), LineNumber);
        }

        public override void GenerateCode(StringBuilder stringBuilder)
        {
            throw new NotImplementedException();
        }
    }
}
