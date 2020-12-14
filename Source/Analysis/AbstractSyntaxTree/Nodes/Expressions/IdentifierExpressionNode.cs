using System.Text;

namespace Krypton.Analysis.AbstractSyntaxTree.Nodes.Expressions
{
    public sealed class IdentifierExpressionNode : ExpressionNode
    {
        public IdentifierExpressionNode(string simpleIdentifier, int lineNumber) : base(lineNumber)
        {
            SimpleIdentifier = simpleIdentifier;
        }

        public string SimpleIdentifier { get; }

        public override IdentifierExpressionNode Clone()
        {
            return new(SimpleIdentifier, LineNumber);
        }

        public override void GenerateCode(StringBuilder stringBuilder)
        {
            throw new System.NotImplementedException();
        }
    }
}
