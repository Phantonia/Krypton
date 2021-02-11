﻿using Krypton.Analysis.Ast.Expressions;
using Krypton.Analysis.Ast.Identifiers;
using Krypton.Analysis.Ast.Symbols;
using Krypton.Analysis.Ast.TypeSpecs;
using System.Collections.Generic;
using System.Diagnostics;

namespace Krypton.Analysis.Ast.Statements
{
    [DebuggerDisplay("{GetType().Name}; VariableIdentifier = {VariableIdentifier}")]
    public sealed class VariableDeclarationStatementNode : StatementNode
    {
        internal VariableDeclarationStatementNode(IdentifierNode identifier, TypeSpecNode? type, ExpressionNode? value, int lineNumber, int index) : base(lineNumber, index)
        {
            VariableIdentifierNode = identifier;
            VariableIdentifierNode.ParentNode = this;

            TypeSpecNode = type;
            if (TypeSpecNode != null)
            {
                TypeSpecNode.ParentNode = this;
            }

            AssignedExpressionNode = value;
            if (AssignedExpressionNode != null)
            {
                AssignedExpressionNode.ParentNode = this;
            }
        }

        public ExpressionNode? AssignedExpressionNode { get; }

        public LocalVariableSymbolNode? VariableNode
        {
            get
            {
                return (VariableIdentifierNode as BoundIdentifierNode)?.Symbol as LocalVariableSymbolNode;
            }
        }

        public string VariableIdentifier => VariableIdentifierNode.Identifier;

        public IdentifierNode VariableIdentifierNode { get; private set; }

        public TypeSpecNode? TypeSpecNode { get; }

        public LocalVariableSymbolNode CreateVariable(TypeSymbolNode? typeSymbol)
        {
            LocalVariableSymbolNode variable = new LocalVariableSymbolNode(VariableIdentifier,
                                                                           typeSymbol,
                                                                           VariableIdentifierNode.LineNumber,
                                                                           VariableIdentifierNode.Index);
            VariableIdentifierNode = new BoundIdentifierNode(VariableIdentifier,
                                                             variable,
                                                             VariableIdentifierNode.LineNumber,
                                                             VariableIdentifierNode.Index)
            {
                ParentNode = this
            };
            return variable;
        }

        public override void PopulateBranches(List<Node> list)
        {
            list.Add(this);
            VariableIdentifierNode.PopulateBranches(list);
            TypeSpecNode?.PopulateBranches(list);
            AssignedExpressionNode?.PopulateBranches(list);
        }
    }
}
