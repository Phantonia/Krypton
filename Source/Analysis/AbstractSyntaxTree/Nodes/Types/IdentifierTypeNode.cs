using System.Text;

namespace Krypton.Analysis.AbstractSyntaxTree.Nodes.Types
{
    public sealed class IdentifierTypeNode : TypeNode
    {
        public IdentifierTypeNode(string identifier, int lineNumber) : base(lineNumber)
        {
            Identifier = new IdentifierNode(identifier, lineNumber);
        }

        private IdentifierTypeNode(IdentifierNode identifier, int lineNumber) : base(lineNumber)
        {
            Identifier = identifier;
        }

        public IdentifierNode Identifier { get; }

        public override IdentifierTypeNode Clone()
        {
            return new(Identifier.Clone(), LineNumber);
        }

        public override void GenerateCode(StringBuilder stringBuilder)
        {
            throw new System.NotImplementedException();
        }
    }
}
