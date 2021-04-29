using Krypton.CompilationData.Syntax.Types;
using Krypton.Utilities;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Krypton.CompilationData.Syntax
{
    public sealed record ProgramNode : SyntaxNode, IExecutableNode
    {
        public ProgramNode(IEnumerable<TopLevelNode> topLevelNodes)
        {
            TopLevelNodes = topLevelNodes.Select(t => t.WithParent(this)).Finalize();
            TopLevelStatementNodes = new BodyNode(openingBrace: null,
                                                  TopLevelNodes.OfType<TopLevelStatementNode>()
                                                               .Select(t => t.StatementNode)
                                                               .Finalize(),
                                                  closingBrace: null) with
                                                  {
                                                      ParentNode = this
                                                  };
        }

        public BodyNode TopLevelStatementNodes { get; } // do not write

        public override bool IsLeaf => false;

        public FinalList<TopLevelNode> TopLevelNodes { get; init; }

        public override void WriteCode(TextWriter output)
        {
            foreach (TopLevelNode topLevelNode in TopLevelNodes)
            {
                topLevelNode.WriteCode(output);
            }
        }

        BodyNode IExecutableNode.BodyNode => TopLevelStatementNodes;

        TypeNode? IExecutableNode.ReturnTypeNode => null;
    }
}
