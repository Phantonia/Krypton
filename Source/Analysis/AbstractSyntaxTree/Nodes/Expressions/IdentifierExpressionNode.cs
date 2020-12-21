using System.Text;

namespace Krypton.Analysis.AbstractSyntaxTree.Nodes.Expressions
{
    public sealed class IdentifierExpressionNode : ExpressionNode
    {
        public IdentifierExpressionNode(string identifier, int lineNumber) : base(lineNumber)
        {
            IdentifierNode = new IdentifierNode(identifier, lineNumber);
        }

        private IdentifierExpressionNode(IdentifierNode identifier, int lineNumber) : base(lineNumber)
        {
            IdentifierNode = identifier;
        }

        public string Identifier => IdentifierNode.Identifier;

        public IdentifierNode IdentifierNode { get; }

        public override IdentifierExpressionNode Clone()
        {
            return new(IdentifierNode.Clone(), LineNumber);
        }

        public override void GenerateCode(StringBuilder stringBuilder)
        {
            throw new System.NotImplementedException();
        }
    }
}
