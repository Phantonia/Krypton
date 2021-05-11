using System;

namespace Krypton.Core.CompilerServices
{
    [KryptonCompilerBan]
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class KryptonNamespaceSymbolsClassAttribute : Attribute
    {
        public KryptonNamespaceSymbolsClassAttribute() { }
    }
}
