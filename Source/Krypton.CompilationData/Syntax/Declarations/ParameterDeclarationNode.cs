using Krypton.CompilationData.Symbols;
using Krypton.CompilationData.Syntax.Clauses;
using Krypton.CompilationData.Syntax.Tokens;
using System.IO;

namespace Krypton.CompilationData.Syntax.Declarations
{
    public sealed record ParameterDeclarationNode : NamedDeclarationNode
    {
        public ParameterDeclarationNode(IdentifierToken name,
                                        AsClauseNode asClause)
            : base(name)
        {
            AsClauseNode = asClause;
        }

        public AsClauseNode AsClauseNode { get; }

        public override bool IsLeaf => false;

        public override Token LexicallyFirstToken => NameToken;

        public override BoundDeclarationNode Bind(Symbol symbol)
            => new(this, symbol);

        public override void WriteCode(TextWriter output)
        {
            NameToken.WriteCode(output);
            AsClauseNode.WriteCode(output);
        }
    } 
}
