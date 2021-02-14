using Krypton.Analysis.Ast;
using Krypton.Analysis.Ast.Declarations;
using Krypton.Analysis.Ast.Statements;
using Krypton.Analysis.Ast.Symbols;
using Krypton.Analysis.Errors;
using Krypton.Analysis.Semantical.IdentifierMaps;
using Krypton.Framework;
using System.Diagnostics;

namespace Krypton.Analysis.Semantical
{
    partial class Binder
    {
        private bool BindInStatement(StatementNode statement,
                                     VariableIdentifierMap variableIdentifierMap)
        {
            switch (statement)
            {
                case BlockStatementNode blockStatement:
                    return BindInStatementBlock(blockStatement.StatementNodes, variableIdentifierMap);
                case FunctionCallStatementNode functionCall:
                    {
                        (_, bool success) = BindInFunctionCall(functionCall.UnderlyingFunctionCallExpressionNode,
                                                               variableIdentifierMap,
                                                               expressionContext: false);
                        return success;
                    }
                case ForStatementNode forStatement:
                    return BindInForStatement(forStatement, variableIdentifierMap);
                case IfStatementNode ifStatement:
                    return BindInIfStatement(ifStatement, variableIdentifierMap);
                case LoopControlStatementNode loopControlStatement:
                    return BindInLoopControlStatement(loopControlStatement);
                case ReturnStatementNode returnStatement:
                    return BindInReturnStatement(returnStatement, variableIdentifierMap);
                case VariableAssignmentStatementNode variableAssignment:
                    return BindInVariableAssignment(variableAssignment, variableIdentifierMap);
                case VariableDeclarationStatementNode variableDeclaration:
                    return BindInVariableDeclaration(variableDeclaration, variableIdentifierMap);
                case WhileStatementNode whileStatement:
                    return BindInWhileStatement(whileStatement, variableIdentifierMap);
                default:
                    Debug.Fail(message: "A type was forgotten...");
                    return false;
            }
        }

        private bool BindInStatementBlock(StatementCollectionNode statements,
                                          VariableIdentifierMap variableIdentifierMap)
        {
            variableIdentifierMap.EnterBlock();

            foreach (StatementNode statement in statements)
            {
                bool success = BindInStatement(statement,
                                               variableIdentifierMap);

                if (!success)
                {
                    return false;
                }
            }

            variableIdentifierMap.LeaveBlock();

            return true;
        }

        private bool BindInForStatement(ForStatementNode forStatement, VariableIdentifierMap variableIdentifierMap)
        {
            variableIdentifierMap.EnterBlock();

            TypeSymbolNode iterationVariableType;

            if (forStatement.DeclaresNew)
            {
                Debug.Assert(forStatement.InitialValueNode != null);

                TypeSymbolNode? assignedValueType = BindInExpression(forStatement.InitialValueNode, variableIdentifierMap);

                if (assignedValueType == null)
                {
                    return false;
                }

                iterationVariableType = assignedValueType;

                VariableSymbolNode variable = forStatement.CreateVariable(iterationVariableType);

                variableIdentifierMap.AddSymbol(forStatement.VariableIdentifier, variable);
            }
            else
            {
                SymbolNode? symbol = GetExpressionSymbol(forStatement.VariableIdentifier, variableIdentifierMap);

                if (symbol == null)
                {
                    ErrorProvider.ReportError(ErrorCode.CantFindIdentifierInScope,
                                              Compilation,
                                              forStatement.VariableIdentifierNode);
                    return false;
                }

                if (symbol is not VariableSymbolNode variable)
                {
                    ErrorProvider.ReportError(ErrorCode.ForNotVariable,
                                              Compilation,
                                              forStatement.VariableIdentifierNode,
                                              $"Identifier: {forStatement.VariableIdentifier}");
                    return false;
                }

                Debug.Assert(variable.TypeNode != null);
                iterationVariableType = variable.TypeNode;

                if (forStatement.InitialValueNode != null)
                {
                    TypeSymbolNode? assignedValueType = BindInExpression(forStatement.InitialValueNode, variableIdentifierMap);

                    if (assignedValueType == null)
                    {
                        return false;
                    }

                    if (!TypeIsCompatibleWith(assignedValueType, iterationVariableType, possiblyOffendingNode: forStatement.InitialValueNode))
                    {
                        return false;
                    }
                }
            }

            {
                TypeSymbolNode? intType = typeManager[FrameworkType.Int];
                TypeSymbolNode? rationalType = typeManager[FrameworkType.Rational];
                TypeSymbolNode? complexType = typeManager[FrameworkType.Complex];

                if (iterationVariableType != intType
                    && iterationVariableType != rationalType
                    && iterationVariableType != complexType)
                {
                    ErrorProvider.ReportError(ErrorCode.ForIterationVariableHasToBeNumberType,
                                              Compilation,
                                              forStatement.VariableIdentifierNode,
                                              $"Type of iteration variable: {iterationVariableType.Identifier}");
                    return false;
                }
            }

            if (forStatement.ConditionNode != null)
            {
                TypeSymbolNode? conditionType = BindInExpression(forStatement.ConditionNode, variableIdentifierMap);

                Debug.Assert(conditionType == typeManager[FrameworkType.Bool]);
            }

            if (forStatement.WithExpressionNode != null)
            {
                TypeSymbolNode? withType = BindInExpression(forStatement.WithExpressionNode, variableIdentifierMap);

                if (withType == null)
                {
                    return false;
                }

                if (!TypeIsCompatibleWith(withType, iterationVariableType, possiblyOffendingNode: forStatement.WithExpressionNode))
                {
                    return false;
                }
            }

            bool success = BindInStatementBlock(forStatement.StatementNodes, variableIdentifierMap);

            if (!success)
            {
                return false;
            }

            variableIdentifierMap.LeaveBlock();

            return true;
        }

