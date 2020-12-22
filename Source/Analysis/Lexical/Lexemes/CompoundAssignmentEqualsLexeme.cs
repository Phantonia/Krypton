using System;
using System.Diagnostics;

namespace Krypton.Analysis.Lexical.Lexemes
{
    public sealed class CompoundAssignmentEqualsLexeme : Lexeme
    {
        public CompoundAssignmentEqualsLexeme(CharacterOperator @operator, int lineNumber) : base(lineNumber)
        {
            Debug.Assert(IsLegalInCompoundAssignment(@operator));
            Operator = @operator;
        }

        public override string Content => Operator switch
        {
            CharacterOperator.DoubleAsterisk => "**=",
            CharacterOperator.Asterisk => "*=",
            CharacterOperator.ForeSlash => "/=",
            CharacterOperator.Plus => "+=",
            CharacterOperator.Minus => "-=",
            CharacterOperator.Ampersand => "&=",
            CharacterOperator.Caret => "^",
            CharacterOperator.Pipe => "|=",
            _ => throw new NotImplementedException(),
        };

        public CharacterOperator Operator { get; }

        private static bool IsLegalInCompoundAssignment(CharacterOperator @operator)
        {
            return @operator switch
            {
                CharacterOperator.DoubleAsterisk or
                CharacterOperator.Asterisk or
                CharacterOperator.ForeSlash or
                CharacterOperator.Plus or
                CharacterOperator.Minus or
                CharacterOperator.Ampersand or
                CharacterOperator.Caret or
                CharacterOperator.Pipe => true,
                _ => false,
            };
        }
    }
}
