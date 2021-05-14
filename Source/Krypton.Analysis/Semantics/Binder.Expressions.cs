//using Krypton.CompilationData;
//using Krypton.CompilationData.Symbols;
//using Krypton.CompilationData.Syntax.Expressions;
//using System;
//using System.Collections.Generic;
//using System.Collections.Immutable;
//using System.Diagnostics;

//namespace Krypton.Analysis.Semantics
//{
//    partial class Binder
//    {
//        private TypedExpressionNode? BindExpression(ExpressionNode expression, VariableIdentifierMap variableIdentifierMap)
//        {
//            switch (expression)
//            {
//                case BinaryOperationExpressionNode binaryOperation:
//                    return BindBinaryOperation(binaryOperation, variableIdentifierMap);
//                //case InvocationExpressionNode:
//                case IdentifierExpressionNode identifierExpression:
//                    return BindIdentifierExpression(identifierExpression, variableIdentifierMap);
//                case LiteralExpressionNode literal:
//                    return BindLiteral(literal);
//                case PropertyGetAccessExpressionNode propertyGetAccess:
//                    return BindPropertyGetAccess(propertyGetAccess, variableIdentifierMap);
//                case UnaryOperationExpressionNode unaryOperation:
//                    return BindUnaryOperation(unaryOperation, variableIdentifierMap);
//            }
//        }

//        private TypedExpressionNode? BindBinaryOperation(BinaryOperationExpressionNode binaryOperation, VariableIdentifierMap variableIdentifierMap)
//        {
//            TypedExpressionNode? typedLeftOperand = BindExpression(binaryOperation.LeftOperandNode, variableIdentifierMap);

//            if (typedLeftOperand == null)
//            {
//                return null;
//            }

//            TypedExpressionNode? typedRightOperand = BindExpression(binaryOperation.RightOperandNode, variableIdentifierMap);

//            if (typedRightOperand == null)
//            {
//                return null;
//            }

//            Operator @operator = binaryOperation.Operator;

//            BinaryOperationSymbol? operationSymbol = FindBestBinaryOperation(@operator, ref typedLeftOperand, ref typedRightOperand);

//            if (operationSymbol == null)
//            {
//                throw new NotImplementedException();
//                //ErrorProvider.ReportError(ErrorCode.OperatorNotAvailableForTypes,
//                //                          Compilation,
//                //                          binaryOperation,
//                //                          $"Left type: {leftType.Identifier}",
//                //                          $"Right type: {rightType.Identifier}");
//                //return null;
//            }

//            return (binaryOperation with
//            {
//                LeftOperandNode = typedLeftOperand,
//                RightOperandNode = typedRightOperand
//            }).Bind(operationSymbol)
//              .Type(operationSymbol.ReturnTypeSymbol);
//        }

//        private TypedExpressionNode? BindIdentifierExpression(IdentifierExpressionNode identifierExpression,
//                                                              VariableIdentifierMap variableIdentifierMap)
//        {
//            Symbol? symbol = GetExpressionSymbol(identifierExpression.IdentifierToken, variableIdentifierMap);

//            if (symbol == null)
//            {
//                throw new NotImplementedException();
//                //ErrorProvider.ReportError(ErrorCode.CantFindIdentifierInScope,
//                //                          Compilation,
//                //                          identifierExpression);
//                //return null;
//            }

//            BoundIdentifierExpressionNode boundIdentifierExpression = identifierExpression.Bind(symbol);

//            TypeSymbol identifierType;

//            switch (symbol)
//            {
//                case VariableSymbol variable:
//                    identifierType = variable.TypeSymbol;
//                    break;
//                case ConstantSymbol constant:
//                    identifierType = constant.TypeSymbol;
//                    break;
//                case FunctionSymbol function:
//                    {
//                        throw new NotImplementedException();
//                        //ErrorProvider.ReportError(ErrorCode.FunctionNotValidInContext,
//                        //                      Compilation,
//                        //                      identifierExpression);
//                        //return null;
//                    }
//                default:
//                    Debug.Fail(message: null);
//                    return null;
//            }

//            return boundIdentifierExpression.Type(identifierType);
//        }

//        private TypedExpressionNode? BindInvocation(InvocationExpressionNode invocation,
//                                                    VariableIdentifierMap variableIdentifierMap,
//                                                    bool expressionContext)
//        {
//            if (invocation.InvokeeNode is not IdentifierExpressionNode identifierExpression)
//            {
//                throw new NotImplementedException();
//                //ErrorProvider.ReportError(ErrorCode.CanOnlyCallFunctions, Compilation, functionCall);
//                //return (null, false);
//            }

//            Symbol? symbol = GetExpressionSymbol(identifierExpression.IdentifierToken, variableIdentifierMap);

