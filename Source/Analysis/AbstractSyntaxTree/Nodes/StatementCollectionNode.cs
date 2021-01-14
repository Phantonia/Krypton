using Krypton.Analysis.AbstractSyntaxTree.Nodes.Statements;
using Krypton.Analysis.Utilities;
using System.Collections.Generic;
using System.Linq;

namespace Krypton.Analysis.AbstractSyntaxTree.Nodes
{
    /* A StatementCollectionNode is a collection of statements.
     * It is used by the ScriptNode to represents top level
     * statements, by control statements like Block or While
     * to represents its nested statements, etc.
     * Branches:
     * - An ordered list of StatementNodes. 
     * LineNumber:
     * - The line number of the first statement
     * - If there are no statements: 0
     */
    public sealed class StatementCollectionNode : Node, IIndexedEnumerable<StatementNode>, IEnumerable<StatementNode>
    {
        public StatementCollectionNode(IEnumerable<StatementNode> statements) : base(statements.FirstOrDefault()?.LineNumber ?? 0)
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

        public override StatementCollectionNode Clone()
        {
            return new(statements.Select(s => s.Clone()));
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
    }
}
