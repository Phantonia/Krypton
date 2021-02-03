using Krypton.Framework;
using System.Collections.Generic;

namespace Krypton.Analysis.Ast.Symbols
{
    public sealed class BinaryOperationSymbolNode : SymbolNode
    {
        internal BinaryOperationSymbolNode(Operator @operator, TypeSymbolNode leftType, TypeSymbolNode rightType, TypeSymbolNode returnType, BinaryGenerator generator, int lineNumber) : base(identifier: string.Empty, lineNumber)
        {
            Operator = @operator;
            LeftOperandTypeNode = leftType;
            RightOperandTypeNode = rightType;
            ReturnTypeNode = returnType;
            Generator = generator;
        }

        public BinaryGenerator Generator { get; }

        public TypeSymbolNode LeftOperandTypeNode { get; }

        public Operator Operator { get; }

        public TypeSymbolNode ReturnTypeNode { get; }

        public TypeSymbolNode RightOperandTypeNode { get; }

        public override void PopulateBranches(List<Node> list)
        {
            list.Add(this);
        }
    }
}
