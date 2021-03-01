namespace Krypton.Framework
{
    public abstract record BinaryOperationCodeGenerationInformation();

    public sealed record MethodCallBinaryOperationCodeGenerationInformation(
        string MethodName)
        : BinaryOperationCodeGenerationInformation;

    public sealed record JsOperatorBinaryOperationCodeGenerationInformation(
        string Operator)
        : BinaryOperationCodeGenerationInformation;

    public sealed record SpecialBinaryOperationCodeGenerationInformation(
        SpecialOperationGenerationKind Kind)
        : BinaryOperationCodeGenerationInformation;
}
