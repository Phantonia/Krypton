using System.Collections.Generic;

namespace Krypton.Analysis.AST
{
    /* A ProgramNode is the root of the syntax tree.
     * It represents the whole program. Therefore, it
     * saves the top level statements of the program
     * and all declared symbols (none yet, but it will
     * save functions etc.)
     * Branches:
     * - A StatementCollection that represents the
     *   top level statements
     */
    public sealed class ProgramNode : Node
    {
        public ProgramNode(StatementCollectionNode statements, int lineNumber) : base(lineNumber)
        {
            TopLevelStatements = statements;
        }

        public StatementCollectionNode TopLevelStatements { get; }

        public override void PopulateBranches(List<Node> list)
        {
            list.Add(this);
            TopLevelStatements.PopulateBranches(list);
        }
    }
}
