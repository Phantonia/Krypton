namespace Krypton.CompilationData.Syntax.Tokens
{
    public sealed class CompoundAssignmentToken : Token
    {
        public CompoundAssignmentToken(Operator @operator,
                                       int lineNumber,
                                       Trivia leadingTrivia)
            : base(lineNumber, leadingTrivia)
        {
            Operator = @operator;
        }

        public Operator Operator { get; }

        public override string Text => Operator.ToText() + "=";
    }
}
