using System;

namespace Krypton.CompilationData.Syntax.Tokens
{
    public sealed class OperatorToken : Token
    {
        public OperatorToken(Operator @operator,
                             ReadOnlyMemory<char> text,
                             int lineNumber,
                             Trivia leadingTrivia)
            : base(text, lineNumber, leadingTrivia)
        {
            Operator = @operator;
        }

        public bool IsBinary => Operator == Operator.Minus | !IsUnary;

        public bool IsUnary => Operator is Operator.Minus or Operator.NotKeyword or Operator.Tilde;

        public Operator Operator { get; }

        public int Precedence => Operator.GetPrecedence();
    }
}
