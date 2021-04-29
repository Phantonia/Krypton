using Krypton.CompilationData.Syntax.Tokens;
using System;

namespace Krypton.CompilationData.Syntax.Declarations
{
    public abstract record NamedDeclarationNode : DeclarationNode
    {
        private protected NamedDeclarationNode(IdentifierToken name)
        {
            NameToken = name;
        }

        public ReadOnlyMemory<char> Name => NameToken.Text;

        public IdentifierToken NameToken { get; init; }

        protected override string GetDebuggerDisplay() => $"{base.GetDebuggerDisplay()}; Name = {Name}";
    }
}
