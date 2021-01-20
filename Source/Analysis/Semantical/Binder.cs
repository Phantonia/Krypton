using Krypton.Analysis.Ast;
using Krypton.Analysis.Ast.Expressions;
using Krypton.Analysis.Ast.Statements;
using Krypton.Analysis.Ast.Symbols;
using Krypton.Analysis.Semantical.IdentifierMaps;

namespace Krypton.Analysis.Semantical
{
    public sealed class Binder
    {
        public Binder(SyntaxTree syntaxTree)
        {
            this.syntaxTree = syntaxTree;
            typeManager = new TypeManager(syntaxTree);
        }

        private readonly SyntaxTree syntaxTree;
        private readonly TypeManager typeManager;

        public bool PerformBinding()
        {
            HoistedIdentifierMap globalIdentifierMap = GatherGlobalSymbols();

            bool success = BindInTopLevelStatements(globalIdentifierMap);

            if (!success)
            {
                return false;
            }

            return true;
        }

        bool BindInBlock(StatementCollectionNode statements, VariableIdentifierMap variableIdentifierMap, HoistedIdentifierMap globalIdentifierMap)
        {
            variableIdentifierMap.EnterBlock();

            foreach (StatementNode stmt in statements)
            {
                bool success = BindInStatement(stmt, variableIdentifierMap, globalIdentifierMap);

                if (!success)
                {
                    return false;
                }
            }

            variableIdentifierMap.LeaveBlock();

            return true;
        }

        private static bool BindInExpression(ExpressionNode expression, VariableIdentifierMap variableIdentifierMap, HoistedIdentifierMap globalIdentifierMap)
        {
            switch (expression)
            {
                case UnaryOperationExpressionNode unaryOperation:
                    {
                        bool success = BindInExpression(unaryOperation.Operand, variableIdentifierMap, globalIdentifierMap);

                        if (!success)
                        {
                            return false;
                        }
                    }
                    break;
                case BinaryOperationExpressionNode binaryOperation:
                    {
                        bool success = BindInExpression(binaryOperation.Left, variableIdentifierMap, globalIdentifierMap)
                                    && BindInExpression(binaryOperation.Right, variableIdentifierMap, globalIdentifierMap);

                        if (!success)
                        {
                            return false;
                        }
                    }
                    break;
                case FunctionCallExpressionNode funcCall:
                    {
                        bool success = BindInExpression(funcCall.FunctionExpression, variableIdentifierMap, globalIdentifierMap);

                        if (!success)
                        {
                            return false;
                        }

                        if (funcCall.Arguments != null)
                        {
                            foreach (ExpressionNode argument in funcCall.Arguments)
                            {
                                success = BindInExpression(argument, variableIdentifierMap, globalIdentifierMap);

                                if (!success)
                                {
                                    return false;
                                }
                            }
                        }
                    }
                    break;
                case IdentifierExpressionNode idExpression:
                    {
                        if (variableIdentifierMap.TryGet(idExpression.Identifier, out SymbolNode? symbol)
                           || globalIdentifierMap.TryGet(idExpression.Identifier, out symbol))
                        {
                            idExpression.Bind(symbol);
                        }
                    }
                    break;
            }

            return true;
        }

        private bool BindInStatement(StatementNode statement, VariableIdentifierMap variableIdentifierMap, HoistedIdentifierMap globalIdentifierMap)
        {
            switch (statement)
            {
                case VariableDeclarationStatementNode variableDeclaration:
                    {
                        if (variableDeclaration.AssignedValue != null)
                        {
                            bool success = BindInExpression(variableDeclaration.AssignedValue, variableIdentifierMap, globalIdentifierMap);

                            if (!success)
                            {
                                return false;
                            }
                        }

                        if (!typeManager.TryGetTypeSymbol(variableDeclaration.Type, out TypeSymbolNode? typeSymbol))
                        {
                            return false;
                        }

                        variableIdentifierMap.AddSymbol(variableDeclaration.VariableIdentifier, variableDeclaration.CreateVariable(typeSymbol));
                    }
                    break;
                case VariableAssignmentStatementNode variableAssignment:
                    {
                        bool success = BindInExpression(variableAssignment.AssignedValue, variableIdentifierMap, globalIdentifierMap);

                        if (!success)
                        {
                            return false;
                        }

                        if (!variableIdentifierMap.TryGet(variableAssignment.VariableIdentifier, out LocalVariableSymbolNode? variable))
                        {
                            return false;
                        }
                        
                        variableAssignment.Bind(variable);
                    }
                    break;
                case FunctionCallStatementNode funcCall:
                    {
                        bool success = BindInExpression(funcCall.FunctionExpression, variableIdentifierMap, globalIdentifierMap);

                        if (!success)
                        {
                            return false;
                        }

                        if (funcCall.Arguments != null)
                        {
                            foreach (ExpressionNode argument in funcCall.Arguments)
                            {
                                success = BindInExpression(argument, variableIdentifierMap, globalIdentifierMap);

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
                        bool success = BindInBlock(blockStmt.Statements, variableIdentifierMap, globalIdentifierMap);

                        if (!success)
                        {
                            return false;
                        }
                    }
                    break;
                case WhileStatementNode whileStmt:
                    {
                        bool success = BindInExpression(whileStmt.Condition, variableIdentifierMap, globalIdentifierMap)
                                    && BindInBlock(whileStmt.Statements, variableIdentifierMap, globalIdentifierMap);

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
            return BindInBlock(syntaxTree.Root.TopLevelStatements, variableIdentifierMap, globalIdentifierMap);
        }

        private HoistedIdentifierMap GatherGlobalSymbols()
        {
            // Placeholder
            return new HoistedIdentifierMap();
        }
    }
}
