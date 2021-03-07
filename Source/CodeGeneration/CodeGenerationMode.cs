using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("UnitTests")]
namespace Krypton.CodeGeneration
{
    public enum CodeGenerationMode
    {
        None = 0,
        ToFile,
        Run,
    }
}
