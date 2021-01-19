using System.Collections;
using System.Collections.Generic;

namespace Krypton.Analysis.Ast
{
    public sealed class SyntaxTree : IEnumerable<Node>
    {
        public SyntaxTree(ProgramNode root)
        {
            Root = root;
        }

        public ProgramNode Root { get; }

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
}
