namespace Krypton.Analysis.AbstractSyntaxTree.Nodes.Types
{
    public abstract class TypeNode : Node
    {
        protected TypeNode(int lineNumber) : base(lineNumber) { }

        public abstract override TypeNode Clone();
    }
}
