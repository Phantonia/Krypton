using System.Collections.Generic;

namespace Krypton.Analysis.AST
{
    public interface INode
    {
        int LineNumber { get; }

        Node? Parent { get; }

        List<Node> GetBranches();

        void PopulateBranches(List<Node> list);
    }
}
