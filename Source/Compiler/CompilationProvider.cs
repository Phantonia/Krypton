using Krypton.Analysis;
using Krypton.Analysis.Errors;
using System;
using System.IO;
using System.Threading.Tasks;
using ErrorEventArgs = Krypton.Analysis.Errors.ErrorEventArgs;

namespace Krypton.Compiler
{
    internal sealed class CompilationProvider
    {
        public CompilationProvider(string kryptonFileLocation)
        {
            this.kryptonFileLocation = kryptonFileLocation;
            ErrorProvider.Error += OnError;
        }

        private Compilation? compilation;
        private readonly string kryptonFileLocation;

        public async Task<Compilation?> GetCompilationAsync()
        {
            if (compilation == null)
            {
                string code = await File.ReadAllTextAsync(kryptonFileLocation);
                compilation = await Task.Run(() => Analyser.Analyse(code));
            }

            return compilation;
        }

        private void OnError(ErrorEventArgs e)
        {
            Console.WriteLine(e.GetFullMessage());
        }
    }
}
