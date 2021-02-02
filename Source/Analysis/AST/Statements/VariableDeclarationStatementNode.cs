using Krypton.Analysis.Ast.Expressions;
using Krypton.Analysis.Ast.Identifiers;
using Krypton.Analysis.Ast.Symbols;
using Krypton.Analysis.Ast.TypeSpecs;
using System.Collections.Generic;
using System.Diagnostics;

namespace Krypton.Analysis.Ast.Statements
{
    public sealed class VariableDeclarationStatementNode : StatementNode
    {
        public VariableDeclarationStatementNode(IdentifierNode identifier, TypeSpecNode? type, ExpressionNode? value, int lineNumber) : base(lineNumber)
        {
            VariableIdentifierNode = identifier;
            VariableIdentifierNode.Parent = this;

            Type = type;
            if (Type != null)
            {
                Type.Parent = this;
            }

            AssignedValue = value;
            if (AssignedValue != null)
            {
                AssignedValue.Parent = this;
            }
        }

        public ExpressionNode? AssignedValue { get; }

        public LocalVariableSymbolNode VariableNode
        {
            get
            {
                LocalVariableSymbolNode? localVariable = (VariableIdentifierNode as BoundIdentifierNode)?.Symbol as LocalVariableSymbolNode;
                Debug.Assert(localVariable != null);
                return localVariable;
            }
        }

        public string VariableIdentifier => VariableIdentifierNode.Identifier;

        public IdentifierNode VariableIdentifierNode { get; private set; }

        public TypeSpecNode? Type { get; }

        public LocalVariableSymbolNode CreateVariable(TypeSymbolNode? typeSymbol)
        {
            LocalVariableSymbolNode var = new LocalVariableSymbolNode(VariableIdentifier, typeSymbol, VariableIdentifierNode.LineNumber);
            VariableIdentifierNode = new BoundIdentifierNode(VariableIdentifier, var, VariableIdentifierNode.LineNumber) { Parent = this };
            return var;
        }

        public override void PopulateBranches(List<Node> list)
        {
            list.Add(this);
            VariableIdentifierNode.PopulateBranches(list);
            Type?.PopulateBranches(list);
            AssignedValue?.PopulateBranches(list);
        }
    }
}
