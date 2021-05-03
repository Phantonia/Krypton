using Krypton.CompilationData;
using Krypton.CompilationData.Syntax.Expressions;
using Krypton.CompilationData.Syntax.Tokens;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Krypton.Analysis.Syntax
{
    // This class in some sense cheats the node hierarchie.
    // Krypton.Analysis.Ast.* types should only save state and no logic.
    // This node however has logic, which is why it is in this namespace
    // instead. I also purposefully violated the policy to keep the
    // base class's name and only adding words in front.
    // No instance of this class should survive syntactical analysis,
    // else there's a bug.
    internal sealed class BinaryOperationChain
    {
        internal BinaryOperationChain() { }

        private readonly List<OperatorToken> operators = new();
        private readonly List<ExpressionNode> operandNodes = new();

        public void AddOperator(OperatorToken @operator)
        {
            Debug.Assert(operandNodes.Count == operators.Count + 1);
            operators.Add(@operator);
        }

        public void AddOperand(ExpressionNode operand)
        {
            Debug.Assert(operandNodes.Count == operators.Count);
            operandNodes.Add(operand);
        }

        public ExpressionNode Resolve()
        {
            Debug.Assert(operandNodes.Count == operators.Count + 1);

            while (true)
            {
                int index = HighestOperator();

                ExpressionNode node = MakeExpressionNodeOfIndex(index);

                operandNodes[index] = node;
                operators.RemoveAt(index);
                operandNodes.RemoveAt(index + 1);

                if (operators.Count == 0)
                {
                    Debug.Assert(operandNodes.Count == 1);

                    return operandNodes[0];
                }
            }
        }

        private int HighestOperator()
        {
            if (operators.Count == 0)
            {
                return -1;
            }

            int index = operators.Select((o, i) => (precedenceGroup: o.Precedence, index: i))
                                 .Aggregate((a, b) => a.precedenceGroup >= b.precedenceGroup ? a : b)
                                 .index;

            // Special case for ** operator: right associative
            if (operators[index].Operator == Operator.DoubleAsterisk)
            {
                for (int i = operators.Count - 1; i >= 0; i--)
                {
                    if (operators[i].Operator == Operator.DoubleAsterisk)
                    {
                        return i;
                    }
                }
            }

            return index;
        }

        private ExpressionNode MakeExpressionNodeOfIndex(int index)
        {
            ExpressionNode left = operandNodes[index];
            ExpressionNode right = operandNodes[index + 1];
            OperatorToken @operator = operators[index];

            return new BinaryOperationExpressionNode(left, @operator, right);
        }
    }
}
