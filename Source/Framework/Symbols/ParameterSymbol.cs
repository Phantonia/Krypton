namespace Krypton.Framework.Symbols
{
    public sealed class ParameterSymbol : NamedFrameworkSymbol
    {
        internal ParameterSymbol(string name, FrameworkType returnType) : base(name)
        {
            ReturnType = returnType;
        }

        public FrameworkType ReturnType { get; }
    }
}
