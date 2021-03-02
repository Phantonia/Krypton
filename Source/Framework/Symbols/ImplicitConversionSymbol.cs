namespace Krypton.Framework.Symbols
{
    public sealed class ImplicitConversionSymbol : FrameworkSymbol
    {
        internal ImplicitConversionSymbol(FrameworkType sourceType,
                                          FrameworkType returnType,
                                          CodeGenerationInformation codeGenerationInfo)
        {
            SourceType = sourceType;
            ReturnType = returnType;
            CodeGenerationInfo = codeGenerationInfo;
        }

        public CodeGenerationInformation CodeGenerationInfo { get; }

        public FrameworkType ReturnType { get; }

        public FrameworkType SourceType { get; }
    }
}
