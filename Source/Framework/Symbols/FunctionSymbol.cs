using Krypton.Utilities;
using System.Collections.Generic;

namespace Krypton.Framework.Symbols
{
    public sealed class FunctionSymbol : NamedFrameworkSymbol
    {
        internal FunctionSymbol(string name, FrameworkType returnType, FunctionCallGenerator generator, IList<ParameterSymbol>? parameters = null) : base(name)
        {
            ReturnType = returnType;
            Generator = generator;
            Parameters = parameters.MakeReadOnly();
        }

        public FunctionCallGenerator Generator { get; }

        public ReadOnlyList<ParameterSymbol>? Parameters { get; }

        public FrameworkType ReturnType { get; }
    }
}
