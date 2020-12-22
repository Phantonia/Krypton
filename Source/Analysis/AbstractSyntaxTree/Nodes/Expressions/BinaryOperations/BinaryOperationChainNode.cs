using Krypton.Analysis.Lexical.Lexemes;
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
                CharacterOperatorLexeme { Operator: CharacterOperator.DoubleAsterisk } => new ExponentiationBinaryOperationExpressionNode(left, right, line),
                CharacterOperatorLexeme { Operator: CharacterOperator.Asterisk } => new MultiplicationBinaryOperationExpressionNode(left, right, line),
                KeywordLexeme { Keyword: ReservedKeyword.Div } => new IntegerDivisionBinaryOperationExpressionNode(left, right, line),
                CharacterOperatorLexeme { Operator: CharacterOperator.ForeSlash } => new RationalDivisionBinaryOperationExpressionNode(left, right, line),
                KeywordLexeme { Keyword: ReservedKeyword.Mod } => new ModuloBinaryOperationExpressionNode(left, right, line),
                CharacterOperatorLexeme { Operator: CharacterOperator.Plus } => new AdditionBinaryOperationExpressionNode(left, right, line),
                CharacterOperatorLexeme { Operator: CharacterOperator.Minus } => new SubtractionBinaryOperationExpressionNode(left, right, line),
                CharacterOperatorLexeme { Operator: CharacterOperator.Ampersand } => new BitwiseAndBinaryOperationExpressionNode(left, right, line),
                CharacterOperatorLexeme { Operator: CharacterOperator.Pipe } => new BitwiseOrBinaryOperationExpressionNode(left, right, line),
                CharacterOperatorLexeme { Operator: CharacterOperator.Caret } => new BitwiseXOrBinaryOperationExpressionNode(left, right, line),
                KeywordLexeme { Keyword: ReservedKeyword.Left } => new BitwiseLeftShiftBinaryOperationExpressionNode(left, right, line),
                KeywordLexeme { Keyword: ReservedKeyword.Right } => new BitwiseRightShiftBinaryOperationExpressionNode(left, right, line),
                CharacterOperatorLexeme { Operator: CharacterOperator.DoubleEquals } => new EqualityBinaryOperationExpressionNode(left, right, line),
                CharacterOperatorLexeme { Operator: CharacterOperator.ExclamationEquals } => new UnequalityBinaryOperationExpressionNode(left, right, line),
                KeywordLexeme { Keyword: ReservedKeyword.And } => new LogicalAndBinaryOperationExpressionNode(left, right, line),
                KeywordLexeme { Keyword: ReservedKeyword.Xor } => new LogicalXOrBinaryOperationExpressionNode(left, right, line),
                KeywordLexeme { Keyword: ReservedKeyword.Or } => new LogicalOrBinaryOperationExpressionNode(left, right, line),
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
