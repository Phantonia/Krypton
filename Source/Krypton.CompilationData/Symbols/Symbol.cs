namespace Krypton.CompilationData.Symbols
{
    public abstract class Symbol : ISymbol
    {
        private protected Symbol(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}
