namespace Krypton.Analysis.AbstractSyntaxTree.Nodes.Statements
{
    public interface IParentStatementNode
    {
        StatementCollectionNode Statements { get; }
    }
}
