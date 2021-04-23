using Krypton.Utilities;
using System.Collections.Generic;

namespace Krypton.CompilationData.Symbols
{
    public sealed class FunctionSymbol : Symbol
    {
        public FunctionSymbol(string name,
                              IEnumerable<ParameterSymbol>? parameters,
                              TypeSymbol returnType)
            : base(name)
        {
            ParameterSymbols = parameters.Finalize();
            ReturnTypeSymbol = returnType;
        }

        public FinalList<ParameterSymbol> ParameterSymbols { get; }

        public TypeSymbol ReturnTypeSymbol { get; }
    }
}
