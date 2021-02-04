using Krypton.Analysis.Ast.Statements;
using Krypton.Utilities;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Krypton.Analysis.Ast
{
    [DebuggerDisplay("{GetType().Name}; Count = {Count}")]
    public sealed class StatementCollectionNode : Node, IIndexedEnumerable<StatementNode>, IEnumerable<StatementNode>
    {
        internal StatementCollectionNode(IEnumerable<StatementNode> statementNodes) : base(statementNodes.FirstOrDefault()?.LineNumber ?? 0, statementNodes.FirstOrDefault()?.Index ?? -1)
        {
            this.statementNodes = statementNodes as IList<StatementNode> ?? statementNodes.ToList();

            for (int i = 0; i < this.statementNodes.Count; i++)
            {
                this.statementNodes[i].ParentNode = this;
                this.statementNodes[i].PreviousStatementNode = this.statementNodes.TryGet(i - 1);
                this.statementNodes[i].NextStatementNode = this.statementNodes.TryGet(i + 1);
            }
        }

        private readonly IList<StatementNode> statementNodes;

        public StatementNode this[int index]
        {
            get => statementNodes[index];
        }

        public int Count => statementNodes.Count;

        public IEnumerator<StatementNode> GetEnumerator() => statementNodes.GetEnumerator();

        public override void PopulateBranches(List<Node> list)
        {
            list.Add(this);

            foreach (StatementNode statement in statementNodes)
            {
                statement.PopulateBranches(list);
            }
        }
    }
}
