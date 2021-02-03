using Krypton.Analysis.Ast;
using System.Collections;
using System.Collections.Generic;

namespace Krypton.Analysis
{
    public sealed class Compilation : IEnumerable<Node>
    {
        public Compilation(ProgramNode program)
        {
            Program = program;
        }

        public ProgramNode Program { get; }

        public IEnumerator<Node> GetEnumerator()
        {
            List<Node> branches = new();
            Program.PopulateBranches(branches);
            return branches.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
