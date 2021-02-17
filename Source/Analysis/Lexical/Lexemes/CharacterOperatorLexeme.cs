using Krypton.Analysis.Syntactical;
using Krypton.Framework;
using System;

namespace Krypton.Analysis.Lexical.Lexemes
{
    internal sealed class CharacterOperatorLexeme : Lexeme, IOperatorLexeme
    {
        public CharacterOperatorLexeme(Operator @operator, int lineNumber, int index) : base(lineNumber, index)
        {
            Operator = @operator;
        }

        public override string Content => Operator switch
        {
            Operator.Tilde => "~",
            Operator.DoubleAsterisk => "**",
            Operator.Asterisk => "*",
            Operator.ForeSlash => "/",
            Operator.Plus => "+",
            Operator.Minus => "-",
            Operator.Ampersand => "&",
            Operator.Caret => "^",
            Operator.Pipe => "|",
            Operator.LessThan => "<",
            Operator.LessThanEquals => "<=",
            Operator.GreaterThanEquals => ">=",
            Operator.GreaterThan => ">",
            Operator.DoubleEquals => "==",
            Operator.ExclamationEquals => "!=",
            Operator.SingleRightArrow => "->",
            Operator.SingleLeftArrow => "<-",
            _ => throw new InvalidOperationException(),
        };

        public Operator Operator { get; }

        public OperatorPrecedenceGroup PrecedenceGroup => Operator switch
        {
            Operator.Tilde => OperatorPrecedenceGroup.BitwiseNotAndNegation,
            Operator.DoubleAsterisk => OperatorPrecedenceGroup.Exponantiation,
            Operator.Asterisk => OperatorPrecedenceGroup.Multiplicative,
            Operator.ForeSlash => OperatorPrecedenceGroup.Multiplicative,
            Operator.Plus => OperatorPrecedenceGroup.Additive,
            Operator.Minus => OperatorPrecedenceGroup.Additive,
            Operator.Ampersand => OperatorPrecedenceGroup.Bitwise,
            Operator.Caret => OperatorPrecedenceGroup.Bitwise,
            Operator.Pipe => OperatorPrecedenceGroup.Bitwise,
            Operator.SingleRightArrow => OperatorPrecedenceGroup.Shift,
            Operator.SingleLeftArrow => OperatorPrecedenceGroup.Shift,
            Operator.LessThan => OperatorPrecedenceGroup.Comparison,
            Operator.LessThanEquals => OperatorPrecedenceGroup.Comparison,
            Operator.GreaterThanEquals => OperatorPrecedenceGroup.Comparison,
            Operator.GreaterThan => OperatorPrecedenceGroup.Comparison,
            Operator.DoubleEquals => OperatorPrecedenceGroup.Equality,
            Operator.ExclamationEquals => OperatorPrecedenceGroup.Equality,
            _ => OperatorPrecedenceGroup.None,
        };
    }
}
