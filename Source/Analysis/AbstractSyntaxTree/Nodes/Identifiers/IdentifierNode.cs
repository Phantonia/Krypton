namespace Krypton.Analysis.AbstractSyntaxTree.Nodes.Identifiers
{
    public abstract class IdentifierNode : Node
    {
        protected private IdentifierNode(string identifier, int lineNumber) : base(lineNumber)
        {
            Identifier = identifier;
        }

        public string Identifier { get; }
    }
}
