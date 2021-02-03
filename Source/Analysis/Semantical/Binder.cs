using Krypton.Analysis.Ast;
using Krypton.Analysis.Ast.Expressions;
using Krypton.Analysis.Ast.Statements;
using Krypton.Analysis.Ast.Symbols;
using Krypton.Analysis.Semantical.IdentifierMaps;
using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Krypton.Analysis.Semantical
{
    public sealed class Binder
    {
        public Binder(Compilation compilation)
        {
            Compilation = compilation;
        }

        public Compilation Compilation { get; }

        public TypeManager? TypeManager { get; set; }

        [MemberNotNull(nameof(TypeManager))]
        public bool PerformBinding()
        {
            (HoistedIdentifierMap globalIdentifierMap, TypeIdentifierMap typeIdentifierMap) = GatherGlobalSymbols();
            TypeManager = new TypeManager(Compilation, typeIdentifierMap);

            bool success = BindInTopLevelStatements(globalIdentifierMap);

            if (!success)
            {
                return false;
            }

            return true;
        }

        private bool BindInBlock(StatementCollectionNode statementNodes, VariableIdentifierMap variableIdentifierMap, HoistedIdentifierMap globalIdentifierMap)
        {
            variableIdentifierMap.EnterBlock();

            foreach (StatementNode statementNode in statementNodes)
            {
                bool success = BindInStatement(statementNode, variableIdentifierMap, globalIdentifierMap);

                if (!success)
                {
                    return false;
                }
            }

            variableIdentifierMap.LeaveBlock();

            return true;
        }

        private static bool BindInExpression(ExpressionNode expressionNode, VariableIdentifierMap variableIdentifierMap, HoistedIdentifierMap globalIdentifierMap)
        {
            switch (expressionNode)
            {
                case UnaryOperationExpressionNode unaryOperationNode:
                    {
                        bool success = BindInExpression(unaryOperationNode.OperandNode, variableIdentifierMap, globalIdentifierMap);

                        if (!success)
                        {
                            return false;
                        }
                    }
                    break;
                case BinaryOperationExpressionNode binaryOperationNode:
                    {
                        bool success = BindInExpression(binaryOperationNode.LeftOperandNode, variableIdentifierMap, globalIdentifierMap)
                                    && BindInExpression(binaryOperationNode.RightOperandNode, variableIdentifierMap, globalIdentifierMap);

                        if (!success)
                        {
                            return false;
                        }
                    }
                    break;
                case FunctionCallExpressionNode functionCallNode:
                    {
                        bool success = BindInExpression(functionCallNode.FunctionExpressionNode, variableIdentifierMap, globalIdentifierMap);

                        if (!success)
                        {
                            return false;
                        }

                        foreach (ExpressionNode argumentNode in functionCallNode.ArgumentNodes)
                        {
                            success = BindInExpression(argumentNode, variableIdentifierMap, globalIdentifierMap);

                            if (!success)
                            {
                                return false;
                            }
                        }
                    }
                    break;
                case IdentifierExpressionNode identifierExpressionNode:
                    {
                        if (variableIdentifierMap.TryGet(identifierExpressionNode.Identifier, out SymbolNode? symbolNode)
                           || globalIdentifierMap.TryGet(identifierExpressionNode.Identifier, out symbolNode))
                        {
                            identifierExpressionNode.Bind(symbolNode);
                        }
                    }
                    break;
            }

            return true;
        }

        private bool BindInStatement(StatementNode statementNode, VariableIdentifierMap variableIdentifierMap, HoistedIdentifierMap globalIdentifierMap)
        {
            switch (statementNode)
            {
                case VariableDeclarationStatementNode variableDeclarationNode:
                    {
                        if (variableDeclarationNode.AssignedValue != null)
                        {
                            bool success = BindInExpression(variableDeclarationNode.AssignedValue, variableIdentifierMap, globalIdentifierMap);

                            if (!success)
                            {
                                return false;
                            }
                        }

                        Debug.Assert(TypeManager != null);

                        if (!TypeManager.TryGetTypeSymbol(variableDeclarationNode.Type, out TypeSymbolNode? typeSymbolNode))
                        {
                            return false;
                        }

                        {
                            bool success = variableIdentifierMap.AddSymbol(variableDeclarationNode.VariableIdentifier, variableDeclarationNode.CreateVariable(typeSymbolNode));

                            if (!success)
                            {
                                throw new NotImplementedException("Error: Duplicate local variable");
                            }
                        }
                    }
                    break;
                case VariableAssignmentStatementNode variableAssignmentNode:
                    {
                        bool success = BindInExpression(variableAssignmentNode.AssignedExpressionNode, variableIdentifierMap, globalIdentifierMap);

                        if (!success)
                        {
                            return false;
                        }

                        if (!variableIdentifierMap.TryGet(variableAssignmentNode.VariableIdentifier, out LocalVariableSymbolNode? variableNode))
                        {
                            throw new NotImplementedException("Error: variable not declared");
                        }

                        variableAssignmentNode.Bind(variableNode);
                    }
                    break;
                case FunctionCallStatementNode functionCallNode:
                    {
                        bool success = BindInExpression(functionCallNode.FunctionExpressionNode, variableIdentifierMap, globalIdentifierMap);

                        if (!success)
                        {
                            return false;
                        }

                        if (functionCallNode.ArgumentNodes != null)
                        {
                            foreach (ExpressionNode argumentNode in functionCallNode.ArgumentNodes)
                            {
                                success = BindInExpression(argumentNode, variableIdentifierMap, globalIdentifierMap);

                                if (!success)
                                {
                                    return false;
                                }
                            }
                        }
                    }
                    break;
                case BlockStatementNode blockStmt:
                    {
                        bool success = BindInBlock(blockStmt.StatementNodes, variableIdentifierMap, globalIdentifierMap);

                        if (!success)
                        {
                            return false;
                        }
                    }
                    break;
                case WhileStatementNode whileStmt:
                    {
                        bool success = BindInExpression(whileStmt.ConditionNode, variableIdentifierMap, globalIdentifierMap)
                                    && BindInBlock(whileStmt.StatementNodes, variableIdentifierMap, globalIdentifierMap);

                        if (!success)
                        {
                            return false;
                        }
                    }
                    break;
            }

            return true;
        }

        private bool BindInTopLevelStatements(HoistedIdentifierMap globalIdentifierMap)
        {
            VariableIdentifierMap variableIdentifierMap = new();
            return BindInBlock(Compilation.Program.TopLevelStatementNodes, variableIdentifierMap, globalIdentifierMap);
        }

        private (HoistedIdentifierMap, TypeIdentifierMap) GatherGlobalSymbols()
        {
            HoistedIdentifierMap globalIdentifierMap = new();
            TypeIdentifierMap typeIdentifierMap = new();

            FrameworkIntegration.PopulateWithFrameworkSymbols(globalIdentifierMap, typeIdentifierMap);

            return (globalIdentifierMap, typeIdentifierMap);
        }
    }
}
