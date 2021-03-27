using Krypton.Utilities;

namespace Krypton.CompilationData.Symbols
{
    public sealed class TypeSymbol : Symbol
    {
        public TypeSymbol(string name,
                          ReadOnlyList<ImplicitConversionSymbol> implicitConversionSymbols,
                          ReadOnlyList<PropertySymbol> propertySymbols)
            : base(name)
        {
            ImplicitConversionSymbols = implicitConversionSymbols;
            PropertySymbols = propertySymbols;
        }

        public ReadOnlyList<ImplicitConversionSymbol> ImplicitConversionSymbols { get; }

        public ReadOnlyList<PropertySymbol> PropertySymbols { get; }
    }
}
