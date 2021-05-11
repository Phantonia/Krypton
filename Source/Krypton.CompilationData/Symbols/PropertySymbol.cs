namespace Krypton.CompilationData.Symbols
{
    public abstract class PropertySymbol : Symbol
    {
        private protected PropertySymbol(string name)
            : base(name) { }

        public abstract TypeSymbol ReturnTypeSymbol { get; }
    }
}
