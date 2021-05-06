using Krypton.CompilationData;
using Krypton.CompilationData.Symbols;
using Krypton.CompilationData.Syntax;
using Krypton.CompilationData.Syntax.Declarations;
using Krypton.CompilationData.Syntax.Expressions;
using Krypton.CompilationData.Syntax.Tokens;
using Krypton.CompilationData.Syntax.Types;
using Krypton.Framework;
using Krypton.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;

namespace Krypton.Analysis.Semantics
{
    partial class Binder
    {
        private bool CheckType(ref TypedExpressionNode typedExpression,
                               TypeSymbol? targetType)
        {
            if (targetType == null)
            {
                return true;
            }

            TypeSymbol sourceType = typedExpression.TypeSymbol;

            ImplicitConversionSymbol? implicitConversion = sourceType.ImplicitConversionSymbols
                                                                     .SingleOrDefault(c => c.TargetTypeSymbol
                                                                                            .Equals(targetType));

            if (implicitConversion == null)
            {
                return sourceType.Equals(targetType);
            }

            typedExpression = typedExpression with { ImplicitConversionSymbol = implicitConversion };

            return true;
        }

        private (bool works, ImplicitConversionSymbol? conversion) CheckTypeWithoutUpdating(TypeSymbol sourceType,
                                                                                                         TypeSymbol targetType)
        {
            ImplicitConversionSymbol? implicitConversion = sourceType.ImplicitConversionSymbols
                                                                     .SingleOrDefault(c => c.TargetTypeSymbol
                                                                                            .Equals(targetType));

            if (implicitConversion == null)
            {
                return (sourceType.Equals(targetType), null);
            }

            return (true, implicitConversion);
        }

        private ConstantSymbol<RationalComplex>? CreateComplexConstantSymbol(ConstantDeclarationNode constantDeclaration)
        {
            if (constantDeclaration.ValueNode is not BinaryOperationExpressionNode
                {
                    Operator: (Operator.Plus or Operator.Minus) and Operator @operator ,
                    LeftOperandNode: ExpressionNode leftExpression and (LiteralExpressionNode or UnaryOperationExpressionNode
                    {
                       Operator: Operator.Minus
                    }),
                    RightOperandNode: LiteralExpressionNode rightLiteral
                } operationExpression)
            {
                throw new NotImplementedException();
                //ErrorProvider.ReportError(ErrorCode.ConstantValueMustBeLiteralOrComplex,
                //                          Compilation,
                //                          constantDeclaration.ValueNode);
                //return null;
            }

            bool realNegative = false;
            bool imagNegative = @operator == Operator.Minus;

            if (leftExpression is UnaryOperationExpressionNode unaryOperation)
            {
                realNegative = true;
                leftExpression = unaryOperation.OperandNode;
            }

            if (leftExpression is not LiteralExpressionNode<Rational> { Value: Rational real })
            {
                if (leftExpression is LiteralExpressionNode<long> { Value: long realInt })
                {
                    real = new Rational(realInt, 1);
                }
                else
                {
                    throw new NotImplementedException();
                    //ErrorProvider.ReportError(ErrorCode.ConstantValueMustBeLiteralOrComplex,
                    //                          Compilation,
                    //                          constantDeclaration.ValueNode);
                    //return null;
                }
            }

            if (rightLiteral is not LiteralExpressionNode<RationalComplex> { Value: { Imaginary: Rational imag } })
            {
                throw new NotImplementedException();
                //ErrorProvider.ReportError(ErrorCode.ConstantValueMustBeLiteralOrComplex,
                //                          Compilation,
                //                          constantDeclaration.ValueNode);
                //return null;
            }

            if (realNegative)
            {
                real = real.Negate();
            }

            if (imagNegative)
            {
                imag = imag.Negate();
            }

            return new ConstantSymbol<RationalComplex>(new string(constantDeclaration.Name.Span),
                                                       typeManager[FrameworkType.Complex],
                                                       new RationalComplex(real, imag));
        }

