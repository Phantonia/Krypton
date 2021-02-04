namespace Krypton.Analysis.Ast.Expressions
{
    public abstract class ExpressionNode : Node
    {
        private protected ExpressionNode(int lineNumber, int index) : base(lineNumber, index) { }
    }
}
