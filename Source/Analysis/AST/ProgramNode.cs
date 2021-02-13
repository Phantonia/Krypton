using Krypton.Analysis.Ast.Declarations;
using Krypton.Utilities;
using System.Collections.Generic;

namespace Krypton.Analysis.Ast
{
    public sealed class ProgramNode : Node
    {
        internal ProgramNode(StatementCollectionNode topLevelStatements,
                             IList<FunctionDeclarationNode> functions,
                             int lineNumber,
                             int index) : base(lineNumber, index)
        {
            TopLevelStatementNodes = topLevelStatements;
            FunctionSymbols = functions.MakeReadOnly();
        }

        public ReadOnlyList<FunctionDeclarationNode> FunctionSymbols { get; }

        public StatementCollectionNode TopLevelStatementNodes { get; }

        public override void PopulateBranches(List<Node> list)
        {
            list.Add(this);
            TopLevelStatementNodes.PopulateBranches(list);

            foreach (FunctionDeclarationNode function in FunctionSymbols)
            {
                function.PopulateBranches(list);
            }
        }
    }
}
