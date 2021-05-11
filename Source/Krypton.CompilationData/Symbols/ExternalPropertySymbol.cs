using Mono.Cecil;

namespace Krypton.CompilationData.Symbols
{
    public sealed class ExternalPropertySymbol : PropertySymbol, IExternalSymbol
    {
        public ExternalPropertySymbol(PropertyDefinition property,
                                      ExternalTypeSymbol returnType)
            : base(property.Name)
        {
            PropertyReference = property;
            ReturnTypeSymbol = returnType;
        }

        public PropertyDefinition PropertyReference { get; }

        public override ExternalTypeSymbol ReturnTypeSymbol { get; }

        MemberReference IExternalSymbol.Reference => PropertyReference;
    }
}
