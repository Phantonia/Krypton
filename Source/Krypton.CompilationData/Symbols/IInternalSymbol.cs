using Krypton.CompilationData.Syntax.Declarations;

namespace Krypton.CompilationData.Symbols
{
    public interface IInternalSymbol : ISymbol
    {
        DeclarationNode DeclarationNode { get; }
    }
}