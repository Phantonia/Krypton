using Krypton.Analysis.Ast;
using Krypton.Analysis.Ast.Statements;
using Krypton.Analysis.Ast.Symbols;
using Krypton.Framework;
using System;
using System.Diagnostics;

namespace Krypton.Analysis.Semantical
{
    partial class TypeChecker
    {
        private bool CheckStatementCollection(StatementCollectionNode statements)
        {
            foreach (StatementNode statement in statements)
            {
                bool success = CheckStatement(statement);

                if (!success)
                {
                    return false;
                }
            }

            return true;
        }

        private bool CheckFunctionCallStatement(FunctionCallStatementNode statement)
        {
            (_, bool success) = CheckFunctionCallExpression(statement.UnderlyingFunctionCallExpressionNode, tolerateNoReturnType: true);
            return success;
        }

        private bool CheckStatement(StatementNode statement)
        {
            switch (statement)
            {
                case VariableAssignmentStatementNode varAssignment:
                    return CheckVariableAssignmentStatement(varAssignment);
                case VariableDeclarationStatementNode varDecl when varDecl.AssignedValue != null:
                    return CheckVariableDeclarationStatement(varDecl);
                case VariableDeclarationStatementNode:
                    return true;
                case FunctionCallStatementNode funcCall:
                    return CheckFunctionCallStatement(funcCall);
                case BlockStatementNode block:
                    return CheckStatementCollection(block.Statements);
                case WhileStatementNode whileStmt:
                    return CheckWhileStatement(whileStmt);
                default:
                    Debug.Fail(null);
                    return default;
            }
        }

        private bool CheckWhileStatement(WhileStatementNode whileStmt)
        {
            TypeSymbolNode boolType = typeManager[FrameworkType.Bool];
            TypeSymbolNode? conditionType = CheckExpression(whileStmt.Condition);

            if (conditionType == null)
            {
                return false;
            }

            return TypeCompatibility.IsCompatibleWith(sourceType: conditionType, targetType: boolType)
                && CheckStatementCollection(whileStmt.Statements);
        }

        private bool CheckVariableAssignmentStatement(VariableAssignmentStatementNode varAssignment)
        {
            TypeSymbolNode? assignedType = CheckExpression(varAssignment.AssignedValue);

            if (assignedType == null)
            {
                return false;
            }

            VariableSymbolNode localVariable = varAssignment.VariableNode;

            if (TypeCompatibility.IsCompatibleWith(assignedType, localVariable.Type))
            {
                return true;
            }
            else
            {
                throw new NotImplementedException("Error: wrong type");
            }
        }

        private bool CheckVariableDeclarationStatement(VariableDeclarationStatementNode varDecl)
        {
            Debug.Assert(varDecl.AssignedValue != null);

            TypeSymbolNode? assignedType = CheckExpression(varDecl.AssignedValue);

            if (assignedType == null)
            {
                return false;
            }

            LocalVariableSymbolNode localVariable = varDecl.VariableNode;

            if (TypeCompatibility.IsCompatibleWith(assignedType, localVariable.Type))
            {
                if (localVariable.Type == null)
                {
                    localVariable.SpecifyType(assignedType);
                }

                return true;
            }
            else
            {
                throw new NotImplementedException("Error: wrong type");
            }
        }
    }
}
