using Krypton.Framework;
using System.Collections.Generic;
using System.Diagnostics;

namespace Krypton.Analysis.Ast.Symbols
{
    [DebuggerDisplay("{GetType().Name}; Operator = {Operator}")]
    public sealed class UnaryOperationSymbolNode : SymbolNode
    {
        internal UnaryOperationSymbolNode(Operator @operator,
                                          TypeSymbolNode operandType,
                                          TypeSymbolNode returnType,
                                          UnaryGenerator generator,
                                          int lineNumber,
                                          int index) : base(string.Empty, lineNumber, index)
        {
            Operator = @operator;
            OperandTypeNode = operandType;
            ReturnTypeNode = returnType;
            Generator = generator;
        }

        public UnaryGenerator Generator { get; }

        public TypeSymbolNode OperandTypeNode { get; }

        public Operator Operator { get; }

        public TypeSymbolNode ReturnTypeNode { get; }

        public override void PopulateBranches(List<Node> list)
        {
            list.Add(this);
        }
    }
}
