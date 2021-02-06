using Krypton.Analysis.Ast.Expressions;
using System.Collections.Generic;

namespace Krypton.Analysis.Ast
{
    public sealed class ElseIfPartNode : Node
    {
        internal ElseIfPartNode(ExpressionNode condition,
                                StatementCollectionNode statements,
                                int lineNumber,
                                int index) : base(lineNumber, index)
        {
            ConditionNode = condition;
            StatementNodes = statements;
        }

        public ExpressionNode ConditionNode { get; }

        public StatementCollectionNode StatementNodes { get; }

        public override void PopulateBranches(List<Node> list)
        {
            list.Add(this);
            ConditionNode.PopulateBranches(list);
            StatementNodes.PopulateBranches(list);
        }
    }
}
