namespace Krypton.Framework.Symbols
{
    public abstract class NamedFrameworkSymbol : FrameworkSymbol
    {
        private protected NamedFrameworkSymbol(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}
