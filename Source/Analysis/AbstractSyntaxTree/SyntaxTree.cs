using Krypton.Analysis.AbstractSyntaxTree.Nodes;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Krypton.Analysis.AbstractSyntaxTree
{
    public sealed class SyntaxTree : IEnumerable<Node>
    {
        public SyntaxTree(Node root)
        {
            Root = root;
        }

        public Node Root { get; }

        public string GenerateCode()
        {
            StringBuilder stringBuilder = new();
            Root.GenerateCode(stringBuilder);
            return stringBuilder.ToString();
        }

        public IEnumerator<Node> GetEnumerator()
        {
            return ((IEnumerable<Node>?)Root)?.GetEnumerator() ?? Enumerable.Empty<Node>().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
