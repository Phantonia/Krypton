using Krypton.CompilationData.Syntax.Statements;
using Krypton.CompilationData.Syntax.Tokens;
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

        public override Token LexicallyFirstToken => StatementNode.LexicallyFirstToken;

        public StatementNode StatementNode { get; init; }

        public override void WriteCode(TextWriter output)
            => StatementNode.WriteCode(output);
    }
}
