using Krypton.CompilationData;
using Krypton.CompilationData.Syntax;
using Krypton.CompilationData.Syntax.Declarations;
using Krypton.CompilationData.Syntax.Statements;
using System;
using System.Collections.Generic;
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
        private SymbolTable symbolTable;
        private TypeManager typeManager;
#nullable restore

        public BindingResult? PerformBinding()
        {
            Loader loader = new(program);
            symbolTable = loader.GatherSymbols();
            typeManager = new TypeManager(program, symbolTable);

            ProgramNode? program = BindTopLevelStatements();

            if (program == null)
            {
                return null;
            }

            foreach (FunctionDeclarationNode functionDeclaration in program.GetFunctionDeclarations())
            {

            }

            return new BindingResult(program, symbolTable);
        }

        private ProgramNode? BindTopLevelStatements()
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
                    return null;
                }

                TopLevelStatementNode boundTopLevelStatement = unboundTopLevelStatement with { StatementNode = boundStatement };

                boundTopLevelNodes = boundTopLevelNodes.SetItem(i, boundTopLevelStatement);
            }

            return program with { TopLevelNodes = boundTopLevelNodes };
        }
    }
}
