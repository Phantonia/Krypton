﻿using Krypton.Framework;
using System;
using System.Diagnostics;

namespace Krypton.Analysis.Lexical.Lexemes
{
    internal sealed class CompoundAssignmentEqualsLexeme : Lexeme
    {
        public CompoundAssignmentEqualsLexeme(Operator @operator, int lineNumber, int index) : base(lineNumber, index)
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
            _ => throw new NotSupportedException(),
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
