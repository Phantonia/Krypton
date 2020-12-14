﻿using Krypton.Analysis.Grammatical;

namespace Krypton.Analysis.Lexical.Lexemes.SyntaxCharacters
{
    public sealed class LessThanLexeme : SyntaxCharacterLexeme, IOperatorLexeme
    {
        public LessThanLexeme(int lineNumber) : base(lineNumber) { }

        public override string Content => "<";

        public OperatorPrecedenceGroup PrecedenceGroup => OperatorPrecedenceGroup.Comparison;
    }
}
