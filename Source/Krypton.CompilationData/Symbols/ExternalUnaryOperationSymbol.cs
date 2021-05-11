using Mono.Cecil;
using System.Diagnostics;

namespace Krypton.CompilationData.Symbols
{
    public sealed class ExternalUnaryOperationSymbol : UnaryOperationSymbol, IExternalSymbol
    {
        public ExternalUnaryOperationSymbol(MethodDefinition operation,
                                            Operator @operator,
                                            ExternalTypeSymbol operandType,
                                            ExternalTypeSymbol returnType)
            : base(operation.Name, @operator)
        {
            Debug.Assert(operation.Name.StartsWith("op_"));
            Debug.Assert((operation.Resolve().Attributes & MethodAttributes.SpecialName) == MethodAttributes.SpecialName);
            Debug.Assert((operation.Resolve().Attributes & MethodAttributes.Static) == MethodAttributes.Static);
            Debug.Assert(operation.Parameters.Count == 1);

            OperationReference = operation;

            OperandTypeSymbol = operandType;
            ReturnTypeSymbol = returnType;
        }

        public override TypeSymbol OperandTypeSymbol { get; }

        public MethodDefinition OperationReference { get; }

        public override ExternalTypeSymbol ReturnTypeSymbol { get; }

        MemberReference IExternalSymbol.Reference => OperationReference;
    }
}
