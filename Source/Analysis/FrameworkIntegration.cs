using Krypton.Analysis.Ast;
using Krypton.Analysis.Ast.Symbols;
using Krypton.Analysis.Semantical.IdentifierMaps;
using Krypton.Framework;
using Krypton.Framework.Literals;
using Krypton.Framework.Symbols;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Krypton.Analysis
{
    public static class FrameworkIntegration
    {
        private static FrameworkVersion? frameworkVersion = null;
        private static TypeIdentifierMap? typeIdentifierMap = null;

        public static void PopulateWithFrameworkSymbols(HoistedIdentifierMap globalIdentifierMap, TypeIdentifierMap typeIdentifierMap)
        {
            FrameworkVersion frameworkVersion = FrameworkProvider.GetFrameworkVersion0();

            PopulateWithTypes(typeIdentifierMap, frameworkVersion);
            PopulateWithFunctions(globalIdentifierMap, typeIdentifierMap, frameworkVersion);
            PopulateWithConstants(globalIdentifierMap, frameworkVersion, typeIdentifierMap);
        }

        public static void PopulateWithFrameworkSymbols(HoistedIdentifierMap globalIdentifierMap)
        {
            Debug.Assert(typeIdentifierMap != null);
            Debug.Assert(frameworkVersion != null);

            PopulateWithFunctions(globalIdentifierMap, typeIdentifierMap, frameworkVersion);
            PopulateWithFunctions(globalIdentifierMap, typeIdentifierMap, frameworkVersion);
        }

        public static void PopulateWithFrameworkTypes(TypeIdentifierMap typeIdentifierMap)
        {
            frameworkVersion = FrameworkProvider.GetFrameworkVersion0();
            FrameworkIntegration.typeIdentifierMap = typeIdentifierMap;

            PopulateWithTypes(typeIdentifierMap, frameworkVersion);
        }

        private static BinaryOperationSymbolNode CreateBinaryOperationSymbolNode(BinaryOperationSymbol binaryOperationSymbol,
                                                                                 TypeIdentifierMap typeIdentifierMap,
                                                                                 FrameworkVersion frameworkVersion)
        {
            TypeSymbolNode leftType = GetTypeSymbolNode(binaryOperationSymbol.LeftType, typeIdentifierMap, frameworkVersion);
            TypeSymbolNode rightType = GetTypeSymbolNode(binaryOperationSymbol.RightType, typeIdentifierMap, frameworkVersion);
            TypeSymbolNode returnType = GetTypeSymbolNode(binaryOperationSymbol.ReturnType, typeIdentifierMap, frameworkVersion);
            return new BinaryOperationSymbolNode(binaryOperationSymbol.Operator,
                                                 leftType,
                                                 rightType,
                                                 returnType,
                                                 binaryOperationSymbol.Generator,
                                                 lineNumber: 0,
                                                 index: -1);
        }

        private static ConstantSymbolNode CreateConstantSymbolNode(ConstantSymbol constantSymbol, TypeIdentifierMap typeIdentifierMap)
        {
            switch (constantSymbol)
            {
                case ConstantSymbol<long> intConst:
                    return new ConstantSymbolNode<long>(intConst.Name,
                                                        intConst.Value,
                                                        typeIdentifierMap[FrameworkType.Int],
                                                        lineNumber: 0,
                                                        index: -1);
                case ConstantSymbol<Rational> ratConst:
                    return new ConstantSymbolNode<Rational>(ratConst.Name,
                                                            ratConst.Value,
                                                            typeIdentifierMap[FrameworkType.Rational],
                                                            lineNumber: 0,
                                                            index: -1);
                case ConstantSymbol<Complex> cmpConst:
                    return new ConstantSymbolNode<Complex>(cmpConst.Name,
                                                           cmpConst.Value,
                                                           typeIdentifierMap[FrameworkType.Complex],
                                                           lineNumber: 0,
                                                           index: -1);
                case ConstantSymbol<string> strConst:
                    return new ConstantSymbolNode<string>(strConst.Name,
                                                          strConst.Value,
                                                          typeIdentifierMap[FrameworkType.String],
                                                          lineNumber: 0,
                                                          index: -1);
                case ConstantSymbol<char> chrConst:
                    return new ConstantSymbolNode<char>(chrConst.Name,
                                                        chrConst.Value,
                                                        typeIdentifierMap[FrameworkType.Char],
                                                        lineNumber: 0,
                                                        index: -1);
                case ConstantSymbol<bool> blnConst:
                    return new ConstantSymbolNode<bool>(blnConst.Name,
                                                        blnConst.Value,
                                                        typeIdentifierMap[FrameworkType.Bool],
                                                        lineNumber: 0,
                                                        index: -1);
                default:
                    Debug.Fail(null);
                    return null;
            }
        }

        private static FunctionSymbolNode CreateFunctionSymbolNode(FunctionSymbol functionSymbol,
                                                                   FrameworkVersion frameworkVersion,
                                                                   TypeIdentifierMap typeIdentifierMap)
        {
            IEnumerable<ParameterSymbolNode>? parameters = functionSymbol.Parameters
                                                         ?.Select(p => CreateParameterNode(p, frameworkVersion, typeIdentifierMap))
                                                        ?? Array.Empty<ParameterSymbolNode>();

            TypeSymbolNode? returnType = null;

            if (functionSymbol.ReturnType != FrameworkType.None)
            {
                returnType = GetTypeSymbolNode(functionSymbol.ReturnType, typeIdentifierMap, frameworkVersion);
            }

            return new FrameworkFunctionSymbolNode(functionSymbol.Name,
                                                   parameters,
                                                   returnType,
                                                   functionSymbol.Generator,
                                                   lineNumber: 0,
                                                   index: -1);
        }

        private static ParameterSymbolNode CreateParameterNode(ParameterSymbol parameterSymbol,
                                                               FrameworkVersion frameworkVersion,
                                                               TypeIdentifierMap typeIdentifierMap)
        {
            return new ParameterSymbolNode(parameterSymbol.Name,
                                           GetTypeSymbolNode(parameterSymbol.Type, typeIdentifierMap, frameworkVersion),
                                           lineNumber: 0,
                                           index: -1);
        }

        private static PropertySymbolNode CreatePropertySymbolNode(PropertySymbol propertySymbol,
                                                                   TypeIdentifierMap typeIdentifierMap,
                                                                   FrameworkVersion frameworkVersion)
        {
            return new PropertySymbolNode(propertySymbol.Name,
                                          GetTypeSymbolNode(propertySymbol.ReturnType, typeIdentifierMap, frameworkVersion),
                                          lineNumber: 0,
                                          index: -1);
        }

        private static TypeSymbolNode CreateTypeSymbolNode(TypeSymbol typeSymbol)
        {
            return new FrameworkTypeSymbolNode(typeSymbol.FrameworkType,
                                               typeSymbol.Name,
                                               lineNumber: 0,
                                               index: -1);
        }

        private static UnaryOperationSymbolNode CreateUnaryOperationSymbolNode(UnaryOperationSymbol unaryOperationSymbol,
                                                                               TypeIdentifierMap typeIdentifierMap,
                                                                               FrameworkVersion frameworkVersion)
        {
            TypeSymbolNode operandType = GetTypeSymbolNode(unaryOperationSymbol.OperandType, typeIdentifierMap, frameworkVersion);
            TypeSymbolNode returnType = GetTypeSymbolNode(unaryOperationSymbol.ReturnType, typeIdentifierMap, frameworkVersion);
            return new UnaryOperationSymbolNode(unaryOperationSymbol.Operator,
                                                operandType,
                                                returnType,
                                                unaryOperationSymbol.Generator,
                                                lineNumber: 0,
                                                index: -1);
        }

        private static TypeSymbolNode GetTypeSymbolNode(FrameworkType frameworkType,
                                                        TypeIdentifierMap typeIdentifierMap,
                                                        FrameworkVersion frameworkVersion)
        {
            string name = frameworkVersion.Types[frameworkType].Name;
            Debug.Assert(typeIdentifierMap.TryGet(name, out _));
            return typeIdentifierMap[name];
        }

        private static void PopulateWithConstants(HoistedIdentifierMap globalIdentifierMap, FrameworkVersion frameworkVersion, TypeIdentifierMap typeIdentifierMap)
        {
            foreach (ConstantSymbol constantSymbol in frameworkVersion.Constants)
            {
                globalIdentifierMap.AddSymbol(constantSymbol.Name, CreateConstantSymbolNode(constantSymbol, typeIdentifierMap));
            }
        }

        private static void PopulateWithFunctions(HoistedIdentifierMap globalIdentifierMap,
                                                  TypeIdentifierMap typeIdentifierMap,
                                                  FrameworkVersion frameworkVersion)
        {
            foreach (FunctionSymbol functionSymbol in frameworkVersion.Functions)
            {
                globalIdentifierMap.AddSymbol(functionSymbol.Name,
                                              CreateFunctionSymbolNode(functionSymbol, frameworkVersion, typeIdentifierMap));
            }
        }

        private static void PopulateWithTypes(TypeIdentifierMap typeIdentifierMap, FrameworkVersion frameworkVersion)
        {
            IEnumerable<TypeSymbol> allTypeSymbols = frameworkVersion.Types.Select(kvp => kvp.Value);

            foreach (TypeSymbol typeSymbol in allTypeSymbols)
            {
                typeIdentifierMap.AddSymbol(typeSymbol.Name, typeSymbol.FrameworkType, CreateTypeSymbolNode(typeSymbol));
            }

            // This loop depends on the TypeIdentifierMap to be fully filled
            foreach (TypeSymbol typeSymbol in allTypeSymbols)
            {
                Dictionary<string, PropertySymbolNode> propertyNameMapping = new();

                foreach (PropertySymbol propertySymbol in typeSymbol.Properties)
                {
                    propertyNameMapping[propertySymbol.Name]
                        = CreatePropertySymbolNode(propertySymbol, typeIdentifierMap, frameworkVersion);
                }

                GetTypeSymbolNode(typeSymbol.FrameworkType, typeIdentifierMap, frameworkVersion)
                    .SetProperties(propertyNameMapping);
            }
        }
    }
}
