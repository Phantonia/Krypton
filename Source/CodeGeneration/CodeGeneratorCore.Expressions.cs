﻿using Krypton.Analysis.Ast.Expressions;
using Krypton.Analysis.Ast.Expressions.Literals;
using Krypton.Analysis.Ast.Symbols;
using Krypton.Framework;
using System.Collections.Generic;
using System.Diagnostics;

namespace Krypton.CodeGeneration
{
    partial class CodeGeneratorCore
    {
        private void EmitExpression(ExpressionNode expression)
        {
            switch (expression)
            {
                case BooleanLiteralExpressionNode booleanLiteral:
                    LiteralGenerator.EmitBoolLiteral(booleanLiteral.Value, output);
                    break;
                case CharLiteralExpressionNode charLiteral:
                    LiteralGenerator.EmitCharLiteral(charLiteral.Value, output);
                    break;
                case ImaginaryLiteralExpressionNode imaginaryLiteral:
                    LiteralGenerator.EmitImaginaryLiteral(imaginaryLiteral.Value, output);
                    break;
                case IntegerLiteralExpressionNode integerLiteral:
                    LiteralGenerator.EmitIntLiteral(integerLiteral.Value, output);
                    break;
                case RationalLiteralExpressionNode rationalLiteral:
                    LiteralGenerator.EmitRationalLiteral(rationalLiteral.Value, output);
                    break;
                case StringLiteralExpressionNode stringLiteral:
                    LiteralGenerator.EmitStringLiteral(stringLiteral.Value, output);
                    break;
                case BinaryOperationExpressionNode binaryOperation:
                    EmitBinaryOperation(binaryOperation);
                    break;
                case FunctionCallExpressionNode functionCall:
                    EmitFunctionCallExpression(functionCall);
                    break;
                case IdentifierExpressionNode identifierExpression:
                    output.Append(identifierExpression.Identifier);
                    break;
                case UnaryOperationExpressionNode unaryOperation:
                    EmitUnaryOperation(unaryOperation);
                    break;
                default:
                    Debug.Fail(message: null);
                    return;
            }
        }

        private void EmitBinaryOperation(BinaryOperationExpressionNode binaryOperation)
        {
            Debug.Assert(binaryOperation.Operation != null);

            switch (binaryOperation.Operation.CodeGenerationInfo)
            {
                case JsOperatorCodeGenerationInformation operatorInfo:
                    output.Append('(');
                    EmitExpression(binaryOperation.LeftOperandNode);
                    output.Append(')');
                    output.Append(operatorInfo.Operator);
                    output.Append('(');
                    EmitExpression(binaryOperation.RightOperandNode);
                    output.Append(')');
                    break;
                case MethodCallCodeGenerationInformation methodInfo:
                    output.Append('(');
                    EmitExpression(binaryOperation.LeftOperandNode);
                    output.Append(").");
                    output.Append(methodInfo.MethodName);
                    output.Append('(');
                    EmitExpression(binaryOperation.RightOperandNode);
                    output.Append(')');
                    break;
                case SpecialCodeGenerationInformation
                { Kind: SpecialCodeGenerationKind.IntegerDivision }:
                    output.Append("Math.floor((");
                    EmitExpression(binaryOperation.LeftOperandNode);
                    output.Append(")/(");
                    EmitExpression(binaryOperation.RightOperandNode);
                    output.Append("))");
                    break;
                case SpecialCodeGenerationInformation { Kind: SpecialCodeGenerationKind.IntPowerInt }:
                    output.Append("new Rational(");
                    EmitExpression(binaryOperation.LeftOperandNode);
                    output.Append(",1).exponentiate(new Rational(");
                    EmitExpression(binaryOperation.RightOperandNode);
                    output.Append(",1)");
                    break;
                case SpecialCodeGenerationInformation { Kind: SpecialCodeGenerationKind.IntToRationalDivision }:
                    output.Append("new Rational(");
                    EmitExpression(binaryOperation.LeftOperandNode);
                    output.Append(',');
                    EmitExpression(binaryOperation.RightOperandNode);
                    output.Append(')');
                    break;
                default:
                    Debug.Fail(message: null);
                    return;
            }
        }

        private void EmitFunctionCallExpression(FunctionCallExpressionNode functionCall)
        {
            if (functionCall.SymbolNode is FrameworkFunctionSymbolNode frameworkFunction)
            {
                switch (frameworkFunction.CodeGenerationInfo)
                {
                    case SpecialCodeGenerationInformation { Kind: SpecialCodeGenerationKind.ConsoleLog }:
                        output.Append("console.log(");
                        EmitExpression(functionCall.ArgumentNodes[0]);
                        output.Append(')');
                        return;
                }
            }

            // at this point this should only be identifiers, so it's okay
            // to not put the expression in parentheses
            EmitExpression(functionCall.FunctionExpressionNode);

            output.Append('(');

            using IEnumerator<ExpressionNode> enumerator
                = functionCall.ArgumentNodes.GetEnumerator();

            if (enumerator.MoveNext())
            {
                while (true)
                {
                    EmitExpression(enumerator.Current);

                    if (enumerator.MoveNext())
                    {
                        output.Append(',');
                    }
                    else
                    {
                        break;
                    }
                }
            }

            output.Append(')');
        }

        private void EmitUnaryOperation(UnaryOperationExpressionNode unaryOperation)
        {
            Debug.Assert(unaryOperation.Operation != null);

            switch (unaryOperation.Operation.CodeGenerationInfo)
            {
                case JsOperatorCodeGenerationInformation operatorInfo: // e.g. -(x)
                    output.Append(operatorInfo.Operator);
                    output.Append('(');
                    EmitExpression(unaryOperation.OperandNode);
                    output.Append(')');
                    break;
                case MethodCallCodeGenerationInformation methodInfo: // e.g. (x).negate()
                    output.Append('(');
                    EmitExpression(unaryOperation.OperandNode);
                    output.Append(").");
                    output.Append(methodInfo.MethodName);
                    output.Append("()");
                    break;
                default:
                    Debug.Fail(message: null);
                    return;
            }
        }
    }
}