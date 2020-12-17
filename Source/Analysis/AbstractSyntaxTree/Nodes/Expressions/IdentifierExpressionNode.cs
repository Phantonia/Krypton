﻿using System.Text;

namespace Krypton.Analysis.AbstractSyntaxTree.Nodes.Expressions
{
    public sealed class IdentifierExpressionNode : ExpressionNode
    {
        public IdentifierExpressionNode(string identifier, int lineNumber) : base(lineNumber)
        {
            Identifier = new IdentifierNode(identifier, lineNumber);
        }

        private IdentifierExpressionNode(IdentifierNode identifier, int lineNumber) : base(lineNumber)
        {
            Identifier = identifier;
        }

        public IdentifierNode Identifier { get; }

        public override IdentifierExpressionNode Clone()
        {
            return new(Identifier.Clone(), LineNumber);
        }

        public override void GenerateCode(StringBuilder stringBuilder)
        {
            throw new System.NotImplementedException();
        }
    }
}
