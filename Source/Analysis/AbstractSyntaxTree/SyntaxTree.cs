using Krypton.Analysis.AbstractSyntaxTree.Nodes;
using System.Collections;
using System.Collections.Generic;

namespace Krypton.Analysis.AbstractSyntaxTree
{
    public abstract class SyntaxTree : IEnumerable<Node>
    {
        public abstract Node Root { get; }

        public IEnumerator<Node> GetEnumerator()
        {
            List<Node> branches = new();
            Root.PopulateBranches(branches);
            return branches.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public sealed class SyntaxTree<TRoot> : SyntaxTree
        where TRoot : Node
    {
        public SyntaxTree(TRoot root)
        {
            Root = root;
        }

        public override TRoot Root { get; }
    }
}
