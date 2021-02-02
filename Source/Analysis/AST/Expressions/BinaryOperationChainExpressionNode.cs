using Krypton.Analysis.Grammatical;
using Krypton.Analysis.Lexical.Lexemes;
using Krypton.Framework;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Krypton.Analysis.Ast.Expressions
{
    /* A BinaryOperationChainExpressionNode is a helper node
     * that should never escape grammatical analysis.
     * It represents multiple binary operations chained one
     * after the other (e.g. 4 + 5 * 6).
     * Its Resolve method performs the act of turning this
     * into the tree 4 + (5 * 6).
     * It saves an ordered list of ExpressionNodes that represents
     * the operands (0 op 1 op 2 op 3 ...) and one that represents
     * the operators (ex 0 ex 1 ex 2 ex ...). In a valid state
     * there is exactly one more operand than operator.
     * An operation chain can only resolved if it is in such
     * a valid state.
     */
    public sealed class BinaryOperationChainExpressionNode : ExpressionNode
    {
        public BinaryOperationChainExpressionNode(int lineNumber) : base(lineNumber) { }

        private readonly List<IOperatorLexeme> operators = new();
        private readonly List<ExpressionNode> operands = new();

        public void AddOperator(IOperatorLexeme @operator)
        {
            Debug.Assert(operands.Count == operators.Count + 1);
            operators.Add(@operator);
        }

        public void AddOperand(ExpressionNode operand)
        {
            Debug.Assert(operands.Count == operators.Count);
            operand.Parent = this;
            operands.Add(operand);
        }

        public override void PopulateBranches(List<Node> list)
        {
            list.Add(this);
            foreach (ExpressionNode operand in operands)
            {
                operand.PopulateBranches(list);
            }
        }

        public ExpressionNode Resolve()
        {
            Debug.Assert(operands.Count == operators.Count + 1);

            while (true)
            {
                int index = HighestOperator();

                ExpressionNode node = MakeExpressionNodeOfIndex(index);

                operands[index] = node;
                operators.RemoveAt(index);
                operands.RemoveAt(index + 1);

                if (operators.Count == 0)
                {
                    Debug.Assert(operands.Count == 1);

                    return operands[0];
                }
            }
        }

        private int HighestOperator()
        {
            if (operators.Count == 0)
            {
                return -1;
            }

            int index = operators.Select((o, i) => (@operator: o.PrecedenceGroup, index: i))
                                 .Aggregate((a, b) => a.@operator >= b.@operator ? a : b)
                                 .index;

            // Special case for ** operator: right associative
            if (operators[index] is { PrecedenceGroup: OperatorPrecedenceGroup.Exponantiation })
            {
                for (int i = operators.Count - 1; i >= 0; i--)
                {
                    if (operators[i] is { PrecedenceGroup: OperatorPrecedenceGroup.Exponantiation })
                    {
                        return i;
                    }
                }
            }

            return index;
        }

        private ExpressionNode MakeExpressionNodeOfIndex(int index)
        {
            Debug.Assert(operators[index] is Lexeme);
            Lexeme operatorLexeme = (Lexeme)operators[index];

            ExpressionNode left = operands[index];
            ExpressionNode right = operands[index + 1];
            int lineNumber = operatorLexeme.LineNumber;

            Operator @operator = operatorLexeme switch
            {
                CharacterOperatorLexeme chrLxm => chrLxm.Operator,
                KeywordLexeme kwdLxm => (Operator)kwdLxm.Keyword,
                _ => OnFailure()
            };

            return new BinaryOperationExpressionNode(left, right, @operator, lineNumber);

            static Operator OnFailure()
            {
                Debug.Fail(message: null);
                return 0;
            }
        }
    }
}
