using Krypton.Analysis.Ast;
using Krypton.Analysis.Ast.Statements;

namespace Krypton.CodeGeneration
{
    internal static class LoopControlStatements
    {
        public static bool IsLeftOrContinued(this ILoopStatementNode loopStatement)
        {
            return ContainsLeaveOrContinueWithLevel(loopStatement.StatementNodes, level: 1);
        }

        private static bool ContainsLeaveOrContinueWithLevel(StatementCollectionNode statements, int level)
        {
            foreach (StatementNode statement in statements)
            {
                switch (statement)
                {
                    case LoopControlStatementNode loopControlStatement
                            when loopControlStatement.Level == level:
                        return true;
                    case ILoopStatementNode loopStatement:
                        return ContainsLeaveOrContinueWithLevel(loopStatement.StatementNodes, level + 1);
                    case IParentStatementNode parentStatement and not ILoopStatementNode:
                        return ContainsLeaveOrContinueWithLevel(parentStatement.StatementNodes, level);
                }
            }

            return false;
        }
    }
}
