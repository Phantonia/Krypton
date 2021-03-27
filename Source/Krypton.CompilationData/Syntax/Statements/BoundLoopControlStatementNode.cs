using System.IO;

namespace Krypton.CompilationData.Syntax.Statements
{
    public sealed class BoundLoopControlStatementNode : StatementNode
    {
        public BoundLoopControlStatementNode(LoopControlStatementNode loopControlStatement,
                                             LoopStatementNode loop,
                                             SyntaxNode? parent = null)
            : base(parent)
        {
            LoopControlStatementNode = loopControlStatement.WithParent(this);
            LoopNode = loop; // not a child, only a reference. So DON'T WithParent it
        }

        public override bool IsLeaf => false;

        public LoopControlStatementNode LoopControlStatementNode { get; }

        public LoopStatementNode LoopNode { get; }

        public override BoundLoopControlStatementNode WithParent(SyntaxNode newParent)
            => new(LoopControlStatementNode, LoopNode);

        public override void WriteCode(TextWriter output) => LoopControlStatementNode.WriteCode(output);
    }
}
