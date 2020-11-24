using Krypton.Analysis.AbstractSyntaxTree;
using Krypton.Analysis.AbstractSyntaxTree.Nodes;
using Krypton.Analysis.Lexical;
using System;

namespace Krypton.Analysis.Grammatical
{
    public sealed class ScriptParser
    {
        public ScriptParser(LexemeCollection lexemes)
        {
            Lexemes = lexemes;
            SyntaxTree = new SyntaxTree();
        }

        public LexemeCollection Lexemes { get; }

        public SyntaxTree SyntaxTree { get; }

        public Node? ParseNextNode()
        {
            throw new NotImplementedException();
        }
    }
}
