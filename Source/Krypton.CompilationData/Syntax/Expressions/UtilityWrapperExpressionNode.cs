using System;
using System.ComponentModel;
using System.IO;

namespace Krypton.CompilationData.Syntax.Expressions
{
    // This is a utility class used to cheat the node hierarchie.
    // If, for example, one would like to temporarily insert a value of a non-node
    // type. However, one should be careful and use the instance as short as possible,
    // as almost all of the methods and properties throw.
    public sealed record UtilityWrapperExpressionNode<T> : ExpressionNode
    {
        private const string DiagnosticsMessage
            = "A " + nameof(UtilityWrapperExpressionNode<T>) + " survived too long. "
            + "If you have no idea, what that means: You found a bug. "
            + "Please open an issue on GitHub/Phantonia/Krypton and let us know that you encountered this bug "
            + "and, importantly, how. Thanks for your help to make Krypton better!";

        public UtilityWrapperExpressionNode(T value)
        {
            Value = value;
        }

        public T Value { get; }

#pragma warning disable CS0809 // Obsolete member overrides non-obsolete member
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete(message: "Will always throw NotSupportedException", error: true)]
        public override bool IsLeaf
            => throw new NotSupportedException(DiagnosticsMessage);

        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete(message: "Will always throw NotSupportedException", error: true)]
        public override void WriteCode(TextWriter output)
            => throw new NotSupportedException(DiagnosticsMessage);
#pragma warning restore CS0809 // Obsolete member overrides non-obsolete member
    }
}
