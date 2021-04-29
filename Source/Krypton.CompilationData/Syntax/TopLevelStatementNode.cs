using Krypton.CompilationData.Syntax.Statements;
using System.IO;

namespace Krypton.CompilationData.Syntax
{
    public sealed record TopLevelStatementNode : TopLevelNode
    {
        public TopLevelStatementNode(StatementNode statement)
        {
            StatementNode = statement;
        }

        public override bool IsLeaf => false;

        public StatementNode StatementNode { get; init; }

        public override void WriteCode(TextWriter output)
            => StatementNode.WriteCode(output);
    }
}
