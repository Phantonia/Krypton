using Krypton.Framework.Symbols;
using Krypton.Utilities;
using System.Collections.Generic;

namespace Krypton.Framework
{
    public sealed class FrameworkVersion
    {
        internal FrameworkVersion(int minimalLanguageVersion, IDictionary<FrameworkType, TypeSymbol>? types, IList<FunctionSymbol>? functions = null, IList<ConstantSymbol>? constants = null)
        {
            MinimalLanguageVersion = minimalLanguageVersion;
            Types = types.MakeReadOnly();
            Functions = functions.MakeReadOnly();
            Constants = constants.MakeReadOnly();
        }

        public ReadOnlyList<ConstantSymbol> Constants { get; }

        public ReadOnlyList<FunctionSymbol> Functions { get; }

        public int MinimalLanguageVersion { get; }

        public ReadOnlyDictionary<FrameworkType, TypeSymbol> Types { get; }
    }
}
