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
        private TypeSymbolNode? CheckExpression(ExpressionNode expression)
        {
            switch (expression)
            {
                case FunctionCallExpressionNode funcCall:
                    {
                        (TypeSymbolNode? symbol, bool success) = CheckFunctionCallExpression(funcCall, tolerateNoReturnType: false);
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

        private TypeSymbolNode? CheckBinaryOperation(BinaryOperationExpressionNode binaryOperation)
        {
            TypeSymbolNode? leftType = CheckExpression(binaryOperation.Left);
            TypeSymbolNode? rightType = CheckExpression(binaryOperation.Right);

            if (leftType == null || rightType == null)
            {
                return null;
            }

            Operator appliedOperator = binaryOperation.Operator;

            if (!leftType.BinaryOperations.TryGetValue(appliedOperator, out BinaryOperationSymbolNode? declaredOperation))
            {
                throw new NotImplementedException("Error: operand not valid on this type");
            }

            if (!TypeCompatibility.IsCompatibleWith(leftType, declaredOperation.LeftType))
            {
                return null;
            }

            if (!TypeCompatibility.IsCompatibleWith(rightType, declaredOperation.RightType))
            {
                return null;
            }

            return declaredOperation.ReturnType;
        }

        private (TypeSymbolNode?, bool) CheckFunctionCallExpression(FunctionCallExpressionNode funcCall, bool tolerateNoReturnType)
        {
            if (funcCall.FunctionExpression is not IdentifierExpressionNode identifier)
            {
                throw new NotImplementedException("Error: not a function that is being called");
                // return (null, false)
            }

            BoundIdentifierNode? boundId = identifier.IdentifierNode as BoundIdentifierNode;
            Debug.Assert(boundId != null);

            if (boundId.Symbol is not FunctionSymbolNode function)
            {
                throw new NotImplementedException("Error: not a function that is being called");
                // return (null, false);
            }

            if (!tolerateNoReturnType && function.ReturnType == null)
            {
                throw new NotImplementedException("Error: function cannot be used as expression, because it doesn't return a value");
                // return (null, false);
            }

            if (function.Parameters.Count != (funcCall.Arguments?.Count ?? 0))
            {
                throw new NotImplementedException("Error: wrong number of arguments");
                // return (null, false);
            }

            for (int i = 0; i < function.Parameters.Count; i++)
            {
                Debug.Assert(funcCall.Arguments != null);

                TypeSymbolNode? argumentType = CheckExpression(funcCall.Arguments[i]);

                if (argumentType == null)
                {
                    return (null, false);
                }

                if (!TypeCompatibility.IsCompatibleWith(argumentType, function.Parameters[i].Type))
                {
                    throw new NotImplementedException("Type error");
                    // return (null, false);
                }
            }

            return (function.ReturnType, true);
        }

        private TypeSymbolNode? CheckIdentifierExpression(IdentifierExpressionNode id)
        {
            BoundIdentifierNode? boundId = id.IdentifierNode as BoundIdentifierNode;
            Debug.Assert(boundId != null);

            switch (boundId.Symbol)
            {
                case VariableSymbolNode var:
                    {
                        TypeSymbolNode? varType = var.Type;
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