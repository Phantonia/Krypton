using Krypton.Analysis.AbstractSyntaxTree.Nodes.Expressions;
using Krypton.Analysis.AbstractSyntaxTree.Nodes.Types;
using System.Collections.Generic;

namespace Krypton.Analysis.AbstractSyntaxTree.Nodes.Statements
{
    public sealed class VariableDeclarationStatementNode : StatementNode
    {
        public VariableDeclarationStatementNode(IdentifierNode identifier, TypeNode? type, ExpressionNode? value, int lineNumber) : base(lineNumber)
        {
            IdentifierNode = identifier;
            IdentifierNode.Parent = this;

            Type = type;
            if (Type != null)
            {
                Type.Parent = this;
            }

            Value = value;
            if (Value != null)
            {
                Value.Parent = this;
            }
        }

        public string Identifier => IdentifierNode.Identifier;

        public IdentifierNode IdentifierNode { get; }

        public TypeNode? Type { get; }

        public ExpressionNode? Value { get; }

        public override VariableDeclarationStatementNode Clone()
        {
            return new(IdentifierNode.Clone(), Type?.Clone(), Value?.Clone(), LineNumber);
        }

        public override void PopulateBranches(List<Node> list)
        {
            list.Add(this);
            IdentifierNode.PopulateBranches(list);
            Type?.PopulateBranches(list);
            Value?.PopulateBranches(list);
        }
    }
}
