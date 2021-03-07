using Krypton.Analysis;
using Krypton.CodeGeneration;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Krypton.Compiler
{
    internal sealed class SaveProvider
    {
        public SaveProvider(Compilation compilation, string template, string inputLocation)
        {
            savableCode = new Lazy<Task<string>>(async ()
                => await Task.Run(()
                    => CodeGenerator.GenerateCode(compilation,
                                                  template,
                                                  CodeGenerationMode.ToFile)));
            outputLocation = Path.ChangeExtension(inputLocation, "js");
        }

        private readonly string outputLocation;
        private readonly Lazy<Task<string>> savableCode;

        public async Task SaveAsync()
        {
            string code = await savableCode.Value;
            await File.WriteAllTextAsync(outputLocation, code);
            Console.WriteLine($"Saved successfully to {outputLocation}!");
            Console.WriteLine();
        }
    }
}
