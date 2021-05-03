using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Krypton.CompilationData.Symbols
{
    public sealed class TypeSymbol : Symbol
    {
        public TypeSymbol(string name,
                          IEnumerable<ImplicitConversionSymbol> implicitConversionSymbols,
                          IEnumerable<PropertySymbol> propertySymbols)
            : base(name)
        {
            ImplicitConversionSymbols = implicitConversionSymbols.ToImmutableHashSet();
            PropertySymbols = propertySymbols.ToImmutableDictionary(p => p.Name);
        }

        public ImmutableHashSet<ImplicitConversionSymbol> ImplicitConversionSymbols { get; }

        public ImmutableDictionary<string, PropertySymbol> PropertySymbols { get; }

        public static TypeSymbol VoidType { get; } = new(name: string.Empty, Array.Empty<ImplicitConversionSymbol>(), Array.Empty<PropertySymbol>());
    }
}
