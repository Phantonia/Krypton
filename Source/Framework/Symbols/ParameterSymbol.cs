namespace Krypton.Framework.Symbols
{
    public sealed class ParameterSymbol : NamedFrameworkSymbol
    {
        internal ParameterSymbol(string name, FrameworkType type) : base(name)
        {
            Type = type;
        }

        public FrameworkType Type { get; }
    }
}
