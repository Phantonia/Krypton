﻿using Krypton.Analysis.AbstractSyntaxTree.Nodes;
using System.Collections;
using System.Collections.Generic;

namespace Krypton.Analysis.AbstractSyntaxTree
{
    public sealed class SyntaxTree : IEnumerable<Node>
    {
        public SyntaxTree(ScriptNode root)
        {
            Root = root;
        }

        public ScriptNode Root { get; }

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
