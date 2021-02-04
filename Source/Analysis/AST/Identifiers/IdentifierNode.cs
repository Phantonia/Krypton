using System.Diagnostics;

namespace Krypton.Analysis.Ast.Identifiers
{
    [DebuggerDisplay("{GetType().Name}; Identifier = {Identifier}")]
    public abstract class IdentifierNode : Node
    {
        private protected IdentifierNode(string identifier, int lineNumber, int index) : base(lineNumber, index)
        {
            Identifier = identifier;
        }

        public string Identifier { get; }
    }
}
