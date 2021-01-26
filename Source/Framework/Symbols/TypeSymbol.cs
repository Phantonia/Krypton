using Krypton.Utilities;
using System.Collections.Generic;

namespace Krypton.Framework.Symbols
{
    public class TypeSymbol : NamedFrameworkSymbol
    {
        internal TypeSymbol(string name,
                            FrameworkType frameworkType,
                            IList<BinaryOperationSymbol>? binaryOperations = null,
                            IList<ImplicitConversionSymbol>? implicitConversions = null,
                            IList<PropertySymbol>? properties = null,
                            IList<UnaryOperationSymbol>? unaryOperations = null) : base(name)
        {
            FrameworkType = frameworkType;
            BinaryOperations = binaryOperations.MakeReadOnly();
            ImplicitConversions = implicitConversions.MakeReadOnly();
            Properties = properties.MakeReadOnly();
            UnaryOperations = unaryOperations.MakeReadOnly();
        }

        public ReadOnlyList<BinaryOperationSymbol> BinaryOperations { get; }

        public FrameworkType FrameworkType { get; }

        public ReadOnlyList<ImplicitConversionSymbol> ImplicitConversions { get; }

        public ReadOnlyList<PropertySymbol> Properties { get; }

        public ReadOnlyList<UnaryOperationSymbol> UnaryOperations { get; }
    }
}
