using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Krypton.Analysis.AbstractSyntaxTree.Nodes.Statements
{
    public sealed class BlockStatementNode : StatementNode, IEnumerable<StatementNode>
    {
        public BlockStatementNode(IEnumerable<StatementNode> statements, int lineNumber) : base(lineNumber)
        {
            this.statements = (statements as IList<StatementNode>) ?? statements.ToList();
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
        
        public override void GenerateCode(StringBuilder stringBuilder)
        {
            stringBuilder.Append("{\r\n");

            foreach (StatementNode statement in statements)
            {
                stringBuilder.Append('\t');
                statement.GenerateCode(stringBuilder);
            }

            stringBuilder.Append("}\r\n");
        }

        public IEnumerator<StatementNode> GetEnumerator() => statements.GetEnumerator();
    }
}
