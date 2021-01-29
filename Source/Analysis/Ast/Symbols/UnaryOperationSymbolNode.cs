using Krypton.Framework;
using System.Collections.Generic;

namespace Krypton.Analysis.Ast.Symbols
{
    public sealed class UnaryOperationSymbolNode : SymbolNode
    {
        internal UnaryOperationSymbolNode(Operator @operator, TypeSymbolNode operandType, TypeSymbolNode returnType, UnaryGenerator generator, int lineNumber) : base(string.Empty, lineNumber)
        {
            Operator = @operator;
            OperandType = operandType;
            ReturnType = returnType;
            Generator = generator;
        }

        public UnaryGenerator Generator { get; }

        public TypeSymbolNode OperandType { get; }

        public Operator Operator { get; }

        public TypeSymbolNode ReturnType { get; }

        public override void PopulateBranches(List<Node> list)
        {
            list.Add(this);
        }
    }
}
