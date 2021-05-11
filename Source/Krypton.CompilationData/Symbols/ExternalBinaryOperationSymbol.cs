using Mono.Cecil;
using System.Diagnostics;

namespace Krypton.CompilationData.Symbols
{
    public sealed class ExternalBinaryOperationSymbol : BinaryOperationSymbol, IExternalSymbol
    {
        public ExternalBinaryOperationSymbol(MethodDefinition operation,
                                             ExternalTypeSymbol leftOperandType,
                                             Operator @operator,
                                             ExternalTypeSymbol rightOperandType,
                                             ExternalTypeSymbol returnType)
            : base(operation.Name, @operator)
        {
            Debug.Assert(operation.Name.StartsWith("op_"));
            Debug.Assert((operation.Resolve().Attributes & MethodAttributes.SpecialName) == MethodAttributes.SpecialName);
            Debug.Assert((operation.Resolve().Attributes & MethodAttributes.Static) == MethodAttributes.Static);
            Debug.Assert(operation.Parameters.Count == 2);

            OperationReference = operation;

            LeftOperandTypeSymbol = leftOperandType;
            RightOperandTypeSymbol = rightOperandType;
            ReturnTypeSymbol = returnType;
        }

        public override ExternalTypeSymbol LeftOperandTypeSymbol { get; }

        public MethodDefinition OperationReference { get; }

        public override ExternalTypeSymbol ReturnTypeSymbol { get; }

        public override ExternalTypeSymbol RightOperandTypeSymbol { get; }

        MemberReference IExternalSymbol.Reference => OperationReference;
    }
}
