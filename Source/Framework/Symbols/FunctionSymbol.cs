using Krypton.Utilities;
using System.Collections.Generic;

namespace Krypton.Framework.Symbols
{
    public sealed class FunctionSymbol : NamedFrameworkSymbol
    {
        internal FunctionSymbol(string name,
                                FrameworkType returnType,
                                CodeGenerationInformation codeGenerationInfo,
                                IList<ParameterSymbol>? parameters = null) : base(name)
        {
            ReturnType = returnType;
            CodeGenerationInfo = codeGenerationInfo;
            Parameters = parameters.Finalize();
        }

        public CodeGenerationInformation CodeGenerationInfo { get; }

        public FinalList<ParameterSymbol>? Parameters { get; }

        public FrameworkType ReturnType { get; }
    }
}
