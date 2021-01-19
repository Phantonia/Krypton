namespace Krypton.Analysis.AbstractSyntaxTree.Nodes.Expressions
{
    /* An ExpressionNode represents an expression according
     * to the spec, so for example:
     * - an argument
     * - a variable initializer
     * - a return value
     * - a sub expression
     * Branches: none as defined by this abstract class
     * LineNumber: the line number of the first lexeme
     *             that makes up the expression
     */
    public abstract class ExpressionNode : Node
    {
        protected private ExpressionNode(int lineNumber) : base(lineNumber) { }
    }
}
