using Krypton.Analysis.Ast.Declarations;
using Krypton.Analysis.Ast.Symbols;
using Krypton.Analysis.Ast.TypeSpecs;
using Krypton.Utilities;
using System.Collections.Generic;

namespace Krypton.Analysis.Ast
{
    public sealed class ProgramNode : Node, IExecutableNode
    {
        internal ProgramNode(StatementCollectionNode topLevelStatements,
                             IList<ConstantDeclarationNode> constants,
                             IList<FunctionDeclarationNode> functions,
                             int lineNumber,
                             int index) : base(lineNumber, index)
        {
            TopLevelStatementNodes = topLevelStatements;
            Constants = constants.MakeReadOnly();
            Functions = functions.MakeReadOnly();
        }

        public ReadOnlyList<ConstantDeclarationNode> Constants { get; }

        public ReadOnlyList<FunctionDeclarationNode> Functions { get; }

        public StatementCollectionNode TopLevelStatementNodes { get; }

        public override void PopulateBranches(List<Node> list)
        {
            list.Add(this);
            TopLevelStatementNodes.PopulateBranches(list);

            foreach (FunctionDeclarationNode function in Functions)
            {
                function.PopulateBranches(list);
            }
        }

        StatementCollectionNode IExecutableNode.BodyNode => TopLevelStatementNodes;

        TypeSpecNode? IExecutableNode.ReturnTypeNode => null;
    }
}
