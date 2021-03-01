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
                    output.Append(LiteralGenerator.ConvertBoolLiteral(booleanLiteral.Value));
                    break;
                case CharLiteralExpressionNode charLiteral:
                    output.Append(LiteralGenerator.ConvertCharLiteral(charLiteral.Value));
                    break;
                case ImaginaryLiteralExpressionNode imaginaryLiteral:
                    output.Append(LiteralGenerator.ConvertImaginaryLiteral(imaginaryLiteral.Value));
                    break;
                case IntegerLiteralExpressionNode integerLiteral:
                    output.Append(LiteralGenerator.ConvertIntLiteral(integerLiteral.Value));
                    break;
                case RationalLiteralExpressionNode rationalLiteral:
                    output.Append(LiteralGenerator.ConvertRationalLiteral(rationalLiteral.Value));
                    break;
                case StringLiteralExpressionNode stringLiteral:
                    output.Append(LiteralGenerator.ConvertStringLiteral(stringLiteral.Value));
                    break;
                case BinaryOperationExpressionNode binaryOperation:
                    EmitBinaryOperation(binaryOperation);
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
                case JsOperatorBinaryOperationCodeGenerationInformation operatorInfo:
                    output.Append('(');
                    EmitExpression(binaryOperation.LeftOperandNode);
                    output.Append(')');
                    output.Append(operatorInfo.Operator);
                    output.Append('(');
                    EmitExpression(binaryOperation.RightOperandNode);
                    output.Append(')');
                    break;
                case MethodCallBinaryOperationCodeGenerationInformation methodInfo:
                    output.Append('(');
                    EmitExpression(binaryOperation.LeftOperandNode);
                    output.Append(").");
                    output.Append(methodInfo.MethodName);
                    output.Append('(');
                    EmitExpression(binaryOperation.RightOperandNode);
                    output.Append(')');
                    break;
                case SpecialBinaryOperationCodeGenerationInformation
                { Kind: SpecialOperationGenerationKind.IntegerDivision }:
                    output.Append("Math.floor((");
                    EmitExpression(binaryOperation.LeftOperandNode);
                    output.Append(")/(");
                    EmitExpression(binaryOperation.RightOperandNode);
                    output.Append("))");
                    break;
                case SpecialBinaryOperationCodeGenerationInformation { Kind: SpecialOperationGenerationKind.IntPowerInt }:
                    output.Append("new Rational(");
                    EmitExpression(binaryOperation.LeftOperandNode);
                    output.Append(",1).exponentiate(new Rational(");
                    EmitExpression(binaryOperation.RightOperandNode);
                    output.Append(",1)");
                    break;
                case SpecialBinaryOperationCodeGenerationInformation { Kind: SpecialOperationGenerationKind.IntToRationalDivision }:
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
    }
}
