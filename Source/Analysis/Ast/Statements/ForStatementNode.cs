using Krypton.Analysis.Ast.Expressions;
using Krypton.Analysis.Ast.Identifiers;
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

            IdentifierNode = identifier;
            identifier.ParentNode = this;

            DeclaresNew = declaresNew;

            InitialValue = initialValue;
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
            Statements = statements;
            if (withValue != null)
            {
                withValue.ParentNode = this;
            }
        }

        public ExpressionNode? ConditionNode { get; }

        public bool DeclaresNew { get; }

        public string Identifier => IdentifierNode.Identifier;

        public IdentifierNode IdentifierNode { get; }

        public ExpressionNode? InitialValue { get; }

        public StatementCollectionNode Statements { get; }

        public ExpressionNode? WithExpressionNode { get; }

        public override void PopulateBranches(List<Node> list)
        {
            list.Add(this);
            IdentifierNode.PopulateBranches(list);
            InitialValue?.PopulateBranches(list);
            ConditionNode?.PopulateBranches(list);
            WithExpressionNode?.PopulateBranches(list);
        }
    }
}
