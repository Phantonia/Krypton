﻿using Krypton.Analysis.Grammatical;

namespace Krypton.Analysis.Lexical.Lexemes.SyntaxCharacters
{
    public sealed class AmpersandLexeme : SyntaxCharacterLexeme, IOperatorLexeme
    {
        public AmpersandLexeme(int lineNumber) : base(lineNumber) { }

        public override string Content => "&";

        public OperatorPrecedenceGroup PrecedenceGroup => OperatorPrecedenceGroup.Bitwise;
    }
}