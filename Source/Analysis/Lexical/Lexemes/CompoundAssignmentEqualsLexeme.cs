using Krypton.Framework;
using System;
using System.Diagnostics;

namespace Krypton.Analysis.Lexical.Lexemes
{
    public sealed class CompoundAssignmentEqualsLexeme : Lexeme
    {
        public CompoundAssignmentEqualsLexeme(Operator @operator, int lineNumber) : base(lineNumber)
        {
            Debug.Assert(IsLegalInCompoundAssignment(@operator));
            Operator = @operator;
        }

        public override string Content => Operator switch
        {
            Operator.DoubleAsterisk => "**=",
            Operator.Asterisk => "*=",
            Operator.ForeSlash => "/=",
            Operator.Plus => "+=",
            Operator.Minus => "-=",
            Operator.Ampersand => "&=",
            Operator.Caret => "^=",
            Operator.Pipe => "|=",
            _ => throw new NotImplementedException(),
        };

        public Operator Operator { get; }

        private static bool IsLegalInCompoundAssignment(Operator @operator)
        {
            return @operator switch
            {
                Operator.DoubleAsterisk or
                Operator.Asterisk or
                Operator.ForeSlash or
                Operator.Plus or
                Operator.Minus or
                Operator.Ampersand or
                Operator.Caret or
                Operator.Pipe => true,
                _ => false,
            };
        }
    }
}
