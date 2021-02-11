using Krypton.Analysis.Ast;
using Krypton.Analysis.Ast.Symbols;
using Krypton.Analysis.Errors;
using Krypton.Analysis.Semantical.IdentifierMaps;

namespace Krypton.Analysis.Semantical
{
    partial class Binder
    {
        private (HoistedIdentifierMap, TypeIdentifierMap) GatherGlobalSymbols()
        {
            HoistedIdentifierMap globalIdentifierMap = new();
            TypeIdentifierMap typeIdentifierMap = new();

            FrameworkIntegration.PopulateWithFrameworkSymbols(globalIdentifierMap, typeIdentifierMap);

            return (globalIdentifierMap, typeIdentifierMap);
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