using Jering.Javascript.NodeJS;
using Krypton.Analysis;
using Krypton.CodeGeneration;
using System;
using System.Threading.Tasks;

namespace Krypton.Compiler
{
    internal sealed class RunProvider
    {
        public RunProvider(Compilation compilation, string template)
        {
            runnableCode = new Lazy<Task<string>>(async ()
                => await Task.Run(()
                    => CodeGenerator.GenerateCode(compilation,
                                                  template,
                                                  CodeGenerationMode.Run)));
        }

        private readonly Lazy<Task<string>> runnableCode;

        public async Task<string> RunAndGetOutputAsync()
        {
            string[] output = await RunCoreAsync();
            return string.Join("\r\n", output);
        }

        public async Task RunAsync()
        {
            string[] output = await RunCoreAsync();

            Console.WriteLine();

            foreach (string line in output)
            {
                Console.WriteLine(line);
            }

            Console.WriteLine();
            Console.WriteLine("Program finished successfully!");
            Console.WriteLine();
        }

        private async Task<string[]> RunCoreAsync()
        {
            string code = await runnableCode.Value;
            string[] output = await StaticNodeJSService.InvokeFromStringAsync<string[]>(code);
            return output;
        }
    }
}
