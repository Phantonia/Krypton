using Krypton.Analysis.Ast;
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
                case IfStatementNode ifStatement:
                    EmitIfStatement(ifStatement);
                    break;
                case ReturnStatementNode returnStatement:
                    EmitReturnStatement(returnStatement);
                    break;
                case VariableAssignmentStatementNode variableAssignment:
                    EmitVariableAssignment(variableAssignment);
                    break;
                case VariableDeclarationStatementNode variableDeclaration:
                    EmitVariableDeclaration(variableDeclaration);
                    break;
                case WhileStatementNode whileStatement:
                    EmitWhileStatement(whileStatement);
                    break;
                default:
                    Debug.Fail(message: null);
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

        private void EmitIfStatement(IfStatementNode ifStatement)
        {
            output.Append("if(");
            EmitExpression(ifStatement.ConditionNode);
            output.Append(')');
            EmitStatementBlock(ifStatement.StatementNodes);

            foreach (ElseIfPartNode elseIfPart in ifStatement.ElseIfPartNodes)
            {
                output.Append("else if(");
                EmitExpression(elseIfPart.ConditionNode);
                output.Append(')');
                EmitStatementBlock(elseIfPart.StatementNodes);
            }

            if (ifStatement.ElsePartNode != null)
            {
                output.Append("else");
                EmitStatementBlock(ifStatement.ElsePartNode.StatementNodes);
            }
        }

        private void EmitReturnStatement(ReturnStatementNode returnStatement)
        {
            output.Append("return");

            if (returnStatement.ReturnExpressionNode != null)
            {
                output.Append(' ');
                EmitExpression(returnStatement.ReturnExpressionNode);
            }

            output.Append(';');
        }

        private void EmitVariableAssignment(VariableAssignmentStatementNode variableAssignment)
        {
            output.Append(variableAssignment.VariableIdentifier);
            output.Append('=');
            EmitExpression(variableAssignment.AssignedExpressionNode);
            output.Append(';');
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

        private void EmitWhileStatement(WhileStatementNode whileStatement)
        {
            if (whileStatement.IsLeftOrContinued())
            {
                // There exists a Leave or Continue statement targeting this loop.
                // We need to add a label so that we can emit a correct break or
                // continue statement.
                output.Append("$loop_").Append(globalLoopCount).Append(':');
                associatedLoopIds[whileStatement] = globalLoopCount;
                globalLoopCount++;
            }

            output.Append("while(");
            EmitExpression(whileStatement.ConditionNode);
            output.Append(')');
            EmitStatementBlock(whileStatement.StatementNodes);
        }
    }
}
