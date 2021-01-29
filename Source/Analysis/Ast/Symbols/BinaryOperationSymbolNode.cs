using Krypton.Framework;
using System.Collections.Generic;

namespace Krypton.Analysis.Ast.Symbols
{
    public sealed class BinaryOperationSymbolNode : SymbolNode
    {
        internal BinaryOperationSymbolNode(Operator @operator, TypeSymbolNode leftType, TypeSymbolNode rightType, TypeSymbolNode returnType, BinaryGenerator generator, int lineNumber) : base(string.Empty, lineNumber)
        {
            Operator = @operator;
            LeftType = leftType;
            RightType = rightType;
            ReturnType = returnType;
            Generator = generator;
        }

        public BinaryGenerator Generator { get; }

        public TypeSymbolNode LeftType { get; }

        public Operator Operator { get; }

        public TypeSymbolNode ReturnType { get; }

        public TypeSymbolNode RightType { get; }

        public override void PopulateBranches(List<Node> list)
        {
            list.Add(this);
        }
    }
}
