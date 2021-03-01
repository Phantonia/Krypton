using Krypton.Analysis.Ast.Statements;
using System.Diagnostics;

namespace Krypton.CodeGeneration
{
    partial class CodeGeneratorCore
    {
        private void EmitStatement(StatementNode statement)
        {
            switch (statement)
            {
                case BlockStatementNode blockStatement:
                    EmitStatementBlock(blockStatement.StatementNodes);
                    break;
                case ForStatementNode forStatement:
                    EmitForStatement(forStatement);
                    break;
                case VariableDeclarationStatementNode variableDeclaration:
                    EmitVariableDeclaration(variableDeclaration);
                    break;
            }
        }

        private void EmitForStatement(ForStatementNode forStatement)
        {
            if (forStatement.IsLeftOrContinued())
            {
                // There exists a Leave or Continue statement targeting this loop.
                // We need to add a label so that we can emit a correct break or
                // continue statement.
                output.Append("$loop_").Append(globalLoopCount).Append(':');
                associatedLoopIds[forStatement] = globalLoopCount;
                globalLoopCount++;
            }

            output.Append("for(");

            if (forStatement.DeclaresNew)
            {
                output.Append("let ");
                output.Append(forStatement.VariableIdentifier);
                output.Append('=');

                Debug.Assert(forStatement.InitialValueNode != null);
                EmitExpression(forStatement.InitialValueNode);
            }

            output.Append(';');

            if (forStatement.ConditionNode != null)
            {
                EmitExpression(forStatement.ConditionNode);
            }

            output.Append(';');

            if (forStatement.WithExpressionNode != null)
            {
                output.Append(forStatement.VariableIdentifier);
                output.Append('=');
                EmitExpression(forStatement.WithExpressionNode);
            }
            else
            {
                output.Append(forStatement.VariableIdentifier);
                output.Append("++");
            }

            output.Append(')');

            EmitStatementBlock(forStatement.StatementNodes);
        }

        private void EmitVariableDeclaration(VariableDeclarationStatementNode variableDeclaration)
        {
            output.Append("let ");
            output.Append(variableDeclaration.VariableIdentifier);

            if (variableDeclaration.AssignedExpressionNode != null)
            {
                output.Append('=');
                EmitExpression(variableDeclaration.AssignedExpressionNode);
            }

            output.Append(';');
        }
    }
}
