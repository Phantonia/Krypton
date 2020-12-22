using Krypton.Analysis.Grammatical;
using System;

namespace Krypton.Analysis.Lexical.Lexemes
{
    public sealed class CharacterOperatorLexeme : Lexeme, IOperatorLexeme
    {
        public CharacterOperatorLexeme(CharacterOperator @operator, int lineNumber) : base(lineNumber)
        {
            Operator = @operator;
        }

        public override string Content => Operator switch
        {
            CharacterOperator.Tilde => "~",
            CharacterOperator.DoubleAsterisk => "**",
            CharacterOperator.Asterisk => "*",
            CharacterOperator.ForeSlash => "/",
            CharacterOperator.Plus => "+",
            CharacterOperator.Minus => "-",
            CharacterOperator.Ampersand => "&",
            CharacterOperator.Caret => "^",
            CharacterOperator.Pipe => "|",
            CharacterOperator.LessThan => "<",
            CharacterOperator.LessThanEquals => "<=",
            CharacterOperator.GreaterThanEquals => ">=",
            CharacterOperator.GreaterThan => ">",
            CharacterOperator.DoubleEquals => "==",
            CharacterOperator.ExclamationEquals => "!=",
            _ => throw new InvalidOperationException(),
        };

        public CharacterOperator Operator { get; }

        public OperatorPrecedenceGroup PrecedenceGroup => Operator switch
        {
            CharacterOperator.Tilde => OperatorPrecedenceGroup.BitwiseNotAndNegation,
            CharacterOperator.DoubleAsterisk => OperatorPrecedenceGroup.Exponantiation,
            CharacterOperator.Asterisk => OperatorPrecedenceGroup.Multiplicative,
            CharacterOperator.ForeSlash => OperatorPrecedenceGroup.Multiplicative,
            CharacterOperator.Plus => OperatorPrecedenceGroup.Additive,
            CharacterOperator.Minus => OperatorPrecedenceGroup.Additive,
            CharacterOperator.Ampersand => OperatorPrecedenceGroup.Bitwise,
            CharacterOperator.Caret => OperatorPrecedenceGroup.Bitwise,
            CharacterOperator.Pipe => OperatorPrecedenceGroup.Bitwise,
            CharacterOperator.LessThan => OperatorPrecedenceGroup.Comparison,
            CharacterOperator.LessThanEquals => OperatorPrecedenceGroup.Comparison,
            CharacterOperator.GreaterThanEquals => OperatorPrecedenceGroup.Comparison,
            CharacterOperator.GreaterThan => OperatorPrecedenceGroup.Comparison,
            CharacterOperator.DoubleEquals => OperatorPrecedenceGroup.Equality,
            CharacterOperator.ExclamationEquals => OperatorPrecedenceGroup.Equality,
            _ => 0,
        };
    }
}
