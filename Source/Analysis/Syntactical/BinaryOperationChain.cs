using Krypton.Analysis.Ast;
using Krypton.Analysis.Ast.Expressions;
using Krypton.Analysis.Lexical.Lexemes;
using Krypton.Framework;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Krypton.Analysis.Syntactical
{
    // This class in some sense cheats the node hierarchie.
    // Krypton.Analysis.Ast.* types should only save state and no logic.
    // This node however has logic, which is why it is in this namespace
    // instead. I also purposefully violated the policy to keep the
    // base class's name and only adding words in front.
    // No instance of this class should survive syntactical analysis,
    // else there's a bug.
    internal sealed class BinaryOperationChain : ExpressionNode
    {
        internal BinaryOperationChain(int lineNumber, int index) : base(lineNumber, index) { }

        private readonly List<IOperatorLexeme> operators = new();
        private readonly List<ExpressionNode> operandNodes = new();

        public void AddOperator(IOperatorLexeme @operator)
        {
            Debug.Assert(operandNodes.Count == operators.Count + 1);
            operators.Add(@operator);
        }

        public void AddOperand(ExpressionNode operand)
        {
            Debug.Assert(operandNodes.Count == operators.Count);
            operand.ParentNode = this;
            operandNodes.Add(operand);
        }

        public override void PopulateBranches(List<Node> list)
        {
            list.Add(this);
            foreach (ExpressionNode operand in operandNodes)
            {
                operand.PopulateBranches(list);
            }
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

            int index = operators.Select((o, i) => (precedenceGroup: o.PrecedenceGroup, index: i))
                                 .Aggregate((a, b) => a.precedenceGroup >= b.precedenceGroup ? a : b)
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

            ExpressionNode left = operandNodes[index];
            ExpressionNode right = operandNodes[index + 1];
            int lineNumber = left.LineNumber;
            int nodeIndex = left.Index;

            Operator @operator = operatorLexeme switch
            {
                CharacterOperatorLexeme chrLxm => chrLxm.Operator,
                KeywordLexeme kwdLxm => (Operator)kwdLxm.Keyword,
                _ => OnFailure()
            };

            return new BinaryOperationExpressionNode(left, right, @operator, lineNumber, nodeIndex);

            static Operator OnFailure()
            {
                Debug.Fail(message: null);
                return 0;
            }
        }
    }
}
