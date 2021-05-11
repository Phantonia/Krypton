using Mono.Cecil;
using System.Diagnostics;

namespace Krypton.CompilationData.Symbols
{
    public sealed class ExternalImplicitConversionSymbol : ImplicitConversionSymbol, IExternalSymbol
    {
        public ExternalImplicitConversionSymbol(MethodDefinition conversion,
                                                ExternalTypeSymbol targetType)
            : base(conversion.Name)
        {
            Debug.Assert(conversion.Name == "op_Implicit");
            Debug.Assert((conversion.Resolve().Attributes & MethodAttributes.SpecialName) == MethodAttributes.SpecialName);

            ConversionReference = conversion;
            TargetTypeSymbol = targetType;
        }

        public MethodDefinition ConversionReference { get; }

        public override TypeSymbol TargetTypeSymbol { get; }

        MemberReference IExternalSymbol.Reference => ConversionReference;
    }
}
