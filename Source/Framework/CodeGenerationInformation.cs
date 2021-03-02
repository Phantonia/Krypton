namespace Krypton.Framework
{
    public abstract record CodeGenerationInformation()
    {
        public static CodeGenerationInformation Identity { get; } = new IdentityRecord();
        private sealed record IdentityRecord() : CodeGenerationInformation;
    }

    public sealed record MethodCallCodeGenerationInformation(
        string MethodName)
        : CodeGenerationInformation;

    public sealed record JsOperatorCodeGenerationInformation(
        string Operator)
        : CodeGenerationInformation;

    public sealed record SpecialCodeGenerationInformation(
        SpecialCodeGenerationKind Kind)
        : CodeGenerationInformation;
}
