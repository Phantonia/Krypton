using Krypton.Utilities;

namespace Krypton.CompilationData.Symbols
{
    public sealed class TypeSymbol : Symbol
    {
        public TypeSymbol(string name,
                          FinalList<ImplicitConversionSymbol> implicitConversionSymbols,
                          FinalList<PropertySymbol> propertySymbols)
            : base(name)
        {
            ImplicitConversionSymbols = implicitConversionSymbols;
            PropertySymbols = propertySymbols;
        }

        public FinalList<ImplicitConversionSymbol> ImplicitConversionSymbols { get; }

        public FinalList<PropertySymbol> PropertySymbols { get; }
    }
}
