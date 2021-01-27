using Krypton.Analysis.Ast;
using Krypton.Analysis.Ast.Symbols;
using Krypton.Analysis.Semantical.IdentifierMaps;
using Krypton.Framework;
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
