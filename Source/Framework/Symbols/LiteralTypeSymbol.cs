using System.Collections.Generic;

namespace Krypton.Framework.Symbols
{
    public sealed class LiteralTypeSymbol<T> : TypeSymbol
    {
        internal LiteralTypeSymbol(string name,
                                  FrameworkType frameworkType,
                                  LiteralGenerator<T> literalGenerator,
                                  IList<BinaryOperationSymbol>? binaryOperations = null,
                                  IList<ImplicitConversionSymbol>? implicitConversions = null,
                                  IList<PropertySymbol>? properties = null,
                                  IList<UnaryOperationSymbol>? unaryOperations = null) : base(name, frameworkType, binaryOperations, implicitConversions, properties, unaryOperations)
        {
            LiteralGenerator = literalGenerator;
        }

        public LiteralGenerator<T> LiteralGenerator { get; }
    }
}
