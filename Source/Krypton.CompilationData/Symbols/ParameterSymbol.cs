namespace Krypton.CompilationData.Symbols
{
    public sealed class ParameterSymbol : Symbol
    {
        public ParameterSymbol(string identifier, TypeSymbol type)
            : base(identifier)
        {
            TypeSymbol = type;
        }

        public TypeSymbol TypeSymbol { get; }
    }
}
