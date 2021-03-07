using Krypton.Analysis;
using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("UnitTests")]
namespace Krypton.Compiler
{
    internal static class Program
    {
        private static async Task Main(string[] args)
        {
#if RELEASE
            try
            {
#endif
            if (args.Length < 1 || !File.Exists(args[0]))
            {
                Console.WriteLine("Please open this program with a Krypton file!");
                Console.ReadKey();
                return;
            }

            string location = args[0];

            Console.WriteLine("Analysing the code...");

            CompilationProvider compilationProvider = new(location);

            Compilation? compilation = await compilationProvider.GetCompilationAsync();

            if (compilation == null)
            {
                await AsyncConsole.ReadKeyAsync();
                return;
            }

            string template = await File.ReadAllTextAsync("template.js");

            Console.WriteLine("The code is valid. Enter a command (Help for help)");
            Console.WriteLine();

            await HandleCommands(compilation, location, template);
#if RELEASE
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred:");
                Console.WriteLine(ex.GetType().FullName);
                Console.WriteLine(ex.Message);
            }
#endif
        }

        private static async Task HandleCommands(Compilation compilation, string originalLocation, string template)
        {
            RunProvider? runProvider = null;
            SaveProvider? saveProvider = null;

            while (true)
            {
                Console.Write(">> ");
                string? command = await AsyncConsole.ReadTrimmedLineAsync();

                if (command == null)
                {
                    return;
                }
                else if (command.Equals("Run", StringComparison.InvariantCultureIgnoreCase))
                {
                    runProvider ??= new RunProvider(compilation, template);
                    await runProvider.RunAsync();
                }
                else if (command.Equals("Compile", StringComparison.InvariantCultureIgnoreCase))
                {
                    saveProvider ??= new SaveProvider(compilation, template, originalLocation);
                    await saveProvider.SaveAsync();
                }
                else if (command.Equals("Exit", StringComparison.InvariantCultureIgnoreCase))
                {
                    return;
                }
                else if (command.Equals("Help", StringComparison.InvariantCultureIgnoreCase))
                {
                    Console.WriteLine("--- Help ---");
                    Console.WriteLine("Run - runs the code directly.");
                    Console.WriteLine($"Compile - compiles the code to the file {Path.ChangeExtension(originalLocation, "js")}.");
                    Console.WriteLine("Exit - leaves");
                }
                else
                {
                    Console.WriteLine("Sorry, this command doesn't exist.");
                }
            }
        }
    }
}
