using System.Collections.Generic;

namespace Krypton.Analysis.Ast
{
    public interface INode
    {
        int LineNumber { get; }

        Node? ParentNode { get; }

        List<Node> GetBranches();

        void PopulateBranches(List<Node> list);
    }
}
