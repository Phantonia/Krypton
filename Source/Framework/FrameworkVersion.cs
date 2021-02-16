using Krypton.Framework.Symbols;
using Krypton.Utilities;
using System.Collections.Generic;

namespace Krypton.Framework
{
    public sealed class FrameworkVersion
    {
        internal FrameworkVersion(int minimalLanguageVersion,
                                  IDictionary<FrameworkType, TypeSymbol>? types,
                                  IList<FunctionSymbol>? functions,
                                  IList<ConstantSymbol>? constants,
                                  IList<BinaryOperationSymbol>? binaryOperations,
                                  IList<UnaryOperationSymbol>? unaryOperations)
        {
            MinimalLanguageVersion = minimalLanguageVersion;
            Types = types.MakeReadOnly();
            Functions = functions.MakeReadOnly();
            Constants = constants.MakeReadOnly();
            BinaryOperations = binaryOperations.MakeReadOnly();
            UnaryOperations = unaryOperations.MakeReadOnly();
        }

        public ReadOnlyList<BinaryOperationSymbol> BinaryOperations { get; }

        public ReadOnlyList<ConstantSymbol> Constants { get; }

        public ReadOnlyList<FunctionSymbol> Functions { get; }

        public ReadOnlyList<UnaryOperationSymbol> UnaryOperations { get; }

        public int MinimalLanguageVersion { get; }

        public ReadOnlyDictionary<FrameworkType, TypeSymbol> Types { get; }
    }
}
