namespace Krypton.Framework.Symbols
{
    public sealed class BinaryOperationSymbol : FrameworkSymbol
    {
        internal BinaryOperationSymbol(Operator @operator,
                                       FrameworkType leftType,
                                       FrameworkType rightType,
                                       FrameworkType returnType,
                                       CodeGenerationInformation codeGenerationInfo)
        {
            Operator = @operator;
            LeftType = leftType;
            RightType = rightType;
            ReturnType = returnType;
            CodeGenerationInfo = codeGenerationInfo;
        }

        public CodeGenerationInformation CodeGenerationInfo { get; }

        public FrameworkType LeftType { get; }

        public Operator Operator { get; }

        public FrameworkType ReturnType { get; }

        public FrameworkType RightType { get; }
    }
}
