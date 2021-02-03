using System.Collections.Generic;
using System.Diagnostics;

namespace Krypton.Analysis.Ast
{
    [DebuggerDisplay("{GetType().Name}")]
    public abstract class Node : INode
    {
        private protected Node(int lineNumber)
        {
            LineNumber = lineNumber;
        }

        public int LineNumber { get; }

        public Node? ParentNode { get; internal set; }

        public List<Node> GetBranches()
        {
            List<Node> branches = new();
            PopulateBranches(branches);
            return branches;
        }

        public abstract void PopulateBranches(List<Node> list);
    }
}
