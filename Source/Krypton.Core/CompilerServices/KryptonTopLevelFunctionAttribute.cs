using System;

namespace Krypton.Core.CompilerServices
{
    [KryptonCompilerBan]
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public abstract class KryptonNamespaceSymbolAttribute : Attribute
    {
        private protected KryptonNamespaceSymbolAttribute() { }
    }
}
