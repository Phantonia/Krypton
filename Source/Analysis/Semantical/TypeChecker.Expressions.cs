using Krypton.Analysis.Ast.Expressions;
using Krypton.Analysis.Ast.Identifiers;
using Krypton.Analysis.Ast.Symbols;
using Krypton.Analysis.Errors;
using Krypton.Framework;
using System;
using System.Diagnostics;

namespace Krypton.Analysis.Semantical
{
    partial class TypeChecker
    {
        private TypeSymbolNode? CheckExpression(ExpressionNode expressionNode)
        {
            switch (expressionNode)
            {
                case FunctionCallExpressionNode functionCallNode:
                    {
                        (TypeSymbolNode? symbol, bool success) = CheckFunctionCallExpression(functionCallNode, expressionContext: false);
                        return success ? symbol : null;
                    }
                case BinaryOperationExpressionNode binaryOperation:
                    return CheckBinaryOperation(binaryOperation);
                case LiteralExpressionNode literal:
                    return CheckLiteralExpression(literal);
                case IdentifierExpressionNode id:
                    return CheckIdentifierExpression(id);
                default:
                    Debug.Fail(message: null);
                    return null;
            }
        }

        private TypeSymbolNode? CheckBinaryOperation(BinaryOperationExpressionNode binaryOperationNode)
        {
            TypeSymbolNode? leftOperandType = CheckExpression(binaryOperationNode.LeftOperandNode);
            TypeSymbolNode? rightOperandType = CheckExpression(binaryOperationNode.RightOperandNode);

            if (leftOperandType == null || rightOperandType == null)
            {
                return null;
            }

            Operator appliedOperator = binaryOperationNode.Operator;

            if (!leftOperandType.BinaryOperationNodes.TryGetValue(appliedOperator, out BinaryOperationSymbolNode? declaredOperationNode))
            {
                throw new NotImplementedException("Notim: no implicit conversions in this case yet; else error");
            }

            if (!TypeCompatibility.IsCompatibleWith(leftOperandType, declaredOperationNode.LeftOperandTypeNode, Compilation.Code, binaryOperationNode))
            {
                return null;
            }

            if (!TypeCompatibility.IsCompatibleWith(rightOperandType, declaredOperationNode.RightOperandTypeNode, Compilation.Code, binaryOperationNode))
            {
                return null;
            }

            return declaredOperationNode.ReturnTypeNode;
        }

        private (TypeSymbolNode?, bool) CheckFunctionCallExpression(FunctionCallExpressionNode functionCallNode, bool expressionContext)
        {
            if (functionCallNode.FunctionExpressionNode is not IdentifierExpressionNode identifierExpressionNode)
            {
                ErrorProvider.ReportError(ErrorCode.CanOnlyCallFunctions, Compilation, functionCallNode);
                return (null, false);
            }

            BoundIdentifierNode? boundIdentifier = identifierExpressionNode.IdentifierNode as BoundIdentifierNode;
            Debug.Assert(boundIdentifier != null);

            if (boundIdentifier.Symbol is not FunctionSymbolNode functionNode)
            {
                ErrorProvider.ReportError(ErrorCode.CanOnlyCallFunctions, Compilation, functionCallNode);
                return (null, false);
            }

            if (!expressionContext && functionNode.ReturnTypeNode == null)
            {
                ErrorProvider.ReportError(ErrorCode.OnlyFunctionWithReturnTypeCanBeExpression, Compilation, functionCallNode);
                return (null, false);
            }

            if (functionNode.ParameterNodes.Count != functionCallNode.ArgumentNodes.Count)
            {
                ErrorProvider.ReportError(ErrorCode.WrongNumberOfArguments,
                                          Compilation,
                                          functionCallNode,
                                          $"Expected number of arguments: {functionNode.ParameterNodes.Count}",
                                          $"Provided number of arguments: {functionCallNode.ArgumentNodes.Count}");
                return (null, false);
            }

            for (int i = 0; i < functionNode.ParameterNodes.Count; i++)
            {
                TypeSymbolNode? argumentType = CheckExpression(functionCallNode.ArgumentNodes[i]);

                if (argumentType == null)
                {
                    return (null, false);
                }

                if (!TypeCompatibility.IsCompatibleWith(argumentType, functionNode.ParameterNodes[i].TypeNode, Compilation.Code, functionCallNode))
                {
                    return (null, false);
                }
            }

            return (functionNode.ReturnTypeNode, true);
        }

        private TypeSymbolNode? CheckIdentifierExpression(IdentifierExpressionNode id)
        {
            BoundIdentifierNode? boundId = id.IdentifierNode as BoundIdentifierNode;
            Debug.Assert(boundId != null);

            switch (boundId.Symbol)
            {
                case VariableSymbolNode var:
                    {
                        TypeSymbolNode? varType = var.TypeNode;
                        Debug.Assert(varType != null);
                        return varType;
                    }
                case ConstantSymbolNode @const:
                    {
                        return @const.Type;
                    }
                default:
                    Debug.Fail(message: null);
                    return null;
            }
        }

        private TypeSymbolNode? CheckLiteralExpression(LiteralExpressionNode literal)
        {
            return typeManager[literal.AssociatedType];
        }
    }
}