using Krypton.Analysis;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("UnitTests")]
namespace Krypton.CodeGeneration
{
    public static class CodeGenerator
    {
        public static string GenerateCode(Compilation compilation, string template, CodeGenerationMode mode)
        {
            CodeGeneratorCore generatorCore = new(compilation, template, mode);
            return generatorCore.GenerateCode();
        }
    }
}
