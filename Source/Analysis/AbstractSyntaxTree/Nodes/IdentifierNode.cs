using System;
using System.Text;

namespace Krypton.Analysis.AbstractSyntaxTree.Nodes
{
    public sealed class IdentifierNode : Node
    {
        public IdentifierNode(string identifier, int lineNumber) : base(lineNumber)
        {
            Identifier = identifier;
        }

        public string Identifier { get; }

        public override IdentifierNode Clone()
        {
            return new(Identifier, LineNumber);
        }

        public override void GenerateCode(StringBuilder stringBuilder)
        {
            throw new NotImplementedException();
        }
    }
}
