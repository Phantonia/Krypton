using System;

namespace Krypton.Core.CompilerServices
{
    [KryptonCompilerBan]
    [AttributeUsage(AttributeTargets.Field, Inherited = true)]
    public abstract class KryptonConstantAttribute : Attribute
    {
        private protected KryptonConstantAttribute(ConstantType type)
        {
            Type = type;
        }

        public abstract object ObjectValue { get; }

        public ConstantType Type { get; }
    }
}
