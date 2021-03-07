using Jering.Javascript.NodeJS;
using Krypton.Analysis;
using Krypton.Analysis.Errors;
using Krypton.CodeGeneration;
using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using ErrorEventArgs = Krypton.Analysis.Errors.ErrorEventArgs;

[assembly: InternalsVisibleTo("UnitTests")]
namespace Krypton.Compiler
{
    internal static class Program
    {
        private const string Code = @"
        Func HelloWorld()
        {
            Output(""Hello world"");
        }

        HelloWorld();
        HelloWorld();
        ";

        private static void Main(string[] args)
        {
#if RELEASE
            try
            {
#endif
            //if (args.Length < 1 || !File.Exists(args[0]))
            //{
            //    Console.WriteLine("Please open this program with a Krypton file!");
            //    Console.ReadKey();
            //    return;
            //}

            //string location = args[0];

            //string code = File.ReadAllText(location);

            string code = Code;
            string location = "";

            ErrorProvider.Error += OnError;

            Compilation? compilation = Analyser.Analyse(code);

            if (compilation == null)
            {
                Console.ReadKey();
                return;
            }

            string template = File.ReadAllText("template.js");

            Console.WriteLine("The code is valid. Enter a command (Help for help)");

            HandleCommands(compilation, location, template);
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

        private static void HandleCommands(Compilation compilation, string originalLocation, string template)
        {
            string? code = null;

            while (true)
            {
                string? command = Console.ReadLine()?.Trim();

                if (command == null)
                {
                    return;
                }
                else if (command.Equals("Run", StringComparison.InvariantCultureIgnoreCase))
                {
                    RunAsync(compilation, template).GetAwaiter().GetResult();
                }
                else if (command.Equals("Compile", StringComparison.InvariantCultureIgnoreCase))
                {
                    InitCode();

                    Debug.Assert(code != null);

                    string outputLocation = Path.ChangeExtension(originalLocation, "js");

                    if (File.Exists(outputLocation))
                    {
                        Console.WriteLine("This file already exists. Do you want to overwrite it? (y/n)");

                        ConsoleKeyInfo key = Console.ReadKey();

                        if (key.Key != ConsoleKey.Y)
                        {
                            Console.WriteLine("Okay, the file will not be overwritten.");
                            continue;
                        }
                    }

                    File.WriteAllText(outputLocation, code);

                    Console.WriteLine($"Successfully compiled to {outputLocation}.");
                }
                else if (command.Equals("Exit", StringComparison.InvariantCultureIgnoreCase))
                {
                    return;
                }
                else if (command.Equals("Help", StringComparison.InvariantCultureIgnoreCase))
                {
                    Console.WriteLine("--- Help ---");
                    Console.WriteLine("Run - runs the code directly. Requires node.js to be installed.");
                    Console.WriteLine("Compile - compiles the code to a file called the same as the Krypton file but with extension .js.");
                    Console.WriteLine("Exit - leaves");
                }
                else
                {
                    Console.WriteLine("Sorry, I don't know this command.");
                }
            }

            void InitCode()
            {
                code ??= CodeGenerator.GenerateCode(compilation, template: "", CodeGenerationMode.ToFile);
            }
        }

        private static void OnError(ErrorEventArgs e)
        {
            Console.WriteLine(e.GetFullMessage());
        }

        private static async Task RunAsync(Compilation compilation, string template)
        {
            Console.WriteLine();

            string code = await Task.Run(() => CodeGenerator.GenerateCode(compilation, template, CodeGenerationMode.Run));
            string output = await StaticNodeJSService.InvokeFromStringAsync<string>(code);

            Console.WriteLine(output);
            Console.WriteLine("Program finished successfully!");
            Console.WriteLine();
        }
    }
}
