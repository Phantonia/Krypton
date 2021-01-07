using System.Collections.Generic;

namespace Krypton.Analysis.AbstractSyntaxTree.Nodes
{
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

        public abstract void PopulateBranches(List<Node> list);
    }
}
