using Krypton.CompilationData.Syntax;
using Krypton.CompilationData.Syntax.Statements;
using System.Collections.Immutable;

namespace Krypton.Analysis.Semantics
{
    partial class Binder
    {
        private StatementNode? BindStatement(StatementNode unboundStatement, VariableIdentifierMap variableIdentifierMap)
        {
            switch (unboundStatement)
            {
                case BlockStatementNode blockStatement:
                    return BindBlockStatement(blockStatement, variableIdentifierMap);
                // missing case: ExpressionStatementNode...
                case ForStatementNode forStatement:


            }
        }

        private BlockStatementNode? BindBlockStatement(BlockStatementNode unboundBlockStatement, VariableIdentifierMap variableIdentifierMap)
        {
            BodyNode? boundBody = BindBody(unboundBlockStatement.BodyNode, variableIdentifierMap);

            if (boundBody == null)
            {
                return null;
            }

            return unboundBlockStatement with { BodyNode = boundBody };
        }

        private BodyNode? BindBody(BodyNode unboundBody, VariableIdentifierMap variableIdentifierMap)
        {
            ImmutableList<StatementNode> unboundStatements = unboundBody.StatementNodes;
            ImmutableList<StatementNode> boundStatements = unboundStatements;

            variableIdentifierMap.EnterBlock();

            for (int i = 0; i < unboundStatements.Count; i++)
            {
                StatementNode? boundStatement = BindStatement(unboundStatements[i], variableIdentifierMap);

                if (boundStatement == null)
                {
                    return null;
                }

                boundStatements = boundStatements.SetItem(i, boundStatement);
            }

            variableIdentifierMap.LeaveBlock();

            return unboundBody with { StatementNodes = boundStatements };
        }

        private ForStatementNode? BindForStatement(ForStatementNode unboundForStatement, VariableIdentifierMap variableIdentifierMap)
        {
            variableIdentifierMap.EnterBlock();
        }
    }
}