﻿using Krypton.Analysis.Ast.Expressions;
using Krypton.Analysis.Ast.Symbols;
using Krypton.Analysis.Errors;
using Krypton.Framework;
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
                case PropertyGetExpressionNode propertyGet:
                    return BindInPropertyGet(propertyGet, variableIdentifierMap);
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

            BinaryOperationSymbolNode? operationSymbol = FindBestBinaryOperation(@operator,
                                                                                 leftType,
                                                                                 rightType,
                                                                                 out ImplicitConversionSymbolNode? implicitConversionLeft,
                                                                                 out ImplicitConversionSymbolNode? implicitConversionRight);

            if (implicitConversionLeft != null)
            {
                binaryOperation.LeftOperandNode.SpecifyImplicitConversion(implicitConversionLeft);
            }

            if (implicitConversionRight != null)
            {
                binaryOperation.RightOperandNode.SpecifyImplicitConversion(implicitConversionRight);
            }

            if (operationSymbol == null)
            {
                ErrorProvider.ReportError(ErrorCode.OperatorNotAvailableForTypes,
                                          Compilation,
                                          binaryOperation,
                                          $"Left type: {leftType.Identifier}",
                                          $"Right type: {rightType.Identifier}");
                return null;
            }

            binaryOperation.Bind(operationSymbol);

            return operationSymbol.ReturnTypeNode;
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
                ErrorProvider.ReportError(ErrorCode.CantFindIdentifierInScope,
                                          Compilation,
                                          identifierExpression);
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

                if (!TypeIsCompatibleWith(argumentType,
                                          functionSymbol.ParameterNodes[i].TypeNode,
                                          functionCall.ArgumentNodes[i],
                                          out ImplicitConversionSymbolNode? conversion))
                {
                    return (null, false);
                }

                if (conversion != null)
                {
                    functionCall.ArgumentNodes[i].SpecifyImplicitConversion(conversion);
                }
            }

            functionCall.Bind(functionSymbol);

            return (functionSymbol.ReturnTypeNode, true);
        }

        private TypeSymbolNode? BindIdentifierExpression(IdentifierExpressionNode identifierExpression,
                                                         VariableIdentifierMap variableIdentifierMap)
        {
            SymbolNode? symbol = GetExpressionSymbol(identifierExpression.Identifier, variableIdentifierMap);

            if (symbol == null)
            {
                ErrorProvider.ReportError(ErrorCode.CantFindIdentifierInScope,
                                          Compilation,
                                          identifierExpression);
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

        private TypeSymbolNode? BindInPropertyGet(PropertyGetExpressionNode propertyGet,
                                                  VariableIdentifierMap variableIdentifierMap)
        {
            TypeSymbolNode? expressionType = BindInExpression(propertyGet.ExpressionNode, variableIdentifierMap);

            if (expressionType == null)
            {
                return null;
            }

            if (!expressionType.PropertyNodes.TryGetValue(propertyGet.PropertyIdentifier, out PropertySymbolNode? property))
            {
                ErrorProvider.ReportError(ErrorCode.PropertyDoesNotExistInType,
                                          Compilation,
                                          propertyGet.PropertyIdentifierNode,
                                          $"Type: {expressionType.Identifier}");
                return null;
            }

            propertyGet.Bind(property);

            return property.TypeNode;
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

            UnaryOperationSymbolNode? operationSymbol = FindBestUnaryOperation(@operator,
                                                                               operandType,
                                                                               out ImplicitConversionSymbolNode? conversion);

            if (operationSymbol == null)
            {
                ErrorProvider.ReportError(ErrorCode.OperatorNotAvailableForTypes,
                                          Compilation,
                                          unaryOperation,
                                          $"Operand type: {operandType.Identifier}");
                return null;
            }

            if (conversion != null)
            {
                unaryOperation.OperandNode.SpecifyImplicitConversion(conversion);
            }

            unaryOperation.Bind(operationSymbol);

            return operationSymbol.ReturnTypeNode;
        }
    }
}