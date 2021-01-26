namespace Krypton.Framework.Symbols
{
    public sealed class PropertySymbol : NamedFrameworkSymbol
    {
        internal PropertySymbol(string name, FrameworkType returnType, UnaryGenerator generator) : base(name)
        {
            ReturnType = returnType;
            Generator = generator;
        }

        public UnaryGenerator Generator { get; }

        public FrameworkType ReturnType { get; }
    }
}
