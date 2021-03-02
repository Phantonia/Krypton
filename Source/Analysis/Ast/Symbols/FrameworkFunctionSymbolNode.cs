using Krypton.Framework;
using System.Collections.Generic;

namespace Krypton.Analysis.Ast.Symbols
{
    public sealed class FrameworkFunctionSymbolNode : FunctionSymbolNode
    {
        internal FrameworkFunctionSymbolNode(string name,
                                             IEnumerable<ParameterSymbolNode> parameters,
                                             TypeSymbolNode? returnType,
                                             CodeGenerationInformation codeGenerationInfo,
                                             int lineNumber,
                                             int index) : base(name, parameters, returnType, lineNumber, index)
        {
            CodeGenerationInfo = codeGenerationInfo;
        }

        public CodeGenerationInformation CodeGenerationInfo { get; }
    }
}
