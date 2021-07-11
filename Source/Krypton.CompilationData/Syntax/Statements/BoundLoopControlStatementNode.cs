using Krypton.CompilationData.Syntax.Tokens;
using System.IO;

namespace Krypton.CompilationData.Syntax.Statements
{
    public sealed record BoundLoopControlStatementNode : StatementNode
    {
        public BoundLoopControlStatementNode(LoopControlStatementNode loopControlStatement,
                                             LoopStatementNode loop)
        {
            LoopControlStatementNode = loopControlStatement;
            LoopNode = loop;
        }

        public override bool IsLeaf => false;

        public override Token LexicallyFirstToken => LoopControlStatementNode.LexicallyFirstToken;

        public LoopControlStatementNode LoopControlStatementNode { get; init; }

        public LoopStatementNode LoopNode { get; init; }

        public override void WriteCode(TextWriter output)
            => LoopControlStatementNode.WriteCode(output);
    }
}
