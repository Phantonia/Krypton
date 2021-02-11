using Krypton.Analysis.Ast.Expressions;
using Krypton.Analysis.Ast.Symbols;
using Krypton.Analysis.Errors;
using Krypton.Analysis.Semantical.IdentifierMaps;
using Krypton.Framework;
using System;
using System.Diagnostics;

namespace Krypton.Analysis.Semantical
{
    partial class Binder
    {
        private TypeSymbolNode? BindInExpression(ExpressionNode expression,
                                                 VariableIdentifierMap variableIdentifierMap)
        {
            switch (expression)
            {
                case BinaryOperationExpressionNode binaryOperation:
                    return BindInBinaryOperation(binaryOperation, variableIdentifierMap);
                case FunctionCallExpressionNode functionCall:
                    {
                        (TypeSymbolNode? type, bool success) = BindInFunctionCall(functionCall,
                                                                                variableIdentifierMap,
                                                                                expressionContext: true);

                        if (!success)
                        {
                            return null;
                        }

                        Debug.Assert(type != null);
                        return type;
                    }
                case IdentifierExpressionNode identifierExpression:
                    return BindIdentifierExpression(identifierExpression, variableIdentifierMap);
                case LiteralExpressionNode literal:
                    return typeManager[literal.AssociatedType];
                case UnaryOperationExpressionNode unaryOperation:
                    return BindInUnaryOperation(unaryOperation, variableIdentifierMap);
                default:
                    Debug.Fail(message: "A type was forgotten...");
                    return null;
            }
        }

        private TypeSymbolNode? BindInBinaryOperation(BinaryOperationExpressionNode binaryOperation,
                                                      VariableIdentifierMap variableIdentifierMap)
        {
            TypeSymbolNode? leftType = BindInExpression(binaryOperation.LeftOperandNode, variableIdentifierMap);

            if (leftType == null)
            {
                return null;
            }

            TypeSymbolNode? rightType = BindInExpression(binaryOperation.RightOperandNode, variableIdentifierMap);

            if (rightType == null)
            {
                return null;
            }

            Operator @operator = binaryOperation.Operator;

            if (!leftType.BinaryOperationNodes.TryGetValue(@operator, out BinaryOperationSymbolNode? declaredOperation))
            {
                throw new NotImplementedException("Notim: can't consider implicit conversions here yet");
            }

            if (!TypeIsCompatibleWith(leftType, declaredOperation.LeftOperandTypeNode, possiblyOffendingNode: binaryOperation))
            {
                return null;
            }

            if (!TypeIsCompatibleWith(rightType, declaredOperation.RightOperandTypeNode, possiblyOffendingNode: binaryOperation))
            {
                return null;
            }

            return declaredOperation.ReturnTypeNode;
        }

        private (TypeSymbolNode?, bool) BindInFunctionCall(FunctionCallExpressionNode functionCall,
                                                   VariableIdentifierMap variableIdentifierMap,
                                                   bool expressionContext)
        {
            if (functionCall.FunctionExpressionNode is not IdentifierExpressionNode identifierExpression)
            {
                ErrorProvider.ReportError(ErrorCode.CanOnlyCallFunctions, Compilation, functionCall);
                return (null, false);
            }

            SymbolNode? symbol = GetExpressionSymbol(identifierExpression.Identifier,
                                                     variableIdentifierMap);

            if (symbol == null)
            {
                return (null, false);
            }

            if (symbol is not FunctionSymbolNode functionSymbol)
            {
                ErrorProvider.ReportError(ErrorCode.CanOnlyCallFunctions, Compilation, functionCall);
                return (null, false);
            }

            identifierExpression.Bind(symbol);

            if (expressionContext && functionSymbol.ReturnTypeNode == null)
            {
                ErrorProvider.ReportError(ErrorCode.OnlyFunctionWithReturnTypeCanBeExpression, Compilation, functionCall);
                return (null, false);
            }

            if (functionSymbol.ParameterNodes.Count != functionCall.ArgumentNodes.Count)
            {
                ErrorProvider.ReportError(ErrorCode.WrongNumberOfArguments,
                                          Compilation,
                                          functionCall,
                                          $"Expected number of arguments: {functionSymbol.ParameterNodes.Count}",
                                          $"Provided number of arguments: {functionCall.ArgumentNodes.Count}");
                return (null, false);
            }

            for (int i = 0; i < functionSymbol.ParameterNodes.Count; i++)
            {
                TypeSymbolNode? argumentType = BindInExpression(functionCall.ArgumentNodes[i], variableIdentifierMap);

                if (argumentType == null)
                {
                    return (null, false);
                }

                if (!TypeIsCompatibleWith(argumentType, functionSymbol.ParameterNodes[i].TypeNode, functionCall))
                {
                    return (null, false);
                }
            }

            return (functionSymbol.ReturnTypeNode, true);
        }

        private TypeSymbolNode? BindIdentifierExpression(IdentifierExpressionNode identifierExpression,
                                                         VariableIdentifierMap variableIdentifierMap)
        {
            SymbolNode? symbol = GetExpressionSymbol(identifierExpression.Identifier, variableIdentifierMap);

            if (symbol == null)
            {
                ErrorProvider.ReportError(ErrorCode.NoVariableOfThisNameInScope,
                                          Compilation,
                                          identifierExpression,
                                          $"Name: {identifierExpression.Identifier}");
                return null;
            }

            identifierExpression.Bind(symbol);

            switch (symbol)
            {
                case VariableSymbolNode variable:
                    {
                        TypeSymbolNode? variableType = variable.TypeNode;
                        Debug.Assert(variableType != null);
                        return variableType;
                    }
                case ConstantSymbolNode constant:
                    return constant.Type;
                case FunctionSymbolNode:
                    ErrorProvider.ReportError(ErrorCode.FunctionNotValidInContext,
                                              Compilation,
                                              identifierExpression);
                    return null;
                default:
                    Debug.Fail(message: "A type was forgotten...");
                    return null;
            }
        }

        private TypeSymbolNode? BindInUnaryOperation(UnaryOperationExpressionNode unaryOperation,
                                                     VariableIdentifierMap variableIdentifierMap)
        {
            TypeSymbolNode? operandType = BindInExpression(unaryOperation.OperandNode, variableIdentifierMap);

            if (operandType == null)
            {
                return null;
            }

            Operator @operator = unaryOperation.Operator;

            if (!operandType.UnaryOperationNodes.TryGetValue(@operator, out UnaryOperationSymbolNode? declaredOperation))
            {
                throw new NotImplementedException("Notim: can't consider implicit conversions here yet");
            }

            if (!TypeIsCompatibleWith(operandType, declaredOperation.OperandTypeNode, possiblyOffendingNode: unaryOperation))
            {
                return null;
            }

            return declaredOperation.ReturnTypeNode;
        }
    }
}