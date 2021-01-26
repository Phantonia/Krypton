namespace Krypton.Framework.Symbols
{
    public abstract class FrameworkSymbol
    {
        private protected FrameworkSymbol() { }
    }

    public abstract class ConstantSymbol : NamedFrameworkSymbol
    {
        private protected ConstantSymbol(string name, object objectValue) : base(name)
        {
            ObjectValue = objectValue;
        }

        public object ObjectValue { get; }
    }
}
