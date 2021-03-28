using Krypton.CompilationData.Syntax.Types;
using Krypton.Utilities;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Krypton.CompilationData.Syntax
{
    public sealed class ProgramNode : SyntaxNode, IExecutableNode
    {
        public ProgramNode(IEnumerable<TopLevelNode> topLevelNodes,
                           SyntaxNode? parent = null)
            : base(parent)
        {
            TopLevelNodes = topLevelNodes.Select(t => t.WithParent(this)).MakeReadOnly();
            TopLevelStatementNodes = new(openingBraceToken: null,
                                         TopLevelNodes.OfType<TopLevelStatementNode>()
                                                      .Select(t => t.StatementNode)
                                                      .MakeReadOnly(),
                                         closingBraceToken: null,
                                         parent: this);
        }

        public BodyNode TopLevelStatementNodes { get; } // do not write

        public override bool IsLeaf => false;

        public ReadOnlyList<TopLevelNode> TopLevelNodes { get; }

        public override ProgramNode WithParent(SyntaxNode newParent)
            => new(TopLevelNodes, newParent);

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
