using Krypton.CompilationData.Syntax.Declarations;
using Krypton.CompilationData.Syntax.Types;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;

namespace Krypton.CompilationData.Syntax
{
    public sealed record ProgramNode : SyntaxNode, IExecutableNode
    {
        // Bug in compiler? Field topLevelNodes is not null when exiting the constructor...
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public ProgramNode(IEnumerable<TopLevelNode> topLevelNodes)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
            topLevelNodes = topLevelNodes.ToImmutableList();
            topLevelStatementNodes = MakeTopLevelStatementsBody(topLevelNodes);
        }

        private readonly ImmutableList<TopLevelNode> topLevelNodes;
        private readonly BodyNode topLevelStatementNodes;

        public override bool IsLeaf => false;

        public ImmutableList<TopLevelNode> TopLevelNodes
        {
            get => topLevelNodes;
            init
            {
                topLevelStatementNodes = MakeTopLevelStatementsBody(value);
                topLevelNodes = value;
            }
        }

        public BodyNode TopLevelStatementNodes => topLevelStatementNodes;

        public IEnumerable<ConstantDeclarationNode> GetConstantDeclarations()
            => GetDeclarations().OfType<ConstantDeclarationNode>();

        public IEnumerable<DeclarationNode> GetDeclarations()
            => TopLevelNodes.OfType<TopLevelDeclarationNode>()
                            .Select(d => d.DeclarationNode);

        public IEnumerable<FunctionDeclarationNode> GetFunctionDeclarations()
            => GetDeclarations().OfType<FunctionDeclarationNode>();

        public override void WriteCode(TextWriter output)
        {
            foreach (TopLevelNode topLevelNode in TopLevelNodes)
            {
                topLevelNode.WriteCode(output);
            }
        }

        private static BodyNode MakeTopLevelStatementsBody(IEnumerable<TopLevelNode> topLevelNodes)
        {
            return new BodyNode(openingBrace: null,
                                topLevelNodes.OfType<TopLevelStatementNode>()
                                             .Select(t => t.StatementNode),
                                closingBrace: null);
        }

        BodyNode IExecutableNode.BodyNode => TopLevelStatementNodes;

        TypeNode? IExecutableNode.ReturnTypeNode => null;
    }
}
