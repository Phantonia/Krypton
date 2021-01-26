namespace Krypton.Framework.Symbols
{
    public sealed class ImplicitConversionSymbol : FrameworkSymbol
    {
        internal ImplicitConversionSymbol(FrameworkType sourceType, FrameworkType returnType, UnaryGenerator generator)
        {
            SourceType = sourceType;
            ReturnType = returnType;
            Generator = generator;
        }

        public UnaryGenerator Generator { get; }

        public FrameworkType ReturnType { get; }

        public FrameworkType SourceType { get; }
    }
}
