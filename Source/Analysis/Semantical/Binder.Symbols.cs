using Krypton.Analysis.Ast;
using Krypton.Analysis.Ast.Declarations;
using Krypton.Analysis.Ast.Expressions;
using Krypton.Analysis.Ast.Expressions.Literals;
using Krypton.Analysis.Ast.Symbols;
using Krypton.Analysis.Ast.TypeSpecs;
using Krypton.Analysis.Errors;
using Krypton.Framework;
using Krypton.Framework.Literals;
using Krypton.Utilities;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Krypton.Analysis.Semantical
{
    partial class Binder
    {
        private ConstantSymbolNode<Complex>? CreateComplexConstantSymbol(ConstantDeclarationNode constantDeclaration)
        {
            if (constantDeclaration.ValueNode is not BinaryOperationExpressionNode
                {
                    Operator: (Operator.Plus or Operator.Minus) and Operator @operator,
                    LeftOperandNode: ExpressionNode leftExpression and (LiteralExpressionNode or UnaryOperationExpressionNode
                    {
                        Operator: Operator.Minus
                    }),
                    RightOperandNode: LiteralExpressionNode rightLiteral
                })
            {
                ErrorProvider.ReportError(ErrorCode.ConstantValueMustBeLiteralOrComplex,
                                          Compilation,
                                          constantDeclaration.ValueNode);
                return null;
            }

            bool realNegative = false;
            bool imagNegative = @operator == Operator.Minus;

            if (leftExpression is UnaryOperationExpressionNode unaryOperation)
            {
                realNegative = true;
                leftExpression = unaryOperation.OperandNode;
            }

            if (leftExpression is not RationalLiteralExpressionNode { Value: Rational real })
            {
                if (leftExpression is IntegerLiteralExpressionNode { Value: long realInt })
                {
                    real = new Rational(realInt, 1);
                }
                else
                {
                    ErrorProvider.ReportError(ErrorCode.ConstantValueMustBeLiteralOrComplex,
                                              Compilation,
                                              constantDeclaration.ValueNode);
                    return null;
                }
            }

            if (rightLiteral is not ImaginaryLiteralExpressionNode { Value: Rational imag })
            {
                ErrorProvider.ReportError(ErrorCode.ConstantValueMustBeLiteralOrComplex,
                                          Compilation,
                                          constantDeclaration.ValueNode);
                return null;
            }

            if (realNegative)
            {
                real = real.Negate();
            }

            if (imagNegative)
            {
                imag = imag.Negate();
            }

            return new ConstantSymbolNode<Complex>(constantDeclaration.Identifier,
                                                   new Complex(real, imag),
                                                   typeManager[FrameworkType.Complex],
                                                   constantDeclaration.LineNumber,
                                                   constantDeclaration.Index);
        }

        private ConstantSymbolNode? CreateConstantSymbol(ConstantDeclarationNode constantDeclaration)
        {
            if (constantDeclaration.ValueNode is not LiteralExpressionNode literal)
            {
                return CreateComplexConstantSymbol(constantDeclaration);
            }

            if (constantDeclaration.TypeSpecNode == null)
            {
                return CreateSymbol();
            }
            else
            {
                TypeSymbolNode? declaredType = GetTypeSymbol(constantDeclaration.TypeSpecNode);

                if (declaredType == null)
                {
                    ErrorProvider.ReportError(ErrorCode.CantFindType,
                                              Compilation,
                                              constantDeclaration.TypeSpecNode);
                    return null;
                }

                TypeSymbolNode? literalType = typeManager[literal.AssociatedType];

                if (!literalType.Equals(declaredType))
                {
                    ErrorProvider.ReportError(ErrorCode.ConstTypeHasToMatchLiteralTypeExactly,
                                              Compilation,
                                              constantDeclaration.ValueNode,
                                              $"Literal type: {literalType.Identifier}");
                    return null;
                }

                return CreateSymbol();
            }

            ConstantSymbolNode CreateSymbol()
            {
                switch (literal)
                {
                    case IntegerLiteralExpressionNode integerLiteral:
                        return CreateSymbolCore(integerLiteral.Value, FrameworkType.Int);
                    case RationalLiteralExpressionNode rationalLiteral:
                        return CreateSymbolCore(rationalLiteral.Value, FrameworkType.Rational);
                    case ImaginaryLiteralExpressionNode imaginaryLiteral:
                        return CreateSymbolCore(new Complex(default, imaginaryLiteral.Value), FrameworkType.Complex);
                    case BooleanLiteralExpressionNode booleanLiteral:
                        return CreateSymbolCore(booleanLiteral.Value, FrameworkType.Bool);
                    case CharLiteralExpressionNode charLiteral:
                        return CreateSymbolCore(charLiteral.Value, FrameworkType.Char);
                    case StringLiteralExpressionNode stringLiteral:
                        return CreateSymbolCore(stringLiteral.Value, FrameworkType.String);
                    default:
                        Debug.Fail(message: null);
                        return null;
                }

                ConstantSymbolNode<T> CreateSymbolCore<T>(T value, FrameworkType frameworkType)
                {
                    return new ConstantSymbolNode<T>(constantDeclaration.Identifier,
                                                     value,
                                                     typeManager[frameworkType],
                                                     constantDeclaration.LineNumber,
                                                     constantDeclaration.Index);
                }
            }
        }

        private FunctionSymbolNode? CreateFunctionSymbol(FunctionDeclarationNode functionDeclaration)
        {
            List<ParameterSymbolNode>? parameters = null;

            if (functionDeclaration.ParameterNodes.Count > 0)
            {
                parameters = new List<ParameterSymbolNode>();

                foreach (ParameterDeclarationNode parameterDeclaration in functionDeclaration.ParameterNodes)
                {
                    if (!typeManager.TryGetTypeSymbol(parameterDeclaration.TypeNode,
                                                      out TypeSymbolNode? parameterType))
                    {
                        return null;
                    }

                    Debug.Assert(parameterType != null);

                    parameters.Add(new ParameterSymbolNode(parameterDeclaration.Identifier,
                                                           parameterType,
                                                           parameterDeclaration.LineNumber,
                                                           parameterDeclaration.Index));
                }
            }


            if (!typeManager.TryGetTypeSymbol(functionDeclaration.ReturnTypeNode,
                                              out TypeSymbolNode? returnType))
            {
                return null;
            }

            return new FunctionSymbolNode(functionDeclaration.Identifier,
                                          parameters,
                                          returnType,
                                          functionDeclaration.LineNumber,
                                          functionDeclaration.Index);
        }

        private BinaryOperationSymbolNode? FindBestBinaryOperation(Operator @operator,
                                                                   TypeSymbolNode leftType,
                                                                   TypeSymbolNode rightType,
                                                                   out ImplicitConversionSymbolNode? choseImplicitConversionLeft,
                                                                   out ImplicitConversionSymbolNode? choseImplicitConversionRight)
        {
            ReadOnlyList<BinaryOperationSymbolNode> allOperations = FrameworkIntegration.GetBinaryOperations();

            var candidates = from op in allOperations
                             where op.Operator == @operator
                             let resultLeft = TypeIsCompatibleWithNoError(leftType, op.LeftOperandTypeNode)
                             let resultRight = TypeIsCompatibleWithNoError(rightType, op.RightOperandTypeNode)
                             where resultLeft.isCompatible && resultRight.isCompatible
                             select (operation: op,
                                     choseImplicitConversionLeft: resultLeft.implicitConversion,
                                     choseImplicitConversionRight: resultRight.implicitConversion);

            if (!candidates.IsSingle(out var tuple))
            {
                tuple = candidates.SingleOrDefault(t => t.operation.LeftOperandTypeNode.Equals(leftType)
                                                     && t.operation.RightOperandTypeNode.Equals(rightType));
            }

            BinaryOperationSymbolNode operationSymbol;
            (operationSymbol, choseImplicitConversionLeft, choseImplicitConversionRight) = tuple;
            return operationSymbol;
        }

        private UnaryOperationSymbolNode? FindBestUnaryOperation(Operator @operator,
                                                                 TypeSymbolNode operandType,
                                                                 out ImplicitConversionSymbolNode? implicitConversion)
        {
            ReadOnlyList<UnaryOperationSymbolNode> allOperations = FrameworkIntegration.GetUnaryOperations();

            var candidates = from op in allOperations
                             where op.Operator == @operator
                             let result = TypeIsCompatibleWithNoError(operandType, op.OperandTypeNode)
                             where result.isCompatible
                             select (operation: op, result.implicitConversion);

            if (!candidates.IsSingle(out var tuple))
            {
                tuple = candidates.SingleOrDefault(t => t.operation.OperandTypeNode.Equals(operandType));
            }

            UnaryOperationSymbolNode operationSymbol;
            (operationSymbol, implicitConversion) = tuple;
            return operationSymbol;
        }

        private HoistedIdentifierMap? GatherGlobalSymbols()
        {
            HoistedIdentifierMap globalIdentifierMap = new();

            FrameworkIntegration.PopulateWithFrameworkSymbols(globalIdentifierMap);

            foreach (FunctionDeclarationNode functionDeclaration in Compilation.Program.Functions)
            {
                FunctionSymbolNode? functionSymbol = CreateFunctionSymbol(functionDeclaration);

                if (functionSymbol == null)
                {
                    return null;
                }

                globalIdentifierMap.AddSymbol(functionDeclaration.Identifier, functionSymbol);
            }

            foreach (ConstantDeclarationNode constantDeclaration in Compilation.Program.Constants)
            {
                ConstantSymbolNode? constantSymbol = CreateConstantSymbol(constantDeclaration);

                if (constantSymbol == null)
                {
                    return null;
                }

                globalIdentifierMap.AddSymbol(constantDeclaration.Identifier, constantSymbol);
            }

            return globalIdentifierMap;
        }

        private TypeIdentifierMap GatherGlobalTypes()
        {
            TypeIdentifierMap typeIdentifierMap = new();
            FrameworkIntegration.PopulateWithFrameworkTypes(typeIdentifierMap);
            return typeIdentifierMap;
        }

        private SymbolNode? GetExpressionSymbol(string identifier, VariableIdentifierMap variableIdentifierMap)
        {
            if (variableIdentifierMap.TryGet(identifier, out SymbolNode? symbolNode)
               || globalIdentifierMap.TryGet(identifier, out symbolNode))
            {
                return symbolNode;
            }

            return null;
        }

        private TypeSymbolNode? GetTypeSymbol(TypeSpecNode typeSpec)
        {
            if (typeManager.TryGetTypeSymbol(typeSpec, out TypeSymbolNode? typeSymbol))
            {
                return typeSymbol;
            }

            return null;
        }

        private bool TypeIsCompatibleWith(TypeSymbolNode sourceType,
                                          TypeSymbolNode? targetType,
                                          Node possiblyOffendingNode,
                                          out ImplicitConversionSymbolNode? implicitConversion)
        {
            // targetType is null when we consider if an expression is assignable to an implicitly
            // typed variable, which is always okay if sourceType is not a pseudo type (which are
            // not implemented yet)
            if (targetType == null)
            {
                implicitConversion = null;
                return true;
            }

            if (!TypeIsCompatibleWithNoError(sourceType, targetType, out implicitConversion))
            {
                ErrorProvider.ReportError(ErrorCode.CantConvertType,
                                          Compilation,
                                          possiblyOffendingNode,
                                          $"Target type: {targetType.Identifier}",
                                          $"Source type: {sourceType.Identifier}");
                return false;
            }

            return true;
        }

        private static (bool isCompatible, ImplicitConversionSymbolNode? implicitConversion) TypeIsCompatibleWithNoError(
            TypeSymbolNode sourceType,
            TypeSymbolNode targetType)
        {
            return TypeIsCompatibleWithNoError(sourceType, targetType, out ImplicitConversionSymbolNode? implicitConversion)
                ? (true, implicitConversion)
                : (false, implicitConversion);
        }

        private static bool TypeIsCompatibleWithNoError(TypeSymbolNode sourceType,
                                                        TypeSymbolNode targetType,
                                                        out ImplicitConversionSymbolNode? implicitConversion)
        {
            implicitConversion = sourceType.ImplicitConversions.SingleOrDefault(c => c.TargetTypeNode.Equals(targetType));

            if (implicitConversion == null)
            {
                return sourceType.Equals(targetType);
            }

            return true;
        }
    }
}