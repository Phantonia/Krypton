using Krypton.Core.CompilerServices;

namespace Krypton.Core
{
    [KryptonCompilerBan]
    [KryptonTypeAliasesClass]
    public static class TypeAliases
    {
        [KryptonTypeAlias(typeof(string))]
        public const string String = nameof(System.String);
    }
}
