using Krypton.Analysis;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("UnitTests")]
namespace Krypton.CodeGeneration
{
    public static class CodeGenerator
    {
        public static string GenerateCode(Compilation compilation, string template)
        {
            CodeGeneratorCore generatorCore = new(compilation, template);
            return generatorCore.GenerateCode();
        }
    }
}
