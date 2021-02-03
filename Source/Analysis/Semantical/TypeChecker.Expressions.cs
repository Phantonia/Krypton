using Krypton.Analysis.Ast.Expressions;
using Krypton.Analysis.Ast.Identifiers;
using Krypton.Analysis.Ast.Symbols;
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
                        (TypeSymbolNode? symbol, bool success) = CheckFunctionCallExpression(functionCallNode, tolerateNoReturnType: false);
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
                throw new NotImplementedException("Error: operator not valid on this type");
            }

            if (!TypeCompatibility.IsCompatibleWith(leftOperandType, declaredOperationNode.LeftOperandTypeNode))
            {
                return null;
            }

            if (!TypeCompatibility.IsCompatibleWith(rightOperandType, declaredOperationNode.RightOperandTypeNode))
            {
                return null;
            }

            return declaredOperationNode.ReturnTypeNode;
        }

        private (TypeSymbolNode?, bool) CheckFunctionCallExpression(FunctionCallExpressionNode functionCallNode, bool tolerateNoReturnType)
        {
            if (functionCallNode.FunctionExpressionNode is not IdentifierExpressionNode identifierExpressionNode)
            {
                throw new NotImplementedException("Error: not a function that is being called");
                // return (null, false)
            }

            BoundIdentifierNode? boundIdentifier = identifierExpressionNode.IdentifierNode as BoundIdentifierNode;
            Debug.Assert(boundIdentifier != null);

            if (boundIdentifier.Symbol is not FunctionSymbolNode functionNode)
            {
                throw new NotImplementedException("Error: not a function that is being called");
                // return (null, false);
            }

            if (!tolerateNoReturnType && functionNode.ReturnTypeNode == null)
            {
                throw new NotImplementedException("Error: function cannot be used as expression, because it doesn't return a value");
                // return (null, false);
            }

            if (functionNode.ParameterNodes.Count != functionCallNode.ArgumentNodes.Count)
            {
                throw new NotImplementedException("Error: wrong number of arguments");
                // return (null, false);
            }

            for (int i = 0; i < functionNode.ParameterNodes.Count; i++)
            {
                TypeSymbolNode? argumentType = CheckExpression(functionCallNode.ArgumentNodes[i]);

                if (argumentType == null)
                {
                    return (null, false);
                }

                if (!TypeCompatibility.IsCompatibleWith(argumentType, functionNode.ParameterNodes[i].TypeNode))
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