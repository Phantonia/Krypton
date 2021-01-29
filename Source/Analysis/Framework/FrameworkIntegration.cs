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

namespace Krypton.Analysis.Framework
{
    public static class FrameworkIntegration
    {
        public static void PopulateWithFrameworkSymbols(HoistedIdentifierMap globalIdentifierMap, TypeIdentifierMap typeIdentifierMap)
        {
            FrameworkVersion frwVersion = FrameworkProvider.GetFrameworkVersion0();

            PopulateWithTypes(typeIdentifierMap, frwVersion);
            PopulateWithFunctions(globalIdentifierMap, typeIdentifierMap, frwVersion);
            PopulateWithConstants(globalIdentifierMap, frwVersion);
        }

        private static BinaryOperationSymbolNode CreateBinaryOperationSymbolNode(BinaryOperationSymbol binaryOperationSymbol, TypeIdentifierMap typeIdentifierMap, FrameworkVersion frwVersion)
        {
            TypeSymbolNode leftType = GetTypeSymbolNode(binaryOperationSymbol.LeftType, typeIdentifierMap, frwVersion);
            TypeSymbolNode rightType = GetTypeSymbolNode(binaryOperationSymbol.RightType, typeIdentifierMap, frwVersion);
            TypeSymbolNode returnType = GetTypeSymbolNode(binaryOperationSymbol.ReturnType, typeIdentifierMap, frwVersion);
            return new BinaryOperationSymbolNode(binaryOperationSymbol.Operator, leftType, rightType, returnType, binaryOperationSymbol.Generator, lineNumber: 0);
        }

        private static ConstantSymbolNode CreateConstantSymbolNode(ConstantSymbol constantSymbol, FrameworkVersion frwVersion)
        {
            switch (constantSymbol)
            {
                case ConstantSymbol<long> intConst: return new ConstantSymbolNode<long>(intConst.Name, intConst.Value, 0);
                case ConstantSymbol<Rational> ratConst: return new ConstantSymbolNode<Rational>(ratConst.Name, ratConst.Value, 0);
                case ConstantSymbol<Complex> cmpConst: return new ConstantSymbolNode<Complex>(cmpConst.Name, cmpConst.Value, 0);
                case ConstantSymbol<string> strConst: return new ConstantSymbolNode<string>(strConst.Name, strConst.Value, 0);
                case ConstantSymbol<char> chrConst: return new ConstantSymbolNode<char>(chrConst.Name, chrConst.Value, 0);
                case ConstantSymbol<bool> blnConst: return new ConstantSymbolNode<bool>(blnConst.Name, blnConst.Value, 0);
                default: Debug.Fail(null); return null;
            }
        }

        private static FunctionSymbolNode CreateFunctionSymbolNode(FunctionSymbol functionSymbol, FrameworkVersion frwVersion, TypeIdentifierMap typeIdentifierMap)
        {
            IEnumerable<ParameterNode>? parameters = functionSymbol.Parameters?.Select(p => CreateParameterNode(p, frwVersion, typeIdentifierMap)) ?? Array.Empty<ParameterNode>();

            TypeSymbolNode? returnType = null;

            if (functionSymbol.ReturnType != FrameworkType.None)
            {
                GetTypeSymbolNode(functionSymbol.ReturnType, typeIdentifierMap, frwVersion);
            }

            return new BuiltinFunctionSymbolNode(functionSymbol.Name, parameters, returnType, functionSymbol.Generator, 0);
        }

        private static ParameterNode CreateParameterNode(ParameterSymbol parameterSymbol, FrameworkVersion frwVersion, TypeIdentifierMap typeIdentifierMap)
        {
            return new ParameterNode(parameterSymbol.Name, GetTypeSymbolNode(parameterSymbol.Type, typeIdentifierMap, frwVersion), 0);
        }

        private static TypeSymbolNode CreateTypeSymbolNode(TypeSymbol typeSymbol)
        {
            return new BuiltinTypeSymbolNode(typeSymbol.FrameworkType, typeSymbol.Name, 0);
        }

        private static UnaryOperationSymbolNode CreateUnaryOperationSymbolNode(UnaryOperationSymbol unaryOperationSymbol, TypeIdentifierMap typeIdentifierMap, FrameworkVersion frwVersion)
        {
            TypeSymbolNode operandType = GetTypeSymbolNode(unaryOperationSymbol.OperandType, typeIdentifierMap, frwVersion);
            TypeSymbolNode returnType = GetTypeSymbolNode(unaryOperationSymbol.ReturnType, typeIdentifierMap, frwVersion);
            return new UnaryOperationSymbolNode(unaryOperationSymbol.Operator, operandType, returnType, unaryOperationSymbol.Generator, lineNumber: 0);
        }

        private static TypeSymbolNode GetTypeSymbolNode(FrameworkType frwType, TypeIdentifierMap typeIdentifierMap, FrameworkVersion frwVersion)
        {
            bool success = typeIdentifierMap.TryGet(frwVersion.Types[frwType].Name, out TypeSymbolNode? node);
            Debug.Assert(success);
            return node!;
        }

        private static void PopulateWithConstants(HoistedIdentifierMap globalIdentifierMap, FrameworkVersion frwVersion)
        {
            foreach (ConstantSymbol constantSymbol in frwVersion.Constants)
            {
                globalIdentifierMap.AddSymbol(constantSymbol.Name, CreateConstantSymbolNode(constantSymbol, frwVersion));
            }
        }

        private static void PopulateWithFunctions(HoistedIdentifierMap globalIdentifierMap, TypeIdentifierMap typeIdentifierMap, FrameworkVersion frwVersion)
        {
            foreach (FunctionSymbol functionSymbol in frwVersion.Functions)
            {
                globalIdentifierMap.AddSymbol(functionSymbol.Name, CreateFunctionSymbolNode(functionSymbol, frwVersion, typeIdentifierMap));
            }
        }

        private static void PopulateWithTypes(TypeIdentifierMap typeIdentifierMap, FrameworkVersion frwVersion)
        {
            IEnumerable<TypeSymbol> typeSymbols = frwVersion.Types.Select(kvp => kvp.Value);

            foreach (TypeSymbol tp in typeSymbols)
            {
                typeIdentifierMap.AddSymbol(tp.Name, CreateTypeSymbolNode(tp));
            }

            foreach (TypeSymbol tp in typeSymbols)
            {
                Dictionary<Operator, BinaryOperationSymbolNode> binaryOperationMapping = new();

                foreach (BinaryOperationSymbol op in tp.BinaryOperations)
                {
                    binaryOperationMapping[op.Operator] = CreateBinaryOperationSymbolNode(op, typeIdentifierMap, frwVersion);
                }

                GetTypeSymbolNode(tp.FrameworkType, typeIdentifierMap, frwVersion).SetBinaryOperations(binaryOperationMapping);

                Dictionary<Operator, UnaryOperationSymbolNode> unaryOperationMapping = new();

                foreach (UnaryOperationSymbol op in tp.UnaryOperations)
                {
                    unaryOperationMapping[op.Operator] = CreateUnaryOperationSymbolNode(op, typeIdentifierMap, frwVersion);
                }

                GetTypeSymbolNode(tp.FrameworkType, typeIdentifierMap, frwVersion).SetUnaryOperations(unaryOperationMapping);
            }
        }
    }
}
