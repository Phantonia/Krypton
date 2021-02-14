using Krypton.Analysis.Ast.Expressions;
using Krypton.Utilities;
using System.Collections.Generic;

namespace Krypton.Analysis.Ast.Statements
{
    public sealed class IfStatementNode : StatementNode, IParentStatementNode
    {
        internal IfStatementNode(ExpressionNode condition,
                                 StatementCollectionNode statements,
                                 int lineNumber,
                                 int index) : this(condition,
                                                   statements,
                                                   elseIfParts: null,
                                                   elsePart: null,
                                                   lineNumber,
                                                   index) { }

        internal IfStatementNode(ExpressionNode condition,
                                 StatementCollectionNode statements,
                                 IList<ElseIfPartNode>? elseIfParts,
                                 ElsePartNode? elsePart,
                                 int lineNumber,
                                 int index) : base(lineNumber, index)
        {
            ConditionNode = condition;
            ConditionNode.ParentNode = this;

            ElseIfPartNodes = elseIfParts.MakeReadOnly();
            foreach (ElseIfPartNode elseIf in ElseIfPartNodes)
            {
                elseIf.ParentNode = this;
            }

            ElsePartNode = elsePart;
            if (ElsePartNode != null)
            {
                ElsePartNode.ParentNode = this;
            }

            StatementNodes = statements;
            StatementNodes.ParentNode = this;
        }

        public ExpressionNode ConditionNode { get; }

        public ReadOnlyList<ElseIfPartNode> ElseIfPartNodes { get; }

        public ElsePartNode? ElsePartNode { get; }

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

            ElsePartNode?.PopulateBranches(list);
        }
    }
}
