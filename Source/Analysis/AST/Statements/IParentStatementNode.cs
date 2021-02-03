namespace Krypton.Analysis.Ast.Statements
{
    public interface IParentStatementNode : INode
    {
        StatementCollectionNode StatementNodes { get; }
    }
}
