using Mono.Cecil;
using System.Collections.Generic;

namespace Krypton.CompilationData.Symbols
{
    public sealed class ExternalTypeSymbol : TypeSymbol, IExternalSymbol
    {
        public ExternalTypeSymbol(TypeDefinition type,
                                  IEnumerable<ExternalImplicitConversionSymbol> implicitConversions,
                                  IEnumerable<ExternalPropertySymbol> properties)
            : base(type.Name, implicitConversions, properties)
        {
            TypeReference = type;
        }

        public TypeDefinition TypeReference { get; }

        MemberReference IExternalSymbol.Reference => TypeReference;
    }
}
