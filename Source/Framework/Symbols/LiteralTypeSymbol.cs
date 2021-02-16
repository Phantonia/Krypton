using System.Collections.Generic;

namespace Krypton.Framework.Symbols
{
    public sealed class LiteralTypeSymbol<T> : TypeSymbol
    {
        internal LiteralTypeSymbol(string name,
                                   FrameworkType frameworkType,
                                   LiteralGenerator<T> literalGenerator,
                                   IList<ImplicitConversionSymbol>? implicitConversions = null,
                                   IList<PropertySymbol>? properties = null) : base(name, frameworkType, implicitConversions, properties)
        {
            LiteralGenerator = literalGenerator;
        }

        public LiteralGenerator<T> LiteralGenerator { get; }
    }
}
