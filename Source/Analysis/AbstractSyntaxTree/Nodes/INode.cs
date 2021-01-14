using System.Collections.Generic;

namespace Krypton.Analysis.AbstractSyntaxTree.Nodes
{
    public interface INode
    {
        int LineNumber { get; }

        Node? Parent { get; }

        Node Clone();

        List<Node> GetBranches();

        void PopulateBranches(List<Node> list);
    }
}
