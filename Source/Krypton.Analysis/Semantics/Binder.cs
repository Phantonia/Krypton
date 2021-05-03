using Krypton.CompilationData.Syntax;
using Krypton.CompilationData.Syntax.Statements;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Krypton.Analysis.Semantical
{
    internal sealed partial class Binder
    {
        public Binder(ProgramNode program)
        {
            this.program = program;
        }

#nullable disable // these are assigned by the only method that calls others, PerformBinding()
        private HoistedIdentifierMap globalIdentifierMap;
        private TypeManager typeManager;
#nullable restore

        private ProgramNode program;

        public bool PerformBinding()
        {
            TypeIdentifierMap typeIdentifierMap = GatherGlobalTypes();
            typeManager = new TypeManager(program, typeIdentifierMap);

            HoistedIdentifierMap? globalIdentifierMap = GatherGlobalSymbols();

            if (globalIdentifierMap == null)
            {
                return false;
            }

            this.globalIdentifierMap = globalIdentifierMap;

            {
                bool success = BindTopLevelStatements();

                if (!success)
                {
                    return false;
                }
            }

            // ...
        }

        private bool BindTopLevelStatements()
        {
            VariableIdentifierMap variableIdentifierMap = new();

            ImmutableList<TopLevelNode> unboundTopLevelNodes = program.TopLevelNodes;
            ImmutableList<TopLevelNode> boundTopLevelNodes = unboundTopLevelNodes;

            for (int i = 0; i < unboundTopLevelNodes.Count; i++)
            {
                if (unboundTopLevelNodes[i] is not TopLevelStatementNode unboundTopLevelStatement)
                {
                    continue;
                }

                StatementNode? boundStatement = BindStatement(unboundTopLevelStatement.StatementNode, variableIdentifierMap);

                if (boundStatement == null)
                {
                    return false;
                }

                TopLevelStatementNode boundTopLevelStatement = unboundTopLevelStatement with { StatementNode = boundStatement };

                boundTopLevelNodes = boundTopLevelNodes.SetItem(i, boundTopLevelStatement);
            }

            program = program with { TopLevelNodes = boundTopLevelNodes };

            return true;
        }
    }
}
