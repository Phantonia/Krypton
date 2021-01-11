using Krypton.Analysis.AbstractSyntaxTree.Nodes.Expressions;
using Krypton.Analysis.AbstractSyntaxTree.Nodes.Identifiers;
using Krypton.Analysis.AbstractSyntaxTree.Nodes.Symbols;
using Krypton.Analysis.AbstractSyntaxTree.Nodes.Types;
using System.Collections.Generic;

namespace Krypton.Analysis.AbstractSyntaxTree.Nodes.Statements
{
    public sealed class VariableDeclarationStatementNode : StatementNode
    {
        public VariableDeclarationStatementNode(IdentifierNode identifier, TypeNode? type, ExpressionNode? value, int lineNumber) : base(lineNumber)
        {
            VariableIdentifierNode = identifier;
            VariableIdentifierNode.Parent = this;

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

        public string Identifier => VariableIdentifierNode.Identifier;

        public IdentifierNode VariableIdentifierNode { get; private set; }

        public TypeNode? Type { get; }

        public ExpressionNode? Value { get; }

        public override VariableDeclarationStatementNode Clone()
        {
            return new(VariableIdentifierNode.Clone(), Type?.Clone(), Value?.Clone(), LineNumber);
        }

        public LocalVariableSymbolNode CreateVariable(TypeSymbolNode? typeSymbol)
        {
            LocalVariableSymbolNode var = new LocalVariableSymbolNode(Identifier, typeSymbol, VariableIdentifierNode.LineNumber);
            VariableIdentifierNode = new BoundIdentifierNode(Identifier, var, VariableIdentifierNode.LineNumber) { Parent = this };
            return var;
        }

        public override void PopulateBranches(List<Node> list)
        {
            list.Add(this);
            VariableIdentifierNode.PopulateBranches(list);
            Type?.PopulateBranches(list);
            Value?.PopulateBranches(list);
        }
    }
}
