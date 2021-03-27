using Krypton.CompilationData.Symbols;
using System.IO;

namespace Krypton.CompilationData.Syntax.Statements
{
    public sealed class BoundReturnStatementNode : StatementNode
    {
        public BoundReturnStatementNode(ReturnStatementNode returnStatement,
                                        FunctionSymbol function,
                                        SyntaxNode? parent = null)
            : base(parent)
        {
            ReturnStatementNode = returnStatement.WithParent(this);
            FunctionSymbol = function;
        }

        public FunctionSymbol FunctionSymbol { get; }

        public ReturnStatementNode ReturnStatementNode { get; }

        public override bool IsLeaf => false;

        public override BoundReturnStatementNode WithParent(SyntaxNode newParent)
            => new(ReturnStatementNode, FunctionSymbol, newParent);

        public override void WriteCode(TextWriter output) => ReturnStatementNode.WriteCode(output);
    }
}
