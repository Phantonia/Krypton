using System.Diagnostics;

namespace Krypton.Analysis.Ast.Identifiers
{
    [DebuggerDisplay("{GetType().Name}; Identifier = {Identifier}")]
    public abstract class IdentifierNode : Node
    {
        private protected IdentifierNode(string identifier, int lineNumber) : base(lineNumber)
        {
            Identifier = identifier;
        }

        public string Identifier { get; }
    }
}
