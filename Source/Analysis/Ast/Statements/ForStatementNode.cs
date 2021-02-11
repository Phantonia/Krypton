using Krypton.Analysis.Ast.Expressions;
using Krypton.Analysis.Ast.Identifiers;
using Krypton.Analysis.Ast.Symbols;
using System.Collections.Generic;
using System.Diagnostics;

namespace Krypton.Analysis.Ast.Statements
{
    public sealed class ForStatementNode : StatementNode
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
            StatementNodes = statements;
            if (withValue != null)
            {
                withValue.ParentNode = this;
            }
        }

        public ExpressionNode? ConditionNode { get; }

        public bool DeclaresNew { get; }

        public ExpressionNode? InitialValueNode { get; }

        public StatementCollectionNode StatementNodes { get; }

        public string VariableIdentifier => VariableIdentifierNode.Identifier;

        public IdentifierNode VariableIdentifierNode { get; private set; }

        public ExpressionNode? WithExpressionNode { get; }

        public LocalVariableSymbolNode CreateVariable(TypeSymbolNode type)
        {
            LocalVariableSymbolNode variable = new LocalVariableSymbolNode(VariableIdentifier,
                                                                           type,
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
