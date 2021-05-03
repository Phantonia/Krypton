namespace Krypton.CompilationData.Symbols
{
    public sealed class VariableSymbol : Symbol
    {
        public VariableSymbol(string name,
                              TypeSymbol type,
                              bool isReadOnly)
            : base(name)
        {
            TypeSymbol = type;
            IsReadOnly = isReadOnly;
        }

        public bool IsReadOnly { get; }

        public TypeSymbol TypeSymbol { get; }
    }
}
