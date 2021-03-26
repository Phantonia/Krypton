using System.Diagnostics;
using System.IO;

namespace Krypton.CompilationData.Syntax
{
    [DebuggerDisplay("{" + nameof(GetDebuggerDisplay) + "(),nq}")]
    public abstract class SyntaxNode : ISyntaxNode
    {
        // New members have to be added to the interface as well!

        private protected SyntaxNode(SyntaxNode? parent)
        {
            Parent = parent;
        }

        public abstract bool IsLeaf { get; }

        public SyntaxNode? Parent { get; }

        public string ToCode()
        {
            StringWriter stringWriter = new();
            WriteCode(stringWriter);
            return stringWriter.ToString();
        }

        public abstract SyntaxNode WithParent(SyntaxNode newParent);

        public abstract void WriteCode(TextWriter output);

        private string GetDebuggerDisplay() => GetType().Name;
    }
}