        private bool BindInIfStatement(IfStatementNode ifStatement, VariableIdentifierMap variableIdentifierMap)
        {
            TypeSymbolNode? ifConditionType = BindInExpression(ifStatement.ConditionNode, variableIdentifierMap);

            if (ifConditionType == null)
            {
                return false;
            }

            TypeSymbolNode boolType = typeManager[FrameworkType.Bool];

            if (!TypeIsCompatibleWith(ifConditionType, boolType, possiblyOffendingNode: ifStatement.ConditionNode))
            {
                return false;
            }

            bool success = BindInStatementBlock(ifStatement.StatementNodes, variableIdentifierMap);

            if (!success)
            {
                return false;
            }

            foreach (ElseIfPartNode elseIfPart in ifStatement.ElseIfPartNodes)
            {
                TypeSymbolNode? elseIfConditionType = BindInExpression(elseIfPart.ConditionNode, variableIdentifierMap);

                if (elseIfConditionType == null)
                {
                    return false;
                }

                success = BindInStatementBlock(elseIfPart.StatementNodes, variableIdentifierMap);

                if (!success)
                {
                    return false;
                }
            }

            success = true;

            if (ifStatement.ElsePartNode != null)
            {
                success = BindInStatementBlock(ifStatement.ElsePartNode.StatementNodes, variableIdentifierMap);
            }

            return success;
        }

        private bool BindInLoopControlStatement(LoopControlStatementNode loopControlStatement)
        {
            ushort level = loopControlStatement.Level;

            Node? parent = loopControlStatement;

            while (level > 0)
            {
                parent = parent.ParentNode;

                switch (parent)
                {
                    case null:
                        ErrorProvider.ReportError(ErrorCode.LoopControlStatementNotThatDeep,
                                                  Compilation,
                                                  loopControlStatement);
                        return false;
                    case ILoopNode:
                        level--;
                        break;
                }
            }

            ILoopNode? loop = parent as ILoopNode;
            Debug.Assert(loop != null);

            loopControlStatement.SetControlledLoop(loop);

            return true;
        }

        private bool BindInReturnStatement(ReturnStatementNode returnStatement, VariableIdentifierMap variableIdentifierMap)
        {
            TypeSymbolNode? returnedType = null;

            if (returnStatement.ReturnExpressionNode != null)
            {
                returnedType = BindInExpression(returnStatement.ReturnExpressionNode, variableIdentifierMap);

                if (returnedType == null)
                {
                    return false;
                }
            }

            IReturnableNode functionOrProgram = FindReturnableParent(returnStatement);

            if (!typeManager.TryGetTypeSymbol(functionOrProgram.ReturnTypeNode,
                                              out TypeSymbolNode? actualReturnType))
            {
                return false;
            }

            if (returnedType == null)
            {
                if (actualReturnType != null)
                {
                    string functionName = functionOrProgram is FunctionDeclarationNode declaration
                                        ? declaration.Identifier
                                        : "<Program>";

                    ErrorProvider.ReportError(ErrorCode.ReturnedNoValueEvenThoughFunctionShouldReturn,
                                              Compilation,
                                              (Node?)returnStatement.ReturnExpressionNode ?? returnStatement,
                                              $"Function: {functionName}");
                    return false;
                }

                return true;
            }
            else if (actualReturnType == null)
            {
                string functionName = functionOrProgram is FunctionDeclarationNode declaration
                                    ? declaration.Identifier
                                    : "<Program>";

                ErrorProvider.ReportError(ErrorCode.ReturnedValueEvenThoughFunctionDoesNotHaveReturnType,
                                          Compilation,
                                          (Node?)returnStatement.ReturnExpressionNode ?? returnStatement,
                                          $"Function: {functionName}");
                return false;
            }

            if (!TypeIsCompatibleWith(returnedType,
                                      actualReturnType,
                                      possiblyOffendingNode: returnStatement.ReturnExpressionNode!))
            {
                return false;
            }

            return true;
        }

