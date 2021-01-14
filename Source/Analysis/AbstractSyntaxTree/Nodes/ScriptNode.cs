using System.Collections.Generic;

namespace Krypton.Analysis.AbstractSyntaxTree.Nodes
{
    /* A ScriptNode is the root of the syntax tree.
     * It represents the whole script. Therefore, it
     * saves the top level statements of the script
     * and all declared symbols (none yet, but it will
     * save functions etc.)
     * Branches:
     * - A StatementCollection that represents the
     *   top level statements
     */
    public sealed class ScriptNode : Node
    {
        public ScriptNode(StatementCollectionNode statements, int lineNumber) : base(lineNumber)
        {
            TopLevelStatements = statements;
        }

        public StatementCollectionNode TopLevelStatements { get; }

        public override ScriptNode Clone()
        {
            return new(TopLevelStatements.Clone(), LineNumber);
        }

        public override void PopulateBranches(List<Node> list)
        {
            list.Add(this);
            TopLevelStatements.PopulateBranches(list);
        }
    }
}
