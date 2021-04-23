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
            Types = types.Finalize();
            Functions = functions.Finalize();
            Constants = constants.Finalize();
            BinaryOperations = binaryOperations.Finalize();
            UnaryOperations = unaryOperations.Finalize();
        }

        public FinalList<BinaryOperationSymbol> BinaryOperations { get; }

        public FinalList<ConstantSymbol> Constants { get; }

        public FinalList<FunctionSymbol> Functions { get; }

        public FinalList<UnaryOperationSymbol> UnaryOperations { get; }

        public int MinimalLanguageVersion { get; }

        public FinalDictionary<FrameworkType, TypeSymbol> Types { get; }
    }
}