//            if (symbol == null)
//            {
//                throw new NotImplementedException();
//                //ErrorProvider.ReportError(ErrorCode.CantFindIdentifierInScope,
//                //                          Compilation,
//                //                          identifierExpression);
//                //return (null, false);
//            }

//            if (symbol is not FunctionSymbol functionSymbol)
//            {
//                throw new NotImplementedException();
//                //ErrorProvider.ReportError(ErrorCode.CanOnlyCallFunctions, Compilation, functionCall);
//                //return (null, false);
//            }

//            ExpressionNode boundIdentifierExpression = identifierExpression.Bind(symbol);

//            if (expressionContext && functionSymbol.ReturnTypeSymbol == TypeSymbol.VoidType)
//            {
//                throw new NotImplementedException();
//                //ErrorProvider.ReportError(ErrorCode.OnlyFunctionWithReturnTypeCanBeExpression, Compilation, functionCall);
//                //return (null, false);
//            }

//            if (functionSymbol.ParameterSymbols.Count != invocation.ArgumentNodes.Count)
//            {
//                //ErrorProvider.ReportError(ErrorCode.WrongNumberOfArguments,
//                //                          Compilation,
//                //                          functionCall,
//                //                          $"Expected number of arguments: {functionSymbol.ParameterNodes.Count}",
//                //                          $"Provided number of arguments: {functionCall.ArgumentNodes.Count}");
//                //return (null, false);
//            }

//            List<ExpressionNode> boundArguments = new(capacity: invocation.ArgumentNodes.Count);

//            for (int i = 0; i < functionSymbol.ParameterSymbols.Count; i++)
//            {
//                TypedExpressionNode? typedArgument = BindExpression(invocation.ArgumentNodes[i], variableIdentifierMap);

//                if (typedArgument == null)
//                {
//                    return null;
//                }

//                if (!ErrorOnIncompatibleType(ref typedArgument, functionSymbol.ParameterSymbols[i].TypeSymbol))
//                {
//                    return null;
//                }

//                boundArguments.Add(typedArgument);
//            }

//            return (invocation with
//            {
//                ArgumentNodes = boundArguments.ToImmutableList(),
//            }).Bind(functionSymbol)
//              .Type(functionSymbol.ReturnTypeSymbol);
//        }

//        private TypedExpressionNode? BindLiteral(LiteralExpressionNode literalExpression)
//        {
//            TypeSymbol symbol = typeManager[literalExpression.AssociatedType];
//            return literalExpression.Type(symbol);
//        }

//        private TypedExpressionNode? BindPropertyGetAccess(PropertyGetAccessExpressionNode propertyGetAccess,
//                                                           VariableIdentifierMap variableIdentifierMap)
//        {
//            TypedExpressionNode? typedSource = BindExpression(propertyGetAccess.SourceNode, variableIdentifierMap);

//            if (typedSource == null)
//            {
//                return null;
//            }

//            TypeSymbol sourceType = typedSource.TypeSymbol;

//            if (!sourceType.PropertySymbols.TryGetValue(propertyGetAccess.PropertyToken.TextToString(),
//                                                        out PropertySymbol? propertySymbol))
//            {
//                throw new NotImplementedException();
//                //ErrorProvider.ReportError(ErrorCode.PropertyDoesNotExistInType,
//                //                          Compilation,
//                //                          propertyGet.PropertyIdentifierNode,
//                //                          $"Type: {expressionType.Identifier}");
//                //return null;
//            }

//            return (propertyGetAccess with { SourceNode = typedSource }).Bind(propertySymbol)
//                                                                        .Type(propertySymbol.ReturnTypeSymbol);
//        }

//        private TypedExpressionNode? BindUnaryOperation(UnaryOperationExpressionNode unaryOperation, VariableIdentifierMap variableIdentifierMap)
//        {
//            TypedExpressionNode? typedOperand = BindExpression(unaryOperation.OperandNode, variableIdentifierMap);

//            if (typedOperand == null)
//            {
//                return null;
//            }

//            Operator @operator = unaryOperation.Operator;

//            UnaryOperationSymbol? operationSymbol = FindBestUnaryOperation(@operator, ref typedOperand);

//            if (operationSymbol == null)
//            {
//                throw new NotImplementedException();
//                //ErrorProvider.ReportError(ErrorCode.OperatorNotAvailableForTypes,
//                //                          Compilation,
//                //                          unaryOperation,
//                //                          $"Operand type: {operandType.Identifier}");
//                //return null;
//            }

//            return (unaryOperation with
//            {
//                OperandNode = typedOperand
//            }).Bind(operationSymbol)
//              .Type(operationSymbol.OperandTypeSymbol);
//        }
//    }
//}