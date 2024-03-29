﻿using Krypton.Analysis.Ast.Expressions;
using Krypton.Analysis.Ast.Identifiers;
using Krypton.Analysis.Ast.Symbols;
using System.Collections.Generic;
using System.Diagnostics;

namespace Krypton.Analysis.Ast.Statements
{
    public sealed class ForStatementNode : StatementNode, ILoopStatementNode
    {
        internal ForStatementNode(IdentifierNode identifier,
                                  bool declaresNew,
                                  ExpressionNode? initialValue,
                                  ExpressionNode? condition,
                                  ExpressionNode? withValue,
                                  StatementCollectionNode statements,
                                  int lineNumber,
                                  int index) : base(lineNumber, index)
        {
            Debug.Assert((condition != null) | (withValue != null));

            VariableIdentifierNode = identifier;
            identifier.ParentNode = this;

            DeclaresNew = declaresNew;

            InitialValueNode = initialValue;
            if (initialValue != null)
            {
                initialValue.ParentNode = this;
            }

            ConditionNode = condition;
            if (condition != null)
            {
                condition.ParentNode = this;
            }

            WithExpressionNode = withValue;
            if (withValue != null)
            {
                withValue.ParentNode = this;
            }

            StatementNodes = statements;
            StatementNodes.ParentNode = this;
        }

        public ExpressionNode? ConditionNode { get; }

        public bool DeclaresNew { get; }

        public ExpressionNode? InitialValueNode { get; }

        public StatementCollectionNode StatementNodes { get; }

        public string VariableIdentifier => VariableIdentifierNode.Identifier;

        public IdentifierNode VariableIdentifierNode { get; private set; }

        public ExpressionNode? WithExpressionNode { get; }

        public VariableSymbolNode CreateVariable(TypeSymbolNode type)
        {
            VariableSymbolNode variable = new VariableSymbolNode(VariableIdentifier,
                                                                 type,
                                                                 isReadOnly: true,
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
            InitialValueNode?.PopulateBranches(list);
            ConditionNode?.PopulateBranches(list);
            WithExpressionNode?.PopulateBranches(list);
        }
    }
}
