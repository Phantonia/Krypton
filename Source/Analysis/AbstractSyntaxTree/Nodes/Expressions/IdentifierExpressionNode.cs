﻿using Krypton.Analysis.AbstractSyntaxTree.Nodes.Identifiers;
using Krypton.Analysis.AbstractSyntaxTree.Nodes.Symbols;
using System.Collections.Generic;

namespace Krypton.Analysis.AbstractSyntaxTree.Nodes.Expressions
{
    public sealed class IdentifierExpressionNode : ExpressionNode, IBindable
    {
        public IdentifierExpressionNode(string identifier, int lineNumber) : base(lineNumber)
        {
            IdentifierNode = new UnboundIdentifierNode(identifier, lineNumber)
            {
                Parent = this
            };
        }

        private IdentifierExpressionNode(IdentifierNode identifier, int lineNumber) : base(lineNumber)
        {
            IdentifierNode = identifier;
        }

        public string Identifier => IdentifierNode.Identifier;

        public IdentifierNode IdentifierNode { get; private set; }

        public void Bind(SymbolNode symbol)
        {
            IdentifierNode = new BoundIdentifierNode(Identifier, symbol, IdentifierNode.LineNumber) { Parent = this };
        }

        public override void PopulateBranches(List<Node> list)
        {
            list.Add(this);
            IdentifierNode.PopulateBranches(list);
        }
    }
}
