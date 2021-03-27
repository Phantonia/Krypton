namespace Krypton.CompilationData.Symbols
{
    public sealed class PropertySymbol : Symbol
    {
        public PropertySymbol(string name, TypeSymbol returnType)
            : base(name)
        {
            ReturnTypeSymbol = returnType;
        }

        public TypeSymbol ReturnTypeSymbol { get; }
    }
}
