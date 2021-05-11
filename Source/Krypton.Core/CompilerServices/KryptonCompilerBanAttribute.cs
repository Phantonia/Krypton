using System;

namespace Krypton.Core.CompilerServices
{
    [AttributeUsage(AttributeTargets.Class 
                  | AttributeTargets.Struct
                  | AttributeTargets.Interface
                  | AttributeTargets.Enum
                  | AttributeTargets.Delegate,
                    Inherited = true, AllowMultiple = false)]
    public sealed class KryptonCompilerBanAttribute : Attribute
    {
        public KryptonCompilerBanAttribute() { }
    }
}
