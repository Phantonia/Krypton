//using Krypton.CompilationData.Syntax.Tokens;
//using Krypton.CompilationData.Syntax.Types;
//using Krypton.Utilities;
//using System;

//namespace Krypton.Analysis.Syntax
//{
//    internal sealed class TypeParser
//    {
//        public TypeParser(FinalList<Token> tokens, Analyser analyser)
//        {
//            this.tokens = tokens;
//            this.analyser = analyser;
//        }

//        private readonly Analyser analyser;
//        private readonly FinalList<Token> tokens;

//        public TypeNode? ParseNextType(ref int index)
//        {
//            if (tokens.TryGet(index) is not IdentifierToken identifier)
//            {
//                throw new NotImplementedException();
//                //ErrorProvider.ReportError(ErrorCode.ExpectedIdentifier,
//                //                          code,
//                //                          tokens[index]);
//                //return null;
//            }

//            index++;
//            return new IdentifierTypeNode(identifier);
//        }
//    }
//}
