﻿using Krypton.Analysis.Ast.Identifiers;
using Krypton.Analysis.Ast.Symbols;
using System.Collections.Generic;
using System.Diagnostics;

namespace Krypton.Analysis.Ast.Expressions
{
    [DebuggerDisplay("{GetType().Name}; Identifier = {Identifier}")]
    public sealed class IdentifierExpressionNode : ExpressionNode
    {
        internal IdentifierExpressionNode(string identifier, int lineNumber, int index) : base(lineNumber, index)
        {
            IdentifierNode = new UnboundIdentifierNode(identifier, lineNumber, index)
            {
                ParentNode = this
            };
        }

        public string Identifier => IdentifierNode.Identifier;

        public IdentifierNode IdentifierNode { get; private set; }

        public void Bind(SymbolNode symbol)
        {
            IdentifierNode = new BoundIdentifierNode(Identifier, symbol, IdentifierNode.LineNumber, Index)
            {
                ParentNode = this
            };
        }

        public override void PopulateBranches(List<Node> list)
        {
            list.Add(this);
            IdentifierNode.PopulateBranches(list);
        }
    }
}
