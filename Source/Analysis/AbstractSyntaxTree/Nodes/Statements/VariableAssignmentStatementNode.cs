﻿using Krypton.Analysis.AbstractSyntaxTree.Nodes.Expressions;
using Krypton.Analysis.AbstractSyntaxTree.Nodes.Identifiers;
using Krypton.Analysis.AbstractSyntaxTree.Nodes.Symbols;
using System.Collections.Generic;
using System.Diagnostics;

namespace Krypton.Analysis.AbstractSyntaxTree.Nodes.Statements
{
    public sealed class VariableAssignmentStatementNode : StatementNode, IBindable
    {
        public VariableAssignmentStatementNode(IdentifierNode identifier, ExpressionNode assignedValue, int lineNumber) : base(lineNumber)
        {
            VariableIdentifierNode = identifier;
            VariableIdentifierNode.Parent = this;
            AssignedValue = assignedValue;
            AssignedValue.Parent = this;
        }

        public ExpressionNode AssignedValue { get; }

        public string VariableIdentifier => VariableIdentifierNode.Identifier;

        public IdentifierNode VariableIdentifierNode { get; private set; }

        public void Bind(LocalVariableSymbolNode symbol)
        {
            VariableIdentifierNode = new BoundIdentifierNode(VariableIdentifier, symbol, VariableIdentifierNode.LineNumber) { Parent = this };
        }

        public override VariableAssignmentStatementNode Clone()
        {
            return new(VariableIdentifierNode.Clone(), AssignedValue.Clone(), LineNumber);
        }

        public override void PopulateBranches(List<Node> list)
        {
            list.Add(this);
            VariableIdentifierNode.PopulateBranches(list);
            AssignedValue.PopulateBranches(list);
        }

        IdentifierNode IBindable.IdentifierNode => VariableIdentifierNode;

        void IBindable.Bind(SymbolNode symbol)
        {
            LocalVariableSymbolNode? var = symbol as LocalVariableSymbolNode;
            Debug.Assert(var != null);
            Bind(var);
        }
    }
}
