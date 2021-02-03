using Krypton.Analysis.Ast.Symbols;
using System;

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

            // this is where we would check for implicit conversions
            if (!ReferenceEquals(sourceType, targetType))
            {
                throw new NotImplementedException("Type error");
            }

            return true;
        }
    }
}
