namespace Krypton.CompilationData.Symbols
{
    public abstract class ImplicitConversionSymbol : Symbol
    {
        private protected ImplicitConversionSymbol(string name)
            : base(name) { }

        public abstract TypeSymbol TargetTypeSymbol { get; }
    }
}
