using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Krypton.Analysis.AbstractSyntaxTree
{
    public abstract class Node : IEnumerable<Node>
    {
        protected Node(int lineNumber)
        {
            LineNumber = lineNumber;
        }

        public int LineNumber { get; }

        public abstract Node Clone();

        protected virtual IEnumerable<Node> GetBranches()
        {
            return Enumerable.Empty<Node>();
        }

        IEnumerator<Node> IEnumerable<Node>.GetEnumerator()
        {
            yield return this;

            foreach (Node node in GetBranches())
            {
                foreach (Node children in node)
                {
                    yield return children;
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<Node>)this).GetEnumerator();
        }
    }
}
