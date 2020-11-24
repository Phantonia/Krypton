using System.Collections;
using System.Collections.Generic;

namespace Krypton.Analysis.AbstractSyntaxTree
{
    public sealed class SyntaxTree : IEnumerable<Node>
    {
        private SyntaxTree() { }

        public Node Root { get; } = null!; // TODO: change

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
