using Mono.Cecil;

namespace Krypton.CompilationData.Symbols
{
    public interface IExternalSymbol : ISymbol
    {
        MemberReference Reference { get; }
    }
}