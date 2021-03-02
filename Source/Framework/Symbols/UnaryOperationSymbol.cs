namespace Krypton.Framework.Symbols
{
    public sealed class UnaryOperationSymbol : FrameworkSymbol
    {
        internal UnaryOperationSymbol(Operator @operator,
                                      FrameworkType operandType,
                                      FrameworkType returnType,
                                      CodeGenerationInformation codeGenerationInfo)
        {
            Operator = @operator;
            OperandType = operandType;
            ReturnType = returnType;
            CodeGenerationInfo = codeGenerationInfo;
        }

        public CodeGenerationInformation CodeGenerationInfo { get; }

        public FrameworkType OperandType { get; }

        public Operator Operator { get; }

        public FrameworkType ReturnType { get; }
    }
}
