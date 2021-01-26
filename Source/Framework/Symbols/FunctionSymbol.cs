using Krypton.Utilities;
using System.Collections.Generic;

namespace Krypton.Framework.Symbols
{
    public sealed class FunctionSymbol : NamedFrameworkSymbol
    {
        internal FunctionSymbol(string name, FrameworkType returnType, IList<ParameterSymbol>? parameters = null) : base(name)
        {
            ReturnType = returnType;
            Parameters = parameters.MakeReadOnly();
        }

        public ReadOnlyList<ParameterSymbol>? Parameters { get; }

        public FrameworkType ReturnType { get; }
    }
}
