using Krypton.Utilities;
using System.Collections.Generic;

namespace Krypton.Framework.Symbols
{
    public class TypeSymbol : NamedFrameworkSymbol
    {
        internal TypeSymbol(string name,
                            FrameworkType frameworkType,
                            IList<ImplicitConversionSymbol>? implicitConversions = null,
                            IList<PropertySymbol>? properties = null) : base(name)
        {
            FrameworkType = frameworkType;
            ImplicitConversions = implicitConversions.MakeReadOnly();
            Properties = properties.MakeReadOnly();
        }

        public FrameworkType FrameworkType { get; }

        public ReadOnlyList<ImplicitConversionSymbol> ImplicitConversions { get; }

        public ReadOnlyList<PropertySymbol> Properties { get; }
    }
}
