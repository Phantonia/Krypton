namespace Krypton.CompilationData.Symbols
{
    public sealed class ImplicitConversionSymbol : Symbol
    {
        public ImplicitConversionSymbol(string name, TypeSymbol targetType)
            : base(name)
        {
            TargetTypeSymbol = targetType;
        }

        public TypeSymbol TargetTypeSymbol { get; }
    }
}
