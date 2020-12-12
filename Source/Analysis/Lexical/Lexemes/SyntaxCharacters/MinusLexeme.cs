﻿using Krypton.Analysis.Grammatical;

namespace Krypton.Analysis.Lexical.Lexemes.SyntaxCharacters
{
    public sealed class MinusLexeme : SyntaxCharacterLexeme, IOperatorLexeme
    {
        public MinusLexeme(int lineNumber) : base(lineNumber) { }

        public override string Content => "-";

        public OperatorPrecedenceGroup PrecedenceGroup => OperatorPrecedenceGroup.Depending; // It could be a unary or an additive operator
    }
}
