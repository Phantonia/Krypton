using Krypton.Analysis.AST.Expressions.BinaryOperations;
using Krypton.Analysis.Lexical.Lexemes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Krypton.Analysis.AST.Expressions
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

            return operators.Select((o, i) => (@operator: o.PrecedenceGroup, index: i))
                            .Aggregate((a, b) => a.@operator > b.@operator ? a : b)
                            .index;
        }

        private ExpressionNode MakeExpressionNodeOfIndex(int index)
        {
            Debug.Assert(operators[index] is Lexeme);
            Lexeme operatorLexeme = (Lexeme)operators[index];

            ExpressionNode left = operands[index];
            ExpressionNode right = operands[index + 1];
            int lineNumber = operatorLexeme.LineNumber;

            return operatorLexeme switch
            {
                CharacterOperatorLexeme { Operator: CharacterOperator.DoubleAsterisk    } => new ExponentiationBinaryOperationExpressionNode   (left, right, lineNumber),
                CharacterOperatorLexeme { Operator: CharacterOperator.Asterisk          } => new MultiplicationBinaryOperationExpressionNode   (left, right, lineNumber),
                KeywordLexeme           { Keyword:  ReservedKeyword  .Div               } => new IntegerDivisionBinaryOperationExpressionNode  (left, right, lineNumber),
                CharacterOperatorLexeme { Operator: CharacterOperator.ForeSlash         } => new RationalDivisionBinaryOperationExpressionNode (left, right, lineNumber),
                KeywordLexeme           { Keyword:  ReservedKeyword  .Mod               } => new ModuloBinaryOperationExpressionNode           (left, right, lineNumber),
                CharacterOperatorLexeme { Operator: CharacterOperator.Plus              } => new AdditionBinaryOperationExpressionNode         (left, right, lineNumber),
                CharacterOperatorLexeme { Operator: CharacterOperator.Minus             } => new SubtractionBinaryOperationExpressionNode      (left, right, lineNumber),
                CharacterOperatorLexeme { Operator: CharacterOperator.Ampersand         } => new BitwiseAndBinaryOperationExpressionNode       (left, right, lineNumber),
                CharacterOperatorLexeme { Operator: CharacterOperator.Pipe              } => new BitwiseOrBinaryOperationExpressionNode        (left, right, lineNumber),
                CharacterOperatorLexeme { Operator: CharacterOperator.Caret             } => new BitwiseXorBinaryOperationExpressionNode       (left, right, lineNumber),
                KeywordLexeme           { Keyword:  ReservedKeyword  .Left              } => new BitwiseLeftShiftBinaryOperationExpressionNode (left, right, lineNumber),
                KeywordLexeme           { Keyword:  ReservedKeyword  .Right             } => new BitwiseRightShiftBinaryOperationExpressionNode(left, right, lineNumber),
                CharacterOperatorLexeme { Operator: CharacterOperator.DoubleEquals      } => new EqualityBinaryOperationExpressionNode         (left, right, lineNumber),
                CharacterOperatorLexeme { Operator: CharacterOperator.ExclamationEquals } => new UnequalityBinaryOperationExpressionNode       (left, right, lineNumber),
                KeywordLexeme           { Keyword:  ReservedKeyword  .And               } => new LogicalAndBinaryOperationExpressionNode       (left, right, lineNumber),
                KeywordLexeme           { Keyword:  ReservedKeyword  .Xor               } => new LogicalXorBinaryOperationExpressionNode       (left, right, lineNumber),
                KeywordLexeme           { Keyword:  ReservedKeyword  .Or                } => new LogicalOrBinaryOperationExpressionNode        (left, right, lineNumber),
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