        private bool BindInVariableAssignment(VariableAssignmentStatementNode variableAssignment,
                                              VariableIdentifierMap variableIdentifierMap)
        {
            TypeSymbolNode? assignedType = BindInExpression(variableAssignment.AssignedExpressionNode,
                                                            variableIdentifierMap);

            if (assignedType == null)
            {
                return false;
            }

            if (!variableIdentifierMap.TryGet(variableAssignment.VariableIdentifier,
                                              out VariableSymbolNode? variable))
            {
                ErrorProvider.ReportError(ErrorCode.CantAssignUndeclaredVariable,
                                          Compilation,
                                          variableAssignment,
                                          $"Variable name: {variableAssignment.VariableIdentifier}");
                return false;
            }

            if (variable.IsReadOnly)
            {
                ErrorProvider.ReportError(ErrorCode.CantReAssignReadOnlyVariable,
                                          Compilation,
                                          variableAssignment);
                return false;
            }

            if (!TypeIsCompatibleWith(assignedType, variable.TypeNode, possiblyOffendingNode: variableAssignment.AssignedExpressionNode))
            {
                return false;
            }

            variableAssignment.Bind(variable);

            return true;
        }

        private bool BindInVariableDeclaration(VariableDeclarationStatementNode variableDeclaration,
                                               VariableIdentifierMap variableIdentifierMap)
        {
            if (variableIdentifierMap.TryGet(variableDeclaration.VariableIdentifier, out VariableSymbolNode? _))
            {
                ErrorProvider.ReportError(ErrorCode.CantRedeclareVariable,
                                          Compilation,
                                          variableDeclaration.VariableIdentifierNode);
                return false;
            }

            VariableSymbolNode variable;

            if (variableDeclaration.AssignedExpressionNode != null)
            {
                TypeSymbolNode? assignedType = BindInExpression(variableDeclaration.AssignedExpressionNode,
                                                                variableIdentifierMap);

                if (assignedType == null)
                {
                    return false;
                }

                if (variableDeclaration.TypeSpecNode == null)
                {
                    variable = variableDeclaration.CreateVariable(typeSymbol: assignedType);
                }
                else
                {
                    if (!typeManager.TryGetTypeSymbol(variableDeclaration.TypeSpecNode, out TypeSymbolNode? variableType))
                    {
                        return false;
                    }
                    else if (!TypeIsCompatibleWith(assignedType, variableType, variableDeclaration.AssignedExpressionNode))
                    {
                        return false;
                    }
                    else
                    {
                        variable = variableDeclaration.CreateVariable(variableType);
                    }
                }
            }
            else
            {
                if (typeManager.TryGetTypeSymbol(variableDeclaration.TypeSpecNode, out TypeSymbolNode? variableType))
                {
                    variable = variableDeclaration.CreateVariable(variableType);
                }
                else
                {
                    return false;
                }
            }

            variableIdentifierMap.AddSymbol(variableDeclaration.VariableIdentifier, variable);

            return true;
        }

        private bool BindInWhileStatement(WhileStatementNode whileStatement,
                                          VariableIdentifierMap variableIdentifierMap)
        {
            TypeSymbolNode? conditionType = BindInExpression(whileStatement.ConditionNode, variableIdentifierMap);

            if (conditionType == null)
            {
                return false;
            }

            TypeSymbolNode? boolType = typeManager[FrameworkType.Bool];

            if (!TypeIsCompatibleWith(conditionType, boolType, possiblyOffendingNode: whileStatement.ConditionNode))
            {
                return false;
            }

            bool success = BindInStatementBlock(whileStatement.StatementNodes, variableIdentifierMap);
            return success;
        }

        private static IReturnableNode FindReturnableParent(StatementNode statement)
        {
            Node? parent = statement;

            while (true)
            {
                parent = parent?.ParentNode;

                Debug.Assert(parent is StatementCollectionNode);

                parent = parent.ParentNode;

                if (parent is IReturnableNode returnable)
                {
                    return returnable;
                }
            }
        }
    }
}