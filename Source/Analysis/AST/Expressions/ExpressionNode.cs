namespace Krypton.Analysis.Ast.Expressions
{
    public abstract class ExpressionNode : Node
    {
        private protected ExpressionNode(int lineNumber) : base(lineNumber) { }
    }
}
