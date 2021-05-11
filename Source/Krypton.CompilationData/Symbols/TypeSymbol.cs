using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Krypton.CompilationData.Symbols
{
    public abstract class TypeSymbol : Symbol
    {
        public TypeSymbol(string name,
                          IEnumerable<ImplicitConversionSymbol> implicitConversions,
                          IEnumerable<PropertySymbol> properties)
            : base(name)
        {
            ImplicitConversionSymbols = implicitConversions.ToImmutableHashSet();
            PropertySymbols = properties.ToImmutableDictionary(p => p.Name);
        }

        public ImmutableHashSet<ImplicitConversionSymbol> ImplicitConversionSymbols { get; }

        public ImmutableDictionary<string, PropertySymbol> PropertySymbols { get; }
    }
}
