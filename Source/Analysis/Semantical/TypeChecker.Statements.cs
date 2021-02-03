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
        private bool CheckStatementCollection(StatementCollectionNode statementNodes)
        {
            foreach (StatementNode statementNode in statementNodes)
            {
                bool success = CheckStatement(statementNode);

                if (!success)
                {
                    return false;
                }
            }

            return true;
        }

        private bool CheckFunctionCallStatement(FunctionCallStatementNode statementNode)
        {
            (_, bool success) = CheckFunctionCallExpression(statementNode.UnderlyingFunctionCallExpressionNode, tolerateNoReturnType: true);
            return success;
        }

        private bool CheckStatement(StatementNode statementNode)
        {
            switch (statementNode)
            {
                case VariableAssignmentStatementNode variableAssignmentNode:
                    return CheckVariableAssignmentStatement(variableAssignmentNode);
                case VariableDeclarationStatementNode variableDeclaration when variableDeclaration.AssignedValue != null:
                    return CheckVariableDeclarationStatement(variableDeclaration);
                case VariableDeclarationStatementNode:
                    return true;
                case FunctionCallStatementNode functionCallNode:
                    return CheckFunctionCallStatement(functionCallNode);
                case BlockStatementNode blockNode:
                    return CheckStatementCollection(blockNode.StatementNodes);
                case WhileStatementNode whileNode:
                    return CheckWhileStatement(whileNode);
                default:
                    Debug.Fail(null);
                    return default;
            }
        }

        private bool CheckWhileStatement(WhileStatementNode whileNode)
        {
            TypeSymbolNode boolType = typeManager[FrameworkType.Bool];
            TypeSymbolNode? conditionType = CheckExpression(whileNode.ConditionNode);

            if (conditionType == null)
            {
                return false;
            }

            return TypeCompatibility.IsCompatibleWith(sourceType: conditionType, targetType: boolType)
                && CheckStatementCollection(whileNode.StatementNodes);
        }

        private bool CheckVariableAssignmentStatement(VariableAssignmentStatementNode variableAssignmentNode)
        {
            TypeSymbolNode? assignedType = CheckExpression(variableAssignmentNode.AssignedExpressionNode);

            if (assignedType == null)
            {
                return false;
            }

            VariableSymbolNode localVariable = variableAssignmentNode.VariableNode;

            if (TypeCompatibility.IsCompatibleWith(assignedType, localVariable.TypeNode))
            {
                return true;
            }
            else
            {
                throw new NotImplementedException("Error: wrong type");
            }
        }

        private bool CheckVariableDeclarationStatement(VariableDeclarationStatementNode variableDeclarationNode)
        {
            Debug.Assert(variableDeclarationNode.AssignedValue != null);

            TypeSymbolNode? assignedType = CheckExpression(variableDeclarationNode.AssignedValue);

            if (assignedType == null)
            {
                return false;
            }

            LocalVariableSymbolNode localVariable = variableDeclarationNode.VariableNode;

            if (TypeCompatibility.IsCompatibleWith(assignedType, localVariable.TypeNode))
            {
                if (localVariable.TypeNode == null)
                {
                    localVariable.SpecifyType(assignedType);
                }

                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
