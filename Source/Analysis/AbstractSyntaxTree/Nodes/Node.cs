using System.Collections.Generic;
using System.Diagnostics;

namespace Krypton.Analysis.AbstractSyntaxTree.Nodes
{
    [DebuggerDisplay("{GetType().Name} -- {GetHashCode()}")]
    public abstract class Node
    {
        protected Node(int lineNumber)
        {
            LineNumber = lineNumber;
        }

        private Node? parent;

        public int LineNumber { get; }

        public Node? Parent
        {
            get => parent;
            internal set
            {
                parent = value;
            }
        }

        public abstract Node Clone();

        public List<Node> GetBranches()
        {
            List<Node> branches = new();
            PopulateBranches(branches);
            return branches;
        }

        public abstract void PopulateBranches(List<Node> list);
    }
}
