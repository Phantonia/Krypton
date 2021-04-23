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
            ImplicitConversions = implicitConversions.Finalize();
            Properties = properties.Finalize();
        }

        public FrameworkType FrameworkType { get; }

        public FinalList<ImplicitConversionSymbol> ImplicitConversions { get; }

        public FinalList<PropertySymbol> Properties { get; }
    }
}
