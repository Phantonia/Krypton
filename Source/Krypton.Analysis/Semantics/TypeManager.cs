using Krypton.CompilationData;
using Krypton.CompilationData.Symbols;
using Krypton.CompilationData.Syntax;
using Krypton.CompilationData.Syntax.Types;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Krypton.Analysis.Semantics
{
    internal sealed class TypeManager
    {
        public TypeManager(ProgramNode program, SymbolTable symbolTable)
        {
            this.program = program;
            this.symbolTable = symbolTable;
        }

        private readonly ProgramNode program;
        private readonly SymbolTable symbolTable;

        public bool TryGetTypeSymbol(TypeNode? typeNode, out TypeSymbol typeSymbol)
        {
            if (typeNode == null)
            {
                typeSymbol = TypeSymbol.VoidType; // TODO
                return true;
            }

            if (typeNode is IdentifierTypeNode identifierTypeSpecNode)
            {
                if (symbolTable.TryGetSymbol(identifierTypeSpecNode.IdentifierToken.TextToString(), out typeSymbol!))
                {
                    identifierTypeSpecNode.Bind(typeSymbol);
                    return true;
                }

                return false;
            }
            else
            {
                Debug.Fail(message: null);
                typeSymbol = null;
                return false;
            }
        }
    }
}
