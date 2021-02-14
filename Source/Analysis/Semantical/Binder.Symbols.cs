using Krypton.Analysis.Ast;
using Krypton.Analysis.Ast.Declarations;
using Krypton.Analysis.Ast.Expressions;
using Krypton.Analysis.Ast.Expressions.Literals;
using Krypton.Analysis.Ast.Symbols;
using Krypton.Analysis.Ast.TypeSpecs;
using Krypton.Analysis.Errors;
using Krypton.Analysis.Semantical.IdentifierMaps;
using Krypton.Framework;
using Krypton.Framework.Literals;
using System.Collections.Generic;
using System.Diagnostics;

namespace Krypton.Analysis.Semantical
{
    partial class Binder
    {
        private ConstantSymbolNode? CreateConstantSymbol(ConstantDeclarationNode constantDeclaration)
        {
            if (constantDeclaration.ValueNode is not LiteralExpressionNode literal)
            {
                if (constantDeclaration.ValueNode is not BinaryOperationExpressionNode
                    {
                        LeftOperandNode: LiteralExpressionNode leftLiteral,
                        RightOperandNode: LiteralExpressionNode rightLiteral
                    })
                {
                    ErrorProvider.ReportError(ErrorCode.ConstantValueMustBeLiteralOrComplex,
                                              Compilation,
                                              constantDeclaration.ValueNode);
                    return null;
                }

                if (leftLiteral is not RationalLiteralExpressionNode { Value: Rational real })
                {
                    if (leftLiteral is IntegerLiteralExpressionNode { Value: long realInt })
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

                return new ConstantSymbolNode<Complex>(constantDeclaration.Identifier,
                                                       new Complex(real, imag),
                                                       typeManager[FrameworkType.Complex],
                                                       constantDeclaration.LineNumber,
                                                       constantDeclaration.Index);
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
                                          Node possiblyOffendingNode)
        {
            // targetType is null when we consider if an expression is assignable to an implicitly
            // typed variable, which is always okay if sourceType is not a pseudo type (which are
            // not implemented yet)
            if (targetType == null)
            {
                return true;
            }

            // this is where we would check for implicit conversions
            if (!object.ReferenceEquals(sourceType, targetType))
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
    }
}