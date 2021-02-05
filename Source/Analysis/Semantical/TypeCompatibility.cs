using Krypton.Analysis.Ast;
using Krypton.Analysis.Ast.Symbols;
using Krypton.Analysis.Errors;

namespace Krypton.Analysis.Semantical
{
    public static class TypeCompatibility
    {
        public static bool IsCompatibleWith(TypeSymbolNode sourceType,
                                            TypeSymbolNode? targetType,
                                            string entireCode,
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
                                          entireCode,
                                          possiblyOffendingNode,
                                          $"Target type: {targetType.Identifier}",
                                          $"Source type: {sourceType.Identifier}");
                return false;
            }

            return true;
        }
    }
}
