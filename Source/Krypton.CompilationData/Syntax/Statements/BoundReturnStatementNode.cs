using Krypton.CompilationData.Symbols;
using System.IO;

namespace Krypton.CompilationData.Syntax.Statements
{
    public sealed record BoundReturnStatementNode : StatementNode
    {
        public BoundReturnStatementNode(ReturnStatementNode returnStatement,
                                        FunctionSymbol function)
        {
            ReturnStatementNode = returnStatement;
            FunctionSymbol = function;
        }

        public FunctionSymbol FunctionSymbol { get; init; }

        public ReturnStatementNode ReturnStatementNode { get; init; }

        public override bool IsLeaf => false;

        public override void WriteCode(TextWriter output)
            => ReturnStatementNode.WriteCode(output);
    }
}
