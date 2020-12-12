using Krypton.Analysis.Lexical.Lexemes;
using Krypton.Analysis.Lexical.Lexemes.Keywords;
using Krypton.Analysis.Lexical.Lexemes.SyntaxCharacters;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Krypton.Analysis.AbstractSyntaxTree.Nodes.Expressions.BinaryOperations
{
    public sealed class BinaryOperationChainNode : ExpressionNode
    {
        public BinaryOperationChainNode(int lineNumber) : base(lineNumber) { }

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
            operands.Add(operand);
        }

        public override BinaryOperationChainNode Clone()
        {
            throw new NotSupportedException();
        }

        public override void GenerateCode(StringBuilder stringBuilder)
        {
            throw new NotSupportedException();
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

            return operators.Select((o, i) => (@operator: o.PrecedenceGroup, index: i))
                            .Aggregate((a, b) => (a.@operator > b.@operator) ? a : b)
                            .index;
        }

        private ExpressionNode MakeExpressionNodeOfIndex(int index)
        {
            Debug.Assert(operators[index] is Lexeme);
            Lexeme operatorLexeme = (Lexeme)operators[index];

            ExpressionNode left = operands[index];
            ExpressionNode right = operands[index + 1];
            int line = operatorLexeme.LineNumber;

            return operatorLexeme switch
            {
                DoubleAsteriskLexeme => new ExponentiationBinaryOperationExpressionNode(left, right, line),
                AsteriskLexeme => new MultiplicationBinaryOperationExpressionNode(left, right, line),
                DivKeywordLexeme => new IntegerDivisionBinaryOperationExpressionNode(left, right, line),
                ForeSlashLexeme => new RealDivisionBinaryOperationExpressionNode(left, right, line),
                ModKeywordLexeme => new ModuloBinaryOperationExpressionNode(left, right, line),
                PlusLexeme => new AdditionBinaryOperationExpressionNode(left, right, line),
                MinusLexeme => new SubtractionBinaryOperationExpressionNode(left, right, line),
                DoubleEqualsLexeme => new EqualityBinaryOperationExpressionNode(left, right, line),
                ExclamationEqualsLexeme => new UnequalityBinaryOperationExpressionNode(left, right, line),
                AndKeywordLexeme => new LogicalAndBinaryOperationExpressionNode(left, right, line),
                OrKeywordLexeme => new LogicalOrBinaryOperationExpressionNode(left, right, line),
                _ => OnFailure()
            };

            static ExpressionNode OnFailure()
            {
                Debug.Fail("Missed operator");
                return null;
            }
        }
    }
}
