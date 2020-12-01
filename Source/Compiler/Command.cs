using System.Diagnostics.CodeAnalysis;

namespace Krypton.Compiler
{
    internal abstract partial class Command
    {
        protected Command() { }

        public abstract string CommandName { get; }

#nullable disable
        public string FullCommand { get; private set; }
#nullable enable

        public abstract void Execute();

        public static partial bool TryParse(string input, [NotNullWhen(true)] out Command? command);
    }
}
