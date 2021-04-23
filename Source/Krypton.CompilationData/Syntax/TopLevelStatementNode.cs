using Krypton.CompilationData.Syntax.Statements;
using System.IO;

namespace Krypton.CompilationData.Syntax
{
    public sealed class TopLevelStatementNode : TopLevelNode
    {
        public TopLevelStatementNode(StatementNode statement,
                                     SyntaxNode? parent = null)
            : base(parent)
        {
            StatementNode = statement.WithParent(this);
        }

        public override bool IsLeaf => false;

        public StatementNode StatementNode { get; }

        public override TopLevelStatementNode WithParent(SyntaxNode newParent)
            => new(StatementNode, newParent);

        public override void WriteCode(TextWriter output) => StatementNode.WriteCode(output);
    }
}
