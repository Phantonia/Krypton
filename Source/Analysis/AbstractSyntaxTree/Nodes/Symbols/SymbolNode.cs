namespace Krypton.Analysis.AbstractSyntaxTree.Nodes.Symbols
{
    public abstract class SymbolNode : Node
    {
        protected SymbolNode(string name, int lineNumber) : base(lineNumber)
        {
            Name = name;
        }

        public string Name { get; }

        public abstract override SymbolNode Clone();
    }
}
