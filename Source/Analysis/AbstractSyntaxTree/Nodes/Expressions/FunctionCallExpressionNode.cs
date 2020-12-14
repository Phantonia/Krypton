using System.Collections.Generic;
using System.Text;

namespace Krypton.Analysis.AbstractSyntaxTree.Nodes.Expressions
{
    public sealed class FunctionCallExpressionNode : ExpressionNode
    {
        public FunctionCallExpressionNode(ExpressionNode functionExpression, int lineNumber) : base(lineNumber)
        {
            FunctionExpression = functionExpression;
        }

        public FunctionCallExpressionNode(ExpressionNode functionExpression, List<ExpressionNode>? arguments, int lineNumber) : base(lineNumber)
        {
            FunctionExpression = functionExpression;
            Arguments = arguments;
        }

        // Null if the function is called without arguments
        public List<ExpressionNode>? Arguments { get; } = null;

        public ExpressionNode FunctionExpression { get; }

        public override FunctionCallExpressionNode Clone()
        {
            return new(FunctionExpression, Arguments, LineNumber);
        }

        public override void GenerateCode(StringBuilder stringBuilder)
        {
            throw new System.NotImplementedException();
        }
    }
}
