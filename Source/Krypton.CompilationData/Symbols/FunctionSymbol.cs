using Krypton.Utilities;
using System.Collections.Generic;

namespace Krypton.CompilationData.Symbols
{
    public abstract class FunctionSymbol : Symbol
    {
        private protected FunctionSymbol(string name,
                                         IEnumerable<ParameterSymbol>? parameters)
            : base(name)
        {
            ParameterSymbols = parameters.Finalize();
        }

        public FinalList<ParameterSymbol> ParameterSymbols { get; }

        public abstract TypeSymbol ReturnTypeSymbol { get; }
    }
}
