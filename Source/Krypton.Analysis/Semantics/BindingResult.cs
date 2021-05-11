using Krypton.CompilationData;
using Krypton.CompilationData.Syntax;

namespace Krypton.Analysis.Semantics
{
    public sealed record BindingResult(ProgramNode BoundProgramNode, SymbolTable GlobalSymbolTable);
}