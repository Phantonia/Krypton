using Krypton.CompilationData.Syntax.Clauses;
using Krypton.CompilationData.Syntax.Tokens;
using System.IO;

namespace Krypton.CompilationData.Syntax.Declarations
{
    public sealed class ParameterDeclarationNode : NamedDeclarationNode
    {
        public ParameterDeclarationNode(IdentifierToken name,
                                        AsClauseNode asClause,
                                        SyntaxNode? parent = null)
            : base(name, parent)
        {
            AsClauseNode = asClause.WithParent(this);
        }

        public AsClauseNode AsClauseNode { get; }

        public override bool IsLeaf => false;

        public override ParameterDeclarationNode WithParent(SyntaxNode newParent)
            => new(NameToken, AsClauseNode, newParent);

        public override void WriteCode(TextWriter output)
        {
            NameToken.WriteCode(output);
            AsClauseNode.WriteCode(output);
        }
    } 
}
