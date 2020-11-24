using Krypton.Analysis.AbstractSyntaxTree.Nodes;
using System.Collections;
using System.Collections.Generic;

namespace Krypton.Analysis.AbstractSyntaxTree
{
    public sealed class SyntaxTree : IEnumerable<Node>
    {
        public SyntaxTree()
        {
            Root = new ScriptNode();
        }

        public Node Root { get; }

        public IEnumerator<Node> GetEnumerator()
        {
            return ((IEnumerable<Node>)Root).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
