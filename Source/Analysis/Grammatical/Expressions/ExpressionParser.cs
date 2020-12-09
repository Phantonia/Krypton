using Krypton.Analysis.AbstractSyntaxTree.Nodes.Expressions;
using Krypton.Analysis.Lexical;
using Krypton.Analysis.Lexical.Lexemes;
using System;

namespace Krypton.Analysis.Grammatical.Expressions
{
    internal class ExpressionParser
    {
        public ExpressionParser(LexemeCollection lexemes)
        {
            Lexemes = lexemes;
        }

        private int index = 0;

        public int Index
        {
            get => index;
            init => index = value;
        }

        public LexemeCollection Lexemes { get; }

        public virtual ExpressionNode ParseNextExpression()
        {
            throw new NotImplementedException();
        }
    }

    internal sealed class OperandExpressionParser : ExpressionParser
    {
        public OperandExpressionParser(LexemeCollection lexemes) : base(lexemes) { }

        public override ExpressionNode ParseNextExpression()
        {
            return base.ParseNextExpression();
        }
    }
}
