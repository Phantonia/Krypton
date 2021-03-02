namespace Krypton.Framework.Symbols
{
    public sealed class PropertySymbol : NamedFrameworkSymbol
    {
        internal PropertySymbol(string name,
                                FrameworkType returnType,
                                CodeGenerationInformation codeGenerationInfo) : base(name)
        {
            ReturnType = returnType;
            CodeGenerationInfo = codeGenerationInfo;
        }

        public CodeGenerationInformation CodeGenerationInfo { get; }

        public FrameworkType ReturnType { get; }
    }
}
