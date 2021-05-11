using System;

namespace Krypton.Core.CompilerServices
{
    [KryptonCompilerBan]
    [AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    public sealed class KryptonTypeAliasAttribute : Attribute
    {
        public KryptonTypeAliasAttribute(Type type)
        {
            Type = type;
        }

        public Type Type { get; }
    }
}
