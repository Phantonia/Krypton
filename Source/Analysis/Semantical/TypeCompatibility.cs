using Krypton.Analysis.Ast.Symbols;

namespace Krypton.Analysis.Semantical
{
    public static class TypeCompatibility
    {
        public static bool IsCompatibleWith(TypeSymbolNode sourceType, TypeSymbolNode? targetType)
        {
            // targetType is null when we consider if an expression is assignable to an implicitly
            // typed variable, which is always okay if sourceType is not a pseudo type (which are)
            // not implemented yet)
            if (targetType == null)
            {
                return true;
            }

            // implicit conversions will be added soon
            return ReferenceEquals(sourceType, targetType);
        }
    }
}
