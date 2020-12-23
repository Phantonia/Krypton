using Krypton.Analysis.Utilities;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Krypton.Analysis.AbstractSyntaxTree.Nodes.Statements
{
    public sealed class BlockStatementNode : StatementNode, IEnumerable<StatementNode>
    {
        public BlockStatementNode(IEnumerable<StatementNode> statements, int lineNumber) : base(lineNumber)
        {
            this.statements = (statements as IList<StatementNode>) ?? statements.ToList();

            for (int i = 0; i < this.statements.Count; i++)
            {
                this.statements[i].Parent = this;
                this.statements[i].Previous = this.statements.TryGet(i - 1);
                this.statements[i].Next = this.statements.TryGet(i + 1);
            }
        }

        private readonly IList<StatementNode> statements;

        public StatementNode this[int index]
        {
            get => statements[index];
        }

        public int Count => statements.Count;

        public override BlockStatementNode Clone()
        {
            return new(statements.Select(s => s.Clone()), LineNumber);
        }

        public IEnumerator<StatementNode> GetEnumerator() => statements.GetEnumerator();

        public override void PopulateBranches(List<Node> list)
        {
            list.Add(this);

            foreach (StatementNode statement in statements)
            {
                statement.PopulateBranches(list);
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
