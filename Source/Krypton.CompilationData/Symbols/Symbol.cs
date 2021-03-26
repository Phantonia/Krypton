namespace Krypton.CompilationData.Symbols
{
    public abstract class Symbol
    {
        private protected Symbol() { }
    }

    public abstract class TypeSymbol : Symbol
    {
        private protected TypeSymbol() : base() { }
    }
}
