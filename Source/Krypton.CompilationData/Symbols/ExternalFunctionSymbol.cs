using Mono.Cecil;
using System.Collections.Generic;

namespace Krypton.CompilationData.Symbols
{
    public sealed class ExternalFunctionSymbol : FunctionSymbol, IExternalSymbol
    {
        public ExternalFunctionSymbol(MethodDefinition function,
                                      IEnumerable<ParameterSymbol>? parameters,
                                      ExternalTypeSymbol returnType)
            : base(function.Name, parameters)
        {
            FunctionReference = function;
            ReturnTypeSymbol = returnType;
        }

        public MethodDefinition FunctionReference { get; }

        public override ExternalTypeSymbol ReturnTypeSymbol { get; }

        MemberReference IExternalSymbol.Reference => FunctionReference;
    }
}
