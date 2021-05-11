using Krypton.CompilationData;
using Krypton.CompilationData.Symbols;
using Krypton.CompilationData.Syntax;
using Mono.Cecil;
using System.Collections.Generic;
using System.Linq;

namespace Krypton.Analysis.Semantics
{
    internal sealed partial class Loader
    {
        public Loader(ProgramNode program)
        {
            this.program = program;
        }

        private readonly ProgramNode program;
        private readonly Dictionary<TypeReference, ExternalTypeSymbol> externalTypes = new();

        private static readonly AssemblyDefinition coreAssembly = AssemblyDefinition.ReadAssembly("Krypton.Core.dll");

        public SymbolTable GatherSymbols()
        {
            return new SymbolTable(Enumerable.Empty<KeyValuePair<string, Symbol>>());

            //List<KeyValuePair<string, Symbol>> symbols = new();

            //AddTypesFromAssembly(coreAssembly, symbols);
        }
    }
}