        private ConstantSymbol? CreateConstantSymbol(ConstantDeclarationNode constantDeclaration)
        {
            if (constantDeclaration.ValueNode is not LiteralExpressionNode literal)
            {
                return CreateComplexConstantSymbol(constantDeclaration);
            }

            if (constantDeclaration.TypeNode == null)
            {
                return CreateSymbol();
            }
            else
            {
                TypeSymbol? declaredType = GetTypeSymbol(constantDeclaration.TypeNode);

                if (declaredType == null)
                {
                    throw new NotImplementedException();
                    //ErrorProvider.ReportError(ErrorCode.CantFindType,
                    //                          Compilation,
                    //                          constantDeclaration.TypeSpecNode);
                    //return null;
                }

                TypeSymbol? literalType = typeManager[literal.AssociatedType];

                if (!literalType.Equals(declaredType))
                {
                    throw new NotImplementedException();
                    //ErrorProvider.ReportError(ErrorCode.ConstTypeHasToMatchLiteralTypeExactly,
                    //                          Compilation,
                    //                          constantDeclaration.ValueNode,
                    //                          $"Literal type: {literalType.Identifier}");
                    //return null;
                }

                return CreateSymbol();
            }

            ConstantSymbol CreateSymbol()
            {
                switch (literal)
                {
                    case LiteralExpressionNode<long> integerLiteral:
                        return CreateSymbolCore(integerLiteral.Value, FrameworkType.Int);
                    case LiteralExpressionNode<Rational> rationalLiteral:
                        return CreateSymbolCore(rationalLiteral.Value, FrameworkType.Rational);
                    case LiteralExpressionNode<RationalComplex> imaginaryLiteral:
                        return CreateSymbolCore(new RationalComplex(default, imaginaryLiteral.Value.Imaginary), FrameworkType.Complex);
                    case LiteralExpressionNode<bool> booleanLiteral:
                        return CreateSymbolCore(booleanLiteral.Value, FrameworkType.Bool);
                    case LiteralExpressionNode<char> charLiteral:
                        return CreateSymbolCore(charLiteral.Value, FrameworkType.Char);
                    case LiteralExpressionNode<string> stringLiteral:
                        return CreateSymbolCore(stringLiteral.Value, FrameworkType.String);
                    default:
                        Debug.Fail(message: null);
                        return null;
                }

                ConstantSymbol<T> CreateSymbolCore<T>(T value, FrameworkType frameworkType)
                    where T : notnull
                {
                    return new ConstantSymbol<T>(new string(constantDeclaration.Name.Span),
                                                 typeManager[frameworkType],
                                                 value);
                }
            }
        }

        private FunctionSymbol? CreateFunctionSymbol(FunctionDeclarationNode functionDeclaration)
        {
            List<ParameterSymbol>? parameters = null;

            if (functionDeclaration.ParameterNodes.Count > 0)
            {
                parameters = new List<ParameterSymbol>();

                foreach (ParameterDeclarationNode parameterDeclaration in functionDeclaration.ParameterNodes)
                {
                    if (!typeManager.TryGetTypeSymbol(parameterDeclaration.AsClauseNode.TypeNode,
                                                      out TypeSymbol? parameterType))
                    {
                        return null;
                    }

                    Debug.Assert(parameterType != null);

                    parameters.Add(new ParameterSymbol(new string(parameterDeclaration.Name.Span),
                                                       parameterType));
                }
            }


            if (!typeManager.TryGetTypeSymbol(functionDeclaration.ReturnTypeClauseNode?.TypeNode,
                                              out TypeSymbol returnType))
            {
                return null;
            }

            return new FunctionSymbol(new string(functionDeclaration.Name.Span),
                                      parameters,
                                      returnType);
        }

        private bool ErrorOnIncompatibleType(ref TypedExpressionNode typedExpression,
                                             TypeSymbol? targetType)
        {
            if (!CheckType(ref typedExpression, targetType))
            {
                throw new NotImplementedException();
                //ErrorProvider.ReportError(ErrorCode.CantConvertType,
                //                          Compilation,
                //                          possiblyOffendingNode,
                //                          $"Target type: {targetType.Identifier}",
                //                          $"Source type: {sourceType.Identifier}");
                //return false;
            }

            return true;
        }

        private BinaryOperationSymbol? FindBestBinaryOperation(Operator @operator,
                                                               ref TypedExpressionNode leftOperand,
                                                               ref TypedExpressionNode rightOperand)
        {
            FinalList<BinaryOperationSymbol> allOperations = FrameworkIntegration.GetBinaryOperations();

            // we cannot close over ref params, so we need to make local copies
            TypedExpressionNode leftOperandCopy = leftOperand;
            TypedExpressionNode rightOperandCopy = rightOperand;

            var candidates = from operation in allOperations
                             where operation.Operator == @operator
                             let resultLeft = CheckTypeWithoutUpdating(leftOperandCopy.TypeSymbol, operation.LeftOperandTypeSymbol)
                             let resultRight = CheckTypeWithoutUpdating(rightOperandCopy.TypeSymbol, operation.RightOperandTypeSymbol)
                             where resultLeft.works && resultRight.works
                             select (operation, conversionLeft: resultLeft.conversion, conversionRight: resultRight.conversion);

#if DEBUG
            var candidatesAsList = candidates.ToList();
#endif

            if (!candidates.IsSingle(out var tuple))
            {
                // there are multiple candidates. Can we find a candidate where at least one of the operand types are exact?
                candidates = candidates.Where(t => t.operation.LeftOperandTypeSymbol.Equals(leftOperandCopy.TypeSymbol)
                                                || t.operation.RightOperandTypeSymbol.Equals(rightOperandCopy.TypeSymbol));

                if (!candidates.IsSingle(out tuple))
                {
                    // We can, and there are multiple actually.
                    // So the only true candidate is the one where both types are exact.
                    // If there is none that fulfills that criteria, there is no best operator.
                    tuple = candidates.SingleOrDefault(t => t.operation.LeftOperandTypeSymbol.Equals(leftOperandCopy.TypeSymbol)
                                                         && t.operation.RightOperandTypeSymbol.Equals(rightOperandCopy.TypeSymbol));
                }
            }

            (BinaryOperationSymbol operationSymbol, ImplicitConversionSymbol? conversionLeft, ImplicitConversionSymbol? conversionRight) = tuple;

            if (conversionLeft != null)
            {
                leftOperand = leftOperand with { ImplicitConversionSymbol = conversionLeft };
            }

            if (conversionRight != null)
            {
                rightOperand = rightOperand with { ImplicitConversionSymbol = conversionRight };
            }

            return operationSymbol;
        }

