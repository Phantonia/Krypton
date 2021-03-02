using Krypton.Analysis.Ast.Symbols;
using Krypton.Framework;
using System.Collections.Generic;

namespace Krypton.Analysis.Ast.Expressions
{
    public class UnaryOperationExpressionNode : ExpressionNode
    {
        internal UnaryOperationExpressionNode(ExpressionNode operand, Operator @operator, int lineNumber, int index) : base(lineNumber, index)
        {
            OperandNode = operand;
            Operator = @operator;
        }

        public ExpressionNode OperandNode { get; }

        public UnaryOperationSymbolNode? Operation { get; private set; }

        public Operator Operator { get; }

        public void Bind(UnaryOperationSymbolNode operation)
        {
            Operation = operation;
        }

        public override void PopulateBranches(List<Node> list)
        {
            list.Add(this);
            OperandNode.PopulateBranches(list);
        }
    }
}
