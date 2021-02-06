using Krypton.Analysis.Ast.Expressions;
using Krypton.Utilities;
using System.Collections.Generic;

namespace Krypton.Analysis.Ast.Statements
{
    public sealed class IfStatementNode : StatementNode, IParentStatementNode
    {
        internal IfStatementNode(ExpressionNode condition,
                                 StatementCollectionNode statements,
                                 IList<ElseIfPartNode>? elseIfParts,
                                 ElsePartNode? elsePart,
                                 int lineNumber,
                                 int index) : base(lineNumber, index)
        {
            ConditionNode = condition;
            ElseIfPartNodes = elseIfParts.MakeReadOnly();
            ElsePart = elsePart;
            StatementNodes = statements;
        }

        public ExpressionNode ConditionNode { get; }

        public ReadOnlyList<ElseIfPartNode> ElseIfPartNodes { get; }

        public ElsePartNode? ElsePart { get; }

        public StatementCollectionNode StatementNodes { get; }

        public override void PopulateBranches(List<Node> list)
        {
            list.Add(this);
            ConditionNode.PopulateBranches(list);
            StatementNodes.PopulateBranches(list);

            foreach (ElseIfPartNode part in ElseIfPartNodes)
            {
                part.PopulateBranches(list);
            }

            ElsePart?.PopulateBranches(list);
        }
    }
}
