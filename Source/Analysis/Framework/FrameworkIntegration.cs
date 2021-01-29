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

            foreach (TypeSymbol typeSymbol in frwVersion.Types.Select(kvp => kvp.Value))
            {
                typeIdentifierMap.AddSymbol(typeSymbol.Name, CreateTypeSymbolNode(typeSymbol));
            }

            foreach (FunctionSymbol functionSymbol in frwVersion.Functions)
            {
                globalIdentifierMap.AddSymbol(functionSymbol.Name, CreateFunctionSymbolNode(functionSymbol, frwVersion, typeIdentifierMap));
            }

            foreach (ConstantSymbol constantSymbol in frwVersion.Constants)
            {
                globalIdentifierMap.AddSymbol(constantSymbol.Name, CreateConstantSymbolNode(constantSymbol, frwVersion));
            }
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
                GetTypeSymbolNode(frwVersion, functionSymbol.ReturnType, typeIdentifierMap);
            }

            return new BuiltinFunctionSymbolNode(functionSymbol.Name, parameters, returnType, functionSymbol.Generator, 0);
        }

        private static ParameterNode CreateParameterNode(ParameterSymbol parameterSymbol, FrameworkVersion frwVersion, TypeIdentifierMap typeIdentifierMap)
        {
            return new ParameterNode(parameterSymbol.Name, GetTypeSymbolNode(frwVersion, parameterSymbol.Type, typeIdentifierMap), 0);
        }

        private static TypeSymbolNode CreateTypeSymbolNode(TypeSymbol typeSymbol)
        {
            return new BuiltinTypeSymbolNode(typeSymbol.FrameworkType, typeSymbol.Name, 0);
        }

        private static TypeSymbolNode GetTypeSymbolNode(FrameworkVersion frwVersion, FrameworkType frwType, TypeIdentifierMap typeIdentifierMap)
        {
            bool success = typeIdentifierMap.TryGet(frwVersion.Types[frwType].Name, out TypeSymbolNode? node);
            Debug.Assert(success);
            return node!;
        }
    }
}