        private UnaryOperationSymbol? FindBestUnaryOperation(Operator @operator,
                                                             ref TypedExpressionNode operand)
        {
            FinalList<UnaryOperationSymbol> allOperations = FrameworkIntegration.GetUnaryOperations();

            // we cannot close over ref params, so we need to make a local copy
            TypedExpressionNode operandCopy = operand;

            var candidates = from operation in allOperations
                             where operation.Operator == @operator
                             let result = CheckTypeWithoutUpdating(operandCopy.TypeSymbol, operation.ReturnTypeNode)
                             where result.works
                             select (operation, result.conversion);

            if (!candidates.IsSingle(out var tuple))
            {
                tuple = candidates.SingleOrDefault(t => t.operation.OperandTypeSymbol.Equals(operandCopy.TypeSymbol));
            }

            (UnaryOperationSymbol? operationSymbol, ImplicitConversionSymbol? conversion) = tuple;

            if (conversion != null)
            {
                operand = operand with { ImplicitConversionSymbol = conversion };
            }

            return operationSymbol;
        }

        private HoistedIdentifierMap? GatherGlobalSymbols()
        {
            HoistedIdentifierMap globalIdentifierMap = new();

            FrameworkIntegration.PopulateWithFrameworkSymbols(globalIdentifierMap);

            HashSet<ReadOnlyMemory<char>> declaredSymbols = (program.GetFunctionDeclarations().Count() + program.GetConstantDeclarations().Count() > 0) ? new() : null!;

            ImmutableList<TopLevelNode> unboundTopLevelNodes = program.TopLevelNodes;

            ImmutableList<TopLevelNode> boundTopLevelNodes = unboundTopLevelNodes;

            for (int i = 0; i < unboundTopLevelNodes.Count; i++)
            {
                switch (unboundTopLevelNodes[i])
                {
                    case TopLevelDeclarationNode { DeclarationNode: FunctionDeclarationNode functionDeclaration } topLevelFunctionDeclaration:
                        {
                            bool success = HandleDeclaration(functionDeclaration, topLevelFunctionDeclaration, CreateFunctionSymbol);

                            if (!success)
                            {
                                return null;
                            }
                        }
                        break;
                    case TopLevelDeclarationNode { DeclarationNode: ConstantDeclarationNode constantDeclaration } topLevelConstantDeclaration:
                        {
                            bool success = HandleDeclaration(constantDeclaration, topLevelConstantDeclaration, CreateConstantSymbol);

                            if (!success)
                            {
                                return null;
                            }
                        }
                        break;
                }

                bool HandleDeclaration<TDeclaration, TSymbol>(TDeclaration declaration,
                                                              TopLevelDeclarationNode topLevelDeclaration,
                                                              Func<TDeclaration, TSymbol> creator)
                    where TDeclaration : NamedDeclarationNode
                    where TSymbol : Symbol?
                {
                    if (!declaredSymbols.Add(declaration.Name))
                    {
                        throw new NotImplementedException();
                        //ErrorProvider.ReportError(ErrorCode.CantRedeclareGlobalSymbol, Compilation, constantDeclaration);
                        //return null;
                    }

                    TSymbol symbol = creator(declaration);

                    if (symbol == null)
                    {
                        return false;
                    }

                    globalIdentifierMap.AddSymbol(new string(declaration.Name.Span), symbol);

                    BoundDeclarationNode boundDeclaration = declaration.Bind(symbol);
                    TopLevelNode boundTopLevelNode = topLevelDeclaration with { DeclarationNode = boundDeclaration };
                    boundTopLevelNodes = boundTopLevelNodes.SetItem(i, boundTopLevelNode);

                    return true;
                }
            }

            program = program with { TopLevelNodes = boundTopLevelNodes };

            return globalIdentifierMap;
        }

        private TypeIdentifierMap GatherGlobalTypes()
        {
            TypeIdentifierMap typeIdentifierMap = new();
            FrameworkIntegration.PopulateWithFrameworkTypes(typeIdentifierMap);
            return typeIdentifierMap;
        }

        private Symbol? GetExpressionSymbol(IdentifierToken identifier, VariableIdentifierMap variableIdentifierMap)
        {
            string identifierString = new(identifier.Text.Span);

            if (variableIdentifierMap.TryGet(identifierString, out Symbol? symbol)
                || globalIdentifierMap.TryGet(identifierString, out symbol))
            {
                return symbol;
            }

            return null;
        }

        private TypeSymbol? GetTypeSymbol(TypeNode typeSpec)
        {
            if (typeManager.TryGetTypeSymbol(typeSpec, out TypeSymbol? typeSymbol))
            {
                return typeSymbol;
            }

            return null;
        }
    }
}