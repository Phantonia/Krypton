using System.Diagnostics;
using System.IO;

namespace Krypton.CompilationData.Syntax
{
    [DebuggerDisplay("{" + nameof(GetDebuggerDisplay) + "(),nq}")]
    public abstract class SyntaxNode : ISyntaxNode, IWritable
    {
        // New members have to be added to the interface as well!

        private protected SyntaxNode(SyntaxNode? parent)
        {
            ParentNode = parent;
        }

        public abstract bool IsLeaf { get; }

        public SyntaxNode? ParentNode { get; }

        protected virtual string GetDebuggerDisplay() => GetType().Name;

        public string ToCode()
        {
            StringWriter stringWriter = new();
            WriteCode(stringWriter);
            return stringWriter.ToString();
        }

        public abstract SyntaxNode WithParent(SyntaxNode newParent);

        public abstract void WriteCode(TextWriter output);
    }
}
