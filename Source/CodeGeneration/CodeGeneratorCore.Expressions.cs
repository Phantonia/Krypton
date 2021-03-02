using Krypton.Analysis.Ast.Expressions;
using Krypton.Analysis.Ast.Expressions.Literals;
using Krypton.Framework;
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
                    output.Append(LiteralGenerator.EmitStringLiteral(stringLiteral.Value));
                    break;
                case BinaryOperationExpressionNode binaryOperation:
                    EmitBinaryOperation(binaryOperation);
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
