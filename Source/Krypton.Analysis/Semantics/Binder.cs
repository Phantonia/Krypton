using Krypton.CompilationData.Syntax;
using Krypton.CompilationData.Syntax.Statements;
using System.Collections.Immutable;

namespace Krypton.Analysis.Semantics
{
    internal sealed partial class Binder
    {
        public Binder(ProgramNode program, Analyser analyser)
        {
            this.analyser = analyser;
            this.program = program;
        }

        private readonly Analyser analyser;
        private ProgramNode program;

#nullable disable // these are assigned by the only method that calls others, PerformBinding()
        private HoistedIdentifierMap globalIdentifierMap;
        private TypeManager typeManager;
#nullable restore

        public BindingResult? PerformBinding()
        {
            TypeIdentifierMap typeIdentifierMap = GatherGlobalTypes();
            typeManager = new TypeManager(program, typeIdentifierMap);

            HoistedIdentifierMap? globalIdentifierMap = GatherGlobalSymbols();

            if (globalIdentifierMap == null)
            {
                return null;
            }

            this.globalIdentifierMap = globalIdentifierMap;

            {
                bool success = BindTopLevelStatements();

                if (!success)
                {
                    return null;
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
