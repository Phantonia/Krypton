using Krypton.Analysis.Ast;
using Krypton.Analysis.Ast.Declarations;
using Krypton.Analysis.Ast.Symbols;
using Krypton.Analysis.Ast.TypeSpecs;
using Krypton.Analysis.Errors;
using Krypton.Analysis.Semantical.IdentifierMaps;
using System.Collections.Generic;
using System.Diagnostics;

namespace Krypton.Analysis.Semantical
{
    partial class Binder
    {
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
                    return default;
                }

                globalIdentifierMap.AddSymbol(functionDeclaration.Identifier, functionSymbol);
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