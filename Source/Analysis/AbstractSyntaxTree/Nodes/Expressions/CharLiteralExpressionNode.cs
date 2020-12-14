﻿using System.Text;

namespace Krypton.Analysis.AbstractSyntaxTree.Nodes.Expressions
{
    public sealed class CharLiteralExpressionNode : LiteralExpressionNode
    {
        public CharLiteralExpressionNode(char value, int lineNumber) : base(lineNumber)
        {
            Value = value;
        }

        public char Value { get; }

        public override CharLiteralExpressionNode Clone()
        {
            return new(Value, LineNumber);
        }

        public override void GenerateCode(StringBuilder stringBuilder)
        {
            stringBuilder.Append((int)Value);
        }
    }
}
