using Krypton.CompilationData.Syntax.Declarations;
using System.Collections.Generic;

namespace Krypton.CompilationData.Symbols
{
    public sealed class InternalFunctionSymbol : FunctionSymbol, IInternalSymbol
    {
        public InternalFunctionSymbol(FunctionDeclarationNode function,
                                      IEnumerable<ParameterSymbol>? parameters,
                                      TypeSymbol returnType)
            : base(new string(function.Name.Span), parameters)
        {
            FunctionDeclarationNode = function;
            ReturnTypeSymbol = returnType;
        }

        public FunctionDeclarationNode FunctionDeclarationNode { get; }

        public override TypeSymbol ReturnTypeSymbol { get; }

        DeclarationNode IInternalSymbol.DeclarationNode => FunctionDeclarationNode;
    }
}
