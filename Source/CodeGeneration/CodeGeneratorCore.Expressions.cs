using Krypton.Analysis.Ast.Expressions;
using Krypton.Analysis.Ast.Expressions.Literals;
using System.Diagnostics;
using System.Text;

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
                default:
                    Debug.Fail(message: null);
                    return;
            }
        }
    }
}
