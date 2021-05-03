namespace Krypton.CompilationData.Syntax.Statements
{
    public abstract record LoopStatementNode : BodiedStatementNode
    {
        private protected LoopStatementNode(BodyNode body)
            : base(body) { }
    }
}